using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLighting : MonoBehaviour
{
    public Light roomLight;  // Assign this in the Inspector
    public float minIntensity = 0.5f;
    public float maxIntensity = 2.0f;
    public Color gravityShiftColor = Color.blue;  // Color for gravity shift
    public Color normalColor = Color.white;  // Default color
    public float changeSpeed = 2.0f;

    private bool isGravityShifted = false;

    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            isGravityShifted = !isGravityShifted;
            ChangeLightColor(isGravityShifted);  // Change color only once on key press
        }
    }

    void ChangeLightColor(bool gravityShifted)
    {
        if (gravityShifted)
        {
            roomLight.color = gravityShiftColor;
        }
        else
        {
            roomLight.color = normalColor;
        }
    }
}