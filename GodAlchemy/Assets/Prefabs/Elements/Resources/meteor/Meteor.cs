using Fusion;
using UnityEngine;

public class Meteor : NetworkBehaviour
{
    [SerializeField]
    private NetworkPrefabRef explosionPrefab;

    private void Explode()
    {
        GameManager.Instance.Runner?.Spawn(explosionPrefab, transform.position);
        Destroy(gameObject);
    }
}
