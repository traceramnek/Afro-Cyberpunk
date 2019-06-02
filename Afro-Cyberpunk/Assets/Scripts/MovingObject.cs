using UnityEngine;
using System.Collections;

public class MovingObject : MonoBehaviour
{

    public GameObject objectToMove; // object to be moved

    //points that object will move between back and forth
    public Transform startPoint; // starting point
    public Transform endPoint; // ending point

    public float moveSpeed; // for fast the object moves

    private Vector3 currentTarget; // the current point it's going to, start or end?

    // how long until the object moves in the opposite direction once it hits an endpoint
    public float waitTime = 0;


    // Use this for initialization
    void Start()
    {
        currentTarget = endPoint.position; // start currTarget as endpoint

    }

    // Update is called once per frame
    void Update()
    {
        objectToMove.transform.position = Vector2.MoveTowards(objectToMove.transform.position, currentTarget, moveSpeed * Time.deltaTime);
        if (objectToMove.transform.position == endPoint.position)
        {
            StartCoroutine(waitToMove(waitTime, true));
        }

        if (objectToMove.transform.position == startPoint.position)
        {
            StartCoroutine(waitToMove(waitTime, false));
        }
    }

    IEnumerator waitToMove(float seconds, bool goToStart)
    {
        yield return new WaitForSeconds(seconds);
        if (goToStart) {
            currentTarget = startPoint.position;
        }
        else {
            currentTarget = endPoint.position;
        }

    }
}
