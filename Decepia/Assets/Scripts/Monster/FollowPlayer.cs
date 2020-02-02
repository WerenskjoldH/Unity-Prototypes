using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;

    [Range(0, 1)]
    public float speed;

    void Update()
    {
        gameObject.GetComponent<Transform>().LookAt(player.transform);

        float monsterForwardAngle = Mathf.Atan2(transform.forward.z, transform.forward.x);
        float playerForwardAngle = Mathf.Atan2(player.transform.forward.z, player.transform.forward.x);

        float angleDiff = Mathf.Abs(playerForwardAngle - monsterForwardAngle);

        // Monster is behind the player
        if (angleDiff < 0.7071)
        {
            transform.position = transform.position + 0.25f * transform.forward * speed;
        }
        else if (angleDiff < 1.5708)
        {
            // Moving towards the player should be slower
            Vector3 monsterPosition = transform.position + 0.1f * transform.forward * speed;
            // Try to move around behind the player
            transform.position = monsterPosition + -.15f * transform.right * speed;
        }
    }
}
