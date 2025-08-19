using UnityEngine;

public class PulsateEffect : MonoBehaviour
{
    public GameObject[] objectsToPulsate; // Assign your two GameObjects in the inspector
    public float pulseSpeed = 2f;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;

    private Vector3[] originalScales;

    private void Start()
    {
        // Store the original scales of all objects
        originalScales = new Vector3[objectsToPulsate.Length];
        for (int i = 0; i < objectsToPulsate.Length; i++)
        {
            if (objectsToPulsate[i] != null)
            {
                originalScales[i] = objectsToPulsate[i].transform.localScale;
            }
        }
    }

    private void Update()
    {
        PulsateObjects();
    }

    private void PulsateObjects()
    {
        float pulse = Mathf.PingPong(Time.time * pulseSpeed, 1f);
        float scale = Mathf.Lerp(minScale, maxScale, pulse);

        for (int i = 0; i < objectsToPulsate.Length; i++)
        {
            if (objectsToPulsate[i] != null)
            {
                objectsToPulsate[i].transform.localScale = originalScales[i] * scale;
            }
        }
    }
}