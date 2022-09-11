using UnityEngine;

public class InputController : MonoBehaviour
{
    public PacmanController Movement;

    public void Awake()
    {
        this.Movement = GetComponent<PacmanController>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow)) {
            this.Movement.SetDir(Vector2.up);
            //print("Haut");
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            this.Movement.SetDir(Vector2.down);
            //print("Bas");
        }
        else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            this.Movement.SetDir(Vector2.left);
            //print("Gauche");
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            this.Movement.SetDir(Vector2.right);
            //print("Droite");
        }
    }
}
