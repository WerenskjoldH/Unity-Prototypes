using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalModeManagerScript : MonoBehaviour
{
    public PlayerControllerScript playerControllerScript;

    // Contains SurvivalModeWallScripts
    public ArrayList environmentObjects = new ArrayList();
    public Queue<SurvivalModeWallScript> activeWalls = new Queue<SurvivalModeWallScript>();

    SurvivalModeWallScript lastAdded;

    float timeBetweenSelections = 3;
    float timeSum;

    bool lastFinishedAdding = true;

    void Update()
    {
        if(timeSum > timeBetweenSelections && lastFinishedAdding)
        {
            timeSum = 0;
            if (activeWalls.Count > 2) {
                Debug.Log("Deactivating");
                Debug.Log("Count: " + activeWalls.Count);
                activeWalls.Dequeue().Deactivate();
            }

            int selectedObject = Random.Range(0, environmentObjects.Count);
            while(((SurvivalModeWallScript)environmentObjects[selectedObject]).IsActivated())
            {
                selectedObject = Random.Range(0, environmentObjects.Count);
            }

            ((SurvivalModeWallScript)environmentObjects[selectedObject]).Activate(timeBetweenSelections);
            activeWalls.Enqueue((SurvivalModeWallScript)environmentObjects[selectedObject]);
            lastAdded = (SurvivalModeWallScript)environmentObjects[selectedObject];
            lastFinishedAdding = false;
        }
        else
        {
            if (lastAdded == null || lastAdded.dangerous)
                lastFinishedAdding = true;
        }

        if(lastFinishedAdding)
            timeSum += Time.deltaTime;
    }
}
