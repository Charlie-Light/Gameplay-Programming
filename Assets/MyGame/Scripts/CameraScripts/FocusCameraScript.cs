using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCameraScript : MonoBehaviour
{

    public Camera player_cam;
    public PlayerController player_character;
    public ObectMovementScript movement_contoller;

    private Camera this_cam;
    private bool is_tacking;
    private bool change_camera;
    public float timer;
    public bool disable_input;

    public GameObject object_to_focus;
    // Start is called before the first frame update
    void Start()
    {
        is_tacking = false;
        change_camera = false;
        this_cam = this.GetComponent<Camera>();
        movement_contoller = GetComponent<ObectMovementScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(is_tacking)
        {
            if (change_camera)
            {
                this_cam.transform.position = player_cam.transform.position;
                movement_contoller.movement_points[0] = this_cam.transform.position = player_cam.transform.position;
                player_cam.enabled = false;
                this_cam.enabled = true;

                change_camera = false;

                if(disable_input)
                {
                    //player_character.player_input_enabled = false;
                    player_character.camera_input_enabled = false;
                }
            }

            //Track camera
            if(timer > 0)
            {
                this_cam.transform.LookAt(object_to_focus.transform.position);
                timer = timer - Time.deltaTime;
            }
            else
            {
                is_tacking = false;
                this_cam.enabled = false;
                player_cam.enabled = true;

                player_character.player_input_enabled = true;
                player_character.camera_input_enabled = true;
                movement_contoller.is_active = false;
            }
        }
    }

    public void StartTracking()
    {
        is_tacking = true;
        change_camera = true;
        movement_contoller.is_active = true;
    }
}
