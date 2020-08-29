using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    public Vector3 rotationVector = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(rotationVector * Time.deltaTime);
    }
}
