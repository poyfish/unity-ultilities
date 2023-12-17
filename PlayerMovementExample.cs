
using UnityEngine;

public class Player : MonoBehaviour
{

    [HideInInspector] public BoxCollider2D coll;
    [HideInInspector] public Rigidbody2D rb;
    [SerializeField] private LayerMask jumpableGround;

    public float Speed;

    public float jumpForce;

    [HideInInspector]
    public Vector2 input;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    void Update()
    {

        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));



        if (input != Vector2.zero)
        {
            Dir = input;
        }

        rb.velocity = new Vector2(input.x * Speed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Z) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetKeyUp(KeyCode.Z) && !IsGrounded() && rb.velocity.y > .2f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
        }


    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size/1.1f, 0f, Vector2.down, .3f, jumpableGround);
    }

}
