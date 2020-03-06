using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalModeWallScript : MonoBehaviour
{
    public Color dangerousColor = Color.red;
    Color defaultColor;
    SpriteRenderer spriteRenderer;
    bool activated = false;
    public bool dangerous = false;
    // This is a little arbitrary right now
    int speedOfBlinks = 3;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Activate(3.0f);
    }

    public void Activate(float timeToActivate)
    {
        if (activated == true)
            return;

        activated = true;
        StartCoroutine(Activating(timeToActivate));
    }

    IEnumerator Activating(float timeToActivate)
    {
        float timePassed = 0;

        while (timePassed < timeToActivate)
        {
            if(Mathf.Sin(speedOfBlinks * (timePassed * timePassed)) < 0)
            {
                spriteRenderer.color = dangerousColor;
            }
            else
            {
                spriteRenderer.color = defaultColor;
            }

            Debug.Log("Running Coroutine: " + timePassed);
            timePassed += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Ending Coroutine");
        dangerous = true;
        spriteRenderer.color = dangerousColor;
    }
}
