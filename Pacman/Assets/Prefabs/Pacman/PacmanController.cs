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


    private void Awake()
    {
        this.RbPacman = GetComponent<Rigidbody2D>();
        //print(this.StartintPosition);
    }

    private void Start()
    {
        // Récupération de la position locale du Pacman
        this.InitialPosition = this.transform.localPosition;

        // Initialisation
        this.speedMultiplier = 1.0f;
        this.Direction = this.initialDirection;
        this.NextDirection = Vector2.zero;
        this.transform.localPosition = this.InitialPosition;
        this.RbPacman.isKinematic = false;
        this.enabled = true;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // Tester la nouvelle direction tout le temps
        if (this.NextDirection != Vector2.zero)
        {
            SetDir(this.NextDirection);
        }

        Vector2 position = this.RbPacman.position;
        Vector2 translation = this.Direction * this.speed * this.speedMultiplier * Time.fixedDeltaTime;
        Vector2 movement = position + translation;

        this.RbPacman.MovePosition(movement);
    }

    public void SetDir(Vector2 dir)
    {
        //print(IsObstacle(direction));

        // Vérifier s'il y a un mur dans la direction soumise
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