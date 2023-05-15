using System.Collections.Generic;
using UnityEngine;
using static Resource;

[CreateAssetMenu(fileName = "StorageSpace", menuName = "Outpost/StorageSpace")]
public class StorageSpace : OutpostModule
{
    public List<ResourceStorageItem> CurrentResourceStorage;
    public int MaxStorage;
    public int BaseMaxStorage;
    public int MaxStorageIncrease;

    public bool CanAddResource(ResourceType resourceType, int amount)
    {
        int currentStorage = 0;

        foreach (ResourceStorageItem item in CurrentResourceStorage)
        {
            if (item.Resource == resourceType)
            {
                currentStorage += item.Amount;
            }
        }

        return currentStorage + amount <= MaxStorage;
    }

    public void AddResource(ResourceType resourceType, int amount)
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
    public int CalculateMaxStorage(int level)
    {
        return BaseMaxStorage + (MaxStorageIncrease * (level - 1));
    }
    public override void Upgrade()
    {
        base.Upgrade();
        MaxStorage = CalculateMaxStorage(level);
    }
}
