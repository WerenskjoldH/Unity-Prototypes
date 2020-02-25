using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuButtonSelectionScript : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    public string defaultText;

    void Start()
    {
        defaultText = uiText.text;
    }

    public void TextSelected()
    {
        Debug.Log("Selected");
        uiText.text = "-" + defaultText + "-";
    }

    public void TextUnselected()
    {
        uiText.text = defaultText;
    }
}
