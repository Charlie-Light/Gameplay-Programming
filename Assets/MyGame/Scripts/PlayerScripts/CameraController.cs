using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{

    private float camera_vert_axis = 0.0f;
    private float camera_hor_axis = 0.0f;
    private float camera_distance;
    private bool zooming;
    private float zoom_speed = 10.0f;
    private float distance_cam_to_player;

    private float input_speed_mod;
    private float input_deadzone;

    private GameObject main_camera;
    private PlayerController player_ref;
    private Camera player_camera;
    private GameObject verticle_movement;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void InitilizeCameraController(float speed_mod, float deadzone, GameObject main_cam, PlayerController Player, float cam_dist)
    {
        input_speed_mod = speed_mod;
        input_deadzone = deadzone;
        main_camera = main_cam;
        player_ref = Player;
        player_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        verticle_movement = GameObject.Find("CameraVerticle");
        camera_distance = cam_dist;
    }

    public void Update()
    {

    }

    public void LateUpdate()
    {
        UpdateCameraMovementInputVert();
        UpdateDistanceToPlayer();
        //print("Camera Rot: " + verticle_movement.transform.eulerAngles);
    }

    private void UpdateCameraMovementInputVert()
    {
        camera_vert_axis = Input.GetAxis("Camera_verticle");

        if (Math.Abs(camera_vert_axis) < input_deadzone)
            camera_vert_axis = 0;


        if (Math.Abs(camera_vert_axis) > 0)
        {
            verticle_movement.transform.Rotate(new Vector3(camera_vert_axis, 0, 0) * (input_speed_mod) * Time.deltaTime);
        }

        Vector3 look_at_pos = new Vector3(player_ref.transform.position.x, player_ref.transform.position.y + 2, player_ref.transform.position.z);

        player_camera.transform.LookAt(look_at_pos);

        var current_rot = verticle_movement.transform.eulerAngles;
        if (current_rot.x > 60 && current_rot.x < 200)
        {
            current_rot.x = 59.9f;
            verticle_movement.transform.eulerAngles = current_rot;
        }
        else if (current_rot.x < 310 && current_rot.x > 70)
        {
            current_rot.x = 310.1f;
            verticle_movement.transform.eulerAngles = current_rot;
        }
    }

    public void UpdateCameraMovementHorz(bool stationary)
    {
        camera_hor_axis = Input.GetAxis("Camera_horizontal");

        if (Math.Abs(camera_hor_axis) < input_deadzone)
            camera_hor_axis = 0;

        if (stationary)
        {
            if (Math.Abs(camera_hor_axis) > 0)
            {
                main_camera.transform.Rotate(new Vector3(0, camera_hor_axis, 0) * (input_speed_mod) * Time.deltaTime);
            }
        }
        else
        {
            //rotate player to camera forward
            var player_rot = player_ref.transform.eulerAngles;
            player_rot.y = main_camera.transform.eulerAngles.y;
            player_ref.transform.eulerAngles = player_rot;


            //rotate camera to forward (needs to be smoothed)
            var camera_rot = main_camera.transform.eulerAngles;
            camera_rot.y = player_ref.transform.eulerAngles.y;
            main_camera.transform.eulerAngles = camera_rot;

        }
    }

    private void UpdateDistanceToPlayer()
    {
        distance_cam_to_player = Vector3.Distance(player_ref.transform.position, player_camera.transform.position);
        RaycastHit hit;

        bool ray_collided = Physics.Raycast(player_ref.transform.position + new Vector3(0, 2, 0), -player_camera.transform.TransformDirection(Vector3.forward), out hit, distance_cam_to_player);
        Debug.DrawRay(player_ref.transform.position + new Vector3(0, 2, 0), -player_camera.transform.TransformDirection(Vector3.forward) * 1000.0f, Color.yellow);

        if (ray_collided)
        {
            if (hit.collider.GetComponent<PlayerController>() == null)
            {
                zooming = true;
            }
        }
        else
        {
            zooming = false;
        }

        if(zooming && distance_cam_to_player > 2.5)
        {
            player_camera.transform.Translate(new Vector3(0,0, zoom_speed/100));
        }
        else
        {
            if(camera_distance/10 > distance_cam_to_player)
            {
                player_camera.transform.Translate(new Vector3(0, 0, -zoom_speed / 100));
            }
        }
    }
}
