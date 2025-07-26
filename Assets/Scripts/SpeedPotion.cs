using UnityEngine;

public class SpeedPotion : MonoBehaviour
{
    public float speedMultiplier = 2f; // چند برابر سریع‌تر بشه

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity *= speedMultiplier; // افزایش سرعت توپ
            }

            Destroy(gameObject); // معجون حذف میشه
        }
    }
}