﻿using System.Collections;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public int damagePerShot = 20;                  // The damage inflicted by each bullet.
    public float timeBetweenBullets = 0.15f;        // The time between each shot.
    public float range = 100f;                      // The distance the gun can fire.
    public int totalAmmo;
    public int startAmmo;
    public int clipSize;
    public float reloadTime;

    int currentClipAmount;
    static bool reloading = false;
    float timer;                                    // A timer to determine when to fire.
    Ray shootRay;                                   // A ray from the gun end forwards.
    RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
    int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
    int obstacleMask;
    ParticleSystem gunParticles;                    // Reference to the particle system.
    float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.
    LineRenderer shotLine;
    int score = 0;
    AudioSource[] sounds;
    AudioSource noAmmoSound;
    AudioSource shotSound;




    void Awake()
    {
        // Create a layer mask for the Shootable layer.
        shootableMask = LayerMask.GetMask("Shootable");
        obstacleMask = LayerMask.GetMask("Obstacle");
        gunParticles = GetComponent<ParticleSystem>();
        shotLine = GetComponent<LineRenderer>();
        sounds = GetComponents<AudioSource>();
        noAmmoSound = sounds[0];
        shotSound = sounds[1];
        currentClipAmount = clipSize;
        startAmmo = totalAmmo;
        totalAmmo -= clipSize;
    }

    void FixedUpdate()
    {
        Debug.Log(currentClipAmount);
    }

    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R) && totalAmmo != 0 && currentClipAmount != clipSize && !reloading)
        {
            if (totalAmmo >= clipSize || totalAmmo < 0 )
            {
                totalAmmo -= clipSize;
                currentClipAmount = clipSize;
            }
            else if (totalAmmo < clipSize && totalAmmo > 0)
            {
                currentClipAmount = totalAmmo;
                totalAmmo = 0;
            }
            StartCoroutine(reloadWait());
        }

        // If the Fire1 button is being press and it's time to fire...
        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && currentClipAmount != 0 && !reloading)
        {
            // ... shoot the gun.
            Shoot();
        } else if(Input.GetButtonDown("Fire1") && currentClipAmount <= 0 && !reloading)
        {
            noAmmoSound.Stop();
            noAmmoSound.loop = true;
            noAmmoSound.Play();

        } else if (Input.GetButtonUp("Fire1")) {
            noAmmoSound.loop = false;
        }

        // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            // ... disable the effects.
            DisableEffects();
        }
    }

    public void DisableEffects()
    {
        // Disable the line renderer and the light.
        shotLine.enabled = false;
    }

    public IEnumerator reloadWait()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
    }

    void Shoot()
    {
        currentClipAmount -= 1;

        // Reset the timer.
        timer = 0f;


        // Stop the particles from playing if they were, then start the particles.
        gunParticles.Stop();
        gunParticles.Play();

        //shotSound.Stop();
        shotSound.Play();


        // Enable the line renderer and set it's first position to be the end of the gun.
        //shotLine.enabled = true;
        //shotLine.SetPosition(0, transform.position);

        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        if (Physics.Raycast(shootRay, out shootHit, range, obstacleMask))
        {
            // Set the second position of the line renderer to the point the raycast hit.
            shotLine.SetPosition(1, shootHit.point);

        }
        else if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            // Try and find an EnemyHealth script on the gameobject hit.
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

            // If the EnemyHealth component exist...
            if (enemyHealth != null)
            {
                // ... the enemy should take damage.
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);

            }

            // Set the second position of the line renderer to the point the raycast hit.
            shotLine.SetPosition(1, shootHit.point);
        }  else
        {
            // ... set the second position of the line renderer to the fullest extent of the gun's range.
            shotLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }

    public int getCurrentClipAmount()
    {
        return currentClipAmount;
    }
    public int getTotalAmmo()
    {
        return totalAmmo;
    }

    public bool getReloading()
    {
        return reloading;
    }

    public void resetAmmo()
    {
        totalAmmo = startAmmo;
    }
}