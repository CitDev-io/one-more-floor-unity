using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowSlideRight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SlowSlide();
    }

    void SlowSlide()
    {
        transform.Translate(.005f, 0, 0);
    }
}
