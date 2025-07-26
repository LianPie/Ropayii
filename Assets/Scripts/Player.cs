using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [Space]
    [SerializeField] private TouchSlider touchSlider;
    [SerializeField] private GameObject paddle;

    private bool isPointerDown;
    private Vector2 targetPosition;
    private Camera mainCamera;
    private float paddleHalfWidth;
    private float screenMinX;
    private float screenMaxX;

    private void Start()
    {
        mainCamera = Camera.main;

        if (paddle != null)
        {
            paddleHalfWidth = paddle.GetComponent<SpriteRenderer>().bounds.extents.x;
        }
        else
        {
            Debug.LogError("Paddle reference is missing!");
        }

        if (touchSlider != null)
        {
            touchSlider.OnPointerDownEvent += OnPointerDown;
            touchSlider.OnPointerUpEvent += OnPointerUp;
            touchSlider.OnPointerDragEvent += OnPointerDrag;
        }
        else
        {
            Debug.LogError("TouchSlider reference is missing!");
        }

        // Calculate screen bounds once
        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(Vector2.zero);
        Vector2 topRight = mainCamera.ViewportToWorldPoint(Vector2.one);
        screenMinX = bottomLeft.x + paddleHalfWidth;
        screenMaxX = topRight.x - paddleHalfWidth;

        targetPosition = paddle.transform.position;
    }

    private void Update()
    {
        if (isPointerDown && paddle != null)
        {
            paddle.transform.position = Vector2.Lerp(
                paddle.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime);
        }
    }

    private void OnPointerDown()
    {
        isPointerDown = true;
    }

    private void OnPointerUp()
    {
        isPointerDown = false;
    }

    private void OnPointerDrag(float sliderValue)
    {
        if (paddle == null) return;

        // Convert slider value (0-1) to world position
        float screenX = Mathf.Lerp(screenMinX, screenMaxX, sliderValue);
        targetPosition = new Vector2(screenX, paddle.transform.position.y);
    }

    private void OnDestroy()
    {
        if (touchSlider != null)
        {
            touchSlider.OnPointerDownEvent -= OnPointerDown;
            touchSlider.OnPointerUpEvent -= OnPointerUp;
            touchSlider.OnPointerDragEvent -= OnPointerDrag;
        }
    }
}