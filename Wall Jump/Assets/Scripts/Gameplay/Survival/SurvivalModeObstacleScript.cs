using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SurvivalModeObstacleScript : MonoBehaviour
{
    public Color dangerousColor = Color.red;
    Color defaultColor;

    // This is automatically set
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

        if (survivalManager == null)
            survivalManager = GameObject.FindWithTag("GameManager").GetComponent<SurvivalModeManagerScript>();
        survivalManager.environmentObjects.Add(gameObject);
    }

    private void OnDestroy()
    {
        // So we don't accidentally destroy the player, we remove them as a child
        PlayerControllerScript pS = gameObject.GetComponentInChildren<PlayerControllerScript>();
        if (pS != null)
        {
            pS.parentObject.transform.SetParent(null);
        }

        // So we don't leave null references, we remove this game object from the manager script
        survivalManager.environmentObjects.Remove(gameObject);
    }

    private void Update()
    {
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
