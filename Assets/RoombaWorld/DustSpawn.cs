using System.Runtime.InteropServices;
using UnityEngine;

public class DustSpawn : MonoBehaviour
{
    public GameObject dustPrefab;
    public float spawnDelay = 5f;
    private float elapsedTime;

    void Start()
    {
        elapsedTime = 0f;
    }

    void Update()
    {
        if (elapsedTime >= spawnDelay)
        {
            GameObject instance = Instantiate(dustPrefab/*, LocationHelper.RandomWalkableLocation(), Quaternion.identity*/);
            instance.transform.position = LocationHelper.RandomWalkableLocation();
            instance.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
            elapsedTime = 0f;
            Debug.Log("print"); 
        }
        elapsedTime += Time.deltaTime;
    }
}
