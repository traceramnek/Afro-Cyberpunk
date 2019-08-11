using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationTriggerer : MonoBehaviour
{
    private LevelManager theLevelManager;
    public GameObject thePlayer;
    public GameObject theConversationToPlay;
    public GameObject theNPC;
    public bool repeatable;
    private bool alreadyPressedUp;
    void Start()
    {
        alreadyPressedUp = false;
        theLevelManager = FindObjectOfType<LevelManager>();

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            theLevelManager.MaintainPressUpCanvas();
            theLevelManager.EnablePressUpCanvas();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (!alreadyPressedUp)
            {
                theLevelManager.MaintainPressUpCanvas();
                if (Constants.PlayerInput.IsPressingUp)
                {
                    AlignCharactersForConversation();
                    theLevelManager.DisablePlayerControls();
                    theLevelManager.DisablePressUpCanvas();
                    theConversationToPlay.SetActive(true);
                    alreadyPressedUp = true;
                }
            }
            else if(theConversationToPlay.GetComponent<Dialog>().IsConversationFinished())
            {
                theLevelManager.EnablePlayerControls();
                alreadyPressedUp = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            theConversationToPlay.SetActive(false);
            if (!repeatable && theConversationToPlay.GetComponent<Dialog>().IsConversationFinished())
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                theLevelManager.DisablePressUpCanvas();
            }
        }
    }

    private void AlignCharactersForConversation()
    {
        float posDiff = theNPC.transform.position.x - thePlayer.transform.position.x;
        if(posDiff >= 0)
        {
            //NPC faces left and player faces right
            theNPC.transform.localScale = new Vector3((-theNPC.transform.localScale.x), theNPC.transform.localScale.y, theNPC.transform.localScale.z);
            thePlayer.transform.localScale = new Vector3(Mathf.Abs(thePlayer.transform.localScale.x), thePlayer.transform.localScale.y, thePlayer.transform.localScale.z);
        }
        else if (posDiff < 0)
        {
            //NPC faces right and player faces left
            theNPC.transform.localScale = new Vector3(Mathf.Abs(theNPC.transform.localScale.x), theNPC.transform.localScale.y, theNPC.transform.localScale.z);
            thePlayer.transform.localScale = new Vector3((-thePlayer.transform.localScale.x), thePlayer.transform.localScale.y, thePlayer.transform.localScale.z);
        }
    }
}
