using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class World
{
    private static readonly World instance = new World();
    public static GameObject[] hidingSpots;

    public static GameObject cop;
    public static GameObject[] robbers;

    static World()
    {
        hidingSpots = GameObject.FindGameObjectsWithTag("hide");

        cop = GameObject.FindGameObjectWithTag("cop");
        robbers = GameObject.FindGameObjectsWithTag("robber");
    }

    private World() { }

    public static World Instance { get { return instance;} }

    public GameObject[] GetHidingSpots()
    {
        return hidingSpots;
    }
}

