using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TurnTowardDirection : MonoBehaviour
{

    private new Rigidbody2D rigidbody2D;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Vector2 moveDirection = rigidbody2D.velocity;
        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
