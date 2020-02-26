using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControllerScript : MonoBehaviour
{
    public int index;
    public int maxIndex;
    public AudioSource audioSource;

    float prevKeyDown;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(Input.GetAxis("Vertical") != 0 && prevKeyDown == 0)
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
