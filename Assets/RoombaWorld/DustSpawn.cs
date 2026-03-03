using System.Runtime.InteropServices;
using UnityEngine;

public class DustSpawn : MonoBehaviour
{
    public GameObject dustPrefab;
    public float spawnDelay =5;
    private float elapsedTime;

    void Start()
    {
        elapsedTime = 0f;
        
    }

    void Update()
    {
        if (elapsedTime >= spawnDelay)
        {
            GameObject instance = Instantiate(dustPrefab);
            instance.transform.position = LocationHelper.RandomWalkableLocation();
            instance.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
            elapsedTime = 0f;
            
        }
        elapsedTime += Time.deltaTime;
    }
}
