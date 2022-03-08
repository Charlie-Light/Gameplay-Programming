using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{

    private float camera_vert_axis = 0.0f;
    private float camera_hor_axis = 0.0f;

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

    public void InitilizeCameraController(float speed_mod, float deadzone, GameObject main_cam, PlayerController Player)
    {
        input_speed_mod = speed_mod;
        input_deadzone = deadzone;
        main_camera = main_cam;
        player_ref = Player;
        player_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        verticle_movement = GameObject.Find("CameraVerticle");
    }

    public void Update()
    {

    }

    public void LateUpdate()
    {
        UpdateCameraMovementInputVert();
        print("Camera Rot: " + verticle_movement.transform.eulerAngles);
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

        Vector3 look_at_pos = new Vector3(player_ref.transform.position.x, player_ref.transform.position.y +2, player_ref.transform.position.z);

        player_camera.transform.LookAt(look_at_pos);
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
        //clamp camera angle
    }
}
