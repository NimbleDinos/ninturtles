using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle: MonoBehaviour
{
    public class Colour {
        public int R;
        public int G;
        public int B;

        public Colour(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }
    }

    public bool LikesCheese;
    public bool IsDumb;
    public Colour TurtleColour;
    public int Cannibalism;
    public int Chonk;
    public bool StrawberryAddict;
    public int Coolness;
    public int Swimming;
    public bool Zombified;
    public int Educaton;

    public float Hunger;
    public bool AtFood = false;
    public float Thirst;
    public bool AtPond = false;
    public bool IsWandering;
    public bool StickOut;

    public float Happiness;
    public bool Breedable;

    public GameObject FoodBowl;
    public GameObject Pond;

    public Turtle P1;
    public Turtle P2;

    public Vector3? WanderPosition;

    public void Start()
    {
        if (P1 != null && P2 != null) TurtleFromParents(P1, P2); else RandomTurtle();
    }

    public void FixedUpdate()
    {
        TurtleThink();
        DecreaseStats();
    }

    public void DecreaseStats()
    {
        Hunger += Hunger >= 0 ? Time.deltaTime * -0.05f : 0;
        Thirst += Thirst >= 0 ? Time.deltaTime * -0.05f : 0;

        CalculateHappiness();
    }

    public void CalculateHappiness()
    {
        switch (Hunger,  Thirst)
        {
            case ( < 25, < 25):
                Happiness += Happiness >= -100 ? Time.deltaTime * -0.3f : 0;
                break;
            case ( < 25, _):
                Happiness += Happiness >= -100 ? Time.deltaTime * -0.01f : 0;
                break;
            case (_, < 25):
                Happiness += Happiness >= -100 ? Time.deltaTime * -0.01f : 0;
                break;
            case ( > 75, > 75):
                Happiness += Happiness <= 100 ? Time.deltaTime * 0.3f : 0;
                break;
            case ( > 75, _):
                Happiness += Happiness <= 100 ? Time.deltaTime * 0.01f : 0;
                break;
            case (_, < 75):
                Happiness += Happiness <= 100 ? Time.deltaTime * 0.01f : 0;
                break;
        }
    }

    public void RandomTurtle() {
        LikesCheese = Random.Range(0f, 1f) > 0.5;
        IsDumb = Random.Range(0f, 1f) > 0.5;
        TurtleColour = new Colour(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
        Cannibalism =Random.Range(0, 100);
        Chonk = Random.Range(0, 100);
        StrawberryAddict = Random.Range(0f, 1f) > 0.5;
        Coolness = Random.Range(0, 100);
        Swimming = Random.Range(0, 100);
        Zombified = Random.Range(0f, 1f) > 0.5;
        Educaton = Random.Range(0, 100);
        Hunger = 100f;
        Thirst = 100f;
        Happiness = 0f;
    }

    public void TurtleFromParents(Turtle Frank, Turtle Derek)
    {
        LikesCheese = Random.Range(0f, 1f) > 0.5 ? Frank.LikesCheese : Derek.LikesCheese;
        IsDumb = Random.Range(0f, 1f) > 0.5 ? Frank.LikesCheese : Derek.LikesCheese;
        TurtleColour = new Colour(Random.Range(Frank.TurtleColour.R, Derek.TurtleColour.R) + Random.Range(-100, 100), 
            Random.Range(Frank.TurtleColour.G, Derek.TurtleColour.G) + Random.Range(-100, 100), 
            Random.Range(Frank.TurtleColour.B, Derek.TurtleColour.B) + Random.Range(-100, 100)
        );
        Cannibalism = Random.Range(Frank.Cannibalism, Derek.Cannibalism) + Random.Range(-20, 20);
        Chonk = Random.Range(Frank.Chonk, Derek.Chonk) + Random.Range(-25, 25);
        StrawberryAddict = Random.Range(0f, 1f) > 0.5 ? Frank.StrawberryAddict : Derek.StrawberryAddict;
        Coolness = Random.Range(Frank.Coolness, Derek.Coolness) + Random.Range(-25, 25);
        Swimming = Random.Range(Frank.Swimming, Derek.Swimming) + Random.Range(-25, 25);
        Zombified = Random.Range(0f, 1f) > 0.5 ? Frank.Zombified : Derek.Zombified;
        Educaton = Random.Range(Frank.Educaton, Derek.Educaton) + Random.Range(-25, 25);
        Hunger = 100f;
        Thirst = 100f;
        Happiness = 25f;
    }

    public void TurtleThink()
    {
        switch((Hunger, AtFood, Thirst, AtPond,  StickOut))
        {
            case ( <= 10, _, _, _, _):
                GoToFood();
                break;
            case ( <= 100, true, _, _, _):
                Hunger += Time.deltaTime * 2.5f;
                break;
            case ( >= 100, true, _, _, _):
                AtFood = false;
                break;
            case (_, _, <= 20, _, _):
                GoToDrink();
                break;
            case (_, _, <= 100, true, _):
                Thirst += Time.deltaTime * 2.5f;
                break;
            case (_, _, >= 100, _, _):
                AtPond = false;
                break;
            case (_, _, _, _, true):
                // go to stick
                break;
            default:
                Wander();
                break;
        }
    }

    public void GoToFood()
    {
        if (!AtFood)
        {
            TurnToPosition(FoodBowl.transform.position);
            transform.position += transform.forward * Time.deltaTime * 0.5f;
        } else
        {
            if (Hunger <= 100f)
            {
                Hunger += Time.deltaTime * 2.5f;
                Debug.Log(Hunger);
            }else
            {
                AtFood = false;
            }
        }
    }

    public void GoToDrink()
    {
        if (!AtPond)
        {
            TurnToPosition(Pond.transform.position);
            transform.position += transform.forward * Time.deltaTime * 0.5f;
        }
        else
        {
            if (Thirst <= 100f)
            {
                Thirst += Time.deltaTime * 2.5f;
                Debug.Log(Hunger);
            }
            else
            {
                AtPond = false;
            }
        }
    }

    public void TurnToPosition(Vector3? AimPosition)
    {
        var LookPos = AimPosition.Value - transform.position;

        LookPos.y = 0;
        var rotation = Quaternion.LookRotation(LookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 0.2f);
    }

    public void Wander()
    {
        Debug.Log(WanderPosition);
        if (WanderPosition != null)
        {
            TurnToPosition(WanderPosition);
            if (Vector3.Distance(transform.position, WanderPosition.Value) < 1)
            {
                WanderPosition = null;
            } else
            {
                transform.position += transform.forward * Time.deltaTime * 0.5f;
            }
        } else
        {
            Vector3 pos = new Vector3(Random.Range(-50, 50), transform.position.y, Random.Range(-50, 50));
            WanderPosition = pos;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "food")
        {
            AtFood = true;
        } else if (collision.gameObject.tag == "pond")
        {
            AtPond = true;
        }
    }
}
