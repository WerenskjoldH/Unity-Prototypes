using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalModeWallScript : MonoBehaviour
{
    public Color dangerousColor = Color.red;
    Color defaultColor;

    SpriteRenderer spriteRenderer;

    public SurvivalModeManagerScript survivalManager;

    bool activated = false;
    public bool dangerous = false;

    int speedOfBlinks = 3;

    public bool IsActivated()
    {
        return activated;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;

        survivalManager.environmentObjects.Add(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Deactivate();
    }

    public void Deactivate()
    {
        activated = false;
        spriteRenderer.color = defaultColor;
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

            timePassed += Time.deltaTime;
            yield return null;
        }

        dangerous = true;
        spriteRenderer.color = dangerousColor;
    }
}
