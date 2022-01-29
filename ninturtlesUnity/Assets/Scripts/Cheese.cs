using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheese : MonoBehaviour
{
    public float CheeseHealth = 100f;

    public void FixedUpdate()
    {
        if (CheeseHealth <= 0) Destroy(this.gameObject);
    }

    public void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "turtle")
        {
            CheeseHealth += -10 * Time.deltaTime;
        }
    }
}
