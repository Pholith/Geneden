using Fusion;
using UnityEngine;

public class Meteor : NetworkBehaviour
{
    [SerializeField]
    private NetworkPrefabRef explosionPrefab;

    [SerializeField]
    private Vector2 velocity = new Vector2(10, 5);

    [SerializeField]
    private Vector2 startPositionOffset = new Vector2(-30, 30);

    private float startY;

    private void Start()
    {
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        startY = transform.position.y;
        transform.position = new Vector3(transform.position.x + startPositionOffset.x, transform.position.y + startPositionOffset.y, transform.position.z);
        rigidbody2D.velocity = velocity;
    }

    private void Update()
    {
        if (transform.position.y < startY) Explode();
    }

    private void Explode()
    {
        GameManager.Instance.Runner?.Spawn(explosionPrefab, transform.position);
        GameManager.ElementManager.ShakeScreenRPC(0.5f, 0.3f);

        Destroy(gameObject);
    }
}
