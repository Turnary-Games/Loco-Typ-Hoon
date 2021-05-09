using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateContinuously : MonoBehaviour
{
    public bool rotateX = false;
    
    public bool rotateY = false;

    public bool rotateZ = false;

    public float rotateSpeed = 1.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var rotation = new Vector3(0.0f, 0.0f, 0.0f);
        if (rotateX)
        {
            rotation.x = rotateSpeed * Time.deltaTime;
        }

         if (rotateY)
        {
            rotation.y = rotateSpeed * Time.deltaTime;
        }

         if (rotateZ)
        {
            rotation.z = rotateSpeed * Time.deltaTime;
        }

        transform.Rotate (rotation);
    }
}
