using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; 
    public float followThreshold = 0.4f;

    private Camera cam;
    private Vector3 offset;

    void Start()
    {
        cam = Camera.main;
        offset = cam.transform.position - player.position;
    }

    void FixedUpdate()
    {
        Vector3 playerViewportPos = cam.WorldToViewportPoint(player.position);

        bool isNearHorizontalEdge = playerViewportPos.x < followThreshold || playerViewportPos.x > 1 - followThreshold;
        bool isNearVerticalEdge = playerViewportPos.y < followThreshold || playerViewportPos.y > 1 - followThreshold;

        if (isNearHorizontalEdge || isNearVerticalEdge)
        {
            Vector3 targetPosition = player.position + offset;
            targetPosition.z = cam.transform.position.z;
            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, Time.deltaTime * 5f);
        }
    }
}
