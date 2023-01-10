using UnityEngine;

[CreateAssetMenu(fileName = "Maison", menuName = "ScriptableObjects/New building house", order = 3)]
public class HouseScriptableObject : BuildingsScriptableObject
{
    [Header("Infos de la maison")]
    public int AdditionalPopulation;
}

