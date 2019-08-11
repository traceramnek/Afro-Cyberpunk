using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class CutsceneTriggerer : MonoBehaviour
{
    public GameObject thePlayer;
    public GameObject theCutsceneToPlay;
    public bool repeatable;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            theCutsceneToPlay.SetActive(true);
            if (!repeatable)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}