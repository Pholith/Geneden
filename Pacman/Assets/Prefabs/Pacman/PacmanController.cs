using UnityEngine;

public class PacmanController : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rbPacman;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        float movementHorizontal = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        float movementVertical = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        MovePlayer(movementHorizontal, movementVertical);
    }
    void MovePlayer(float _movementHorizontal, float _movementVertical)
    {
        //_movement = Mathf.Abs(_movement);
        Vector3 targetVelocity = new Vector2(_movementHorizontal, rbPacman.velocity.y);
        rbPacman.velocity = Vector3.SmoothDamp(rbPacman.velocity, targetVelocity, ref velocity, .05f);
     
        targetVelocity = new Vector2(rbPacman.velocity.x, _movementVertical);
        rbPacman.velocity = Vector3.SmoothDamp(rbPacman.velocity, targetVelocity, ref velocity, .05f);
    }

    //Input.GetKey(KeyCode.Z);
}

