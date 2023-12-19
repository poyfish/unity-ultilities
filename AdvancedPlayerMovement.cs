using UnityEngine;
using NaughtyAttributes;

public class Player : MonoBehaviour
{

    [HideInInspector] public BoxCollider2D coll;
    [HideInInspector] public Rigidbody2D rb;
    [SerializeField] private LayerMask jumpableGround;

    
    private float ExelerationTimer;
    private float DecelerationTimer;

    [Foldout("Movement")]
    public float Speed;
    [Foldout("Movement")]
    public float ExelerationSpeed;
    [Foldout("Movement")] [CurveRange(0, 0, 1, 1)]
    public AnimationCurve ExelerationCurve;
    [Foldout("Movement")]
    public float DecelerationSpeed;
    [Foldout("Movement")] [CurveRange(0, 0, 1, 1)]
    public AnimationCurve DecelerationCurve;

    [Foldout("Jump")]
    public float jumpForce;
    [Foldout("Jump")]
    public float JumpGravity;
    [Foldout("Jump")]
    public float FallGravity;


    [HideInInspector]
    public Vector2 input;
    [HideInInspector]
    public Vector2 Dir;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        setJumpGravity();

        rb.velocity = new Vector2(Dir.x * Speed * GetExeleration(input.x), rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Z) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetKeyUp(KeyCode.Z) && !IsGrounded() && rb.velocity.y > .2f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
        }
        

    }


    private void setJumpGravity()
    {
        if(rb.velocity.y < -.2f)
        {
            rb.gravityScale = FallGravity;
        }
        
        if(IsGrounded())
        {
            rb.gravityScale = JumpGravity;
        }
    }

    private float GetExeleration(float input)
    {

        float Exeleration = 0;

        if (input != 0)
        {
            Dir.x = input;


            Exeleration = Mathf.Clamp01(ExelerationCurve.Evaluate(ExelerationTimer));

            ExelerationTimer += Time.deltaTime * ExelerationSpeed;

            DecelerationTimer = 0;
        }
        else
        {

            if (ExelerationTimer != 0) DecelerationTimer = 1 - ExelerationTimer;

            Exeleration = Mathf.Clamp01(DecelerationCurve.Evaluate(DecelerationTimer));

            DecelerationTimer += Time.deltaTime * DecelerationSpeed;

            ExelerationTimer = 0;
        }

        return Exeleration;
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size / 1.1f, 0f, Vector2.down, .3f, jumpableGround);
    }

}
