using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public enum DoorStates : int
    {
        open = 1,
        moving = 2,
        closed = 3,
    }

    private bool player_in_range;
    private GameObject player_character;
    private DoorStates currnet_door_state;

    // Start is called before the first frame update
    void Start()
    {
        player_character = GameObject.Find("RPG-Character");
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
                if (player_in_range)
                {
                    //if player interacts open

                }
                break;
            case DoorStates.moving:
                //door is moving
                break;
            case DoorStates.open:
                //door is open
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        player_in_range = true;
    }

    private void OnTriggerExit(Collider other)
    {
        player_in_range = false;
    }
}
