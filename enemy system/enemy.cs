using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{

    public float HitBounceForce;

    [HideInInspector]
    public BoxCollider2D coll;

    [HideInInspector]
    public Rigidbody2D rb;

    [HideInInspector]
    public SpriteRenderer sprite;


    public void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite= GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        CheckForHitHead();
    }

    public void CheckForHitHead()
    {
        RaycastHit2D hit = Physics2D.BoxCast(coll.bounds.center + new Vector3(0, .5f), coll.bounds.size + new Vector3(-.1f, -.7f), 0f, Vector2.down, 0, LayerMask.GetMask("Player"));

        if(hit.collider != null)
        {
            OnDeath(hit.collider.gameObject);
        }
    }

    public virtual void OnDeath(GameObject hit)
    {
        Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

        rb.velocity = new Vector2(rb.velocity.x, HitBounceForce);

        Destroy(this.gameObject);
    }
}
