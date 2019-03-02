using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {
    // put the game object to be selected if somehow no objects get selected
    public GameObject defaultSelectedGameObject;

	// Update is called once per frame
	void Update () {
        //EventSystem.current grabs the current EventSystem it sees in the world
        if (!EventSystem.current.currentSelectedGameObject)
        {
            EventSystem.current.SetSelectedGameObject(defaultSelectedGameObject);
        }
    }
}
