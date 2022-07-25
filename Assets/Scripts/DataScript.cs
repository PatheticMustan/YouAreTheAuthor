using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataScript
{
    public static bool initialized = false;

    // Progression.
    public static int currentDay = 0;
    public static float money = 0;
    public static float moneyPerChar = 0.17f;
    public static float billsCost = 25.0f; // should modify bills to increase difficulty?
    public static int wordsCompleted;
    public static int charactersTyped;

    public static Upgrade[] upgradesInfo = new Upgrade[]{
        new Upgrade {
            name = "Money Rain",
            description = "For the next round you get 1.5x money.",
            cost = 50.0f,
            incrementPerPurchase = 20.0f,
            id = "moneyMultiplier",
            stackable = false
        },

        new Upgrade {
            name = "Time Increase",
            description = "You get 30 more seconds to type words.",
            cost = 30.0f,
            incrementPerPurchase = 10.0f,
            id = "timeIncrease",
            stackable = false
        }
    };
}





// custom types
public class Upgrade
{
    public string name;
    public string description;
    public float cost;
    public float incrementPerPurchase;
    public string id;
    public bool stackable;
    public int value;

    public override string ToString()
    {
        return
            name + ", " +
            description + ", " +
            cost + ", " +
            incrementPerPurchase + ", " +
            id + ", " +
            stackable + ", " +
            value;
    }
}
