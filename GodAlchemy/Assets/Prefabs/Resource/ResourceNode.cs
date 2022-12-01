using UnityEngine;

[ExecuteAlways]
public class ResourceNode : MonoBehaviour
{

    public ResourceScriptableObject serializableDescriptor;
    private SpriteRenderer spriteRenderer;

    public int resource_Amount;

    private void Start()
    {
        resource_Amount = serializableDescriptor != null ? serializableDescriptor.MaxResource : 1;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = serializableDescriptor?.Sprite;
    }

    private void Update()
    {
        DestroyObject();
    }

    public void DestroyObject()
    {
        if (resource_Amount <= 0)
        {
            Destroy(gameObject);
        }
    }

    public bool DeleteResource(int amount)
    {
        int _tempResource = resource_Amount;
        if (_tempResource - amount >= 0)
        {
            resource_Amount -= amount;
            return true;
        }

        return false;
    }

    public ResourceManager.RessourceType GetResourceType()
    {
        return serializableDescriptor.ResourceType;
    }
}
