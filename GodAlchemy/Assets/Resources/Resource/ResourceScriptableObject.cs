using UnityEngine;

[CreateAssetMenu(fileName = "New Node", menuName = "ScriptableObjects/New resource", order = 5)]
public class ResourceScriptableObject : ScriptableObject
{

    [Header("Informations")]
    [TextArea]
    public string ResourceDescription;
    [SerializeField]
    public Sprite Sprite;

    [Header("Resource")]
    [Min(0)]
    public int MaxResource;
    public ResourceManager.RessourceType ResourceType;
    public float gatheringSpeed;

}
