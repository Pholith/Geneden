using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    protected int value;

    void Start()
    {
        value = 10;
    }
    

    //Fonction de CallBack lorsqu'un Collider hit une pièce
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            print("Eat!");
            FindObjectOfType<GameManager>().CoinEaten(this);
        }
    }

    public int getValue()
    {
        return value;
    }
}
