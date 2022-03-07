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
                if(button.transform.localPosition != button_stop_vec)
                {
                    button.transform.Translate(button_move_dir);
                }
                else
                {
                    currnet_door_state = DoorStates.move_door;
                }

                print(button.transform.position);
                break;

            case DoorStates.move_door:
                //door is opening
                if (door.transform.localPosition != door_stop_vec)
                {
                    door.transform.Translate(door_move_dir);
                }
                else
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
}
