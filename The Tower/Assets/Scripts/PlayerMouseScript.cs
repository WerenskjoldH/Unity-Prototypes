using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseScript : MonoBehaviour
{
    Vector2 mouseLocation;

    float mouseHoldDuration;

    public Vector3 GetMousePosition()
    {
        // Do we need to be so protective of the mouse location, I wonder?
        return new Vector3(mouseLocation.x, mouseLocation.y);
    }

    public bool GetMouseDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool GetMouseHold()
    {
        return Input.GetMouseButton(0);
    }

    public bool GetMouseUp()
    {
        return Input.GetMouseButtonUp(0);
    }

    public float GetMouseHoldDuration()
    {
        return mouseHoldDuration;
    }

    private void Update()
    {
        mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.position = mouseLocation;

        if (GetMouseHold())
            mouseHoldDuration += Time.deltaTime;

        if (GetMouseUp())
            mouseHoldDuration = 0;
    }
}
