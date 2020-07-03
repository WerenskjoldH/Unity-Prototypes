using UnityEngine;

public class LevelSpawnPointScript : MonoBehaviour
{
    PlayerControllerScript playerScript;

    void Awake()
    {
        // Gets the player script
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerScript>();
    }

    // Sets the player's position and camera rotation to the spawn point's tranform
    public void MovePlayerToSpawn()
    {
        playerScript.SetPosition(transform.position);
        playerScript.SetRotation(transform.rotation);
        playerScript.ResetPlayer();
    }
}
