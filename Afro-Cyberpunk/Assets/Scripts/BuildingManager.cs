using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private BoxCollider2D buildingBoxCollider;
    public bool insideBuilding = false;
    public bool buildingTriggerActive = true;
    private PlayerPlatformer player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerPlatformer>();
        buildingBoxCollider = GetComponent<BoxCollider2D>();
        if (player == null)
        {
            insideBuilding = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Constants.PlayerInput.pressedBuildingToggle && !insideBuilding)
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
