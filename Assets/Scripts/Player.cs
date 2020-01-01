using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool dead = false;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotateDownSpeed;
    [SerializeField] float rotateUpSpeed;
    //Thrust Configuration
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float ShortJumpMultiplier = 2f;
    [SerializeField] float thrustSpeed = 0;
    [SerializeField] bool jumping = false;
    [SerializeField] bool shortJump = false;
    [SerializeField] bool falling = false;


    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField]ParticleSystem fireParticles;
    [SerializeField] ParticleSystem smokeParticle;


    AudioSource thrustAudioSource;
    AudioSource deathAudioSource;
    bool thrustAudioSourcePlaying = false;

    [SerializeField] Rigidbody2D[] platforms;
    [SerializeField]GameSession gameSession;
    Singleton singleton;
    Rigidbody2D rb;

    public float MoveSpeed
    {
        get {return moveSpeed;} 
        set { moveSpeed = value; }
    }
    public bool Dead
    {
        get { return dead; }
    }
    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        thrustAudioSource = audioSources[0];
        deathAudioSource = audioSources[1];

        rb = GetComponent<Rigidbody2D>();
        gameSession = FindObjectOfType<GameSession>();
        singleton = FindObjectOfType<Singleton>();
    }
    private void Update()
    {
        if (!dead && gameSession.GameRunning)
        {
            GetJumpInput();
            JumpImprovement();
            //SpawnThrustParticles();
        }
        PlaythrustSFX();
    }

    private void PlaythrustSFX()
    {
        if (singleton.SFXEnabled)
        {
            if (rb.velocity.y > 0 && thrustAudioSourcePlaying == false && !dead)
            {
                thrustAudioSource.Play();
                thrustAudioSourcePlaying = true;
            }
            else if (thrustAudioSourcePlaying == true && rb.velocity.y < 0 || dead)
            {
                thrustAudioSourcePlaying = false;
                thrustAudioSource.Stop();
            }
        }
    }

    private void SpawnThrustParticles()
    {
        var fireEmission = fireParticles.emission;
        var smokeEmission = smokeParticle.emission;
        if (rb.velocity.y > 0)
        {
            fireEmission.rateOverTime = 250f;
            smokeEmission.rateOverTime = 25f;
        }
        else
        {
            fireEmission.rateOverTime = 5f;
            smokeEmission.rateOverTime = 100f;
        }
    }

    void FixedUpdate()  
    {
        if (!dead && gameSession.GameRunning)
        {
            MovePlayer();
            RotatePlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Platform" && !dead)
        {
            if (!dead) //Only adds torque when the player collides with the first spike/platform
            {
                rb.AddTorque(500);
            }
            dead = true;
            for (int i = 0; i < 2; i++)
            {
                platforms[i].velocity = Vector2.right * 0;
            }
            ParticleSystem explosion = Instantiate(explosionParticles, transform.position, Quaternion.identity);
            if (singleton.SFXEnabled)
            {
                deathAudioSource.Play();
            }
            gameSession.handleDeath();

        }
    }
    private void JumpImprovement()
    {
        shortJump = false;
        falling = false;
        if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            shortJump = true;
        }
        else if (rb.velocity.y < 0)
        {
            falling = true;
        }
    }
    void GetJumpInput()
    {
        if (Input.GetButton("Jump"))
        {
            jumping = true;
        }
        else
        {
            jumping = false;
        }
    }
    private void RotatePlayer()
    {
        if (rb.velocity.y < 0 && transform.rotation.eulerAngles.z - rotateDownSpeed * Time.deltaTime> 0)
        {
            transform.Rotate(new Vector3(0f, 0f, -rotateDownSpeed) * Time.deltaTime);
        }
        else if (rb.velocity.y > 0 && transform.rotation.eulerAngles.z  < 135)
        {
            transform.Rotate(new Vector3(0f, 0f, +rotateUpSpeed) * Time.deltaTime);
        }
    }
    private void MovePlayer()
    {
        rb.velocity = new Vector2(moveSpeed * Time.deltaTime, rb.velocity.y);
        if (jumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, 1 * thrustSpeed * Time.deltaTime);
        }
        if (falling)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (shortJump)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (ShortJumpMultiplier - 1) * Time.deltaTime;
        }
        for (int i = 0; i < 2; i++)
        {
            platforms[i].velocity = Vector2.right * moveSpeed * Time.deltaTime;
        }
    }
}
