﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    // List of nodes which the object has to move to
    List<Node> pathNodes;

    // Reference to object animation
    private Animator animator;

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

    private bool hasToReset;
    // Cannot be set in start method as position on creation will be (0,0,0)
    private Vector3 startPosition;

    // Variables to check if object is not stuck
    private const float noMovementThreshold = 0.0001f;
    private const int noMovementFrames = 3;
    private const int maxStuckFrames = 5;
    private int stuckFrames = 0;
    private const int maxJumpTries = 2;
    private Vector3 jumpStartPosition = Vector3.zero;
    private int jumpTries = 0;
    Vector3[] previousLocations = new Vector3[noMovementFrames];
    private bool isMoving;

    // Use this for initialization
    void Start () {
        pathNodes = new List<Node>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        gravity = minGravity;

        // Fill the previousLocations array
        for (int i = 0; i < previousLocations.Length; i++)
        {
            previousLocations[i] = Vector3.zero;
        }
    }

    // Is called every frame
    void Update()
    {
        // Check if object is defined to be reset
        if(hasToReset)
        {
            ResetObjectState();
        }
        // Else check if object has to move to a node
        else if (pathNodes.Count > 0)
        {
            // Update the previous locations array
            UpdatePreviousLocations();

            if ((transform.position - GetCurrentTarget().target).sqrMagnitude < 0.1 * 0.1)
            {
                Debug.Log("Arrived at a point, so imma better delete it");
                TextManager.Instance.LetCatSpeak("Ik ben gearriveerd.");
                pathNodes.RemoveAt(0);
                setPlayerIdleAnimation();
                return;
            }
            else
            {
                MoveMethod();
            }
        }
        // If not moving check if in air
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

            // If object is grounded, not going to jump and it's horizontal position is different from the jumpStartPosition reset jumpTries
            if(jumpSpeed <= 0 && !WithinMovementThreshold(transform.position, jumpStartPosition))
            {
                jumpTries = 0;

                // Stopped jumping but has not arrived yet, so continue walk animation
                if (GetCurrentTarget() != null)
                {
                    setPlayerWalkAnimation();
                }
            }
        }
    }

    /// <summary>
    /// Move object to location
    /// </summary>
    /// <param name="hitObject">Object from the target location</param>
    /// <param name="target">Target location</param>
    public void MoveToLocation(GameObject hitObject, Vector3 target)
    {
        // Check if startposition has to be set as a variable for resetting
        if (startPosition == Vector3.zero)
        {
            startPosition = transform.position;
            Debug.Log("Startposition: " + startPosition.ToString());
        }

        Debug.Log("Moving player object from: " + transform.position.ToString("F4") + " to : " + target.ToString("F4"));
        TextManager.Instance.LetCatSpeak("Oke ik ga er heen.");

        Node node = new Node(hitObject, target);
        CalculatePathToTarget(node);

        setPlayerWalkAnimation();
    }

    /// <summary>
    /// Check which kind of Movement has to be done next
    /// Continue to walk or does it have to jump?
    /// </summary>
    private void MoveMethod()
    {
        // Make sure the object can still reach the target point
        if(!CheckIfTargetIsReachable())
        {
            // If the point can't be reached, delete it from the list and notify the user
            pathNodes.RemoveAt(0);
            TextManager.Instance.LetCatSpeak("Ik kan er niet bij..");
            setPlayerIdleAnimation();
            return;
        }

        // Make sure the object is facing the target at all times
        Vector3 targetLook = GetCurrentTarget().target;
        targetLook.y = transform.position.y;
        transform.LookAt(targetLook);

        // Normalize it and account for movement speed.
        moveDirection = GetCurrentTarget().target - transform.position;
        moveDirection = moveDirection.normalized * speed;

        // Make sure it is not magically able to fly
        moveDirection.y = transform.position.y;
        if(!isMoving)
        {
            // If the object can't move try to move him up 1/10th of its own size
            moveDirection.y += (transform.lossyScale.y / 10) + gravity;
        }

        Debug.Log("Is grounded: " + controller.isGrounded);
        Debug.Log("Has to jump: " + HasToJump());
        // Make sure the object is not in mid air
        if (controller.isGrounded && HasToJump() && jumpSpeed <= 0)
        {
            // Not yet jumping but object has to jump to reach plane
            Debug.Log("Jumping time!");
            jumpSpeed = maxJumpSpeed;
            jumpTries += 1;
            jumpStartPosition = transform.position;
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
        
        //Debug.Log("Current Y value: " + transform.position.y + " -> Target Y value: " + moveDirection.y);
        //Debug.Log("Substract value: " + gravity * Time.deltaTime);

        controller.Move(moveDirection * Time.deltaTime);
    }

    /// <summary>
    /// Let object jump
    /// </summary>
    private void Jump()
    {
        setPlayerJumpAnimation();
        transform.Translate(Vector3.up * jumpSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Apply gravity to object
    /// </summary>
    private void ApplyGravity()
    {
        gravity += minGravity;
        moveDirection.y -= gravity * Time.deltaTime;
    }

    /// <summary>
    /// Calculate a path to the target location
    /// </summary>
    /// <param name="target">Target location</param>
    /// <returns>List of nodes to get to the target location</returns>
    private List<Node> CalculatePathToTarget(Node node)
    {
        pathNodes.Clear();
        stuckFrames = 0;
        jumpTries = 0;

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

        if(GetCurrentTarget().target.y > (transform.position.y + (transform.lossyScale.y / 10)))
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

    /// <summary>
    /// Check if object is still moving and on point to reach the target location
    /// </summary>
    /// <returns>true is object still might be able to reach target location</returns>
    private bool CheckIfTargetIsReachable()
    {
        if (!isMoving)
        {
            stuckFrames += 1;
        }
        else
        {
            stuckFrames = 0;
        }

        Debug.Log("Jump tries: " + jumpTries + " / Stuck frames: " + stuckFrames);
        Debug.Log("Can I reach a target? => Max jump tries? " + (jumpTries >= maxJumpTries) + " => Max stuck frames? " + (stuckFrames >= maxStuckFrames));

        return (jumpTries < maxJumpTries && stuckFrames < maxStuckFrames);
    }

    /// <summary>
    /// Update the previous locations array and set the isMoving bool 
    /// </summary>
    private void UpdatePreviousLocations()
    {
        // Store the newest vector at the end of the list of vectors
        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            previousLocations[i] = previousLocations[i + 1];
        }
        previousLocations[previousLocations.Length - 1] = transform.position;

        // Check the distances between the points in your previous locations
        // If for the past several updates, there are no movements smaller than the threshold,
        // you can most likely assume that the object is not moving
        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            if(!WithinMovementThreshold(previousLocations[i], previousLocations[i + 1]))
            {
                // The minimum movement has been detected between frames
                isMoving = true;
                break;
            }
            else
            {
                isMoving = false;
            }
        }

        Debug.Log("Is moving? => " + isMoving);
    }

    /// <summary>
    /// Set reset bool to be triggerd in next update
    /// </summary>
    public void Reset()
    {
        hasToReset = true;
    }

    /// <summary>
    /// Reset object to begin state
    /// </summary>
    private void ResetObjectState()
    {
        hasToReset = false;

        transform.position = startPosition;
        pathNodes.Clear();
        setPlayerIdleAnimation();
    }

    /// <summary>
    /// Get method for isMoving
    /// </summary>
    private bool IsMoving
    {
        get { return isMoving; }
    }

    /// <summary>
    /// A helper method to check if two vectors are between a movement threshold
    /// </summary>
    /// <param name="one">First Vector3 for comparison</param>
    /// <param name="two">Second Vector3 for comparison</param>
    /// <returns></returns>
    private bool WithinMovementThreshold(Vector3 one, Vector3 two)
    {
        return Vector3.Distance(one, two) < noMovementThreshold;
    }

    /// <summary>
    /// Reset all triggers before a new animation to avoid mixups
    /// </summary>
    private void resetPlayerAnimations()
    {
        animator.ResetTrigger("playerWalk");
        animator.ResetTrigger("playerJump");
        animator.ResetTrigger("playerIdle");
    }

    /// <summary>
    /// Start the object idle animation
    /// </summary>
    private void setPlayerIdleAnimation()
    {
        resetPlayerAnimations();
        animator.SetTrigger("playerIdle");
    }

    /// <summary>
    /// Start the object walk animation
    /// </summary>
    private void setPlayerWalkAnimation()
    {
        resetPlayerAnimations();
        animator.SetTrigger("playerWalk");
    }

    /// <summary>
    /// Start the object jump animation
    /// </summary>
    private void setPlayerJumpAnimation()
    {
        resetPlayerAnimations();
        animator.SetTrigger("playerJump");
    }
}
