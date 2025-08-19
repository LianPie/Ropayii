using UnityEngine;

public class PotionSpawner : MonoBehaviour
{
    public GameObject speedPotionPrefab;
    public GameObject lifePotionPrefab;
    public float spawnInterval = 10f;
    public Vector2 spawnAreaMin = new Vector2(-3.5f, -2f);
    public Vector2 spawnAreaMax = new Vector2(3.5f, 2.5f);

    private float timer;
    private GameObject[] potionsInScene;

    private void Start()
    {
        timer = spawnInterval; // Start counting down immediately
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Check if there are any potions in the scene
            potionsInScene = GameObject.FindGameObjectsWithTag("potion"); // Make sure your potions have the "Potion" tag
            potionsInScene = GameObject.FindGameObjectsWithTag("ExtraLife"); // Make sure your potions have the "Potion" tag

            if (potionsInScene.Length == 0)
            {
                SpawnRandomPotion();
            }

            timer = spawnInterval; // Reset timer regardless of whether we spawned or not
        }
    }

    private void SpawnRandomPotion()
    {
        // Choose a random position within the defined area
        Vector2 spawnPosition = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        // Randomly choose which potion to spawn
        GameObject potionToSpawn = Random.Range(0, 2) == 0 ? speedPotionPrefab : lifePotionPrefab;

        // Instantiate the chosen potion
        Instantiate(potionToSpawn, spawnPosition, Quaternion.identity);
    }
}
