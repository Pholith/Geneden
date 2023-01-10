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

        var resources = GameManager.GridManager.GetResourcesInRange(transform.position.ToVector3Int(), 6);
        foreach (var resource in resources)
        {
            var buildingComponent = resource.GetComponent<BuildingGeneric>();
            if (buildingComponent != null) buildingComponent.Damage(ElementsManager.DAMAGE_HIGH);
            else Destroy(resource.gameObject);
        }

        GameManager.ElementManager.ShakeScreenRPC(0.5f, 0.3f);

        Destroy(gameObject);
    }
}
