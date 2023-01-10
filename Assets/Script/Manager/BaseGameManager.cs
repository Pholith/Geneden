using UnityEngine;

/// <summary>
/// Type de base pour votre GameManager. <br/>
/// Vous devez définir dans cette classe tous les Managers que vous utiliserez et les initialiser avec <see cref="BaseManager{T}.Initialization"/> : <br/>
///<inheritdoc cref="BaseManager{T}.Initialization"/>
/// </summary>
[ExecuteAlways]
public abstract class BaseGameManager : BaseManager<BaseGameManager>
{

    /// <summary>
    /// Parent pour les objets générés dans la scène par <see cref="GameObject.Inst()"/>
    /// </summary>
    private static GameObject tempInstancesParents;

    /// <inheritdoc cref="tempInstancesParents"/>
    public static Transform TempInstParent => tempInstancesParents.transform;

    private void Awake()
    {

        if (!Application.isPlaying) return;


        tempInstancesParents = new GameObject("TempInstances");
        DontDestroyOnLoad(tempInstancesParents);

        Initialization();
    }
}


