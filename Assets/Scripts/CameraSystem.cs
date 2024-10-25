using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private float desiredRatio = 16f / 9f, zoomMinBoundary = .1f, touchZoomSpeed = 0.1f, mouseWheelZoomSpeed = 10f;
    private Camera mainCamera;
    private float cameraInitialSize, zoomRatio = 1;
    private Vector3 topLeftBoundary, bottomRightBoundary, initialPosition, mousePositionDifference, mouseOrigin;
    private bool doFollowMouse = false;

    private void Start()
    {
        mainCamera = Camera.main;
        cameraInitialSize = mainCamera.orthographicSize;
        topLeftBoundary = transform.position;
        bottomRightBoundary = transform.position;
        initialPosition = transform.position;
    }


    private void Update()
    {
        float currentRatio = (float)Screen.width / Screen.height;
        if (desiredRatio > currentRatio)
        {
            mainCamera.orthographicSize = cameraInitialSize * zoomRatio * (desiredRatio / currentRatio);
        }
        
        // Apply different zoom methods
        ApplyTouchZoom();
        ApplyMouseWheelZoom();

        if (doFollowMouse)
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.position = ApplyPositionBoundary(mouseOrigin - difference);
        }
    }

    private void ApplyMouseWheelZoom()
    {
        float scrollVal = Input.GetAxis("Mouse ScrollWheel") * mouseWheelZoomSpeed;
        if (scrollVal > 0)
        {
            ZoomIn(scrollVal);
        }
        else if (scrollVal < 0)
        {
            ZoomOut(-scrollVal);
        }
    }

    private void ApplyTouchZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            Vector2 firstTouchPrevPosition = firstTouch.position - firstTouch.deltaPosition;
            Vector2 secondTouchPrevPosition = secondTouch.position - secondTouch.deltaPosition;

            float touchesPrevPosDifference = (firstTouchPrevPosition - secondTouchPrevPosition).magnitude;
            float touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

            float zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * touchZoomSpeed;

            if (touchesPrevPosDifference > touchesCurPosDifference) ZoomIn(zoomModifier);
            else if (touchesPrevPosDifference < touchesCurPosDifference) ZoomOut(zoomModifier);
        }
    }

    private void ApplyZoomBoundary()
    {
        if (zoomRatio < zoomMinBoundary)
        {
            zoomRatio = zoomMinBoundary;
        }
        else if (zoomRatio > 1)
        {
            zoomRatio = 1;
        }
    }

    private Vector3 ApplyPositionBoundary(Vector3 position)
    {
        if (position.x < topLeftBoundary.x)
        {
            position.x = topLeftBoundary.x;
        }
        else if (position.x > bottomRightBoundary.x)
        {
            position.x = bottomRightBoundary.x;
        }

        if (position.y > topLeftBoundary.y)
        {
            position.y = topLeftBoundary.y;
        }
        else if (position.y < bottomRightBoundary.y)
        {
            position.y = bottomRightBoundary.y;
        }
        return position;
    }

    public void FollowMouse(bool doFollow)
    {
        doFollowMouse = doFollow;
        if (doFollowMouse)
        {
            mouseOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public void ZoomIn(float zoomVal = .0125f)
    {
        if (zoomRatio > zoomMinBoundary)
        {
            zoomRatio -= zoomVal;
            ApplyZoomBoundary();
            UpdateBoundary();
        }
    }

    public void ZoomOut(float zoomVal = .125f)
    {
        if (zoomRatio < 1)
        {
            zoomRatio += zoomVal;
            ApplyZoomBoundary();
            UpdateBoundary();
            transform.position = ApplyPositionBoundary(transform.position);
        }
    }

    private void UpdateBoundary()
    {
        float temp = cameraInitialSize * (1 - zoomRatio);
        topLeftBoundary = new(-temp * desiredRatio + initialPosition.x, temp + initialPosition.y, initialPosition.z);
        bottomRightBoundary = new(temp * desiredRatio + initialPosition.x, -temp + initialPosition.y, initialPosition.z);
        mainCamera.orthographicSize = cameraInitialSize * zoomRatio;
    }
}
