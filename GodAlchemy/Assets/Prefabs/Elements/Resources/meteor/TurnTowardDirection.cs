using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TurnTowardDirection : MonoBehaviour
{
    private Rigidbody2D body2D;

    private void Start()
    {
        body2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Vector2 moveDirection = body2D.velocity;
        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
