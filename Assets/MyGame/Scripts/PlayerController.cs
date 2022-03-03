﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{

    //input vars
    private float horizontal_axis = 0.0f; private float vertical_axis = 0.0f;
    private float camera_vert_axis = 0.0f; private float camera_hor_axis = 0.0f;
    private float attackL; private float attackR;
    private bool sprint = false;

    //movement / attack vars
    public float speed_modifier = 10.0f; public float camera_speed_mod = 20.0f;
    public float dead_zone = 0.1f;
    public float attack_cooldown = 1.0f;
    private float current_attack_cooldown = 0.0f;
    public int current_attack = 0;
    private bool in_air = false;
    private float distance_to_ground;
    private int jump_count = 0; private float jump_timer;
    ParticleSystem power_up;

    //objects attached to character
    private Animator animation_handeler;
    private Rigidbody rb;
    private Camera main_camera;
    private GameObject camera_holder;
    private GameData game_data;

    void Start()
    {
        main_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        camera_holder = GameObject.Find("CameraCentre");
        animation_handeler = GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        game_data = GameObject.Find("GameManager").GetComponent<GameData>();
        power_up = gameObject.transform.Find("Player_speed_particle").GetComponent<ParticleSystem>();

        Physics.gravity = new Vector3(0, -10, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (current_attack_cooldown > -4)
        {
            current_attack_cooldown -= Time.deltaTime;
        }
        updateVerticleMovement();
    }

    private void FixedUpdate()
    {
        update_movement();
        if(!in_air)
            attack();

        if (jump_timer <= 0)
        {
            if (Input.GetButton("Jump") && jump_count < game_data.jump_count)
            {
                jump_timer = 1f;
                Jump();
            }
            else if(IsGrounded())
            {
                jump_count = 0;
            }
        }
        else
        {
            jump_timer -= Time.deltaTime;
        }
    }

    void Jump()
    {
        in_air = true;
        rb.AddForce(Vector2.up * 50, ForceMode.Impulse);
        animation_handeler.SetBool("Jump", true);
        animation_handeler.SetBool("InAir", false);
        jump_count += 1;
    }

    private void updateVerticleMovement()
    {

        if (IsGrounded() != true)
        {
            if (rb.velocity.y > 1)
            {
                animation_handeler.SetBool("Jump", true);
                if (animation_handeler.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Unarmed-Jump")
                {
                    animation_handeler.SetBool("InAir", true);
                }
                //print("in air");
            }
            else if (rb.velocity.y < -1)
            {
                animation_handeler.SetBool("Jump", false);
                animation_handeler.SetBool("Falling", true);
                animation_handeler.SetBool("InAir", true);
                //print("falling");
            }

        }
        else if (rb.velocity.y < 0.001 && rb.velocity.y > -0.001)
        {
            in_air = false;
            animation_handeler.SetBool("Jump", false);
            animation_handeler.SetBool("Falling", false);
            animation_handeler.SetBool("InAir", false);
            //print("floor");
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 1);
    }

    private void attack()
    {
        attackL = Input.GetAxis("AttackL");
        attackR = Input.GetAxis("AttackR");

        if(animation_handeler.GetCurrentAnimatorClipInfo(1)[0].clip.name == "Unarmed-Idle")
        {
            animation_handeler.SetLayerWeight(1, 0.6f);
        }
        else
        {
            animation_handeler.SetLayerWeight(1, 1f);
        }
     
        if (current_attack_cooldown <= 0 && attackL > 0.1)
        {
            animation_handeler.SetBool("AttackLeft", true);
            current_attack += 1;
            current_attack_cooldown = attack_cooldown;
        }
        else
        {
                animation_handeler.SetInteger("current_attack", current_attack);
                animation_handeler.SetBool("AttackLeft", false);
        }

        if(current_attack_cooldown < -1 || current_attack >= 4)
        {
            animation_handeler.SetInteger("current_attack", 0);
            current_attack = 0;
        }
    }

    private void update_movement()
    {
        horizontal_axis = Input.GetAxis("Horizontal");
        vertical_axis = Input.GetAxis("Vertical");
        camera_hor_axis = Input.GetAxis("Camera_horizontal");
        camera_vert_axis = Input.GetAxis("Camera_verticle");
        sprint = Input.GetButton("Sprint");

        if (Math.Abs(horizontal_axis) < dead_zone)
            horizontal_axis = 0;
        if (Math.Abs(vertical_axis) < dead_zone)
            vertical_axis = 0;
        if (Math.Abs(camera_hor_axis) < dead_zone)
            camera_hor_axis = 0;
        if (Math.Abs(camera_vert_axis) < dead_zone)
            camera_vert_axis = 0;

        animation_handeler.SetFloat("Strafe", horizontal_axis);
        animation_handeler.SetFloat("Forward", vertical_axis);

        if (Math.Abs(horizontal_axis) > 0 || Math.Abs(vertical_axis) > 0)
        {
            if(sprint && IsGrounded() && vertical_axis > 0)
            {
                animation_handeler.SetBool("Sprint", true);
                vertical_axis = vertical_axis * 2;
            }
            else
            {
                animation_handeler.SetBool("Sprint", false);
            }
            transform.Translate(new Vector3(horizontal_axis, 0, vertical_axis) * speed_modifier * Time.deltaTime);
        }

        if(Math.Abs(camera_hor_axis) > 0)
        {
                transform.Rotate(new Vector3(0, camera_hor_axis, 0) * (camera_speed_mod) * Time.deltaTime);
        }

        if(Math.Abs(camera_vert_axis) > 0)
        {
            if (camera_holder.transform.localRotation.x > -0.2 && camera_holder.transform.localRotation.x < 0.5)
            {
                camera_holder.transform.Rotate(new Vector3(camera_vert_axis, 0, 0) * (camera_speed_mod) * Time.deltaTime);
            }
            else if(camera_holder.transform.localRotation.x < -0.2)
            {
                camera_holder.transform.Rotate(new Vector3(0.06f, 0, 0) * (camera_speed_mod) * Time.deltaTime);
            }
            else if(camera_holder.transform.localRotation.x > 0.5)
            {
                camera_holder.transform.Rotate(new Vector3(-0.06f, 0, 0) * (camera_speed_mod) * Time.deltaTime);
            }
        }
    }

    public void PowerUp(float value, string type)
    {
        if (type == "speed")
        {
            StartCoroutine(powerUpSpeed(value));
        }
    }

    IEnumerator powerUpSpeed(float value)
    {
        animation_handeler.speed += (value / 10);
        speed_modifier += value;
        power_up.Play();

        yield return new WaitForSeconds(5);

        animation_handeler.speed -= (value / 10);
        speed_modifier -= value;
        power_up.Stop();
    }
}
