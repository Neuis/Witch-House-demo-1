using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoulsCounterPrefab : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textField;

    private void Awake()
    {
        User.instance.OnCurrencyChanged += UpdateAfterCurrencyChanged;
        UpdateAfterCurrencyChanged();
    }

    private void UpdateAfterCurrencyChanged()
    {
        textField.text = User.instance.GetMoney().ToString();
    }
}
