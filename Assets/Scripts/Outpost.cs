using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static OutpostManager;
using static Resource;

public class Outpost
{
    public Location Location;
    public List<OutpostModule> Modules;
    public List<OngoingUpgrade> OngoingUpgrades = new List<OngoingUpgrade>();
    public OutpostManager OutpostManager;
    public OutpostResourceManager ResourceManager;
    private List<Resource> startingResources;




    public Outpost(Location location, OutpostManager outpostManager, List<Resource> startingResource)
    {
        Location = location;
        Modules = new List<OutpostModule>();
        OutpostManager = outpostManager;
        ResourceManager = new OutpostResourceManager(this);
        startingResources = startingResource;
        
    }

    public void AddModuleToOutpost(Outpost outpost, OutpostModule module)
    {
        outpost.Modules.Add(module);
    }

    public void InitializeStartingResources()
    {
        foreach (Resource resource in startingResources)
        {
            if(resource.Type == ResourceType.Habitation)
            {
                ResourceManager.AddCrew((int)resource.Amount);
            }
            else
            {
                ResourceManager.resources[resource.Type] = resource.Amount;
            }
            
        }
    }

    public void addCrew(int newCrewNumber)
    {
        ResourceManager.AddCrew(newCrewNumber);
    }

    public void UpgradeModule(OutpostModule module)
    {
        List<Resource> upgradeCost = module.CalculateUpgradeCost(module.level + 1);
        bool resourcesAvailable = ResourceManager.AreResourcesAvailableForUpgrade(upgradeCost);

        if (resourcesAvailable)
        {
            if (module.IsUpgrading)
            {
                Debug.Log($"Module is already upgrading. Time left: {GetUpgradeTimeLeft(module)} seconds.");
                return;
            }
            OngoingUpgrade upgrade = new OngoingUpgrade
            {
                Module = module,
                StartTime = Time.time
            };
            OngoingUpgrades.Add(upgrade);
            module.IsUpgrading = true;

            // Start the coroutine using the OutpostManager instance
            OutpostManager.StartCoroutine(UpgradeModuleCoroutine(module));
        }
        else
        {
            Debug.Log("Not enough resources to upgrade the module.");
        }
    }



    public void MaintainOutpost(Outpost outpost)
    {
        // Implement the logic for maintaining the outpost, such as consuming power, crew, and resources
    }

    public void ExtractResources()
    {

        ResourceManager.UpdateResourceManagement();
        
    }
    private IEnumerator UpgradeModuleCoroutine(OutpostModule module)
    {
        module.IsUpgrading = true;

        float elapsedTime = 0f;
        while (elapsedTime < module.UpgradeTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        module.Upgrade(); // Call the Upgrade method instead of directly increasing the level

        // CrewRequirement, PowerRequirement, WasteProduction, and UpgradeTime will be updated automatically
        module.IsUpgrading = false;
        UIController.instance.UpdateResurces(module);
    }

    public float GetTotalHabitableSpace()
    {
        float totalHabitableSpace = 0;

        foreach (OutpostModule module in Modules)
        {
            if (module is Habitation habitation)
            {
                totalHabitableSpace += habitation.HabitableSpace;
            }
        }

        return totalHabitableSpace;
    }
    public float GetUpgradeTimeLeft(OutpostModule module)
    {
        if (!module.IsUpgrading) return 0;

        int upgradeIndex = OngoingUpgrades.FindIndex(upgrade => upgrade.Module == module);
        if (upgradeIndex < 0) return 0;

        OngoingUpgrade ongoingUpgrade = OngoingUpgrades[upgradeIndex];
        float elapsedTime = Time.time - ongoingUpgrade.StartTime;
        return Mathf.Max(0, module.UpgradeTime - elapsedTime);
    }

}
