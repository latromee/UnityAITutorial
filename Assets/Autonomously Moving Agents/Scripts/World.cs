using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class World
{
    private static readonly World instance = new World();
    public static GameObject[] hidingSpots;

    static World()
    {
        hidingSpots = GameObject.FindGameObjectsWithTag("hide");
    }

    private World() { }

    public static World Instance { get { return instance;} }

    public GameObject[] GetHidingSpots()
    {
        return hidingSpots;
    }
}

