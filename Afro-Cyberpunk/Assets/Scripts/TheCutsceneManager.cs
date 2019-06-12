using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TheCutsceneManager : MonoBehaviour
{
    public GameObject thePlayer;
    private Vector3 playerEndingPosition;
    private bool fix;
    public Animator playerAnimator;
    private RuntimeAnimatorController playerAnim;
    public PlayableDirector director;

    // Start is called before the first frame update
    void OnEnable()
    {
        fix = false;
        //this cutscene requires that the player is facing the right
        thePlayer.transform.localScale = new Vector3(Mathf.Abs(thePlayer.transform.localScale.x), thePlayer.transform.localScale.y, thePlayer.transform.localScale.z);
        thePlayer.GetComponent<PlayerPlatformer>().enabled = false;
        playerAnim = playerAnimator.runtimeAnimatorController;
        playerAnimator.runtimeAnimatorController = null;
    }

    // Update is called once per frame
    void Update()
    {
        if ((director.state != PlayState.Playing) && (!fix))
        {
            fix = true;
            playerEndingPosition = thePlayer.transform.position;
            playerAnimator.runtimeAnimatorController = playerAnim;
            thePlayer.transform.position = playerEndingPosition;
            playerAnimator.SetBool("grounded", true);
            thePlayer.GetComponent<PlayerPlatformer>().enabled = true;
            this.gameObject.SetActive(false);
        }
    }
}