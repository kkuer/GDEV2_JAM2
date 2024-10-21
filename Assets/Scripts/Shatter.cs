using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : MonoBehaviour
{
    public GameObject shattered;
    public float extraBoost;
    public Vector3 rotationAmount;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "player")
        {
            GameObject newWall = Instantiate(shattered, transform.position, Quaternion.Euler(rotationAmount));
            Destroy(gameObject);
            Destroy(newWall, 5f);

            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.forward * extraBoost, ForceMode.Impulse);
        }
    }
}
