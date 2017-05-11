using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GestureSelection : MonoBehaviour, IInputClickHandler {

    public InputManager input;
    public Camera camera;
    private GameObject player;

    public float speed = 0.1f;

    private Transform target;
    private Vector3 hitPoint;
    private Vector3 hit;

    // Use this for initialization
    void Start () {
        input.AddGlobalListener(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        
        if (player == null)
        {
            Debug.Log("Looking for player object!");

            player = GameObject.FindGameObjectWithTag("Player");

            if(player != null)
            {
                Debug.Log("Found player object!");
            }
        }
        else if (player.transform.position.x != hitPoint.x)
        {
            Debug.Log("Moving player object from: " + player.transform.position.ToString() + " to : " + hitPoint.ToString());
            player.transform.position = Vector3.Lerp(player.transform.position, hitPoint, speed * 0.1f * Time.deltaTime);
        } 
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("Clicked!");
        CastRayToWorld();
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
        }
    }
}
