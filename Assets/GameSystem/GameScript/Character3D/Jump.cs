using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private int groundContacts = 0;
    public float jumpForce = 5f;
    private Rigidbody rb;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame

    void OnCollisionEnter(Collision col)
    {
        if (col.contacts.Length > 0) groundContacts++;
    }

    void OnCollisionExit(Collision col)
    {
        if (groundContacts > 0) groundContacts--;
    }

    bool IsGrounded() => groundContacts > 0;

    public void HandleJump()
    {
        bool grounded = IsGrounded();
        if (grounded)
        {
            if (animator != null)
            animator.SetTrigger("Jump");
            PerformJump(); //can be use in animator
        }
        if (animator != null)
            animator.SetBool("InAir", !grounded);
    }

    public void PerformJump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

}
