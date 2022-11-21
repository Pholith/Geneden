using Fusion;
using System;
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

    private void ChangeSpriteRPC(int spriteIndex)
    {
        randomSprites.Sort(new Comparison<Sprite>((sprite1, sprite2) => sprite1.name.CompareTo(sprite2.name)));
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = randomSprites[spriteIndex];     
    }

    private void Start()
    {
        // Seulement le serveur fait un random pour demander à tous les clients de changer le sprite.
        if (HasStateAuthority) ChangeSpriteRPC(randomSprites.RandomIndex());
    }

}
