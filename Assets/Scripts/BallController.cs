using UnityEngine;

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
        // اگه توپ از پایین افتاد، بازی تموم شه
        if (transform.position.y < -5f)
        {
            Debug.Log("Game Over!");
        }
    }
    void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.name == "WallBottom")
    {
        Debug.Log("Game Over!");
        // یا بازی رو ریست کن:
        // UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

}
