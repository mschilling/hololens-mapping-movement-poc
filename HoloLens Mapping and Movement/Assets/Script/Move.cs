using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    // List of nodes which the object has to move to
    List<Node> pathNodes;

    // Object movement speed
    public float speed = 0.5f;
    // Object max jump height
    public float maxJumpSpeed = 4f;
    private float jumpSpeed = 0;
    // Gravity to be applied to object
    public float minGravity = 0.5f;
    private float gravity = 0;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    // Use this for initialization
    void Start () {
        pathNodes = new List<Node>();
        controller = GetComponent<CharacterController>();
        gravity = minGravity;
	}

    void Update()
    {
        if (pathNodes.Count > 0)
        {
            if ((transform.position - GetCurrentTarget().target).sqrMagnitude < 0.1 * 0.1)
            {
                Debug.Log("Arrived at a point, so imma better delete it");
                TextManager.Instance.LetCatSpeak("Ik ben gearriveerd.");
                pathNodes.RemoveAt(0);
                return;
            }
            else
            {
                MoveMethod();
            }
        }
        else if(!controller.isGrounded)
        {
            // Reset the MoveVector
            moveDirection = transform.position;
            ApplyGravity();

            //Apply our move Vector , remember to multiply by Time.delta
            controller.Move(moveDirection * Time.deltaTime);
        } 
        
        // When on the ground reset gravity
        if(controller.isGrounded)
        {
            gravity = minGravity;
        }
    }

    public void MoveToLocation(GameObject hitObject, Vector3 target)
    {
        Debug.Log("Moving player object from: " + transform.position.ToString("F4") + " to : " + target.ToString("F4"));
        TextManager.Instance.LetCatSpeak("Oke ik ga er heen.");

        Node node = new Node(hitObject, target);
        CalculatePathToTarget(node);
    }

    /// <summary>
    /// Check which kind of Movement has to be done next
    /// Continue to walk or does it have to jump?
    /// </summary>
    private void MoveMethod()
    {
        // Make sure the object is facing the target at all times
        Vector3 targetLook = GetCurrentTarget().target;
        targetLook.y = transform.position.y;
        transform.LookAt(targetLook);

        // Normalize it and account for movement speed.
        moveDirection = GetCurrentTarget().target - transform.position;
        moveDirection = moveDirection.normalized * speed;

        // Make sure it is not magically able to fly
        moveDirection.y = transform.position.y;

        Debug.Log("Is grounded: " + controller.isGrounded);
        Debug.Log("Has to jump: " + HasToJump());
        // Make sure the object is not in mid air
        if (controller.isGrounded && HasToJump())
        {
            // Not yet jumping but object has to jump to reach plane
            Debug.Log("Jumping time!");
            jumpSpeed = maxJumpSpeed;
            Jump();         
        }
        else if(jumpSpeed > 0)
        {
            Debug.Log("Still Jumping time...");
            jumpSpeed -= 5 * Time.deltaTime;
            Jump();
        }

        // Keep on walking...
        ApplyGravity();
        
        Debug.Log("Current Y value: " + transform.position.y + " -> Target Y value: " + moveDirection.y);
        Debug.Log("Substract value: " + gravity * Time.deltaTime);

        controller.Move(moveDirection * Time.deltaTime);
    }

    private void Jump()
    {
        transform.Translate(Vector3.up * jumpSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        gravity += minGravity;
        moveDirection.y -= gravity * Time.deltaTime;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private List<Node> CalculatePathToTarget(Node node)
    {
        pathNodes.Clear();

        Vector3 currPos = gameObject.transform.position;

        GameObject firstHit = CastLineToLocation(currPos, node.target);
        if(firstHit != null)
        {
            pathNodes.Add(node);
        } else {
            pathNodes.Add(node);
        }

        return pathNodes;
    }

    /// <summary>
    /// Cast a linecast 
    /// </summary>
    /// <param name="start">start of line</param>
    /// <param name="end">end of line</param>
    /// <returns>Hit GameObject</returns>
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

    /// <summary>
    /// Get the current node to which the object has to move
    /// </summary>
    /// <returns>Node if there is atarget, null if not</returns>
    private Node GetCurrentTarget()
    {
        return pathNodes.Count > 0 ? pathNodes[0] : null;
    }

    /// <summary>
    /// Helper
    /// Check if it is the right time to make a jump
    /// </summary>
    /// <returns>true is object should jump right now</returns>
    private bool HasToJump()
    {
        Node currTarget = GetCurrentTarget();
        Collider collider = currTarget.gameObject.GetComponent<Collider>();

        if(GetCurrentTarget().target.y > transform.position.y)
        {
            foreach(RaycastHit ray in Physics.SphereCastAll(transform.position, maxJumpSpeed * 0.1f, Vector3.forward, maxJumpSpeed * 0.1f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.UseGlobal))
            {
                if(ray.collider.gameObject.Equals(GetCurrentTarget().gameObject))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
