using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;

    [Range(1, 10)]
    public float speed;

    void Update()
    {
        gameObject.GetComponent<Transform>().LookAt(player.transform);
    }
}
