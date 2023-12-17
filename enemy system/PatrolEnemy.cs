using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PatrolEnemy : enemy
{
    private bool IsFacingRight;

    public float MovementSpeed;

    public bool FlipSprite;


    void Update()
    {
        base.Update();

        TryFlipAround();
        CheckForTurnAround();
        Move();
    }


    void TryFlipAround()
    {
        sprite.flipX = !IsFacingRight;
    }

    void CheckForTurnAround()
    {
        bool HitWall = Physics2D.BoxCast(coll.bounds.center + new Vector3((IsFacingRight ? .07f : -.07f), 0), coll.bounds.size + new Vector3(0, -.5f), 0f, Vector2.down, 0f, LayerMask.GetMask("platform"));

        bool CanFall = Physics2D.BoxCast(coll.bounds.center + new Vector3((IsFacingRight ? .1f : -.1f) * 5, -.7f), coll.bounds.size + new Vector3(-.7f, -.7f), 0f, Vector2.down, 0f, LayerMask.GetMask("platform"));

        if (HitWall || CanFall)
        {
            IsFacingRight = !IsFacingRight;
        }
    }

    void Move()
    {
        switch (IsFacingRight)
        {
            case true:
                rb.velocity = new Vector2(MovementSpeed, rb.velocity.y);
                break;

            case false:
                rb.velocity = new Vector2(-MovementSpeed, rb.velocity.y);
                break;
        }
    }

    public override void OnDeath(GameObject hit)
    {
        base.OnDeath(hit);
    }

}
