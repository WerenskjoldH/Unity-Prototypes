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

    [SerializeField]
    Transform endOfBarrel;

    [SerializeField]
    GameObject hitLocationObject;

    Vector3 mouseWorldPosition;

    bool cannonFiring = false;
    RaycastHit cannonHit;

    void FixedUpdate()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit mouseHit;

        if (Physics.Raycast(mouseRay, out mouseHit))
        {
            if (cannonFiring)
            {
                Ray barrelDirection = new Ray(endOfBarrel.position, endOfBarrel.position - transform.position);

                if (Physics.Raycast(barrelDirection, out cannonHit))
                {
                    Instantiate(hitLocationObject, cannonHit.point, Quaternion.identity);

                    cannonFiring = false;
                }
            }

            mouseWorldPosition = mouseHit.point;
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !cannonFiring)
            cannonFiring = true;

        Vector3 aimDirection = mouseWorldPosition - transform.position;
        Quaternion newRotation = Quaternion.LookRotation(aimDirection, Vector3.up);

        float xRotation = newRotation.eulerAngles.x > 90 ? newRotation.eulerAngles.x - 360 : newRotation.eulerAngles.x;

        float clampedXRotation = Mathf.Clamp(xRotation, minimumAngle, maximumAngle);
        newRotation = Quaternion.Euler(clampedXRotation, newRotation.eulerAngles.y, newRotation.eulerAngles.z);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, Time.deltaTime * rotationSmoothing);
    }
}
