using System;
using System.Collections.Generic;
using UnityEngine;
using static Resource;

public class OutpostResourceManager
{
    private Outpost outpost;
    public Dictionary<ResourceType, int> resources;

    int totalCurrentStorage = 0;
    int totalMaxStorage = 0;
    int totalIdlePopulation = 0;
    int totalWorkingPopulation = 0;
    int totalHabitationCapacity = 0;
    public OutpostResourceManager(Outpost outpost)
    {
        this.outpost = outpost;
        resources = new Dictionary<ResourceType, int>();
        InitializeResources();
    }

    private void InitializeResources()
    {
        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            resources[resourceType] = 0;
        }
    }

    public void UpdateResourceManagement()
    {
        ProcessResourceProduction();
        ProcessFoodConsumption();
        ProcessWasteProduction();
        ProcessWasteManagement();
        ProcessResourceStorage();
        ProcessHabitation();

    }

    private void ProcessHabitation()
    {
         totalIdlePopulation = 0;
         totalWorkingPopulation = 0;
         totalHabitationCapacity = 0;

        foreach (OutpostModule module in outpost.Modules)
        {
            if (module is Habitation habitation)
            {
                totalIdlePopulation += habitation.IdlePopulation;
                totalWorkingPopulation += habitation.WorkingPopulation;
                totalHabitationCapacity += habitation.CalculateHabitableSpace(module.level);
            }
        }

        // Update the resources dictionary with the new crew counts

        //UIController.instance.UpdateHabitationLabels(totalIdlePopulation, totalWorkingPopulation, totalHabitationCapacity);
    }

    public (int CrewIdle, int CrewWorking, int MaxPopulation) GetHabitationData()
    {
        return (totalIdlePopulation, totalWorkingPopulation, totalHabitationCapacity);
    }
    public void AddCrew(int newCrewNumber)
    {
        foreach (OutpostModule module in outpost.Modules)
        {
            if (module is Habitation habitation )
            {
                habitation.AddIdleCrew(newCrewNumber);
            }
        }
       
    }

    private void ProcessResourceProduction()
    {
        foreach (OutpostModule module in outpost.Modules)
        {
            if (module.level > 0 && module is ResourceExtractor extractor)
            {
                ResourceType resourceType = extractor.ResourceType;
                int extractionAmount = extractor.CalculateResourceProduction(extractor.level);

                if (CanAddResource(resourceType, extractionAmount))
                {
                    resources[resourceType] += extractionAmount;
                    foreach (OutpostModule storageModule in outpost.Modules)
                    {
                        if (storageModule is StorageSpace storage)
                        {
                            storage.AddResource(resourceType, extractionAmount);
                        }
                    }
                }
            }
        }
    }
    private bool CanAddResource(ResourceType resourceType, int amount)
    {
        float maxStorage = 0;
        float currentStorage = 0;

        foreach (OutpostModule module in outpost.Modules)
        {
            if (module is StorageSpace storage)
            {
                maxStorage += storage.MaxStorage;
                if (storage.CanAddResource(resourceType, amount))
                {
                    currentStorage += amount;
                }
            }
        }

        return currentStorage + amount <= maxStorage;
    }

    private void ProcessWasteProduction()
    {
        foreach (OutpostModule module in outpost.Modules)
        {
            List<Resource> wastProduction = module.CalculateWasteProduction(module.level);

            foreach (Resource resource in wastProduction)
            {
                if (CanAddResource(resource.Type, resource.Amount))
                {
                    resources[resource.Type] += resource.Amount;
                    foreach (OutpostModule storageModule in outpost.Modules)
                    {
                        if (storageModule is StorageSpace storage)
                        {
                            storage.AddResource(resource.Type, resource.Amount);
                        }
                    }
                }
            }
        }
    }

    private void ProcessWasteManagement()
    {
        foreach (OutpostModule module in outpost.Modules)
        {
            //if (module is WasteProcessor processor)
            //{
            //    ResourceType resourceType = ResourceType.Waste;
            //    float processingAmount = processor.ProcessingRate;
            //    resources[resourceType] -= Mathf.Min(processingAmount, resources[resourceType]);
            //}
        }
    }
    private void ProcessFoodConsumption()
    {
        int totalFoodConsumption = 0;

        foreach (OutpostModule module in outpost.Modules)
        {
            if (module is Habitation habitation)
            {
                totalFoodConsumption += habitation.FoodConsumption * habitation.CrewCurrent;
            }
        }

        resources[ResourceType.Food] -= totalFoodConsumption;
    }
   


    private void ProcessResourceStorage()
    {
        

        foreach (OutpostModule module in outpost.Modules)
        {
            if (module is StorageSpace storage)
            {
                int maxStorage = storage.MaxStorage;
                int currentStorage = 0;

                foreach (ResourceStorageItem item in storage.CurrentResourceStorage)
                {
                    currentStorage += item.Amount;
                }

                totalCurrentStorage += currentStorage;
                totalMaxStorage += maxStorage;

                if (currentStorage > maxStorage)
                {
                    // Implement logic for handling excess storage, e.g. discarding resources
                }
            }
        }

    }

    public (int totalCurrentStorage, int totalMaxStorage) GetStorageData()
    {
        return (totalCurrentStorage, totalMaxStorage);
    }

    public float GetResourceAmount(ResourceType resourceType)
    {
        return resources[resourceType];
    }
    public bool AreResourcesAvailableForUpgrade(List<Resource> upgradeCost)
    {
        foreach (Resource cost in upgradeCost)
        {
            if (resources[cost.Type] < cost.Amount)
            {
                return false;
            }
        }

        return true;
    }

    public void AddCrewToModule(OutpostModule module)
    {
        if (module.CrewCurrent < module.CrewRequirement)
        {
            foreach (OutpostModule habitationModule in outpost.Modules)
            {
                if (habitationModule is Habitation habitation && habitation.IdlePopulation > 0)
                {
                    habitation.RemoveIdleCrew(1);
                    module.AddCrew();
                    //UIController.instance.UpdateModuleCrewLabel(module);
                    break;
                }
            }
        }
    }

    public void RemoveCrewFromModule(OutpostModule module)
    {
        if (module.CrewCurrent > 0)
        {
            module.RemoveCrew();
            //UIController.instance.UpdateModuleCrewLabel(module);

            foreach (OutpostModule habitationModule in outpost.Modules)
            {
                if (habitationModule is Habitation habitation)
                {
                    habitation.AddIdleCrew(1);
                    break;
                }
            }
        }
    }
}
