using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Large_Coin : Coin
{
    // Start is called before the first frame update
    void Start()
    {
        value = 100;
    }

    //Fonction de CallBack lorsqu'un Collider hit une pièce
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            print("Eat large coin!");
            FindObjectOfType<GameManager>().LargeCoinEaten(this);
        }
    }

}
