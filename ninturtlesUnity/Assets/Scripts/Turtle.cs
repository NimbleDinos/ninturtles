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
    public bool AtCheese = false;
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

    public GameObject FoodBowl;
    public GameObject Pond;
    public GameObject Player;

    public Turtle P1;
    public Turtle P2;

    public Vector3? WanderPosition;
    public float MovementSpeed = 1.5f;
    public float TurnSpeed = 1.0f;

    // Textures
    public Texture textureHappy;
    public Texture textureNeutral;
    public Texture textureLike;
    public Texture textureNotLike;
    public Texture textureSad;

    // rendering things
    public GameObject torso;
    public GameObject fins;
    Renderer rend;
    Renderer rendFin;

    // variables needed to assign faces
    public bool cheeseIsNear; // Lottie blease update when cheese is near, or not

    public void Start()
    {
        if (P1 != null && P2 != null) TurtleFromParents(P1, P2); else RandomTurtle();

        // allows texture changes
        rend = torso.GetComponent<Renderer>();
        rendFin = fins.GetComponent<Renderer>();

        Color set = new Color(TurtleColour.R / 255f, TurtleColour.G / 255f, TurtleColour.B / 255f);
        Debug.Log(set);
        rend.material.color = set;
        rendFin.material.color = set;
    }

    public void FixedUpdate()
    {
        TurtleThink();
        DecreaseStats();

        textureUpdate();
    }

    public void textureUpdate()
    {

        // kinda messy but if else statement to decide current face
        if (cheeseIsNear && LikesCheese)
        {
            rend.material.mainTexture = textureLike;
        }
        else if (cheeseIsNear && !LikesCheese)
        {
            rend.material.mainTexture = textureNotLike;
        }
        else if (Happiness <= 1)
        {
            rend.material.mainTexture = textureSad;
        }
        else if (Happiness <= 2)
        {
            rend.material.mainTexture = textureNeutral;
        }
        else
        {
            rend.material.mainTexture = textureHappy;
        }
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
        TurtleColour = new Colour(Random.Range(50, 200), Random.Range(50, 200), Random.Range(50, 200));
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
        switch((Hunger, AtFood, Thirst, AtPond, GlobalVariables.CheeseOut, StickOut))
        {
            case ( <= 10, _, _, _, _, _):
                GoToFood();
                break;
            case ( <= 100, true, _, _, _, _):
                Hunger += Time.deltaTime * 2.5f;
                break;
            case ( >= 100, true, _, _, _, _):
                AtFood = false;
                break;
            case (_, _, <= 20, _, _, _):
                GoToDrink();
                break;
            case (_, _, <= 100, true, _, _):
                Thirst += Time.deltaTime * 2.5f;
                break;
            case (_, _, >= 100, _, _, _):
                AtPond = false;
                break;
            case (_, _, _, _, true, _):
                CheeseTime();
                break;
            case (_, _, _, _, _, true):
                GoToPlayer();
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
            transform.position += transform.forward * Time.deltaTime * MovementSpeed;
        } else
        {
            if (Hunger <= 100f)
            {
                Hunger += Time.deltaTime * 2.5f;
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
            transform.position += transform.forward * Time.deltaTime * MovementSpeed;
        }
        else
        {
            if (Thirst <= 100f)
            {
                Thirst += Time.deltaTime * 2.5f;
            }
            else
            {
                AtPond = false;
            }
        }
    }

    public void GoToPlayer()
    {
        Debug.Log("PLAYER TIME");
        TurnToPosition(Player.transform.position);
        transform.position += transform.forward * Time.deltaTime * MovementSpeed;
    }

    public void TurnToPosition(Vector3? AimPosition)
    {
        var LookPos = AimPosition.Value - transform.position;

        LookPos.y = 0;
        var rotation = Quaternion.LookRotation(LookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * TurnSpeed);
    }

    public void CheeseTime()
    {
        Debug.Log("Turtle Cheese");
        if (LikesCheese)
        {
            if (!AtCheese)
            {
                GameObject cheese = GlobalVariables.Cheeses[0];
                TurnToPosition(cheese.transform.position);
                transform.position += transform.forward * Time.deltaTime * MovementSpeed;
            } else
            {
                Happiness += 10 * Time.deltaTime;
                Hunger += 5 * Time.deltaTime;
            }
        } else
        {
            Happiness += -10 * Time.deltaTime;
            Wander();
        }
    }

    public void Wander()
    {
        if (WanderPosition != null)
        {
            TurnToPosition(WanderPosition);
            if (Vector3.Distance(transform.position, WanderPosition.Value) < 1)
            {
                WanderPosition = null;
            } else
            {
                transform.position += transform.forward * Time.deltaTime * MovementSpeed;
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
        } else if (collision.gameObject.tag == "cheese")
        {
            AtCheese = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "cheese")
        {
            AtCheese = false;
        }
    }

    public void SetStickOut(bool stick)
    {
        StickOut = stick;
    }
}
