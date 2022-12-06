using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string minMaxAmmoDenominator = "/";

    [Header("UI elements")]
    [SerializeField] private Text selectedWeaponText;
    [SerializeField] private Text ammoAmountText;

    public void SetWeaponName(string name)
    {
        selectedWeaponText.text = name;
    }

    public void SetAmmoAmount(int current, int capacity)
    {
        ammoAmountText.text = current.ToString() + minMaxAmmoDenominator + capacity.ToString();
    }
}
