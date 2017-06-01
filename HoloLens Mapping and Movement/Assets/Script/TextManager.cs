using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;

public class TextManager : Singleton<TextManager>
{

    [Tooltip("The UIText in which the cat is giving feedback.")]
    public UnityEngine.UI.Text catText;

    private Queue<string> userFeedBackQueue;
    private bool isCoroutineExecuting = false;
    private float delayerTimer = 1.5f;
    private float delayReadTimer = 2f;
    private string catIsTyping = "Typing...";

    // Use this for initialization
    void Start () {
        userFeedBackQueue = new Queue<string>();
        LetCatSpeak("Hallo! Om te beginnen, scan de ruimte. Klik met je vingers om verder te gaan.");
	}
    
    public void LetCatSpeak(string userFeedback)
    {
        userFeedBackQueue.Enqueue(userFeedback);
        CallUserFeedback();
    }

    private void CallUserFeedback()
    {
        Debug.Log("Feedback");
        if (!isCoroutineExecuting && userFeedBackQueue.Count > 0)
        {
            catText.text = catIsTyping;
            Debug.Log("Start Coroutine");
            StartCoroutine(ShowUserFeedback());
        }
    }

    private IEnumerator ShowUserFeedback()
    {
        isCoroutineExecuting = true;

        Debug.Log("Coroutine");

        yield return new WaitForSeconds(delayerTimer);

        // Code to execute after the delay
        catText.text = userFeedBackQueue.Dequeue();
        
        yield return new WaitForSeconds(delayReadTimer);

        isCoroutineExecuting = false;
        CallUserFeedback();
    }
}
