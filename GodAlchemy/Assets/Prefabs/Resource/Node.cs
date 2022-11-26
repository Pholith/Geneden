using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    public ResourceScriptableObject rockScript;
    private SpriteRenderer rockSpriteRenderer;
    private ResourceManager.RessourceType resourceType;

    public int resource_Amount;
    // Start is called before the first frame update
    void Start()
    {
        resourceType = rockScript.ResourceType;
        resource_Amount = rockScript.MaxResource;
        rockSpriteRenderer = GetComponent<SpriteRenderer>();
        rockSpriteRenderer.sprite = rockScript.Sprite;
    }

    // Update is called once per frame
    void Update()
    {
        DestroyObject();
    }

    public void DestroyObject()
    {
        if(resource_Amount <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public bool DeleteResource(int amount)
    {
        int _tempResource = resource_Amount;
        if(_tempResource - amount >= 0)
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
