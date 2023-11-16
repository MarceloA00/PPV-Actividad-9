using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunRouletteButton : MonoBehaviour
{
    public GunObject gun;
    private TextMeshProUGUI buttonText;
    public GunUI gunUI;

    // Start is called before the first frame update
    void Start()
    {
        buttonText = transform.Find("GunText").GetComponent<TextMeshProUGUI>();
        buttonText.text = gun.name;
    }

    public void changeWeapon()
    {
        gunUI.ChangeWeapon(gun);
    }
}
