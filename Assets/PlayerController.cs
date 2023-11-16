using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    Controls controls;
    CharacterController charCon;
    Vector3 ray;
    public GameObject bullet;
    public GameObject head;
    public Camera cam;
    public float speed;
    float rotationX, rotationY;
    public float mouseSensibility;
    private readonly static float gravity = 9.8f;
    private float vForce = 0;
    private float jumpForce = 7;
    Queue<GameObject> pool = new();
    public GunObject activeGun;
    public int limit;
    bool shootable = true;

    public GameObject gunPivot;

    private void Awake()
    {
        charCon = GetComponent<CharacterController>();
        controls = new();
        controls.FPSPlayer.Jump.performed += Jump;
        controls.FPSPlayer.Shoot.performed += Shoot;
    }

    private void OnEnable()
    {
        controls.FPSPlayer.Enable();
    }

    private void OnDisable()
    {
        controls.FPSPlayer.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        newGunChange();
    }

    private void Update()
    {
        Rotation();
    }

    void FixedUpdate()
    {
        Move();
        OffGroundMove();
        AutoShoot();
    }

    private void Move()
    {
        Vector2 movement = controls.FPSPlayer.Movement.ReadValue<Vector2>();
        Vector3 direction = (transform.forward * movement.y + transform.right * movement.x).normalized;
        charCon.Move(direction * speed * Time.fixedDeltaTime);
    }

    private void Rotation()
    {
        Vector2 aim = controls.FPSPlayer.Aim.ReadValue<Vector2>();
        rotationX += aim.x * mouseSensibility;
        rotationY += aim.y * mouseSensibility;
        rotationY = Math.Clamp(rotationY, -70f, 70f);
        head.transform.localRotation = Quaternion.Euler(-rotationY, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, rotationX, 0f);
    }

    private void OffGroundMove()
    {

        vForce -= gravity * Time.deltaTime;
        charCon.Move(new(0, vForce * Time.deltaTime, 0));

    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (charCon.isGrounded)
        {
            vForce = Mathf.Sqrt(jumpForce * -2 * -gravity);
        }
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (!ActiveGun.IsAuto && shootable)
        {
            StartCoroutine(fireRateDelay());
            CastRay();
        }
    }

    private void AutoShoot()
    {
        if (ActiveGun.IsAuto)
        {
            bool isShooting = controls.FPSPlayer.Shoot.IsPressed();

            if (isShooting && shootable)
            {
                StartCoroutine(fireRateDelay());
                CastRay();
            }
        }
    }

    private void CastRay()
    {
        ray = cam.ViewportToWorldPoint(new(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, cam.transform.forward, out hit, 20))
        {
            InstantiateNewBullet(hit);
        }
    }

    public void InstantiateNewBullet(RaycastHit hit)
    {

        if (HasFreeSpace())
        {
            pool.Enqueue(Instantiate(bullet, hit.point, Quaternion.identity));
        }
        else
        {
            GameObject go = pool.Dequeue();
            go.transform.position = hit.point;
            pool.Enqueue(go);
        }
    }

    private bool HasFreeSpace() { return pool.Count >= limit ? false : true; }

    void OnDrawGizmosSelected()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Gizmos.DrawRay(cam.transform.position, cam.transform.forward * 20);
    }

    private GunObject ActiveGun
    {
        get => activeGun;
    }

    private IEnumerator fireRateDelay()
    {
        shootable = false;
        yield return new WaitForSeconds(ActiveGun.FireRate);
        shootable = true;
    }

    public void newGunChange()
    {
        if (gunPivot.transform.childCount != 0)
        {
            Destroy(gunPivot.transform.GetChild(0).gameObject);
        }

        Instantiate(activeGun.GunModel, gunPivot.transform);
    }
}
