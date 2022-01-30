using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheese : MonoBehaviour
{
    public float CheeseHealth = 100f;
    private List<GameObject> CollidingTurtles = new();

    public void FixedUpdate()
    {
        if (CheeseHealth <= 0) { Destroy(this.gameObject); GlobalVariables.Cheeses.Remove(this.gameObject); CollidingTurtles.ForEach(turtle => turtle.GetComponent<Turtle>().AtCheese = false); }
    }

    public void OnCollisionStay(Collision collisionInfo)
    {
        Debug.Log("TURTLE HIT CHEESE");
        if (collisionInfo.gameObject.CompareTag("turtle"))
        {
            CheeseHealth += -10;
            if (!CollidingTurtles.Contains(collisionInfo.gameObject))
            {
                CollidingTurtles.Add(collisionInfo.gameObject);
            }
        }
    }
}
