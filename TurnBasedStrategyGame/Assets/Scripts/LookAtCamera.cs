using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform cameraTransform;
    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }
    private void LateUpdate()
    {
        Vector3 dirToCamera = (cameraTransform.position-transform.position ).normalized;
        transform.LookAt(transform.position + dirToCamera * -1);
    }
}
