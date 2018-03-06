using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultiTargetCamera : MonoBehaviour {

    [Tooltip("List of targets for camera to track")]
    public List<Transform> targets;

    [Tooltip("Camera offset")]
    public Vector3 offset;

    [Tooltip("Factor of how quickly camera follows target position")]
    [SerializeField] private float smoothTime = 0.5f;

    [Tooltip("Largest FOV camera can ultiise [must be larget than minFOV for desired effects]")]
    [SerializeField] private float maxFOV = 40f;
    [Tooltip("Smallest FOV camera can ultiise [must be smaller than maxFOV for desired effects]")]
    [SerializeField] private float minFOV = 10f;
    [Tooltip("Sensitivity of camera zoom [Higher value equates to camera reaching full zoom sooner]")]
    [SerializeField] private float zoomSensitivity = 50f;
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
        if (targets.Count == 0)
        {
            return;
        }

        Move();
        Zoom();
    }


    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = cam.transform.rotation * (centerPoint + offset);

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }


    void Zoom()
    {

        float newZoom = Mathf.Lerp(minFOV,maxFOV, GetGreatestDistance() * zoomSensitivity/100);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView,newZoom,Time.deltaTime*zoomSpeed);

    }

    float GetGreatestDistance()
    {
        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);

        for(int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.magnitude;
    }

    Vector3 GetCenterPoint()
    {
        if(targets.Count == 1)
        {
            return targets[0].position;
        }

        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);

        for(int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }
}
