using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA : MonoBehaviour {

    public GameObject bullet;
    public Transform spawn;
    private float nextFire;

    public GameObject EnemyHitParticle;
    public AudioSource DamageSound;
    private Vector3 InitialPosition;
    
    bool die = false;

    void Start () 
    {
        InitialPosition = transform.position;
        DamageSound = GetComponent<AudioSource>();
    }
	
	void Update () 
    {
        // if the enemy has been touched
        if (die)
        {
            // fall to the ground
            if (transform.position.y > 0.2f)
            {
                transform.Translate(0, -1 * Time.deltaTime, 0);
            }
            else
            {
                transform.Translate(0, 0, 3 * Time.deltaTime);
                Destroy(gameObject, 10.0f); // destroy the enemy ship
            }
        }
        else
        {
            // move towards the player
            if (transform.position.z > (InitialPosition.z - 25))
            {
                transform.Translate(0, 0, 2 * Time.deltaTime);
            }

            // shoot a bullet at a random interval
            if (Time.time > nextFire)
            {
                nextFire = Time.time + Random.Range(2, 6);
                Instantiate(bullet, spawn.position, spawn.rotation);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // if an enemy ship is touched by a player's bolt 
        if (other.tag == "PlayerBullet" && die == false)
        {
            DamageSound.Play(); // play a sound
            Instantiate(EnemyHitParticle, gameObject.transform.position, gameObject.transform.rotation); // emit a particle effect
            Destroy(other.gameObject); // destroy the player's bolt
            die = true;
            GameObject.FindGameObjectWithTag("GameController").SendMessage("EnemyTakeDamage", 100); // notify the game controller
            Instantiate(EnemyHitParticle, gameObject.transform.position, gameObject.transform.rotation); // emit a particle effect
        }
    }
}
