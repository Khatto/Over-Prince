using UnityEngine;

public class CameraZoom : MonoBehaviour {

    public Camera zoomCamera;
    public bool zooming = false;
    public float zoomDuration = 1.0f;
    public float cameraStartSize = 5.0f;
    public float cameraEndSize = 10.0f;
    private float zoomStartTime = 0.0f;

    void Update() {
        if (zooming) {
            Zoom();
            if (Time.time - zoomStartTime >= zoomDuration) {
                FinishZoom();
            }
        }
    }

    public void Zoom() {
        zoomCamera.orthographicSize = Mathf.Lerp(
            cameraStartSize,
            cameraEndSize, 
            EasingFuncs.EaseInOut((Time.time - zoomStartTime) / zoomDuration)
        );
    }

    public void SetZoomAndStart(float startSize, float endSize, float duration) {
        cameraStartSize = startSize;
        cameraEndSize = endSize;
        zoomDuration = duration;
        StartZoom();
    }

    public void StartZoom() {
        if (!zooming) {
            zooming = true;
            zoomStartTime = Time.time;
        }
    }

    public void FinishZoom() {
        zoomCamera.orthographicSize = cameraEndSize;
        zooming = false;
    }
}