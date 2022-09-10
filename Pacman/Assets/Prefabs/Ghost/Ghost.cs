using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{

    public GhostColors ghostColor;
    public Sprite ghostSprite;
    private SpriteRenderer ghostSpriteRenderer;

    public GameObject ghostEyes;
    private SpriteRenderer eyesSprite;

    public float speed = 3.0f;
    public GhostDirections actualDirection;
    public int steps = 0;
    private Vector2 target;

    void Awake()
    {
        ghostSpriteRenderer = this.GetComponent<SpriteRenderer>();
        ghostSpriteRenderer.sprite = ghostSprite;
        ghostSpriteRenderer.size = new Vector2(1, 1);
        eyesSprite = ghostEyes.GetComponent<SpriteRenderer>();

        if (ghostColor == GhostColors.Orange)
        {
            ghostSpriteRenderer.color = new Color(1.0f, 0.6825919f, 0.0f);
        }

        if (ghostColor == GhostColors.Cyan)
        {
            ghostSpriteRenderer.color = Color.cyan;
        }

        if (ghostColor == GhostColors.Red)
        {
            ghostSpriteRenderer.color = Color.red;
        }

        if (ghostColor == GhostColors.Pink)
        {
            ghostSpriteRenderer.color = new Color(1.0f, 0.6273585f, 0.8231753f);
        }
        print(ghostColor);
    }

    void Start()
    {
        target = this.transform.localPosition;
    }

    void Update()
    {
       


    }

    void FixedUpdate()
    {
       GhostBehaviour(ghostColor);
    }
    public void GhostBehaviour(GhostColors color)
    {
        switch (color)
        {
            case GhostColors.Cyan:
                AmbusherAI();
                break;
            case GhostColors.Red:
                ChaserAI();
                break;
            case GhostColors.Orange:
                RandomAI();
                break;
            case GhostColors.Pink:
                AmbusherAI();
                break;
        }
    }

    public void ChaserAI()
    {

    }

    public void AmbusherAI()
    {

    }

    public void pickTarget()
    {
        Vector2 localpos = this.transform.localPosition;
        if (localpos == target)
        {
            Vector2 picked = new Vector2(Random.Range(-14, 14), Random.Range(-17, 14));
            print(picked);
            Collider2D isWall = Physics2D.OverlapBox(picked, new Vector2(1, 1),0.0f,6);
            Debug.DrawLine(new Vector2(picked.x - 0.5f, picked.y), new Vector2(picked.x + 0.5f, picked.y),Color.red);
            print(isWall);
            if (isWall)
            {
                target = localpos;
            }
            else
            {
                target = picked;
            }
        }
    }

    public void RandomAI()
    {
        pickTarget();
        print(target);
    }
    //(-3.42, -8.27, 0.00)

    public bool checkWalls(Vector2 dir)
    {
        print(dir);
        Vector2 localpos = this.transform.localPosition;
        RaycastHit2D isWall = Physics2D.Raycast(localpos, dir, 1.0f);
        if(isWall)
        {
            if (isWall.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                return true;
            }
        }
        return false;
    }
    public enum GhostColors
    {
        Cyan,
        Red,
        Orange,
        Pink
    }

    public enum GhostDirections
    {
        Up,
        Down,
        Left,
        Right
    }
}
