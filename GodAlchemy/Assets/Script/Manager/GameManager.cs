using UnityEngine;

/// <summary>
/// Exemple de GameManager
/// </summary>
[ExecuteAlways]
public class GameManager : BaseGameManager
{


    private static GameManager instance;
    public static new GameManager Instance => instance;




    //[SerializeField] private TranslationManager translation;
    //public static TranslationManager Translation => instance.translation;


    protected override void InitManager()
    {
        if(instance == null)
        instance = this;
        else
        {
            Debug.LogError("Multiple GameManager on this scene !! Destoying this one.");
            Destroy(gameObject);
            return;
        }
        //translation?.Initialization();
    }

#if UNITY_EDITOR
    private void Update()
    {
        //if (translation == null) translation = GetComponentInChildren<TranslationManager>();
    }
#endif
}
