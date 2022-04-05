using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformForce : MonoBehaviour
{

    private Vector3 start_pos;
    private Vector3 start_rot;

    private float dip = 1.0f;
    private bool player_on_platform;
    private GameObject player_ref;
    private bool add_movement;

    public float dip_timer;
    public float force;
    // Start is called before the first frame update
    void Start()
    {
        start_pos = transform.localPosition ;
        start_rot = transform.eulerAngles;
        player_on_platform = false;
        dip = dip_timer;
        add_movement = true;
    }

    private void LateUpdate()
    {
        if(player_on_platform)
        {
            if (dip < 0)
            {
                if (!AlmostEqual(start_pos, transform.localPosition))
                {
                    print("Raise");
                    re_position();
                }
            }
            else
            {
                if (add_movement)
                {
                    add_force();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            player_on_platform = true;
            player_ref = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            player_on_platform = false;
        }
        add_movement = true;
    }

    private void add_force()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, (transform.localPosition.y - force), transform.localPosition.z);
        dip -= Time.deltaTime;
    }

    private void re_position()
    {
        add_movement = false;
        transform.localPosition = new Vector3(transform.localPosition.x, (transform.localPosition.y + force), transform.localPosition.z);
        if (AlmostEqual(start_pos, transform.localPosition))
        {
            dip = dip_timer;
        }
    }

    private bool AlmostEqual(Vector3 v1, Vector3 v2)
    {
        if (Mathf.Abs(v1.x - v2.x) > 0.1) return false;
        if (Mathf.Abs(v1.y - v2.y) > 0.1) return false;
        if (Mathf.Abs(v1.z - v2.z) > 0.1) return false;

        return true;
    }
}
