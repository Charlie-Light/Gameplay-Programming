using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpin : MonoBehaviour
{

    public Vector3 spin;
    public bool spin_on;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if(spin_on)
        {
            transform.Rotate(spin);
        }
    }
}
