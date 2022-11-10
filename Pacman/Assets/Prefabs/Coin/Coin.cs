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
            if (this.GetType() == typeof(Cherry))
            {
                GameManager.Instance.eat_fruit.Play();
            }
            else
            {
                if (GameManager.Instance.currentMunch == 0)
                {
                    GameManager.Instance.munch1.Play();
                    GameManager.Instance.currentMunch = 1;
                }
                else if (GameManager.Instance.currentMunch == 1)
                {
                    GameManager.Instance.munch2.Play();
                    GameManager.Instance.currentMunch = 0;
                }
            }
            FindObjectOfType<GameManager>().EatCoin(this);
        }
    }

    public int GetValue()
    {
        return value;
    }
}
