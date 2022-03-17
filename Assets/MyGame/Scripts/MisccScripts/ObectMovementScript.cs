using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObectMovementScript : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Vector3> movement_points;

    public bool is_active = false;
    public bool loop = false;
    public float speed_mod;

    private bool reverse = false;
    private bool move_to_next_point = true;
    private int current_point;


    void Start()
    {
        movement_points[0] = this.transform.localPosition;
        current_point = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(is_active)
        {
            print(current_point);
            print(this.transform.localPosition);
            print(movement_points[current_point]);

            if(move_to_next_point)
            {
                updateMovement();
            }
            flipMovement();
        }
    }

    private void updateMovement()
    {

        float step = speed_mod * Time.deltaTime;
        this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, movement_points[current_point], step);
    }

    private void flipMovement()
    {
        if (AlmostEqual(this.transform.localPosition, movement_points[current_point]))
        {

            if (reverse)
            {
                //is reverseing
                if (current_point == 0)
                {
                    reverse = false;
                }
                else
                {
                    current_point -= 1;
                }
            }
            else
            {
                //is not reverseing 
                if (current_point == movement_points.Count - 1)
                {
                    if (loop)
                    {
                        reverse = true;
                    }
                    else
                    {
                        is_active = false;
                    }
                }
                else
                {
                    current_point += 1;
                }
            }
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
