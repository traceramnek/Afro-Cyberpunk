using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{

    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        // Physics2D.queriesStartInColliders = false; // ray does not detect it's own objects collider
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 target = new Vector3(transform.position.x + 90, transform.position.y, 0);
        // Debug.DrawLine(transform.position, target, Color.red);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance);
        Debug.DrawLine(transform.position, hitInfo.point, Color.red);
        // if (hitInfo.collider != null && hitInfo.collider.CompareTag("Player"))
        // {
        //     Debug.DrawRay(transform.position, hitInfo.point, Color.red);
        // }
        // else
        // {
        //     Debug.DrawRay(transform.position, transform.position + transform.right * distance, Color.green);
        // }
    }

    void RaycastLogic()
    {

    }
}
