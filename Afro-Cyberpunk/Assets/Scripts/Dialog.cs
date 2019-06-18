using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    public GameObject continueButton;
    public Animator textDisplayAnim;
    private bool conversationFinished;
    //when we find an audio source for the continue button place it on the DemoConversation gameobject
    //private AudioSource source;

    void OnEnable()
    {
        //source = GetComponent<AudioSource>();
        StartCoroutine(Type());
        conversationFinished = false;
    }

    void Update()
    {
        if(textDisplay.text == sentences[index])
        {
            continueButton.SetActive(true);
            if (Constants.PlayerInput.IsPressingEnter)
            {
                NextSentence();
            }
        }
    }
    IEnumerator Type(){
        foreach(char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);

        }
    }

    public void NextSentence()
    {
        //source.Play();
        textDisplayAnim.SetTrigger("Change");
        continueButton.SetActive(false);
        if(index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
            conversationFinished = false;
        }
        else
        {
            textDisplay.text = "";
            continueButton.SetActive(false);
            RestartConversation();
            conversationFinished = true;

        }
    }

    public void RestartConversation()
    {
        index = 0;
    }
    
    public bool IsConversationFinished()
    {
        return conversationFinished;
    }
}
