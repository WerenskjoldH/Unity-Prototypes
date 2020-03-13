using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManagerScript : MonoBehaviour
{
    public TutorialPlayerControllerScript playerScript;

    public GameObject topWall, rightWall, bottomWall, leftWall; 

    public TextMeshProUGUI textArea;

    bool playerFirstClick = false;
    bool playerExhausted = false;

    int tutorialStep = 0;

    IEnumerator WaitToMoveToNextStep(float waitTime, int toWhichTutorialStep)
    {
        float timePassed = 0.0f;

        // This prevents repeated calls
        tutorialStep = -1;

        while(timePassed < waitTime)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        tutorialStep = toWhichTutorialStep;
    }

    private void Start()
    {
        textArea.text = "Welcome to the <color=#df7126>Tutorial</color>\nPress <color=#d95763>Left Mouse Button</color> to begin";

        playerScript.DisableGrinding();
        playerScript.DisableSticking();
    }

    void Update()
    {
        // This is going to be/get terrible, hopefully we can find a way to get around this
        // Maybe function chaining?
        if (tutorialStep == 0)
        {
            if (playerFirstClick == false && Input.GetMouseButtonUp(0))
            {
                textArea.text = "Fantastic!\nYou now know how to <color=#df7126>launch</color>";
                playerFirstClick = true;
                tutorialStep = 1;
            }
        }else if(tutorialStep == 1) {
            textArea.text = "You now know how to <color=#df7126>launch</color>\nYou have <color=#df7126>" + (playerScript.totalJumpsAllowed - playerScript.GetNumberJumpsUsed()) + "</color> jumps left";
            if(playerScript.GetNumberJumpsUsed() == playerScript.totalJumpsAllowed)
            {
                StartCoroutine(WaitToMoveToNextStep(2.0f, 2));
            }
        }
        else if(tutorialStep == 2)
        {
            if (!playerExhausted && playerScript.GetNumberJumpsUsed() == playerScript.totalJumpsAllowed)
            {
                playerScript.EnableSticking();
                textArea.text = "Try hitting the <color=#d95763>Right Mouse Button</color>";
                playerExhausted = true;
            }

            if(playerExhausted && playerScript.IsStuckToSurface() && Input.GetMouseButtonUp(1))
            {
                textArea.text = "This is how you recover <color=#df7126>jumps</color>\nPractice a bit";
                /// Maybe count how many times the player recovers their jumps? Then move on
                StartCoroutine(WaitToMoveToNextStep(8.0f, 3));
            }
        }
        else if(tutorialStep == 3)
        {
            // Sticking To Walls
            // React differently based on the amount of jumps made in last step?
            textArea.text = "Now then, let's try <color=#df7126>sticking</color> to walls";
            StartCoroutine(WaitToMoveToNextStep(4.0f, 4));
        }
        else if(tutorialStep == 4)
        {
            textArea.text = "To <color=#df7126>stick</color> to a wall hold the <color=#d95763>Right Mouse Button</color> before colliding with it\n" +
                "I have highlighted the <color=#99e550>left wall</color> try to stick to it";
            leftWall.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(0.2517f, 0.6507f, 0.8980f);
            GameObject stuckToObject = playerScript.GetObjectStuckTo();
            if (stuckToObject == leftWall)
            {
                tutorialStep = 5;
            }
        }
        else if(tutorialStep == 5)
        {
            textArea.text = "Now try to stick to the <color=#99e550>right wall</color>\n<color=#df7126>without</color> touching the ground";
            leftWall.GetComponent<SpriteRenderer>().color = Color.white;
            rightWall.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(0.2517f, 0.6507f, 0.8980f);
            bottomWall.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(0, 0.7093f, 0.6745f);
        }
        else if(tutorialStep == 6)
        {
            // Player touched the ground, tell them to go back to the left wall
            // Point back to step 5
        }
        else if(tutorialStep == 7)
        {
            // Player made it to the right wall without touching the ground
        }
        


    }
}
