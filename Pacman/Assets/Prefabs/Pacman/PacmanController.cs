using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private const float DEFAULT_COULDOWN_SPEED = 0.2f; // More it is big, more it is slow
    private float couldownBeforeNextMove = DEFAULT_COULDOWN_SPEED;
    // Update is called once per frame
    void Update()
    {
        couldownBeforeNextMove -= Time.deltaTime;
        if (couldownBeforeNextMove <= 0)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                gameObject.transform.Translate(Vector2.up);
                couldownBeforeNextMove = DEFAULT_COULDOWN_SPEED;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                gameObject.transform.Translate(Vector2.left);
                couldownBeforeNextMove = DEFAULT_COULDOWN_SPEED;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                gameObject.transform.Translate(Vector2.down);
                couldownBeforeNextMove = DEFAULT_COULDOWN_SPEED;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                gameObject.transform.Translate(Vector2.right);
                couldownBeforeNextMove = DEFAULT_COULDOWN_SPEED;
            }

        }

    }
}
