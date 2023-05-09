using System.Collections.Generic;
using UnityEngine;
using static OutpostManager;

[CreateAssetMenu(fileName = "StorageSpace", menuName = "Outpost/StorageSpace")]
public class StorageSpace : OutpostModule
{
    public List<ResourceStorageItem> CurrentResourceStorage;
    public float MaxStorage;
    public float BaseMaxStorage;
    public float MaxStorageIncrease;

    public bool CanAddResource(ResourceType resourceType, float amount)
    {
        float currentStorage = 0;

        foreach (ResourceStorageItem item in CurrentResourceStorage)
        {
            if (item.Resource == resourceType)
            {
                currentStorage += item.Amount;
            }
        }

        return currentStorage + amount <= MaxStorage;
    }

    public void AddResource(ResourceType resourceType, float amount)
    {
        bool resourceFound = false;
        foreach (ResourceStorageItem item in CurrentResourceStorage)
        {
            if (item.Resource == resourceType)
            {
                item.Amount += amount;
                resourceFound = true;
                break;
            }
        }

        if (!resourceFound)
        {
            ResourceStorageItem newItem = new ResourceStorageItem { Resource = resourceType, Amount = amount };
            CurrentResourceStorage.Add(newItem);
        }
    }
    public float CalculateMaxStorage(int level)
    {
        return BaseMaxStorage + (MaxStorageIncrease * (level - 1));
    }
    public override void Upgrade()
    {
        base.Upgrade();
        MaxStorage = CalculateMaxStorage(level);
    }
}
