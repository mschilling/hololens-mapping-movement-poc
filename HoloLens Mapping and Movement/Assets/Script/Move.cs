using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    List<Vector3> pathNodes;

    public float speed = 0.5f;

    // Use this for initialization
    void Start () {
        pathNodes = new List<Vector3>();
	}

    void Update()
    {
        if (pathNodes.Count > 0)
        {
            if ((transform.position-pathNodes[0]).sqrMagnitude<0.1*0.1)
            {
                Debug.Log("Arrived at a point, so imma better delete it");
                pathNodes.RemoveAt(0);
                return;
            }

            Vector3 target = pathNodes[0];
            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        }
    }

    public void MoveToLocation(Vector3 target)
    {
        Debug.Log("Moving player object from: " + transform.position.ToString("F4") + " to : " + target.ToString("F4"));
        CalculatePathToTarget(target);
    }

    private List<Vector3> CalculatePathToTarget(Vector3 target)
    {
        pathNodes.Clear();

        Vector3 currPos = gameObject.transform.position;

        GameObject firstHit = CastLineToLocation(currPos, target);
        if(firstHit != null)
        {
            pathNodes.Add(target);
        } else {
            pathNodes.Add(target);
        }

        return pathNodes;
    }

    private GameObject CastLineToLocation(Vector3 start, Vector3 end)
    {
        RaycastHit hitInfo;
        if (Physics.Linecast(
                start,
                end,
                out hitInfo,
                Physics.DefaultRaycastLayers))
        {
            return hitInfo.transform.gameObject;
        }

        return null;
    }
}
