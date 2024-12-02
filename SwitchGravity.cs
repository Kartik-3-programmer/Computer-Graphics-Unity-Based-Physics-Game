using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGravity : MonoBehaviour
{
    private Rigidbody rb;
    private bool isGravityReversed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            isGravityReversed = !isGravityReversed;
            ToggleGravity();
        }
    }

    void ToggleGravity()
    {
        if (isGravityReversed)
        {
            Physics.gravity = new Vector3(0, 0, 9.81f); // Reverse gravity along the z-axis
        }
        else
        {
            Physics.gravity = new Vector3(0, 0, -9.81f); // Normal gravity along the z-axis
        }
    }
}
