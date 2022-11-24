using UnityEngine;

public class Dust : MonoBehaviour
{
    [SerializeField]
    private Vector2 speedMax;
    private Vector2 speed;

    private void Start()
    {
        speed = new Vector2(Random.Range(-speedMax.x, speedMax.x), Random.Range(-speedMax.y, speedMax.y));
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = transform.position + (Vector3)speed * Time.deltaTime;
    }
}
