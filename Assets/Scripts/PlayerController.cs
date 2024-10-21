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
    public float boostSpeed;

    public decimal score;

    private Rigidbody rb;

    public float boost;
    public bool boosting = false;
    public float maxBoost;

    public Slider boostBar;

    public GameLoop gameLoop;

    public GameObject loseText;
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;

    public bool gameActive = true;

    public ParticleSystem boostParticles;

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

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        ppfxVolume.profile.TryGet(out ca);
        boostBar.maxValue = maxBoost;
        speed = baseSpeed;
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
        
        if (Input.GetKey(KeyCode.Space))
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
        if (Input.GetKeyUp(KeyCode.Space))
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

        if (Input.GetAxis("Horizontal") > 0)
        {
            rb.AddForce(Vector3.right * speed);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            rb.AddForce(-Vector3.right * speed);
        }

        if (boost < 0)
        {
            boostBar.value = 0;
        }
        else if (boost > maxBoost)
        {
            boostBar.value = maxBoost;
        }

        boostBar.value = boost;

        if (boosting)
        {
            speed = boostSpeed;
            boost = boost - 1f * Time.deltaTime;
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
            //boost = Mathf.RoundToInt(boost + 1f);
            Destroy(other.gameObject);
            //if (boost > maxBoost)
            //{
            //    boost = maxBoost;
            //}

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

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "room")
        {
            gameLoop.setRoom(other.gameObject);
        }
        if (other.gameObject.tag == "boost")
        {
            Destroy(other.gameObject);

            boost = Mathf.RoundToInt(boost + 1f);

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
