using System.Collections;
using UnityEngine;

public class CameraSizeSync : MonoBehaviour
{
    [Header("Target Camera (Leave empty = Main Camera)")]
    public Camera targetCamera;

    [Header("Orthographic Settings")]
    public bool changeSize = true;
    public float cameraSize = 2.27f;

    [Header("Position Settings")]
    public bool changeYPosition = true;
    public float yPosition = 0.39f;

    [Header("Reset Camera Rect")]
    public bool resetCameraRect = false;
    public float resetCameraRectAfter = 0.5f;
    public Rect resetCameraRectValue = new Rect(0f, 0f, 1f, 1f);
    private void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (targetCamera == null)
        {
            Debug.LogWarning("CameraSizeSync: No target camera found.");
            return;
        }

        if (changeSize && targetCamera.orthographic)
        {
            targetCamera.orthographicSize = cameraSize;
        }

        if (changeYPosition)
        {
            Vector3 pos = targetCamera.transform.position;
            pos.y = yPosition;
            targetCamera.transform.position = pos;
        }

        if (resetCameraRect)
        {
            StartCoroutine(ResetRect());
        }
    }
    private IEnumerator ResetRect()
    {
        yield return new WaitForSecondsRealtime(resetCameraRectAfter);

        targetCamera.rect = resetCameraRectValue;
    }
}