using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private Collider2D buildingBoxCollider;
    // make it static so that there is only 1 boolean for all buildings
    public static bool insideBuilding = false;
    public bool buildingTriggerActive = true;
    private PlayerPlatformer player;

    void OnEnable()
    {
        LevelManager.buildingToggler += toggleBuildingCollision;
    }
    void OnDisable()
    {
        LevelManager.buildingToggler -= toggleBuildingCollision;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerPlatformer>();
        if (GetComponent<BoxCollider2D>() != null)
        {
            buildingBoxCollider = GetComponent<BoxCollider2D>();
        }
        else if (GetComponent<PolygonCollider2D>() != null)
        {
            buildingBoxCollider = GetComponent<PolygonCollider2D>();
        }
        buildingBoxCollider.isTrigger = true;
        if (player == null)
        {
            insideBuilding = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    // Player pressed toggle button which causes this method to run
    void toggleBuildingCollision()
    {
        Debug.Log("Inside toggle collision");
        // player has already pressed toggle button so no need to check
        if (!insideBuilding & player.IsGrounded())
        {
            buildingTriggerActive = !buildingTriggerActive;
        }
        if (!buildingTriggerActive && !insideBuilding)
        {
            buildingBoxCollider.isTrigger = false;
        }
        else
        {
            buildingBoxCollider.isTrigger = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        insideBuilding = true;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        insideBuilding = false;
    }
}
