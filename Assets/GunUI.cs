using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunUI : MonoBehaviour
{

    Controls controls;
    public GameObject player;
    public GameObject gunRoulette;
    bool onGunWheel = false;
    public TextMeshProUGUI gunNameText, fireRateText, isAutoText;

    private void Awake()
    {
        controls = new();
        controls.UI.GunWheel.performed += GunWheel;
    }

    private void OnEnable()
    {
        controls.UI.Enable();
    }

    private void OnDisable()
    {
        controls.UI.Disable();
    }

    void Start()
    {
        gunNameText.text = player.GetComponent<PlayerController>().activeGun.name;
        fireRateText.text = "Fire Rate: " + player.GetComponent<PlayerController>().activeGun.FireRate.ToString();
        isAutoText.text = "Automatic: " + (player.GetComponent<PlayerController>().activeGun.IsAuto ? "Yes" : "No");

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GunWheel(InputAction.CallbackContext context)
    {
        if (!onGunWheel)
        {
            ChangeStatsText();
            Cursor.lockState = CursorLockMode.None;
            player.GetComponent<PlayerController>().enabled = false;
            gunRoulette.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            player.GetComponent<PlayerController>().enabled = true;
            gunRoulette.SetActive(false);
        }

        onGunWheel = !onGunWheel;

    }

    public void ChangeStatsText()
    {
        gunNameText.text = player.GetComponent<PlayerController>().activeGun.name;
        fireRateText.text = "Fire Rate: " + player.GetComponent<PlayerController>().activeGun.FireRate.ToString();
        isAutoText.text = "Automatic: " + (player.GetComponent<PlayerController>().activeGun.IsAuto ? "Yes" : "No");
    }

    public void ChangeWeapon(GunObject newGun)
    {
        player.GetComponent<PlayerController>().activeGun = newGun;
        ChangeStatsText();
        player.GetComponent<PlayerController>().newGunChange();
    }
}
