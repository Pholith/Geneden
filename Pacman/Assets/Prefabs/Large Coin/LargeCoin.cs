using UnityEngine;

public class LargeCoin : Coin
{

    private GameManager gameManager;

    // Start is called before the first frame update
    private void Start()
    {
        value = 100;
        gameManager = FindObjectOfType<GameManager>();
    }

    //Fonction de CallBack lorsqu'un Collider hit une pièce
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            gameManager.EatLargeCoin(this);
        }
    }

}
