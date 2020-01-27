using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debuggravity : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 m_velocity;
    public bool m_overrideVel;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //print(rb.velocity);
        if (m_overrideVel)
        {
            rb.velocity = m_velocity;
        }
    }
}
