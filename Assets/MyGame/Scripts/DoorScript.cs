using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public enum DoorStates : int
    {
        open = 1,
        moving_button = 2,
        move_door = 3,
        closed = 4,
    }

    public bool player_in_range;
    private GameObject player_character;
    private DoorStates currnet_door_state;

    public Vector3 button_move_dir;
    public Vector3 button_stop_vec;
    public GameObject button;

    public Vector3 door_move_dir;
    public Vector3 door_stop_vec;
    public GameObject door;

    // Start is called before the first frame update
    void Start()
    {
        player_character = GameObject.Find("RPG-Character");
        currnet_door_state = DoorStates.closed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        switch (currnet_door_state)
        {
            case DoorStates.closed:
                //door is closed
                break;

            case DoorStates.moving_button:
                //door is moving
                button.transform.Translate(button_move_dir);
                if(AlmostEqual(button.transform.localPosition, button_stop_vec))
                {
                    currnet_door_state = DoorStates.move_door;
                }


                print(button.transform.position);
                break;

            case DoorStates.move_door:
                //door is opening
                door.transform.Translate(door_move_dir);
                if(AlmostEqual(door_stop_vec, door.transform.localPosition))
                {
                    currnet_door_state = DoorStates.open;
                }
                break;

            case DoorStates.open:
                //door is open
                break;
        }
    }

    public void tiggerDoor()
    {
        if (currnet_door_state == DoorStates.closed)
        {
            //open door
            currnet_door_state = DoorStates.moving_button;
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
