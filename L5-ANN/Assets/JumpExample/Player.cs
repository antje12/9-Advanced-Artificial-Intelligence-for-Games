using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float jumpForce = 1;
    Rigidbody rb;
    bool jump = false;
    public bool canJump = false;

    public double distanceToProjectile;

    public ANNComponent net;

    public float recordRate = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        InvokeRepeating("Record", recordRate, recordRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            jump = true;
            net.SaveData(distanceToProjectile, CanJump(), 1);
        }

        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Enemy");
        distanceToProjectile = double.MaxValue;
        foreach (GameObject g in projectiles)
        {
            if (Vector3.Distance(transform.position, g.transform.position) < distanceToProjectile)
            {
                distanceToProjectile = Vector3.Distance(transform.position, g.transform.position);
            }
        }
    }

    private void FixedUpdate()
    {
        if (jump)
        {
            Jump();
        }
            
    }

    void Record()
    {
        net.SaveData(distanceToProjectile, CanJump(), 0);
    }

    public void Jump()
    {
        if (canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jump = false;
            canJump = false;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            canJump = true;
    }

    public double CanJump()
    {
        if (canJump)
            return 1;
        else
            return 0;
    }
}
