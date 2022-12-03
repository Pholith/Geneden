using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField]
    private int resourceAmount = 100;
    [SerializeField]
    private ResourceManager.RessourceType type;

    private void Start()
    {
    }

    private void Update()
    {
        DestroyObject();
    }

    public void DestroyObject()
    {
        if (resourceAmount <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool DeleteResource(int amount)
    {
        int _tempResource = resourceAmount;
        if (_tempResource - amount >= 0)
        {
            resourceAmount -= amount;
            return true;
        }

        return false;
    }

    public ResourceManager.RessourceType GetResourceType()
    {
        return type;
    }
}
