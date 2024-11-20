using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player Configuration")]
    [SerializeField] Vector3 playerStartPosition = Vector3.zero;
    [SerializeField] float playerSpeed = 0f;
    [SerializeField] float playerRotation = 0f;

    [Header("Camera Setting")]
    public float followThreshold = 1f;

    [Header("Fade Transition")]
    [SerializeField] Image blackScreenPanel;
    [SerializeField] float fadeDuration;

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

                StartCoroutine(FadeAndTeleport(teleportTo.transform.position, teleportOffset));
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
    }
    

    void OnCollisionStay(Collision collision)
    {
        // Stop movement while the player is touching the wall
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Item"))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private IEnumerator FadeAndTeleport(Vector3 teleportPosition, Vector3 teleportOffset)
    {
        yield return StartCoroutine(FadeToBlack());

        yield return new WaitForSeconds(0.25f);

        transform.position = new Vector3(teleportPosition.x, transform.position.y, teleportPosition.z) + teleportOffset;
        cam.transform.position = transform.position + offset;

        yield return new WaitForSeconds(0.25f);

        yield return StartCoroutine(FadeToClear());
    }

    private IEnumerator FadeToBlack()
    {
        float elapsed = 0;
        Color color = blackScreenPanel.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / fadeDuration);
            blackScreenPanel.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeToClear()
    {
        float elapsed = 0;
        Color color = blackScreenPanel.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(1 - (elapsed / fadeDuration));
            blackScreenPanel.color = color;
            yield return null;
        }
    }
}
