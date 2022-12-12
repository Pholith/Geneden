using Fusion;
using UnityEngine;

public class NetworkManager : BaseManager<NetworkManager>
{
    [Networked]
    private ResourceManager player1ResourceManager { get; set; }
    [Networked]
    private ResourceManager player2ResourceManager { get; set; }

    private NetworkObject playerCursor;

    [SerializeField]
    private NetworkPrefabRef cursorPrefab;

    protected override void InitManager()
    {
    }

    // Lorsque le client se connecte et est pr�t (Spawned) il appel le serveur pour commencer la game.
    public override void Spawned()
    {
        base.Spawned();
        if (Runner.IsClient)
        {
            OnGameStartRPC(Runner.LocalPlayer);
        }
    }
    /// <summary>
    /// Initialisation de la game c�t� serveur, mettre ici toutes les r�initialisations et les choses � faire au d�but de la game.
    /// </summary>
    /// <param name="clientPlayer"></param>
    [Rpc]
    public void OnGameStartRPC(PlayerRef clientPlayer)
    {
        NetworkObject player2Cursor;
        if (Runner.IsPlayer && Runner.IsServer) // host is the player 1
        {
            //player1ResourceManager = GameManager.ResourceManager;
            playerCursor = Runner.Spawn(cursorPrefab);
            playerCursor.name = "host cursor";
            playerCursor.GetComponent<SpriteRenderer>().enabled = false;
            playerCursor.GetComponent<SpriteRenderer>().color = Color.cyan;

            // Fait spawn le 2�me curseur et envoie un RPC au client pour qu'il le modifie de son c�t�.
            player2Cursor = Runner.Spawn(cursorPrefab, inputAuthority: clientPlayer);
            player2Cursor.name = "client cursor";
            Player2HideCursorRPC(player2Cursor.Id);
        }
        else // client is the player 2
        {
            //player2ResourceManager = GameManager.ResourceManager;
        }
    }
    [Rpc]
    private void Player2HideCursorRPC(NetworkId id)
    {
        if (!Runner.IsClient) return;
        playerCursor = Runner.FindObject(id);
        playerCursor.GetComponent<SpriteRenderer>().enabled = false;
        playerCursor.GetComponent<SpriteRenderer>().color = Color.magenta;
    }

    private NetworkObject clientCursor = null;
    /// <summary>
    /// Permet de d�placer le curseur du client sur l'host m�me si le client n'a pas l'authorit� sur le curseur.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="objectId"></param>
    [Rpc(RpcSources.Proxies, RpcTargets.StateAuthority)]
    private void MoveClientCurserRPC(Vector3 position, NetworkId objectId)
    {
        if (!clientCursor) clientCursor = Runner.FindObject(objectId);
        clientCursor.transform.position = position;
    }

    private void Update()
    {
        if (!Runner || playerCursor == null) return;

        Vector3 newPosition = GameManager.GridManager.GetMouseGridPos();
        if (Runner.IsServer)
            playerCursor.transform.position = newPosition;
        else
            MoveClientCurserRPC(newPosition, playerCursor.Id);
    }

    public bool GetWinner()
    {
        return true; /*
        if (Runner != null)
        {
            int myCiv = GameManager.ResourceManager.GetCivLevel();
            if (Runner.IsClient)
            {
                int opponentCiv = player1ResourceManager.GetCivLevel();
                return myCiv > opponentCiv;
            }
            else
            {
                int opponentCiv = player2ResourceManager.GetCivLevel();
                return myCiv >= opponentCiv;
            }
        }
        return false;
        */
    }
}

