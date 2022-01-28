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

    public int Hunger;
    public int Thirst;
    public bool IsWandering;
    public bool StickOut;

    public GameObject FoodBowl;

    public Turtle P1;
    public Turtle P2;

    public void Start()
    {
        if (P1 != null && P2 != null) TurtleFromParents(P1, P2); else RandomTurtle();
    }

    public void FixedUpdate()
    {
        TurtleThink();
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
        Hunger = 100;
        Thirst = 100; 
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
        Hunger = 100;
        Thirst = 100;
    }

    public void TurtleThink()
    {
        switch((Hunger, Thirst, StickOut))
        {
            case ( <= 10, _, _):
                GoToFood();
                break;
            case (_, <= 20, _):
                // go drink
                break;
            case (_, _, true):
                // go to stick
                break;
            default:
                // wonder
                break;
        }
    }

    public void GoToFood()
    {
        Transform FoodBowlTransform = FoodBowl.transform;

        var LookPos = FoodBowlTransform.position - transform.position;

        Turn();

        void Turn()
        {
            LookPos.y = 0;
            var rotation = Quaternion.LookRotation(LookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1);
        }

        transform.position += Vector3.forward * Time.deltaTime * 2f;
    }
}
