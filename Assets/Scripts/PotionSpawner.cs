using UnityEngine;

public class PotionSpawner : MonoBehaviour
{
    public GameObject speedPotionPrefab;
    public float spawnInterval = 10f; // هر چند ثانیه یکبار ظاهر بشه
    public Vector2 spawnAreaMin = new Vector2(-3.5f, -2f);
    public Vector2 spawnAreaMax = new Vector2(3.5f, 2.5f);

    void Start()
    {
        InvokeRepeating("SpawnPotion", 2f, spawnInterval); // از ثانیه ۲ شروع بشه، هر ۱۰ ثانیه تکرار شه
    }

    void SpawnPotion()
    {
        Vector2 spawnPos = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        Instantiate(speedPotionPrefab, spawnPos, Quaternion.identity);
    }
}