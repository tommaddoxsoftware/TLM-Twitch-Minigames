using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectTargetTypes))]
public class GetCameraTarget : MonoBehaviour, IGetTarget {

    [Tooltip("Attach a Camera that has the MultiTargetCamera Script")]
    [SerializeField]
    private MultiTargetCamera multiTarget_Cam;

    private void Awake()
    {
        AddThis();
    }

    private void OnDisable()
    {
        RemoveThis();
    }

    public void AddThis()
    {
        multiTarget_Cam.AddObject(this.gameObject);
    }

    public void RemoveThis()
    {
        multiTarget_Cam.RemoveObject(this.gameObject);
    }
}
