using UnityEngine;

public class CarMove : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [Tooltip("Seconds until this object destroys itself. 0 or negative = never auto-destroy.")]
    [SerializeField] float lifetime = 5f;

    void Start()
    {
        if (lifetime > 0f)
        {
            Destroy(gameObject, lifetime);
        }
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
