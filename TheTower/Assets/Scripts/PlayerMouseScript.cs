using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseScript : MonoBehaviour
{
    Vector2 mouseLocation;

    public Vector2 GetMousePosition()
    {
        // Do we need to be so protective of the mouse location, I wonder?
        return new Vector2(mouseLocation.x, mouseLocation.y);
    }

    private void Update()
    {
        mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.position = mouseLocation;
    }
}
