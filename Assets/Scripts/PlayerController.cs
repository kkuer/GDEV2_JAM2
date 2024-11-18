using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Globalization;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float baseSpeed;

    public float boostSpeed1x;
    public float boostSpeed2x;
    public float boostSpeed4x;

    public float jumpHeight;
    public bool grounded = true;

    public decimal score;

    private Rigidbody rb;

    public float boost;
    public bool boosting = false;
    public float maxBoost;

    public Slider boostBar;
    public Image boostBarFill;
    public Color boost1x;
    public Color boost2x;
    public Color boost4x;

    public GameLoop gameLoop;

    public GameObject loseText;
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;

    public bool gameActive = true;

    public ParticleSystem boostParticles;
    public GameObject jumpParticles;
    public GameObject boostCollectParticles;

    public GameObject breakParticlesObject;
    public ParticleSystem breakParticles;

    public GameObject WOD_POV;

    public Volume ppfxVolume;
    ChromaticAberration ca;

    public ScreenShake shake;

    public AudioSource boostSFX;
    public AudioSource breakSFX;
    public AudioSource pickupSFX;
    public AudioSource loseSFX;
    public AudioSource jumpSFX;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        ppfxVolume.profile.TryGet(out ca);
        boostBar.maxValue = maxBoost;
        speed = baseSpeed;
        grounded = true;
    }
    private void Update()
    {
        scoreText.text = score.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));

        if (gameObject.transform.position.y <= -2)
        {
            gameActive = false;
        }

        if (!gameActive)
        {
            loseSFX.Play();
            loseText.SetActive(true);
            gameObject.SetActive(false);
            finalScoreText.text = score.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
        }
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (boost > 0f)
            {
                boosting = true;
                ca.intensity.value = 0.8f;
                //shake.TriggerShake();
                if (!boostSFX.isPlaying)
                {
                    boostSFX.Play();
                }
            }
            if (boost <= 0f)
            {
                boosting = false;
                boostSFX.Stop();
                ca.intensity.value = 0.058f;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            boosting = false;
            ca.intensity.value = 0.058f;
            boostSFX.Stop();
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            WOD_POV.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            WOD_POV.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.forward * speed);

        score += (decimal)0.05;

        if (Input.GetAxis("Horizontal") > 0)
        {
            rb.AddForce(Vector3.right * speed);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            rb.AddForce(-Vector3.right * speed);
        }
        if (Input.GetKey(KeyCode.Space) && grounded == true)
        {
            GameObject newJumpParticles = Instantiate(jumpParticles,
                new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f, gameObject.transform.position.z),
                Quaternion.Euler(0, 0, 0));
            newJumpParticles.GetComponent<ParticleSystem>().Play();
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            jumpSFX.Play();
            grounded = false;
        }

        if (boost < 0)
        {
            boostBar.value = 0;
        }
        else if (boost >= 0 && boost < 25)
        {
            boostBarFill.color = boost1x;
        }
        else if (boost >= 25 && boost < 40)
        {
            boostBarFill.color = boost2x;
        }
        else if (boost >= 40)
        {
            boostBarFill.color = boost4x;
        }
        else if (boost > maxBoost && boost >= 40)
        {
            boostBar.value = maxBoost;
        }

        boostBar.value = boost;

        if (boosting)
        {
            if (boost >= 0 && boost < 25)
            {
                speed = boostSpeed1x;
            }
            else if (boost >= 25 && boost < 40)
            {
                speed = boostSpeed2x;
            }
            else if (boost >= 40)
            {
                speed = boostSpeed4x;
            }
            boost = boost - 6f * Time.deltaTime;
            boostParticles.Play();
        }
        else
        {
            speed = baseSpeed;
            boostParticles.Stop();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "breakableObject")
        {
            Destroy(other.gameObject);

            float randomCents = UnityEngine.Random.value;
            float randomDollar = UnityEngine.Random.Range(50, 400);
            decimal randomScore = (decimal)randomDollar + (decimal)randomCents;

            breakSFX.Play();
            GameObject newParticles = Instantiate(breakParticlesObject, other.gameObject.transform.position, Quaternion.identity);
            breakParticles = newParticles.gameObject.GetComponent<ParticleSystem>();
            breakParticles.Play();
            Destroy(newParticles.gameObject, 5f);
            shake.TriggerShake();

            score += randomScore;

        }
        else if (other.gameObject.tag == "floor")
        {
            grounded = true;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "room")
        {
            gameLoop.setRoom(other.gameObject);
        }
        if (other.gameObject.tag == "boost")
        {
            //ParticleSystem boostCollectParticles = other.gameObject.GetComponent<ParticleSystem>();

            GameObject newBoostCollectParticles = Instantiate(boostCollectParticles, other.gameObject.transform.position, Quaternion.identity);
            newBoostCollectParticles.GetComponent<ParticleSystem>().Play();

            Destroy(other.gameObject);

            boost = Mathf.RoundToInt(boost + 5f);

            pickupSFX.Play();

            if (boost > maxBoost)
            {
                boost = maxBoost;
            }
        }
        if (other.gameObject.tag == "wall")
        {
            gameActive = false;
            GameObject newParticles = Instantiate(breakParticlesObject, gameObject.transform.position, Quaternion.identity);
            breakParticles = newParticles.gameObject.GetComponent<ParticleSystem>();
            breakParticles.Play();
            Destroy(newParticles.gameObject, 5f);
            shake.TriggerShake();
            breakSFX.Play();
        }
    }
}
