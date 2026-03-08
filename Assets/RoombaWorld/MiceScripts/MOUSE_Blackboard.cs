using UnityEngine;
using Steerings;
using Pathfinding;

public class MOUSE_Blackboard : MonoBehaviour
{
    public GameObject pooPrefab;
    public float roombaDetectionRadius = 50;

    public float initSpeed;
    public float initAcc;

    void Awake()
    {
        pooPrefab = Resources.Load<GameObject>("POO");
    }
}
