using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class BallController : MonoBehaviour
{
    public float speed = 10f;

    void Start()
    {
        // این خط به توپ یک سرعت اولیه میده
        GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1).normalized * speed;

    }

    void Update()
    {
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "WallBottom")
        {
            GameManager.Instance.LoseLife();
            if (GameManager.Instance.lives != 0)
                GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1).normalized * speed;
        }
        if (other.gameObject.tag == "ExtraLife")
        {
            GameManager.Instance.GainLife();
        }

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(1);
        }

    }
}

