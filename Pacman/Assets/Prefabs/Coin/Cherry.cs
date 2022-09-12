using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : Coin
{
    [SerializeField]
    private GameObject uiCherry;

    private bool isSpawned;
    private bool isEaten;

    void Start()
    {
        isSpawned = false;
        isEaten = false;
        value = 100;
    }

    override public void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        isEaten = true;
        uiCherry.SetActive(false);
        // show score
    }

    public void GameStart()
    {
        this.gameObject.SetActive(false);
        InvokeRepeating("CherryBehavior", 10.0f, 10.0f);
    }
    void CherryBehavior()
    {
        if (!isEaten)
        {
            if (isSpawned)
            {
                this.gameObject.SetActive(false);
                isSpawned = false;
            }
            else
            {
                this.gameObject.SetActive(true);
                isSpawned = true;
            }
        }
    }
}
