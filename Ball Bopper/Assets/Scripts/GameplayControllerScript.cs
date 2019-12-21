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

    [SerializeField]
    private Text counterTextObject = null;

    /// Public Variables
    public float maxPushForce = 600.0f;

    /// Private Variables
    private float falloffDistance = 5.0f;
     
    float pointCounter = 0;

    // Adds the amount to the pointCounter
    public void modifyPoints(int amount)
    {
        pointCounter += amount;
        counterTextObject.text = pointCounter.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
    }

}
