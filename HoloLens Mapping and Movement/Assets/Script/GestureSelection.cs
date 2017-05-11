using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GestureSelection : MonoBehaviour, IInputClickHandler {

    public InputManager input;
    public Camera camera;
    private GameObject player;

    public float speed = 10;

    private Transform target;
    private Vector3 hitPoint;
    private float step;

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
            player.transform.position = Vector3.MoveTowards(player.transform.position, hitPoint, step);
        }
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("Clicked!");
        CastRayToWorld();
    }

    public void CastRayToWorld()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawLine(ray.origin, Camera.main.transform.forward * 50000000, Color.red);

        if (Physics.Raycast(ray, out hit))
        {
            hitPoint = hit.point;
            step = speed * Time.deltaTime;

            // Do something with the object that was hit by the raycast.
            Debug.Log("World Location: " + hit.point.ToString("F4"));
        }
    }
}
