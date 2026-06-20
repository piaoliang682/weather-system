using UnityEngine;

public class JitterAnimation : MonoBehaviour
{
    public enum JitterType
    {
        LeftRight,
        UpDown,
        ForwardBack,
        Circle,
        Random
    }

    [Header("Jitter Settings")]
    public JitterType type = JitterType.LeftRight;
    public float amplitude = 0.1f;
    public float speed = 10f;

    private Vector3 startLocalPos;

    void Start()
    {
        startLocalPos = transform.localPosition;
    }

    void Update()
    {
        Vector3 offset = GetOffset(Time.time * speed);
        transform.localPosition = startLocalPos + offset;
    }

    Vector3 GetOffset(float t)
    {
        switch (type)
        {
            case JitterType.LeftRight:
                return transform.right * Mathf.Sin(t) * amplitude;

            case JitterType.UpDown:
                return transform.up * Mathf.Sin(t) * amplitude;

            case JitterType.ForwardBack:
                return transform.forward * Mathf.Sin(t) * amplitude;

            case JitterType.Circle:
                float x = Mathf.Cos(t);
                float y = Mathf.Sin(t);
                return (transform.right * x + transform.up * y) * amplitude;

            case JitterType.Random:
                return new Vector3(
                    Mathf.PerlinNoise(t, 0f) - 0.5f,
                    Mathf.PerlinNoise(0f, t) - 0.5f,
                    Mathf.PerlinNoise(t, t) - 0.5f
                ) * amplitude;

            default:
                return Vector3.zero;
        }
    }
}