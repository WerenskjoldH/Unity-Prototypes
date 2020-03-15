using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField]
    GameObject towerBase;
    [SerializeField]
    GameObject towerEye;
    [SerializeField]
    GameObject mouseGameObject;

    Vector2 eyeDefaultPosition;

    private void Start()
    {
        eyeDefaultPosition = towerEye.transform.position;
    }

    void EyeMovement()
    {

    }

    private void Update()
    {
        EyeMovement();
    }
}
