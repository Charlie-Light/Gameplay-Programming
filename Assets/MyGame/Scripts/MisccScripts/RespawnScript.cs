using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public Vector3 spawn_point;
    public List<GameObject> collidable_objects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (GameObject x in collidable_objects)
        {
            if (other.gameObject == x)
            {
                print("back to spawn!");
                x.transform.position = spawn_point;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
