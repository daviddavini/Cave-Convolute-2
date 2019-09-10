using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAnimationHandler : MonoBehaviour
{

    private Animator animator;
    new private Rigidbody2D rigidbody;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        TellAnimatorMovement(rigidbody.velocity);
    }

    void TellAnimatorMovement(Vector3 velocity)
    {
        if(velocity.sqrMagnitude > 0.01)
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("dirX", velocity.x);
            animator.SetFloat("dirY", velocity.y);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
}
