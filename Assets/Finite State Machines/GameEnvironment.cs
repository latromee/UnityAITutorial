using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public sealed class GameEnvironment : MonoBehaviour
{
    private static GameEnvironment I;
    private List<GameObject> checkpoints = new List<GameObject>();
    private GameObject safeZone;
    public List<GameObject> Checkpoints { get { return checkpoints; } }
    public GameObject SafeZone { get { return safeZone; } }

    public static GameEnvironment Singleton
    {
        get { if (I == null) {
                I = new GameEnvironment();
                I.checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
                I.checkpoints = I.checkpoints.OrderBy(poin => poin.name).ToList();
                I.safeZone = GameObject.FindGameObjectWithTag("Safe");
            } 
        return I; 
        }
    }
}