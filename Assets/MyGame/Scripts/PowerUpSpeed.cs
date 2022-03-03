using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpeed : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player_character;
    public ParticleSystem attached_particle;
    public float power;
    bool picked_up = false;
    bool emit = false;
    bool emmiting = false;
    public string power_up_type;

    void Start()
    {
        player_character = GameObject.Find("RPG-Character");
        attached_particle = gameObject.transform.Find("P_done").GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if(picked_up)
        {
            Remove();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //handle overlap
        print("picked up");

        if(!picked_up)
        {
            if(power_up_type == "speed")
                player_character.GetComponent<PlayerController>().PowerUp(power, "speed");
            if (power_up_type == "attack")
                player_character.GetComponent<PlayerController>().attack_cooldown -= power;
            picked_up = true;
        }
    }

    private void Remove()
    {

        if (!emit)
        {
            this.transform.localScale = this.transform.localScale * (0.9f);
            if(this.transform.localScale.x < 0.1)
            {
                print("emit");
                emit = true;
            }
        }
        else if(emit && !emmiting)
        {
            emmiting = true;
            attached_particle.Play();
        }

        if(!attached_particle.isEmitting && emmiting)
        {
            StartCoroutine(delete_object());
        }
    }

    IEnumerator delete_object()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
