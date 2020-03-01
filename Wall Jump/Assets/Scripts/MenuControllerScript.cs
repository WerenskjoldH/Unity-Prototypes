using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuControllerScript : MonoBehaviour
{
    public int index;
    public int maxIndex;
    public AudioSource audioSource;

    float prevKeyDown;
    GraphicRaycaster graphicRaycaster;
    MenuButtonScript selectedButton;

    private void Awake()
    {
        graphicRaycaster = GetComponent<GraphicRaycaster>();
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        
        // Mouse Selection Input
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        List<RaycastResult> raycastResults = new List<RaycastResult>();

        pointerData.position = Input.mousePosition;
        this.graphicRaycaster.Raycast(pointerData, raycastResults);

        if (selectedButton)
            selectedButton.mouseHovering = false;

        foreach (RaycastResult result in raycastResults)
        {
            MenuButtonScript resultScript = result.gameObject.GetComponentInParent<MenuButtonScript>();

            if (resultScript)
            {
                index = resultScript.GetIndex();
                resultScript.mouseHovering = true;
                selectedButton = resultScript;
            }
        }

        // Keyboard Selection Input
        if (Input.GetAxis("Vertical") != 0 && prevKeyDown == 0)
        {
            if(Input.GetAxis("Vertical") < 0)
            {
                index = index + 1;
                if (index > maxIndex)
                    index = 0;
            }
            else if(Input.GetAxis("Vertical") > 0)
            {
                index -= 1;
                if (index < 0)
                    index = maxIndex;
            }
        }
        prevKeyDown = Input.GetAxis("Vertical");
    }
}
