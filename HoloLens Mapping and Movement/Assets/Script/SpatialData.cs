using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialData : MonoBehaviour {
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hitInfo;
        if (Physics.Raycast(
                Camera.main.transform.position,
                Camera.main.transform.forward,
                out hitInfo))
        {
            // If the Raycast has succeeded and hit a hologram
            // hitInfo's point represents the position being gazed at
            // hitInfo's collider GameObject represents the hologram being gazed at
            //System.Diagnostics.Debug.WriteLine(hitInfo.collider.gameObject.ToString());
        }
    }
}
