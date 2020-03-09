using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public float distanceFromCenter;
    public float revolveSpeed;

    private void Start()
    {
        transform.position = new Vector3(0, distanceFromCenter, 0);
        transform.LookAt(Vector3.zero);
    }
    private void Update()
    {
        transform.RotateAround(Vector3.zero,transform.up,revolveSpeed*Time.deltaTime);
    }
}
