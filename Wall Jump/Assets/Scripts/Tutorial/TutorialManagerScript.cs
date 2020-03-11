using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManagerScript : MonoBehaviour
{
    public TutorialPlayerControllerScript playerScript;

    public TextMeshProUGUI textArea;

    bool playerFirstClick = false;
    bool playerExhausted = false;

    int tutorialStep = 0;

    IEnumerator WaitToMoveToNextStep(float waitTime, int toWhichTutorialStep)
    {
        float timePassed = 0.0f;

        while(timePassed < waitTime)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }

        tutorialStep = toWhichTutorialStep;
    }

    private void Start()
    {
        textArea.text = "Welcome to the Tutorial\nPress <color=#d95763>Left Mouse Button</color> to begin";

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
                tutorialStep++;
            }
        }else if(tutorialStep == 1) {
            textArea.text = "Fantastic!\nYou have <color=#df7126>" + (playerScript.totalJumpsAllowed - playerScript.GetNumberJumpsUsed()) + "</color> jumps left";
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
                textArea.text = "Hmm... You seem a bit tired\nTry hitting the<color=#d95763> Right Mouse Button </color> while on a wall";
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
            textArea.text = "Now let's practice <color=#df7126>sticking</color> to walls";
        }
        


    }
}
