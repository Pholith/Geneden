using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    PacmanController movement;

    public void Awake()
    {
        movement = GetComponent<PacmanController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            movement.SetDir(Vector2.up);
            //print("Haut");
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement.SetDir(Vector2.down);
            //print("Bas");
        }
        else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement.SetDir(Vector2.left);
            //print("Gauche");
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement.SetDir(Vector2.right);
            //print("Droite");
        }

        // Rotation de Pacman dans la direction précédement indiquée
        float angle = Mathf.Atan2(this.movement.getDir().y, this.movement.getDir().x);
        this.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }
}
