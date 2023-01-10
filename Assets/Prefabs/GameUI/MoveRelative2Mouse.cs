using UnityEngine;

public class MoveRelative2Mouse : MonoBehaviour
{
    public Vector3 StartPosition;
    public Vector2 moveMultiplier;
    public float moveSpeedMultiplier;
    private void Start()
    {
        StartPosition = transform.position;
    }

    private void LateUpdate()
    {
        var mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f, 0); // center the mouse pos
        //move based on the starting position and its modified value.
        transform.position = Vector3.Lerp(transform.position,
            new Vector3(StartPosition.x + (mousePosition.x * moveMultiplier.x), StartPosition.y + (mousePosition.y * moveMultiplier.y), StartPosition.z),
            moveSpeedMultiplier);
        transform.position = new Vector3(transform.position.x, transform.position.y, StartPosition.z);
    }
}