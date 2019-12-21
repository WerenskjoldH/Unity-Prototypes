using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBallScript : MonoBehaviour
{
    public int pointWorth = 1;

    private GameObject gameplayController = null;

    // Start is called before the first frame update
    void Start()
    {
        gameplayController = GameObject.FindGameObjectWithTag("GameController");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

        if (string.Compare(col.gameObject.name, "BottomBoundary") == 0)
        {
            Destroy(this.gameObject);
            return;
        }

        gameplayController.GetComponent<GameplayControllerScript>().modifyPoints(pointWorth);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
