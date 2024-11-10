using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Configuration")]
    [SerializeField] Vector3 playerStartPosition = Vector3.zero;
    [SerializeField] float playerSpeed = 0f;
    [SerializeField] float playerRotation = 0f;

    [Header("Camera Setting")]
    public float followThreshold = 1f;

    private Rigidbody rb;

    private Camera cam;
    private Vector3 offset;

    LayerMask doorMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        transform.position = playerStartPosition;

        doorMask = 1 << 6;

        cam = Camera.main;
        offset = cam.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.3f, doorMask))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("1");
                GameObject door = hit.collider.gameObject;
                if (door.GetComponent<DoorTeleport>() == null) Debug.LogError("This door has no 'DoorTeleport' component!");

                GameObject teleportTo = door.GetComponent<DoorTeleport>().doorToWarp;
                Vector3 teleportOffset = door.GetComponent<DoorTeleport>().warpOffset;

                transform.position = teleportTo.transform.position + teleportOffset;
                cam.transform.position = transform.position + offset;
            }
            Debug.Log("2");
        }
        Debug.DrawRay(transform.position, transform.forward, Color.white, 0.3f);
    }

    private void FixedUpdate()
    {
        // Player Control
        float translationX = Input.GetAxis("Horizontal");
        float translationZ = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(translationX, 0, translationZ);

        if (direction.magnitude > 0.1f)
        {
            direction.Normalize();
            transform.Translate(direction * playerSpeed * Time.deltaTime, Space.World);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * playerRotation);
        }

        // Camera Follow
        Vector3 playerViewportPos = cam.WorldToViewportPoint(transform.position);

        bool isNearHorizontalEdge = playerViewportPos.x < followThreshold || playerViewportPos.x > 1 - followThreshold;
        bool isNearVerticalEdge = playerViewportPos.y < followThreshold || playerViewportPos.y > 1 - followThreshold;

        if (isNearHorizontalEdge || isNearVerticalEdge)
        {
            Vector3 targetPosition = cam.transform.position;
            targetPosition.x = transform.position.x + offset.x;

            
            targetPosition.y = cam.transform.position.y;
            targetPosition.z = cam.transform.position.z;

            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, Time.deltaTime * 5f);
        }
    }
    

    void OnCollisionStay(Collision collision)
    {
        // Stop movement while the player is touching the wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
