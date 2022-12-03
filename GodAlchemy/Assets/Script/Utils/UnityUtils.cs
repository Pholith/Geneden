using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public static class UnityUtils
{

    //////////////// Fonctions utilitaires à Unity dans les editeurs
#if UNITY_EDITOR
    public static T[] GetAllInstances<T>() where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).FullName);  //FindAssets uses tags check documentation for more info
        T[] a = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }

        return a;

    }
#endif
    //////////////// Fonctions utilitaires à UnityEngine
    
    public static Vector3Int ToVector3Int(this Vector3 vector3)
    {
        return Vector3Int.RoundToInt(vector3);
    }

    public static GameObject Instantiate(this GameObject gameObject, Vector3 position)
    {
        GameObject go = GameObject.Instantiate(gameObject);
        go.transform.position = position;
        return go;
    }

    public static void DrawRect(this Rect rect, float duration = 10f)
    {
        Debug.DrawLine(new Vector3(rect.x, rect.y), new Vector3(rect.x + rect.width, rect.y), Color.blue, duration);
        Debug.DrawLine(new Vector3(rect.x, rect.y), new Vector3(rect.x, rect.y + rect.height), Color.blue, duration);
        Debug.DrawLine(new Vector3(rect.x + rect.width, rect.y + rect.height), new Vector3(rect.x + rect.width, rect.y), Color.blue, duration);
        Debug.DrawLine(new Vector3(rect.x + rect.width, rect.y + rect.height), new Vector3(rect.x, rect.y + rect.height), Color.blue, duration);
    }

    public static T AddOrGetComponent<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject.TryGetComponent(out T t))
        {
            return t;
        }
        else
        {
            return gameObject.AddComponent<T>();
        }
    }
}
