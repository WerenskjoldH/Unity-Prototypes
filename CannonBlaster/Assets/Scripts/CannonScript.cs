using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    [SerializeField]
    float minimumPitchAngle = 15;
    [SerializeField]
    float maximumPitchAngle = 90;

    [SerializeField]
    float rotationSmoothing = 150.0f;

    [SerializeField]
    Transform endOfBarrel;

    [SerializeField]
    GameObject hitLocationTestObject;

    // This is used for ensuring the mouse is targetting somewhere in the world, this is an edge case, but a good one to cover
    bool mouseIntersectsWorld = false;
    Vector3 mouseWorldPosition;

    bool cannonFiring = false;
    RaycastHit cannonHit;

    void FireCannon()
    {
        Instantiate(hitLocationTestObject, cannonHit.point, Quaternion.identity);
        cannonFiring = false;
    }

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
                    FireCannon();
            }

            mouseIntersectsWorld = true;
            mouseWorldPosition = mouseHit.point;
        }
        else
        {
            mouseIntersectsWorld = false;
        }

    }

    void CannonRotation()
    {
        Vector3 aimDirection = mouseWorldPosition - transform.position;
        Quaternion newRotation = Quaternion.LookRotation(aimDirection, Vector3.up);

        float xRotation = newRotation.eulerAngles.x > 90 ? newRotation.eulerAngles.x - 360 : newRotation.eulerAngles.x;

        float clampedXRotation = Mathf.Clamp(xRotation, minimumPitchAngle, maximumPitchAngle);
        newRotation = Quaternion.Euler(clampedXRotation, newRotation.eulerAngles.y, newRotation.eulerAngles.z);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, Time.deltaTime * rotationSmoothing);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && mouseIntersectsWorld && !cannonFiring)
            cannonFiring = true;

        CannonRotation();
    }
}
