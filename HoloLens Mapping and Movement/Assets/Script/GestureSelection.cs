using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GestureSelection : MonoBehaviour, IInputClickHandler {

    public InputManager input;
    public Camera camera;

    public float speed = 10;

    // Use this for initialization
    void Start () {
        input.AddGlobalListener(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
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
            Vector3 hitPoint = hit.point;

            // Do something with the object that was hit by the raycast.
            Debug.Log("World Location: " + hit.point.ToString("F4"));
        }
    }
}
