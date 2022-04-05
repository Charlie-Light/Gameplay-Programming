using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;


public class SplineActivator : MonoBehaviour
{
    public enum pathState : int
    {
        Merge_to_start = 0,
        following__spline = 1,
        exit = 2,
        NULL = 3,
    }

    pathState current_path_state = pathState.Merge_to_start;
    Vector3 end_pos;
    Vector3 start_pos;

    public Camera player_cam;
    public Camera spline_cam;
    public GameObject focus_object;
    public GameObject player_ref;
    private GameObject transform_point; 

    public VertexPath merge_path;
    public PathCreator spline;
    public EndOfPathInstruction spline_end;

    private bool do_once = true;
    private bool is_active = false;
    private bool delay_timer_active = false;
    private float delay = 1.0f;
    private float distance;
    private Vector3 player_rotation;
    private PlayerController player_script_ref;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(is_active)
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime;
            }
            else
            {
                if(do_once)
                {
                    current_path_state = pathState.Merge_to_start;

                    spline_cam.transform.position = player_cam.transform.position;
                    spline_cam.transform.rotation = player_cam.transform.rotation;

                    //activate
                    print("Spline activate!");
                    player_cam.enabled = false;
                    spline_cam.enabled = true;

                    player_script_ref.camera_input_enabled = false;
                    player_script_ref.player_input_horz_enabled = false;

                    start_pos = spline_cam.transform.position;
                    end_pos = spline.path.GetPointAtDistance(distance, spline_end);
                    end_pos.y = player_ref.transform.localPosition.y + 2.5f;

                    createMergePath(start_pos, end_pos);
                    do_once = false;
                }

                switch (current_path_state)
                {
                    case pathState.Merge_to_start:
                        moveAlongCustomPath();
                        break;

                    case pathState.following__spline:
                        MoveCamera();
                        break;
                }
            }

        }

        if(current_path_state == pathState.exit)
        {
            distance += 0.1f;
            spline_cam.transform.position = merge_path.GetPointAtDistance(distance);

            spline_cam.transform.LookAt(focus_object.transform);

            if (AlmostEqual(spline_cam.transform.position, end_pos))
            {
                print("true");
                current_path_state = pathState.NULL;
                spline_cam.enabled = false;
                player_cam.enabled = true;
                Destroy(transform_point);
                distance = 0.0f;
            }
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
        cam_pos.y = player_ref.transform.position.y + 2.5f;

        spline_cam.transform.localPosition = cam_pos;

        spline_cam.transform.LookAt(focus_object.transform);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            player_script_ref = other.GetComponent<PlayerController>();

            is_active = true;
            delay = 1.0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            //deactivate
            if (delay < 0)
            {
                current_path_state = pathState.exit;
                player_script_ref = other.GetComponent<PlayerController>();


                player_script_ref.camera_input_enabled = true;
                player_script_ref.player_input_horz_enabled = true;

                start_pos = spline_cam.transform.position;
                end_pos = player_cam.transform.position;
                end_pos.y += 2.5f;

                createMergePath(start_pos, end_pos);

            }

            is_active = false;
            do_once = true;
        }
    }

    private void createMergePath(Vector3 vec1, Vector3 vec2)
    {
        Destroy(transform_point);
        distance = spline.path.GetClosestDistanceAlongPath(player_ref.transform.position);

        transform_point = new GameObject();
        var trans = transform_point.GetComponent<Transform>();
        Vector3[] points = { vec1, vec2 };

        BezierPath new_path = new BezierPath(points, true, PathSpace.xyz);

        merge_path = new VertexPath(new_path, trans);
    }

    private void moveAlongCustomPath()
    {
        distance += 0.03f;
        spline_cam.transform.position = merge_path.GetPointAtDistance(distance);

        spline_cam.transform.LookAt(focus_object.transform);

        if (AlmostEqual(spline_cam.transform.position, end_pos))
        {
            print("true");
            current_path_state = pathState.following__spline;
            distance = 0.0f;
        }

        Vector3 player_rot = spline_cam.transform.eulerAngles;
        player_rot.y = spline_cam.transform.eulerAngles.y + 90;
        player_rot.x = player_ref.transform.eulerAngles.x;
        player_rot.z = player_ref.transform.eulerAngles.z;
        player_ref.transform.eulerAngles = player_rot;
    }


    private bool AlmostEqual(Vector3 v1, Vector3 v2)
    {
        if (Mathf.Abs(v1.x - v2.x) > 0.1) return false;
        if (Mathf.Abs(v1.y - v2.y) > 0.1) return false;
        if (Mathf.Abs(v1.z - v2.z) > 0.1) return false;

        return true;
    }
}
