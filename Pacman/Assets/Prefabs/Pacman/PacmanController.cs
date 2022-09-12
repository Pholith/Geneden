using UnityEngine;

public class PacmanController : MonoBehaviour
{
    private Rigidbody2D rbPacman;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float speedMultiplier;

    [SerializeField]
    private Vector2 initialDirection;
    [SerializeField]
    private LayerMask obstacle;
    private Vector2 direction;
    private Vector2 nextDirection;
    private Vector3 initialPosition;


    private void Awake()
    {
        rbPacman = GetComponent<Rigidbody2D>();
        //print(this.StartintPosition);
    }

    private void Start()
    {
        // R�cup�ration de la position locale du Pacman
        initialPosition = transform.localPosition;

        // Initialisation
        speedMultiplier = 1.0f;
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.localPosition = initialPosition;
        rbPacman.isKinematic = false;
        enabled = true;
    }


    private const float DEFAULT_COULDOWN_SPEED = 0.2f; // More it is big, more it is slow
    private readonly float couldownBeforeNextMove = DEFAULT_COULDOWN_SPEED;
    public bool isDead = false;

    // Update is called once per frame
    private void Update()
    {
        // Tester la nouvelle direction tout le temps
        if (nextDirection != Vector2.zero)
        {
            SetDir(nextDirection);
        }

        Vector2 position = rbPacman.position;
        Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;
        Vector2 movement = position + translation;

        rbPacman.MovePosition(movement);
    }

    public void SetDir(Vector2 dir)
    {
        //print(IsObstacle(direction));

        // V�rifier s'il y a un mur dans la direction soumise
        // Si c'est pas un obstacle
        if (!IsWall(dir))
        {
            direction = dir;
            nextDirection = Vector2.zero;
        }
        // Si c'est un obstacle
        else
        {
            nextDirection = dir;
        }

        if (isDead)
        {
            transform.position = new Vector3(-0.5f, -9.50f, 0.0f);
            isDead = false;
        }
    }

    public Vector2 getDir()
    {
        return this.direction;
    }

    private bool IsWall(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.localPosition, Vector2.one * 0.75f, 0.0f, direction, 1.5f, obstacle);
        //Physics2D.BoxCast(this.transform.localPosition, Vector2.one * 0.75f, 0.0f, direction, 1.5f, this.obstacle);

        return hit.collider;
    }
}
