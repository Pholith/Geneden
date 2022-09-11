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

    public List<GhostDirections> availableDir;
    public GhostDirections actualDirection;
    public Animator animator;
    public Rigidbody2D rgbody;
    public float timer = 0.0f;
    private bool has_change_dir;

    
    public float speed;

    public bool isDebuff = false;
    public float debuffTime = 0.0f;
    public float deathTimer = 0.0f;

    public int point = 200;

    public Vector3 initial_pos;

    void Awake()
    {
        ghostSpriteRenderer = this.GetComponent<SpriteRenderer>();
        ghostSpriteRenderer.sprite = ghostSprite;
        ghostSpriteRenderer.size = new Vector2(1, 1);
        eyesSprite = ghostEyes.GetComponent<SpriteRenderer>();
        choseColor(ghostColor);
    }



    void Start()
    {
        this.initial_pos = this.transform.localPosition;
    }

    void Update()
    {
        if(deathTimer <= 0)
        {
            this.ghostSpriteRenderer.enabled = true;
            this.GetComponent<BoxCollider2D>().enabled = true;
            this.ghostEyes.SetActive(true);

            if (debuffTime <= 0)
            {
                isDebuff = false;
            }
            animator.SetBool("isVulnerable", isDebuff);
            if (isDebuff)
            {
                speed = 1.0f;
                ghostSpriteRenderer.color = Color.white;
                debuffTime -= Time.smoothDeltaTime;
            }
            else
            {
                choseColor(ghostColor);
                speed = 3.0f;
            }
        }
        else
        {
            deathTimer -= Time.smoothDeltaTime;
            debuffTime = 0;
            isDebuff = false;
        }
    }

    void FixedUpdate()
    {
        if (deathTimer <= 0)
        {
            GhostBehaviour(ghostColor);
            if (timer >= 0)
            {
                timer -= Time.smoothDeltaTime;
            }
        }
    }

    public void choseColor(GhostColors ghostCol)
    {
        if (ghostCol == GhostColors.Orange)
        {
            ghostSpriteRenderer.color = new Color(1.0f, 0.6825919f, 0.0f);
        }

        if (ghostCol == GhostColors.Cyan)
        {
            ghostSpriteRenderer.color = Color.cyan;
        }

        if (ghostCol == GhostColors.Red)
        {
            ghostSpriteRenderer.color = Color.red;
        }

        if (ghostCol == GhostColors.Pink)
        {
            ghostSpriteRenderer.color = new Color(1.0f, 0.6273585f, 0.8231753f);
        }
        if (ghostCol == GhostColors.White)
        {
            ghostSpriteRenderer.color = Color.white;
        }
    }
    public void deBuffGhost()
    {
        isDebuff = true;
    }

    public void GhostBehaviour(GhostColors color)
    {
        switch (color)
        {
            case GhostColors.Cyan:
                RandomAI();
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

    public List<GhostDirections> availableDirection()
    {

        List<GhostDirections> dirList = new List<GhostDirections>();

        if(!checkWalls(Vector2.down))
        {
            dirList.Add(GhostDirections.Up);
        }
        if (!checkWalls(Vector2.up))
        {
            dirList.Add(GhostDirections.Down);
        }
        if (!checkWalls(Vector2.right))
        {
            dirList.Add(GhostDirections.Right);
        }
        if (!checkWalls(Vector2.left))
        {
            dirList.Add(GhostDirections.Left);
        }
        print(string.Join(", ", dirList));

        return dirList;
    }

    public void pickDirection()
    {
        List<GhostDirections> availDir = availableDirection();
        if((!(availableDir.Equals(availDir)))& (!(has_change_dir)))
        {
            print(availDir.ElementAt(0));
            GhostDirections picked = availDir.ElementAt(Random.Range(0, availDir.Count));
            actualDirection = picked;
        }        
    }

    public void RandomAI()
    {
        Vector2 velocity = new Vector2(0, 0);
        
        if (timer <= 0)
        {
            pickDirection();
            timer = 0.5f;
        }
        switch (actualDirection)
        {
            case GhostDirections.Up:
                {
                    velocity = Vector2.down;
                    Debug.DrawRay(this.transform.localPosition, velocity, Color.red, 1.5f);
                    break;
                }
            case GhostDirections.Down:
                {
                    velocity = Vector2.up;
                    Debug.DrawRay(this.transform.localPosition, velocity, Color.red, 1.5f);
                    break;
                }
            case GhostDirections.Right:
                {
                    velocity = Vector2.right;
                    Debug.DrawRay(this.transform.localPosition, velocity, Color.red, 1.5f);
                    break;
                }
            case GhostDirections.Left:
                {
                    velocity = Vector2.left;
                    Debug.DrawRay(this.transform.localPosition, velocity, Color.red, 1.5f);
                    break;
                }
        }
        rgbody.MovePosition(rgbody.position + velocity * Time.fixedDeltaTime * speed);
    }
    //(-3.42, -8.27, 0.00)

    public bool checkWalls(Vector2 dir)
    {
        Vector2 localpos = this.transform.localPosition;
        RaycastHit2D isWall = Physics2D.Raycast(localpos,dir, 1.5f);
        if(isWall)
        {
            if (isWall.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                return true;
            }
        }
        return false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if(isDebuff == false)
            {
                FindObjectOfType<GameManager>().PacmanDie();
            }
            else
            {
                FindObjectOfType<GameManager>().GhostEaten(this);
                GhostDie();
            }
        }
    }
    

    public void GhostDie()
    {
        this.ghostSpriteRenderer.enabled = false;
        this.ghostEyes.SetActive(false);
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.transform.position = new Vector3(-6.0f, -10.0f, 0.0f);
        deathTimer = 8.0f;
    }

    public void GhostReset()
    {
        isDebuff = false;
        debuffTime = 0.0f;
        deathTimer = 0.0f;
        speed = 3.0f;
        transform.position = initial_pos;
    }
    public enum GhostColors
    {
        Cyan,
        Red,
        Orange,
        Pink,
        White
    }

    public enum GhostDirections
    {
        Up,
        Down,
        Left,
        Right
    }
}
