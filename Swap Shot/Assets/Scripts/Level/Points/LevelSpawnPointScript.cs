using UnityEngine;

public class LevelSpawnPointScript : MonoBehaviour
{
    PlayerControllerScript playerScript;

    void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
    }

    void Start()
    {
        // DetectMultipleSpawnPoints();
    }

    public void MovePlayerToSpawn()
    {
        playerScript.SetPosition(transform.position);
        playerScript.SetRotation(transform.rotation);
        playerScript.SetPlayerAlive(true);
    }

    // Depricated until I decide if this is a consistent fact throughout the game
    //Checks if the designer accidentally placed two spawn points
    //void DetectMultipleSpawnPoints()
    //{
    //    if (multiplesFound)
    //        return;

    //    int numberOfSpawnPoints = GameObject.FindGameObjectsWithTag("StartPoint").Length;

    //    if (numberOfSpawnPoints > 1)
    //    {
    //        Debug.LogError(numberOfSpawnPoints + " Start Points Found, Fix This Immediately");
    //        multiplesFound = true;
    //    }
    //}
}
