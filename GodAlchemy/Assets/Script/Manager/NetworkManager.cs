using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : BaseManager<NetworkManager>
{

    [Networked]
    private ResourceManager Player1ResourceManager { get; set; }
    [Networked]
    private ResourceManager Player2ResourceManager { get; set; }


    protected override void InitManager()
    {
    }

    public override void Spawned()
    {
        base.Spawned();
        if (Runner.IsClient)
        {
            Player2ResourceManager = GameManager.ResourceManager;
        } else
        {
            Player1ResourceManager = GameManager.ResourceManager;
        }
    }

    public bool GetWinner()
    {
        if (Runner != null) {
            int myCiv = GameManager.ResourceManager.GetCivLevel();
            if (Runner.IsClient) {
                int opponentCiv = Player1ResourceManager.GetCivLevel();
                return myCiv > opponentCiv;
            }
            else {
                int opponentCiv = Player2ResourceManager.GetCivLevel();
                return myCiv >= opponentCiv;
            }
        }
    return false;
    }
}

