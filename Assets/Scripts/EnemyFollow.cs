using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyFollow : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    public float speedIncrease;
    public GameObject player;

    public TMP_Text wallDistanceText;

    public float wallDistance;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        wallDistance = Mathf.RoundToInt(Vector3.Distance(gameObject.transform.position, player.transform.position));

        if (player.transform.position.z >= 20) 
        {
            rb.AddForce(Vector3.forward * speed);
            gameObject.transform.Translate(new Vector3(0, 0, speed));
        }

        wallDistanceText.text = (wallDistance - 4).ToString() + "m";

    }
}
