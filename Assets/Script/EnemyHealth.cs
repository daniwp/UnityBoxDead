﻿using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
    {
        public int startingHealth = 100;            // The amount of health the enemy starts the game with.
        public int currentHealth;                   // The current health the enemy has.
        public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.
        public int scoreValue = 10;                 // The amount added to the player's score when the enemy dies.
        public GameObject[] drops;    

        ParticleSystem hitParticles;                // Reference to the particle system that plays when the enemy is damaged.
        CapsuleCollider capsuleCollider;            // Reference to the capsule collider.
        bool isDead;                                // Whether the enemy is dead.
        bool isSinking;                             // Whether the enemy has started sinking through the floor.
        Text text;


        void Awake()
        {
            // Setting up the references.
            hitParticles = GetComponentInChildren<ParticleSystem>();
            capsuleCollider = GetComponent<CapsuleCollider>();

            // Setting the current health when the enemy first spawns.
            currentHealth = startingHealth;
        }

        void Update()
        {
            // If the enemy should be sinking...
            if (isSinking)
            {
                // ... move the enemy down by the sinkSpeed per second.
                transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
            }
        }


        public void TakeDamage(int amount, Vector3 hitPoint)
        {
            // If the enemy is dead...
            if (isDead)
                // ... no need to take damage so exit the function.
                return;

            // Reduce the current health by the amount of damage sustained.
            currentHealth -= amount;

            // Set the position of the particle system to where the hit was sustained.
            //hitParticles.transform.position = hitPoint;

            // And play the particles.
            hitParticles.Play();

            // If the current health is less than or equal to zero...
            if (currentHealth <= 0)
            {
                // ... the enemy is dead.       
                Death();

            }
        }


        void Death()
        {
            DropRoll();
            // The enemy is dead.
            isDead = true;
            GetComponent<CapsuleCollider>().enabled = false;
            StartSinking();
            // Turn the collider into a trigger so shots can pass through it.
            capsuleCollider.isTrigger = true;
            ScoreManager.score += scoreValue;

    }


        public void StartSinking()
        {
            // Find and disable the Nav Mesh Agent.
            GetComponent<NavMeshAgent>().enabled = false;

            // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
            GetComponent<Rigidbody>().isKinematic = true;

            // The enemy should no sink.
            isSinking = true;

            // Increase the score by the enemy's score value.
            //ScoreManager.score += scoreValue;

            // After 2 seconds destory the enemy.
            Destroy(gameObject, 2f);
        }

        void DropRoll()
        {
            if (Random.Range(0,100) <= 20)
            {
                Debug.Log("DROP!");
                Vector3 spawnPos = transform.position;
                spawnPos.y = 1.42f;
                Instantiate(drops[Random.Range(0, 1)], spawnPos, transform.rotation);
            }
        }
    }
