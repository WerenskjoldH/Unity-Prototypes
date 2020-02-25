using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuSelectScript : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    public bool selected = false;

    private string startingText;
    void Start()
    {
        startingText = uiText.text;
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool intersect = gameObject.GetComponent<Collider2D>().bounds.Contains(mousePos);

        if (!selected && intersect)
        {
            Debug.Log("selected");
            uiText.text = "-" + uiText.text + "-";
            selected = true;
        }
        else if(selected && !intersect)
        {
            uiText.text = startingText;
            selected = false;
        }
    }
}
