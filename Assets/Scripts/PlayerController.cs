using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float baseSpeed;
    public float boostSpeed;

    public float score;

    private Rigidbody rb;

    public float boost;
    public bool boosting = false;
    public float maxBoost;

    public Slider boostBar;

    public GameLoop gameLoop;

    public GameObject loseText;
    public TMP_Text scoreText;

    public bool gameActive = true;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        boostBar.maxValue = maxBoost;
        speed = baseSpeed;
    }
    private void Update()
    {
        rb.AddForce(Vector3.forward * speed);

        scoreText.text = score.ToString();

        if (gameObject.transform.position.y <= -5)
        {
            gameActive = false;
        }

        if (!gameActive)
        {
            loseText.SetActive(true);
            gameObject.SetActive(false);
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            rb.AddForce(Vector3.right * speed);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            rb.AddForce(-Vector3.right * speed);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (boost > 0f)
            {
                boosting = true;
            }
            if (boost <= 0f)
            {
                boosting = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            boosting = false;
        }
    }

    private void FixedUpdate()
    {
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
        }
        else
        {
            speed = baseSpeed;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "breakableObject")
        {
            boost = Mathf.RoundToInt(boost + 1f);
            Destroy(other.gameObject);
            if (boost > maxBoost)
            {
                boost = maxBoost;
            }

            score++;

        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "room")
        {
            gameLoop.setRoom(other.gameObject);
        }
        if (other.gameObject.tag == "wall")
        {
            gameActive = false;
        }
    }
}
