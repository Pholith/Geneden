using Fusion;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Remplace un sprite choisi par un sprite aléatoire dans la liste.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class RandomSpriteOnStart : NetworkBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private List<Sprite> randomSprites;

    [Rpc]
    private void ChangeSpriteRPC(int spriteIndex)
    {
        randomSprites.Sort();
        spriteRenderer.sprite = randomSprites[spriteIndex];
    }
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeSpriteRPC(randomSprites.RandomIndex());
    }
}
