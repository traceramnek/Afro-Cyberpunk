using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{

    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false; // so ray does not detect it's own objects collider
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance);
    
        if (hitInfo.collider != null && hitInfo.collider.CompareTag("Player"))
        {
            Debug.Log(hitInfo.collider);
            Debug.DrawRay(transform.position, hitInfo.point, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.right * distance, Color.green);
        }
    }

    void RaycastLogic()
    {

    }
}
