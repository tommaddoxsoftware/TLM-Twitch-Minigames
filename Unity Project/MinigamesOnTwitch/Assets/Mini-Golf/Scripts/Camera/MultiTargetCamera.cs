using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultiTargetCamera : MonoBehaviour, ISetTargets {

    [Tooltip("List of targetTransforms for camera to track")]
    public List<GameObject> targetObjects = new List<GameObject>();

    private List<Transform> targetTransforms = new List<Transform>();

    [Tooltip("Camera position offset")]
    public Vector3 offsetPosition;

    [Tooltip("Factor of how quickly camera follows target position")]
    [SerializeField] private float smoothTime = 0.5f;

    [Tooltip("Largest FOV camera can ultiise [must be larget than minFOV for desired effects]")]
    [SerializeField] private float maxFOV = 40f;
    [Tooltip("Smallest FOV camera can ultiise [must be smaller than maxFOV for desired effects]")]
    [SerializeField] private float minFOV = 10f;
    [Tooltip("Sensitivity of camera zoom [Update Description]")]
    [SerializeField] private float zoomSensitivity = 500f;
    [Tooltip("Rate camera zoom changes")]
    [SerializeField] private float zoomSpeed = 5.0f;


    private Vector3 velocity;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();        
    }

    private void LateUpdate()
    {
        //Debug.Log(targetObjects.Count);

        if(targetObjects.Count == 0)
        {
            return;
        }

        GetTransforms();

        if (targetTransforms.Count == 0)
        {
            return;
        }

    // camera manipulation methods (could be moved to interfaces at a later date for having camera with different requirements)
        Move();
        Zoom();
    }

    private List<Transform> GetTransforms()
    {
        targetTransforms = new List<Transform>();

        // only one valid target
        if (targetObjects.Count == 1)
        {
            if(targetObjects[0] != null)
            {
                targetTransforms.Add(targetObjects[0].transform);               
            }
            return targetTransforms;
        }

        // multiple targets
        for (int i = 0; i < targetObjects.Count; i++)
        {
            if (targetObjects[i] != null)
            {
                targetTransforms.Add(targetObjects[i].transform);
            }

        }

        return targetTransforms;
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();


        Vector3 newPosition = (centerPoint + offsetPosition);


        transform.localPosition = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }


    void Zoom()
    {
        // get value for FOV between minFOV and maxFOV.
        float newZoom = Mathf.Lerp(minFOV,maxFOV, (minFOV/maxFOV) * (zoomSensitivity/(100 * GetGreatestDistance() ) ) );
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView,newZoom,Time.deltaTime*zoomSpeed);
    }

    float GetGreatestDistance()
    {
        Bounds bounds = new Bounds(targetTransforms[0].position, Vector3.zero);

        for(int i = 0; i < targetTransforms.Count; i++)
        {
            bounds.Encapsulate(targetTransforms[i].position);
        }

        return bounds.size.magnitude;
    }

    Vector3 GetCenterPoint()
    {
        if(targetTransforms.Count == 1)
        {
            return targetTransforms[0].position;
        }

        Bounds bounds = new Bounds(targetTransforms[0].position, Vector3.zero);

        for(int i = 0; i < targetTransforms.Count; i++)
        {
            bounds.Encapsulate(targetTransforms[i].position);
        }

        return bounds.center;
    }

    
    // Interface methods

        // begin tracking object
    public void AddObject(GameObject t)
    {
        Debug.Log(t);
        if (!targetObjects.Contains(t))
        {
            targetObjects.Add(t);
        }
    }

        // stop tracking object
    public void RemoveObject(GameObject t)
    {
        if(targetObjects.Contains(t)){
            targetObjects.Remove(t);
        }
    }
}
