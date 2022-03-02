using UnityEngine;
using TMPro;

public class SimpleButtonWithText : SimpleButton
{
    [SerializeField]
    private TextMeshProUGUI textField;
    
    public void SetText(string val)
    {
        textField.text = val;
    }  
}
