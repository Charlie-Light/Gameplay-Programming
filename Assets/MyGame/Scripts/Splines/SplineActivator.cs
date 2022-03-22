using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;


public class SplineActivator : MonoBehaviour
{

    public Camera player_cam;
    public Camera spline_cam;
    public GameObject focus_object;
    public GameObject player_ref;

    public PathCreator spline;
    public EndOfPathInstruction spline_end;

    private bool is_active = false;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(is_active)
        {
            MoveCamera();

        }

    }

    private void MoveCamera()
    {

        distance = spline.path.GetClosestDistanceAlongPath(player_ref.transform.position);

        spline_cam.transform.position = spline.path.GetPointAtDistance(distance, spline_end);

        Vector3 player_rot = spline_cam.transform.eulerAngles;
        player_rot.y = spline_cam.transform.eulerAngles.y + 90;
        player_rot.x = player_ref.transform.eulerAngles.x;
        player_rot.z = player_ref.transform.eulerAngles.z;

        player_ref.transform.eulerAngles = player_rot;

        Vector3 cam_pos = spline_cam.transform.localPosition;
        cam_pos.y = player_ref.transform.localPosition.y + 2.5f;

        spline_cam.transform.localPosition = cam_pos;

        spline_cam.transform.LookAt(focus_object.transform);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            var player_script_ref = other.GetComponent<PlayerController>();

            //activate
            print("Spline activate!");
            player_cam.enabled = false;
            spline_cam.enabled = true;

            player_script_ref.camera_input_enabled = false;
            player_script_ref.player_input_horz_enabled = false;
            is_active = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            //deactivate
            var player_script_ref = other.GetComponent<PlayerController>();

            spline_cam.enabled = false;
            player_cam.enabled = true;


            player_script_ref.camera_input_enabled = true ;
            player_script_ref.player_input_horz_enabled = true;
            is_active = false;
        }
    }
}
