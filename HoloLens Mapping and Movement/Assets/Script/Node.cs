using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public GameObject gameObject { get; private set; }
    public Vector3 target { get; private set; }

    public Node(GameObject gameObject, Vector3 target)
    {
        this.gameObject = gameObject;
        this.target = target;
    }
}
