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
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            FindObjectOfType<GameManager>().EatCoin(this);
        }
    }

    public int GetValue()
    {
        return value;
    }
}
