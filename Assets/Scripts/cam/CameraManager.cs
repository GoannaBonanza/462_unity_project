using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager im;
    //Object to follow
    public Transform targetTransform;
    public Transform camPivot;
    public Transform camTransform;
    public LayerMask collisionLayers;
    private float defaultPos;
    private Vector3 camFollowVel = Vector3.zero;
    private Vector3 camVectorPos;

    public float camCollisionOffset = 0.2f; //how much camera jumps off when collide with wall
    public float minCollisionOffset = 0.2f;
    public float camCollisionRadius = 0.2f;
    public float camFollowSpeed = 0.2f;
    public float camLookSpeed = 1;
    public float camPivotSpeed = 1;

    public float lookAngle; //up down
    public float pivotAngle; //left right
    public float minPivAng = -18;
    public float maxPivAng = 35;

    private void Awake()
    {
        im = FindObjectOfType<InputManager>();
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        defaultPos = camTransform.localPosition.z;
        float playerSensValue = PlayerPrefs.GetFloat("Sensitivity", 1);
        camLookSpeed = playerSensValue;
        camPivotSpeed = playerSensValue;
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollision();
    }

    private void FollowTarget()
    {
        //deprecated smooth camera follow, too clunky
        //Vector3 targetPos = Vector3.SmoothDamp(transform.position, targetTransform.position, ref camFollowVel, camFollowSpeed);
        //transform.position = targetPos;
        transform.position = targetTransform.position;
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRot;

        lookAngle = lookAngle + (im.camInputX * camLookSpeed);
        pivotAngle = pivotAngle - (im.camInputY * camPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minPivAng, maxPivAng);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRot = Quaternion.Euler(rotation);
        transform.rotation = targetRot;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRot = Quaternion.Euler(rotation);
        camPivot.localRotation = targetRot;
    }

    private void HandleCameraCollision()
    {
        float targetPos = defaultPos;
        RaycastHit hit;
        Vector3 direction = camTransform.position - camPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(camPivot.transform.position, camCollisionRadius, direction, out hit, Mathf.Abs(targetPos), collisionLayers))
        {
            float distance = Vector3.Distance(camPivot.position, hit.point);
            targetPos = - (distance - camCollisionOffset);
        }
        if (Mathf.Abs(targetPos) < minCollisionOffset)
        {
            targetPos = targetPos - minCollisionOffset;
        }
        camVectorPos.z = Mathf.Lerp(camTransform.localPosition.z, targetPos, 0.2f);
        camTransform.localPosition = camVectorPos;
    }
}
