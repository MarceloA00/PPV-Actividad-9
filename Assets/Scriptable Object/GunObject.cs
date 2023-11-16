using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun Object", menuName = "Gun Object", order = 1)]

public class GunObject : ScriptableObject
{
    [SerializeField] GameObject gunModel;
    [SerializeField] GameObject flare;
    [SerializeField] float fireRate;
    [SerializeField] bool isAuto;

    public GameObject GunModel => gunModel;
    public GameObject Flare => flare;
    public LineRenderer FlareLine => flare.GetComponent<LineRenderer>();
    public float FireRate => fireRate;
    public bool IsAuto => isAuto;
}
