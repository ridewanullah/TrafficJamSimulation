using UnityEngine;

public class FourCameraGrid : MonoBehaviour
{
    public Camera topLeftCamera;
    public Camera topRightCamera;
    public Camera bottomLeftCamera;
    public Camera bottomRightCamera;

    void Start()
    {
        topLeftCamera.rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
        topRightCamera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        bottomLeftCamera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
        bottomRightCamera.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
    }
}
