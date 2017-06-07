using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System;

public class GestureSelection : MonoBehaviour, IInputClickHandler {

    public PlaySpaceManager playSpaceManager;
    public InputManager input;
    public Camera camera;
    private GameObject player;
    private Move playerMove; 

    private Transform target;
    private Vector3 hitPoint;
    private Vector3 hit;

    // Use this for initialization
    void Start () {
        input.AddGlobalListener(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        // As long as the user has not finished scanning do nothing in Update
        if (!playSpaceManager.finishedScanning())
            return;
        
        // If player variable is null add player object to variable
        if (player == null)
        {
            Debug.Log("Looking for player object!");

            player = GameObject.FindGameObjectWithTag("Player");

            // When player is found also add the move script to a variable for easy access
            if(player != null)
            {
                Debug.Log("Found player object!");

                CallPlayer();
                playerMove = player.GetComponent<Move>();
            }
        }
    }

    /// <summary>
    /// Global input listener
    /// Listen for every click performed by user
    /// </summary>
    /// <param name="eventData">Input event data</param>
    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("Clicked!");

        // Room has not been finished
        if (!playSpaceManager.finishedScanning())
        {
            playSpaceManager.finishScanning();
        }
        // Room has been finished, check where user has clicked
        else
        {
            CastRayToWorld();
        }
    }

    /// <summary>
    /// Call player object reset
    /// </summary>
    public void ResetPlayer()
    {
        Debug.Log("Reset");

        // Only perform the reset if reset can be called
        if (playerMove != null)
        {
            CallPlayer();
            playerMove.Reset();
        }
    }

    /// <summary>
    /// Cast a raycast to get the location where the user was gazing at when he performed a click
    /// </summary>
    public void CastRayToWorld()
    {
        // If raycast hits an object it returns true
        RaycastHit hitInfo;
        if (Physics.Raycast(
                Camera.main.transform.position,
                Camera.main.transform.forward,
                out hitInfo,
                20.0f,
                Physics.DefaultRaycastLayers))
        {
            hitPoint = hitInfo.point;

            // Do something with the object that was hit by the raycast.
            Debug.Log("World Location: " + hitInfo.point.ToString("F4"));

            // Pass the location of the gaze location when clicked to the player object
            playerMove.MoveToLocation(hitInfo.collider.gameObject, hitPoint);
        }
    }

    /// <summary>
    /// Show user feedback when calling player object
    /// </summary>
    private void CallPlayer()
    {
        TextManager.Instance.LetCatSpeak("Ik kom eraan!");
        TextManager.Instance.LetCatSpeak("Als ik vast zit zeg \"Reset\" om mij te resetten.");
        TextManager.Instance.LetCatSpeak("Klik met je vingers om mij te bewegen.");
    }
}
