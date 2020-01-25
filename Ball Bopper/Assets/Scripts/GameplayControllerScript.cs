using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayControllerScript : MonoBehaviour
{
    #region Helper Functions
    public Vector3 RetrieveMousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        return mousePosition;
    }
    #endregion

    /// Pre-assigned Variables
    public GameObject ballGameObject = null;
    public GameObject pointBallGameObject = null;

    [SerializeField]
    private Text counterTextObject = null;

    /// Public Variables
    public float maxPushForce = 600.0f;

    /// Private Variables
    private float falloffDistance = 5.0f;
     
    float pointCounter = 0;

    float timeToSpawn = 0;
    float maxSpawnTime = 4;
    float timeCounter = 0;

    // Spawning range is  [-spawnWidth, spawnWidth]
    float spawnWidth = 2.7f;
    // Specify lower and upper bounds
    float spawnHeightTop = 2.5f;
    float spawnHeightBottom = -0.5f;

    // Adds the amount to the pointCounter
    public void modifyPoints(int amount)
    {
        pointCounter += amount;
        counterTextObject.text = pointCounter.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Wait four seconds to spawn first ball
        timeToSpawn = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenuScript.isPaused) {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 touchPosition = RetrieveMousePosition();
                // Debug.Log("Touch occured at: <" + touchPosition.x + ", " + touchPosition.y + ">");

                Vector2 forceDirection = -1 * (touchPosition - ballGameObject.GetComponent<Transform>().position);
                float distance = forceDirection.magnitude;

                forceDirection.Normalize();

                float forceStrengthMultiplier = Mathf.Exp(-(distance / falloffDistance));

                ballGameObject.GetComponent<Rigidbody2D>().AddForce(forceDirection * forceStrengthMultiplier * maxPushForce);
            }

            if (timeCounter >= timeToSpawn)
            {
                Instantiate(pointBallGameObject, new Vector3(Random.Range(-spawnWidth, spawnWidth), Random.Range(-spawnHeightBottom, spawnHeightTop), 0), Quaternion.identity);
                timeToSpawn = Random.Range(0.3f, maxSpawnTime);
                timeCounter = 0;
            }
            else
                timeCounter += Time.deltaTime;
        }
    }

}
