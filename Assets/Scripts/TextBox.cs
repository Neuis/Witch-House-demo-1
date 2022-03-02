using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBox : MonoBehaviour
{
    [SerializeField]
    private RectTransform background;
    [SerializeField]
    private TextMeshProUGUI textField;
    public void SetText(string newText)
    {
        textField.text = newText;
        StartCoroutine(WaitAndUpdateBoxHeight());
    }

    private IEnumerator WaitAndUpdateBoxHeight()
    {
        yield return new WaitForEndOfFrame();
        background.sizeDelta = new Vector2(background.rect.width, textField.preferredHeight);
        yield return null;
    }
}
