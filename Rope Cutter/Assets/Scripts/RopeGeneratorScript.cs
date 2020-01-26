using UnityEngine;

public class RopeGeneratorScript : MonoBehaviour
{
    public Rigidbody2D hook;
    public GameObject ropeLinkPrefab;
    public LineRenderer lineRenderer;
    public int numberOfLinks = 7;

    private GameObject firstLink;
    private Vector2 selectedLocation = Vector2.zero;
    private float nextBoxOffset = 0;

    // 0 = pull, 1 = cut
    private int pullOrCut = 0;

    void Start()
    {
        lineRenderer.positionCount = 2;
        GenerateRope();
        AddLink();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // For Debugging - Remove or Comment out
        if (Input.GetKeyDown(KeyCode.Slash))
            AddLink();

        if (Input.GetMouseButtonDown(0))
        {
            // Pull start code
            if (pullOrCut == 0 && selectedLocation == Vector2.zero)
            {
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                if (hit.rigidbody != null)
                {
                    selectedLocation = hit.point;
                }
            }

            // Cut start code
            if (pullOrCut == 1)
            {
                selectedLocation = new Vector2(mousePos.x, mousePos.y);
            }
        }

        // Update the rope pull code
        if(pullOrCut == 0 && selectedLocation != Vector2.zero)
        {
            if (mousePos.y < selectedLocation.y - (nextBoxOffset + 0.5))
            {
                AddLink();
                nextBoxOffset++;
            }
        }

        if (pullOrCut == 1 && selectedLocation != Vector2.zero)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, selectedLocation);
            lineRenderer.SetPosition(1, mousePos);
        }
        else
            lineRenderer.enabled = false;

        bool shouldSwapMode = false;
        if (Input.GetMouseButtonUp(0)) {

            // If we were supposed to pull and at least one box was pulled
            if ((pullOrCut == 0) && (nextBoxOffset > 0))
            {
                shouldSwapMode = true;
                nextBoxOffset = 0;
            }

            // If we were supposed to cut and the rope was cut
            if((pullOrCut == 1))
            {
                // Cut the rope along the line drawn
                RaycastHit2D hit = Physics2D.Linecast(selectedLocation, mousePos);
                if(hit.transform != null)
                {
                    Destroy(hit.transform.gameObject.GetComponent<HingeJoint2D>());
                    shouldSwapMode = true;
                }
            }

            if(shouldSwapMode)
            {
                pullOrCut = 1 - pullOrCut;
                shouldSwapMode = false;
            }

            selectedLocation = Vector2.zero;
        }
    }
    void GenerateRope()
    {
        Rigidbody2D previousPoint = hook.GetComponent<Rigidbody2D>();
        for(int i = 0; i < numberOfLinks; i++)
        {
            GameObject tempLink = Instantiate(ropeLinkPrefab, transform);
            if (i == 0)
                firstLink = tempLink;
            HingeJoint2D tempJoint = tempLink.GetComponent<HingeJoint2D>();
            tempJoint.connectedBody = previousPoint;
            previousPoint = tempLink.GetComponent<Rigidbody2D>();
        }
    }

    void AddLink()
    {
        GameObject tempLink = Instantiate(ropeLinkPrefab, transform);
        tempLink.GetComponent<HingeJoint2D>().connectedBody = hook;
        firstLink.GetComponent<HingeJoint2D>().connectedBody = tempLink.GetComponent<Rigidbody2D>();
        firstLink = tempLink;
    }
}
