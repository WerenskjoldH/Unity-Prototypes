using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCounterScript : MonoBehaviour
{
    public float placementWidth = 1.0f;
    public float placementHeight = 0.5f;
    public float lerpSpeedMultiplier = 20.0f;
    public GameObject counterObjectPrefab;
    public GameObject playerObject;

    List<GameObject> jumpCounterObjects = new List<GameObject>();
    int numberOfJumps;
    int jumpsUsed = 0;

    public void ResetJumps()
    {
        for(int i = jumpCounterObjects.Count-1; i > jumpCounterObjects.Count - 1 - jumpsUsed; i--)
        {
            Color c = jumpCounterObjects[i].GetComponent<SpriteRenderer>().color;
            c.a = 1f;
            jumpCounterObjects[i].GetComponent<SpriteRenderer>().color = c;
        }
        jumpsUsed = 0;
    }

    public void ReduceJumps()
    {
        Color c = jumpCounterObjects[jumpCounterObjects.Count - 1 - jumpsUsed].GetComponent<SpriteRenderer>().color;
        c.a = 0.5f;
        jumpCounterObjects[jumpCounterObjects.Count - 1 - jumpsUsed].GetComponent<SpriteRenderer>().color = c;
        jumpsUsed++;
    }

    public void SetNumberOfJumps(int n)
    {
        numberOfJumps = n;

        foreach (GameObject o in jumpCounterObjects)
            Destroy(o);

        // Getting pretty Python-y with this formatting
        for (int i = 1; i < numberOfJumps+1; i++)
            jumpCounterObjects.Add(
                Instantiate(
                    counterObjectPrefab, 
                    transform.position + new Vector3((i * placementWidth / (numberOfJumps+1)) - placementWidth / 2.0f, placementHeight), 
                    Quaternion.identity, 
                    transform)
                );
        
    }

    private void Start()
    {
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerObject.transform.position, lerpSpeedMultiplier * Time.deltaTime);
    }
}
