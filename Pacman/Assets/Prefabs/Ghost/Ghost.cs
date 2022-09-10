using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ghost : MonoBehaviour
{

    public GhostColors ghostColor;
    public Sprite ghostSprite;
    private SpriteRenderer ghostSpriteRenderer;

    public GameObject ghostEyes;
    private SpriteRenderer eyesSprite;

    public GhostDirections[] availableDir;
    public GhostDirections actualDirection;
    public Rigidbody2D rgbody;

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

    public GhostDirections[] availableDirection()
    {

        GhostDirections[] dirList = new GhostDirections[4];

        if(!checkWalls(Vector2.down))
        {
            dirList[0] = (GhostDirections.Up);
        }
        if (!checkWalls(Vector2.up))
        {
            dirList[1] = (GhostDirections.Down);
        }
        if (!checkWalls(Vector2.right))
        {
            dirList[2] = (GhostDirections.Right);
        }
        if (!checkWalls(Vector2.left))
        {
            dirList[3] = (GhostDirections.Left);
        }

        return dirList;
    }

    public void pickDirection()
    {
        GhostDirections[] availDir = availableDirection();

        if(!(Enumerable.SequenceEqual(availableDir, availDir)))
        {
            GhostDirections picked = availDir[Random.Range(0, 4)];
            actualDirection = picked;
        }        
    }

    public void RandomAI()
    {
        pickDirection();
        print(actualDirection);
        Vector2 velocity = new Vector2(0,0);
        switch(actualDirection)
        {
            case GhostDirections.Up:
            {
                velocity = Vector2.down;
                break;
            }
            case GhostDirections.Down:
            {
                velocity = Vector2.up;
                break;
            }
            case GhostDirections.Right:
            {
                velocity = Vector2.right;
                break;
            }
            case GhostDirections.Left:
            {
                velocity = Vector2.left;
                break;
            }
        }
        print(this.transform.localPosition);
        rgbody.MovePosition(this.transform.localPosition * velocity * Time.fixedDeltaTime * 1.0f);
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
