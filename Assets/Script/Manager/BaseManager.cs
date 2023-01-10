using Fusion;
using System;
using UnityEngine;

/// <summary>
/// Type de base pour tous vos managers-singletons de votre scène
/// </summary>
/// <typeparam name="T">Type de votre Manager à récrire (pour le champ `Instance` du singleton)</typeparam>
public abstract class BaseManager<T> : NetworkBehaviour where T : MonoBehaviour
{
    [NonSerialized] private bool alreadyInit = false;

    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<T>();
            return instance;
        }
    }

    /// <summary>
    /// Méthode d'Initialisation de votre manager (appelée par le GameManager). <br/>
    /// Cela permet d'initialiser tout les Managers dans un explicitement déterminé
    /// contrairement aux fonction event Start(), Awake().. qui s'executent parfois de manière   
    /// aléatoire. Par exemple, c'est assez pénible quand votre jeu démare avant que vos resources soient initialisés.
    /// </summary>
    public void Initialization()
    {
        if (alreadyInit) return;
        alreadyInit = true;
        InitManager();
    }


    ///<inheritdoc cref="Initialization"/>
    protected abstract void InitManager();
}

