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
    }

    public void MovePlayerToSpawn()
    {
        playerScript.SetPosition(transform.position);
        playerScript.SetRotation(transform.rotation);
        playerScript.ResetPlayer();
    }
}
