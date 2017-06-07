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
        if (!playSpaceManager.finishedScanning())
            return;

        if (player == null)
        {
            Debug.Log("Looking for player object!");

            player = GameObject.FindGameObjectWithTag("Player");

            if(player != null)
            {
                Debug.Log("Found player object!");

                CallPlayer();
                playerMove = player.GetComponent<Move>();
            }
        }
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("Clicked!");

        if (!playSpaceManager.finishedScanning())
        {
            playSpaceManager.finishScanning();
        }
        else
        {
            CastRayToWorld();
        }
    }

    public void ResetPlayer()
    {
        Debug.Log("Reset");

        if (playSpaceManager.finishedScanning())
        {
            CallPlayer();
            playerMove.Reset();
        }
    }

    public void CastRayToWorld()
    {
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

            playerMove.MoveToLocation(hitInfo.collider.gameObject, hitPoint);
        }
    }

    private void CallPlayer()
    {
        TextManager.Instance.LetCatSpeak("Ik kom eraan!");
        TextManager.Instance.LetCatSpeak("Als ik vast zit zeg \"Reset\" om mij te resetten.");
        TextManager.Instance.LetCatSpeak("Klik met je vingers om mij te bewegen.");
    }
}
