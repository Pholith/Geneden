using UnityEngine;

[ExecuteAlways]
public class Node : MonoBehaviour
{

    public ResourceScriptableObject serializableDescriptor;
    private SpriteRenderer rockSpriteRenderer;
    private ResourceManager.RessourceType resourceType;

    public int resource_Amount;

    // Start is called before the first frame update
    private void Start()
    {
        resourceType = serializableDescriptor.ResourceType;
        resource_Amount = serializableDescriptor.MaxResource;
        rockSpriteRenderer = GetComponent<SpriteRenderer>();
        rockSpriteRenderer.sprite = serializableDescriptor.Sprite;
    }

    // Update is called once per frame
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
        return resourceType;
    }
}
