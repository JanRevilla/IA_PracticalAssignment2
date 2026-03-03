using UnityEngine;

public class MiceSpawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject micePrefab;
    private float spawnDelay = 0f;
    private float elapsedTime;
    public float minRange;
    public float maxRange;

    void Start()
    {
        elapsedTime = 0f;
        spawnDelay = Random.Range(minRange, maxRange);
    }

    void Update()
    {
        if (elapsedTime >= spawnDelay)
        {
            GameObject instance = Instantiate(micePrefab);
            instance.transform.position = LocationHelper.RandomEntryExitPoint().transform.position;
            instance.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
            elapsedTime = 0f;
            spawnDelay = Random.Range(minRange, maxRange);

        }
        elapsedTime += Time.deltaTime;
    }
}

