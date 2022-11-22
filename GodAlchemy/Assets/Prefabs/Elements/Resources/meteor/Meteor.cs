using Fusion;
using UnityEngine;

public class Meteor : NetworkBehaviour
{
    [SerializeField]
    private NetworkPrefabRef explosionPrefab;

    private float startY;

    private void Start()
    {
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        startY = transform.position.y;
        transform.position = new Vector3(transform.position.x - 45, transform.position.y + 10, transform.position.z);
        rigidbody2D.AddForce(new Vector2(10, 20), ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (transform.position.y < startY) Explode();
    }

    private void Explode()
    {
        GameManager.Instance.Runner?.Spawn(explosionPrefab, transform.position);
        Destroy(gameObject);
    }
}
