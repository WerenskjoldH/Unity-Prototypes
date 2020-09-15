using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    [SerializeField]
    float minimumAngle = 15;
    [SerializeField]
    float maximumAngle = 90;

    [SerializeField]
    float rotationSmoothing = 10.0f;

    void Start()
    {
        
    }

    void Update()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit mouseHit;

        if(Physics.Raycast(mouseRay, out mouseHit))
        {

            Vector3 dir = mouseHit.point - transform.position;
            Quaternion newRotation = Quaternion.LookRotation(dir, Vector3.up);

            float xRotation = newRotation.eulerAngles.x > 90 ? newRotation.eulerAngles.x - 360 : newRotation.eulerAngles.x;

            float clampedXRotation = Mathf.Clamp(xRotation, minimumAngle, maximumAngle);
            newRotation = Quaternion.Euler(clampedXRotation, newRotation.eulerAngles.y, newRotation.eulerAngles.z);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, Time.deltaTime * rotationSmoothing);
        }
    }
}
