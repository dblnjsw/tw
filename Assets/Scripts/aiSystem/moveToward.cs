using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class moveToward : Action
{
    public float speed;
    public SharedTransform target;
    public float jumpDuring = 1;
    bool canJump = true;
    float jumpStartTime;

    Movement movement;
    Rigidbody2D rb;

    public override void OnAwake()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<Movement>();
    }

    public override TaskStatus OnUpdate()
    {
        if (Vector3.SqrMagnitude(transform.position - target.Value.position) < 0.1f)
        {
            return TaskStatus.Success;
        }
        Vector3 targetPos = new Vector3(target.Value.position.x, 0,0);
        if (target.Value.position.x - transform.position.x < 0)
        {
            movement.hMove = -1;
        }
        else
        {
            movement.hMove = 1;
        }

        if (canJump)
        {
            movement.Jump();
            canJump = false;
            jumpStartTime = Time.time;
        }


        if (Time.time - jumpStartTime < jumpDuring)
        {
            canJump = true;
        }



        return TaskStatus.Running;
    }

}
