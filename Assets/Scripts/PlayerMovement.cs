using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer playerSprite;
    private PlayerControls playerControls;
    private Rigidbody rb;
    private Vector3 movement;

    private const string IS_WALK_PARAM = "IsWalk";
    private const string DIRECTION_PARAM = "Direction";


    private void Awake()
    {
        playerControls = new PlayerControls();

    }
    private void OnEnable()
    {
        playerControls.Enable();

    }

    private void OnDisable()
    {
        playerControls.Disable();
    }


    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = playerControls.Player.Move.ReadValue<Vector2>().x;
        float z = playerControls.Player.Move.ReadValue<Vector2>().y;

        movement = new Vector3(x, 0, z).normalized;

        anim.SetBool(IS_WALK_PARAM, movement != Vector3.zero);

        if (x < 0)
        {
            anim.SetFloat(DIRECTION_PARAM, -1); // 播放左移动动画
        }
        else if (x > 0)
        {
            anim.SetFloat(DIRECTION_PARAM, 1); // 播放右移动动画
        }
        else
        {
            anim.SetFloat(DIRECTION_PARAM, 0); // 回到Idle状态
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);
    }
}

