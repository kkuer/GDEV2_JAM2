using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : MonoBehaviour
{
    public GameObject shatteredWall;
    public float shatterBoost;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "player")
        {
            GameObject newWall = Instantiate(shatteredWall, transform.position, Quaternion.Euler(0f, 90f, 0f));
            Destroy(gameObject);
            Destroy(newWall, 5f);

            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.forward * shatterBoost, ForceMode.Impulse);
        }
    }
}
