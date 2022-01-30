using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public Camera cam;
    public bool HoldingItem;
    public Transform Destination;
    public GameObject Stick;
    public List<Turtle> turtles;

    public GameObject CheesePrefab;

    public void Update()
    {
        GlobalVariables.CheeseOut = GlobalVariables.Cheeses.Count > 0;
        if (Input.GetMouseButtonDown(0))
        {
            if (!HoldingItem)
            {
                Debug.Log("MOUSE");
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 10))
                {
                    if (hit.transform.tag == "stick")
                    {
                        HoldingItem = true;
                        Stick.GetComponent<Rigidbody>().useGravity = false;
                        Stick.GetComponent<Rigidbody>().isKinematic = true;
                        Stick.transform.position = Destination.position;
                        Stick.transform.parent = Destination;
                        Stick.transform.localRotation = Quaternion.Euler(90, 0, -90);
                        Debug.Log("STICK");

                        turtles.ForEach(turtle => turtle.SetStickOut(true));
                    }

                }
            } 
            else {
                Stick.GetComponent<Rigidbody>().useGravity = true;
                Stick.GetComponent<Rigidbody>().isKinematic = false;
                HoldingItem = false;
                Stick.transform.parent = null;

                turtles.ForEach(turtle => turtle.SetStickOut(false));
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Cheese time");
            GameObject cheese = Instantiate(CheesePrefab);
            GlobalVariables.Cheeses.Add(cheese);
            GlobalVariables.CheeseOut = true;
            cheese.transform.position = transform.position;
            cheese.GetComponent<Rigidbody>().AddForce(cam.transform.forward * Random.Range(0.5f, 2f), ForceMode.Impulse);
        }
    }
}