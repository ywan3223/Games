using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CarAIHandler : MonoBehaviour
{
    public enum AIMode { followPlayer, followWaypoints, followMouse };

    [Header("AI settings")]
    public AIMode aiMode;
    public float maxSpeed = 16;
    public bool isAvoidingCars = true;
    [Range(0.0f, 1.0f)]
    public float skillLevel = 1.0f;

    //Local variables
    Vector3 targetPosition = Vector3.zero;
    Transform targetTransform = null;
    float orignalMaximumSpeed = 0;

    //Stuck handling
    bool isRunningStuckCheck = false;
    bool isFirstTemporaryWaypoint = false;
    int stuckCheckCounter = 0;
    List<Vector2> temporaryWaypoints = new List<Vector2>();
    float angleToTarget = 0;

    //Avoidance
    Vector2 avoidanceVectorLerped = Vector3.zero;

    //Waypoints
    WaypointNode currentWaypoint = null;
    WaypointNode previousWaypoint = null;
    WaypointNode[] allWayPoints;

    //Colliders
    PolygonCollider2D polygonCollider2D;

    //Components
    TopDownCarController topDownCarController;
    AStarLite aStarLite;

    //Awake is called when the script instance is being loaded.
    void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
        allWayPoints = FindObjectsOfType<WaypointNode>();

        aStarLite = GetComponent<AStarLite>();

        polygonCollider2D = GetComponentInChildren<PolygonCollider2D>();

        orignalMaximumSpeed = maxSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetMaxSpeedBasedOnSkillLevel(maxSpeed);
    }

    // Update is called once per frame and is frame dependent
    void FixedUpdate()
    {
        if (GameManager.instance.GetGameState() == GameStates.countDown)
            return;

        Vector2 inputVector = Vector2.zero;

        switch (aiMode)
        {
            case AIMode.followPlayer:
                FollowPlayer();
                break;

            case AIMode.followWaypoints:
                if (temporaryWaypoints.Count == 0)
                    FollowWaypoints();
                else FollowTemporaryWayPoints();

                break;

            case AIMode.followMouse:
                FollowMousePosition();
                break;
        }

        inputVector.x = TurnTowardTarget();
        inputVector.y = ApplyThrottleOrBrake(inputVector.x);

        //If the AI is applying throttle but not manging to get any speed then lets run our stuck check.
        if (topDownCarController.GetVelocityMagnitude() < 0.5f && Mathf.Abs(inputVector.y) > 0.01f && !isRunningStuckCheck)
            StartCoroutine(StuckCheckCO());

        //Handle special case where the car has reversed for a while then it should check if it is still stuck. If it is not then it will drive forward again.
        if (stuckCheckCounter >= 4 && !isRunningStuckCheck)
            StartCoroutine(StuckCheckCO());


        //Send the input to the car controller.
        topDownCarController.SetInputVector(inputVector);
    }

    //AI follows player
    void FollowPlayer()
    {
        if (targetTransform == null)
            targetTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (targetTransform != null)
            targetPosition = targetTransform.position;
    }

    //AI follows waypoints
    void FollowWaypoints()
    {
        //Pick the cloesest waypoint if we don't have a waypoint set.
        if (currentWaypoint == null)
        {
            currentWaypoint = FindClosestWayPoint();
            previousWaypoint = currentWaypoint;
        }

        //Set the target on the waypoints position
        if (currentWaypoint != null)
        {
            //Set the target position of for the AI. 
            targetPosition = currentWaypoint.transform.position;

            //Store how close we are to the target
            float distanceToWayPoint = (targetPosition - transform.position).magnitude;

            //Check if we are close enough to consider that we have reached the waypoint
            if (distanceToWayPoint <= currentWaypoint.minDistanceToReachWaypoint)
            {
                if (currentWaypoint.maxSpeed > 0)
                    SetMaxSpeedBasedOnSkillLevel(currentWaypoint.maxSpeed);
                else SetMaxSpeedBasedOnSkillLevel(1000);

                //Store the current waypoint as previous before we assign a new current one.
                previousWaypoint = currentWaypoint;

                //If we are close enough then follow to the next waypoint, if there are multiple waypoints then pick one at random.
                currentWaypoint = currentWaypoint.nextWaypointNode[Random.Range(0, currentWaypoint.nextWaypointNode.Length)];
            }
        }
    }

    //AI follows waypoints
    void FollowTemporaryWayPoints()
    {
        //Set the target position of for the AI. 
        targetPosition = temporaryWaypoints[0];

        //Store how close we are to the target
        float distanceToWayPoint = (targetPosition - transform.position).magnitude;

        //Drive a bit slower than usual
        SetMaxSpeedBasedOnSkillLevel(5);

        //Check if we are close enough to consider that we have reached the waypoint
        float minDistanceToReachWaypoint = 1.5f;

        if (!isFirstTemporaryWaypoint)
            minDistanceToReachWaypoint = 3.0f;

        if (distanceToWayPoint <= minDistanceToReachWaypoint)
        {
            temporaryWaypoints.RemoveAt(0);
            isFirstTemporaryWaypoint = false;
        }
    }

    //AI follows the mouse position
    void FollowMousePosition()
    {
        //Take the mouse position in screen space and convert it to world space
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Set the target position of for the AI. 
        targetPosition = worldPosition;
    }

    //Find the cloest Waypoint to the AI
    WaypointNode FindClosestWayPoint()
    {
        return allWayPoints
            .OrderBy(t => Vector3.Distance(transform.position, t.transform.position))
            .FirstOrDefault();
    }

    float TurnTowardTarget()
    {
        Vector2 vectorToTarget = targetPosition - transform.position;
        vectorToTarget.Normalize();

        //Apply avoidance to steering
        if (isAvoidingCars && !topDownCarController.IsJumping())
            AvoidCars(vectorToTarget, out vectorToTarget);

        //Calculate an angle towards the target 
        angleToTarget = Vector2.SignedAngle(transform.up, vectorToTarget);
        angleToTarget *= -1;

        //We want the car to turn as much as possible if the angle is greater than 45 degrees and we wan't it to smooth out so if the angle is small we want the AI to make smaller corrections. 
        float steerAmount = angleToTarget / 45.0f;

        //Clamp steering to between -1 and 1.
        steerAmount = Mathf.Clamp(steerAmount, -1.0f, 1.0f);

        return steerAmount;
    }

    float ApplyThrottleOrBrake(float inputX)
    {
        if (topDownCarController.GetVelocityMagnitude() > maxSpeed)
            return 0;

        float reduceSpeedDueToCornering = Mathf.Abs(inputX) / 1.0f;
        float throttle = 1.05f - reduceSpeedDueToCornering * skillLevel;

        if (temporaryWaypoints.Count() != 0)
        {
            if (angleToTarget > 70)
                throttle = throttle * -1;
            else if (angleToTarget < -70)
                throttle = throttle * -1;
            else if (stuckCheckCounter > 3)
                throttle = throttle * -1;
        }
        return throttle;
    }

    void SetMaxSpeedBasedOnSkillLevel(float newSpeed)
    {
        maxSpeed = Mathf.Clamp(newSpeed, 0, orignalMaximumSpeed);

        float skillbasedMaxiumSpeed = Mathf.Clamp(skillLevel, 0.3f, 1.0f);
        maxSpeed = maxSpeed * skillbasedMaxiumSpeed;
    }

    //nearest point
    Vector2 FindNearestPointOnLine(Vector2 lineStartPosition, Vector2 lineEndPosition, Vector2 point)
    {
        //Get heading as a vector
        Vector2 lineHeadingVector = (lineEndPosition - lineStartPosition);
        float maxDistance = lineHeadingVector.magnitude;
        lineHeadingVector.Normalize();

        //Do projection 
        Vector2 lineVectorStartToPoint = point - lineStartPosition;
        float dotProduct = Vector2.Dot(lineVectorStartToPoint, lineHeadingVector);

        //maxDistance
        dotProduct = Mathf.Clamp(dotProduct, 0f, maxDistance);

        return lineStartPosition + lineHeadingVector * dotProduct;
    }

    bool IsCarsInFrontOfAICar(out Vector3 position, out Vector3 otherCarRightVector)
    {
        polygonCollider2D.enabled = false;
        RaycastHit2D raycastHit2d = Physics2D.CircleCast(transform.position + transform.up * 0.5f, 1.2f, transform.up, 12, 1 << LayerMask.NameToLayer("Car"));
        polygonCollider2D.enabled = true;

        if (raycastHit2d.collider != null)
        {
            Debug.DrawRay(transform.position, transform.up * 12, Color.red);

            position = raycastHit2d.collider.transform.position;
            otherCarRightVector = raycastHit2d.collider.transform.right;
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.up * 12, Color.black);
        }
        position = Vector3.zero;
        otherCarRightVector = Vector3.zero;

        return false;
    }

    void AvoidCars(Vector2 vectorToTarget, out Vector2 newVectorToTarget)
    {
        if (IsCarsInFrontOfAICar(out Vector3 otherCarPosition, out Vector3 otherCarRightVector))
        {
            Vector2 avoidanceVector = Vector2.zero;

            //reflecing vector 
            avoidanceVector = Vector2.Reflect((otherCarPosition - transform.position).normalized, otherCarRightVector);
            float distanceToTarget = (targetPosition - transform.position).magnitude;
            float driveToTargetInfluence = 6.0f / distanceToTarget;
            driveToTargetInfluence = Mathf.Clamp(driveToTargetInfluence, 0.30f, 1.0f);
            float avoidanceInfluence = 1.0f - driveToTargetInfluence;
            avoidanceVectorLerped = Vector2.Lerp(avoidanceVectorLerped, avoidanceVector, Time.fixedDeltaTime * 4);
            newVectorToTarget = (vectorToTarget * driveToTargetInfluence + avoidanceVector * avoidanceInfluence);
            newVectorToTarget.Normalize();
            Debug.DrawRay(transform.position, avoidanceVector * 10, Color.green);
            Debug.DrawRay(transform.position, newVectorToTarget * 10, Color.yellow);
            return;
        }

        newVectorToTarget = vectorToTarget;
    }

    IEnumerator StuckCheckCO()
    {
        Vector3 initialStuckPosition = transform.position;

        isRunningStuckCheck = true;

        yield return new WaitForSeconds(0.7f);

        if ((transform.position - initialStuckPosition).sqrMagnitude < 3)
        {
            temporaryWaypoints = aStarLite.FindPath(currentWaypoint.transform.position);
            if (temporaryWaypoints == null)
                temporaryWaypoints = new List<Vector2>();

            stuckCheckCounter++;

            isFirstTemporaryWaypoint = true;
        }
        else stuckCheckCounter = 0;
        isRunningStuckCheck = false;
    }
}