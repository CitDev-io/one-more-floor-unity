using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowRotateZ : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SlowRotate();
    }

    void SlowRotate()
    {
        transform.Rotate(0, 0, -.006f);
    }
}
