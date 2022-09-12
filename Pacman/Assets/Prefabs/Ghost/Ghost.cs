using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Ghost : MonoBehaviour
{
    public GameObject ghostEyes;

    [SerializeField]
    [Tooltip("Vitesse de déplacement du fantôme")]
    private float speed = 3f;
    public const int POINTS_PER_GHOST = 200;

    [Tooltip("Temps de debuff en secondes")]
    public const float DEFAULT_DEBUFF_DURATION = 6f;
    private float debuffDuration = DEFAULT_DEBUFF_DURATION;
    private bool isDebuffed = false;
    private SpriteRenderer spriteRenderer;
    private Color color;
    private Vector3 spawnTransformPosition;

    [Header("Tout ce qui concerne l'IA des fantômes")]
    [SerializeField] LayerMask collideWith;
    [SerializeField] GameObject ghostBaseEntrance;
    [SerializeField, Range(0, 2)] int alternativeIA;
    private Tilemap tilemap;
    private PacmanController pacman;
    private Vector2Int currentDirection = Vector2Int.up;
    private bool isInStarterPoint = true;


    private void Start()
    {
        spawnTransformPosition = transform.position;
        tilemap = FindObjectOfType<Tilemap>();
        pacman = FindObjectOfType<PacmanController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Gestion du temps de debuff
        if (isDebuffed) {
            debuffDuration -= Time.deltaTime;
            if (debuffDuration <= 0) RebuffGhost();
        }
        
        
        // Gestion de la sortie de base
        if (Vector2.Distance(ghostBaseEntrance.transform.position, transform.position) < 0.1f) isInStarterPoint = false;
        if (isInStarterPoint)
        {
            if (Mathf.Abs(transform.position.x - ghostBaseEntrance.transform.position.x) > 0.01f)
            {
                Vector2 direction = (ghostBaseEntrance.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right;
                transform.Translate(((Vector3)direction) * Time.deltaTime * speed);
            } else
            {
                transform.Translate(((Vector3)Vector2.up) * Time.deltaTime * speed);
            }
            return;
        }

        GameObject target;
        if (isInStarterPoint) target = ghostBaseEntrance;
        else target = pacman.gameObject;

        Vector2 vectorBetweenObjects = target.transform.position - transform.position;
        Vector2Int favoriteDirection = Direction(vectorBetweenObjects);
        Vector2Int secondFavoriteDirection = SecondDirection(vectorBetweenObjects);
        if (alternativeIA == 1) favoriteDirection = secondFavoriteDirection;

        if (isDebuffed)
        {
            favoriteDirection = -favoriteDirection;
            secondFavoriteDirection = -secondFavoriteDirection;
        }

        // Si l'IA peut changer de direction
        if (!IsWall(favoriteDirection, 1f))
        {
            currentDirection = favoriteDirection;
        } else if (!IsWall(secondFavoriteDirection, 1f))
        {
            currentDirection = secondFavoriteDirection;
        }

        if (!IsWall(currentDirection, 0.02f))
        {
            transform.Translate(((Vector3)(Vector3Int)currentDirection) * Time.deltaTime * speed);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.localPosition + ((Vector3)(Vector2) currentDirection) * 1f, Vector3.one);
    }

    private bool IsWall(Vector2 direction, float distance)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.localPosition, Vector2.one*0.95f, 0.0f, direction, distance, collideWith);
        return hit.collider != null;
    }

    private Vector2Int Direction(Vector2 vectorBetweenObjects)
    {
        if (Mathf.Abs(vectorBetweenObjects.x) > Mathf.Abs(vectorBetweenObjects.y))
        {
            return vectorBetweenObjects.x > 0 ? Vector2Int.right : Vector2Int.left;
        }
        return vectorBetweenObjects.y > 0 ? Vector2Int.up : Vector2Int.down;
    }    
    private Vector2Int SecondDirection(Vector2 vectorBetweenObjects)
    {
        if (Mathf.Abs(vectorBetweenObjects.y) > Mathf.Abs(vectorBetweenObjects.x))
        {
            return vectorBetweenObjects.x > 0 ? Vector2Int.right : Vector2Int.left;
        }
        return vectorBetweenObjects.y > 0 ? Vector2Int.up : Vector2Int.down;
    }

    public void DebuffGhost()
    {
        print("Debuff");
        isDebuffed = true;
        debuffDuration = DEFAULT_DEBUFF_DURATION;
        color = spriteRenderer.color;
        spriteRenderer.color = Color.blue;
        speed -= 2f;
    }

    public void RebuffGhost()
    {
        isDebuffed = false;
        spriteRenderer.color = color;
        speed = DEFAULT_DEBUFF_DURATION;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (isDebuffed == false)
            {
                GameManager.Instance.PacmanDie();
            }
            else
            {
                GameManager.Instance.ScoreManager.AddUpScore(POINTS_PER_GHOST);
                GameManager.Instance.eat_ghost.Play();
                GhostDie();
            }
        }
    }

    public void GhostDie()
    {
        transform.position = spawnTransformPosition;
        isInStarterPoint = true;
        RebuffGhost();
        GameManager.Instance.StartCoroutine(WaitAndRespawnGhost());
        gameObject.SetActive(false);
    }

    IEnumerator WaitAndRespawnGhost()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(true);
    }
}
