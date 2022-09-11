using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanController : MonoBehaviour
{
    public Rigidbody2D RbPacman;
    public float speed;
    public float speedMultiplier;

    public Vector2 initialDirection;
    public LayerMask obstacle;
    public Vector2 Direction;
    public Vector2 NextDirection;
    public Vector3 InitialPosition;

    public bool isDead;
    public float deathTimer;
    private void Awake()
    {
        this.RbPacman = GetComponent<Rigidbody2D>();
        //print(this.StartintPosition);
    }

    private void Start()
    {
        // R�cup�ration de la position locale du Pacman
        this.InitialPosition = this.transform.localPosition;
        this.deathTimer = 3.0f;
        this.isDead = false;
        // Initialisation
        this.speedMultiplier = 1.0f;
        this.Direction = this.initialDirection;
        this.NextDirection = Vector2.zero;
        this.transform.localPosition = this.InitialPosition;
        this.RbPacman.isKinematic = false;
        this.enabled = true;
    }


    //private const float DEFAULT_COULDOWN_SPEED = 0.2f; // More it is big, more it is slow
    //private float couldownBeforeNextMove = DEFAULT_COULDOWN_SPEED;
    
    // Update is called once per frame
    void Update()
    {
        // Tester la nouvelle direction tout le temps
        if (isDead & (deathTimer > 0))
        {
            GetComponent<Animator>().SetBool("isDead", true);
            deathTimer -= Time.smoothDeltaTime;
        }
        else if (isDead & (deathTimer <= 0))
        {
            this.transform.position = new Vector3(-0.5f, -9.50f, 0.0f);
            isDead = false;
            GetComponent<Animator>().SetBool("isDead", false);
            GetComponent<CircleCollider2D>().enabled = true;
            deathTimer = 3.0f;
        }
        
        if(!isDead)
        {
            if (this.NextDirection != Vector2.zero)
            {
                SetDir(this.NextDirection);
            }
            Vector2 position = this.RbPacman.position;
            Vector2 translation = this.Direction * this.speed * this.speedMultiplier * Time.fixedDeltaTime;
            Vector2 movement = position + translation;

            this.RbPacman.MovePosition(movement);
        }
        
    }

    public void SetDir(Vector2 dir)
    {
        //print(IsObstacle(direction));

        // V�rifier s'il y a un mur dans la direction soumise
        // Si c'est pas un obstacle
        if (!IsWall(dir))
        {
            this.Direction = dir;
            this.NextDirection = Vector2.zero;
        }
        // Si c'est un obstacle
        else
        {
            this.NextDirection = dir;
        }
    }

    public bool IsWall(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.localPosition, Vector2.one * 0.75f, 0.0f, direction, 1.5f, this.obstacle);
        //Physics2D.BoxCast(this.transform.localPosition, Vector2.one * 0.75f, 0.0f, direction, 1.5f, this.obstacle);

        return hit.collider;
    }
}
