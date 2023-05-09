using System.Collections.Generic;
using UnityEngine;


public abstract class OutpostModule : ScriptableObject
{
    public string Name;
    public int level;
    public bool IsUpgrading { get; set; }

    public Sprite image;

    public int resourceProductionBase;
    public int resourceProductionIncrease;

    public int powerBase;
    public int powerIncrease;
    public int PowerRequirement => CalculatePowerRequirement(level);


    public int baseCrewRequirement;
    public int crewIncrease;
    public int CrewCurrent;
    public int CrewRequirement => GetCrewRequirementForLevel(level);

    public float TimeBase;
    public float UpgradeTime;
    public float upgradeTimeIncrease;

    public List<Resource> wasteProduction;
    public int wasteProductionIncrease;



    public List<Resource> upgradeCost;
    public int upgradeCostIncrease;

    public virtual void Upgrade()
    {
        level++;
    }
    public int GetCrewRequirementForLevel(int level)
    {
        float crewIncrease = baseCrewRequirement * level * (0.9f - (level / 100f));
        int crewRequirement = Mathf.RoundToInt(baseCrewRequirement * crewIncrease);
        return crewRequirement;
    }
    public int CalculatePowerRequirement(int level)
    {
        return powerBase + (powerIncrease * (level - 1));
    }

    public int CalculateResourceProduction(int level)
    {
        return Mathf.RoundToInt(resourceProductionBase + (resourceProductionIncrease * (level - 1)) * GetEfficiency());


    }



    public List<Resource> CalculateWasteProduction(int level)
    {
        List<Resource> wasteProductionAtLevel = new List<Resource>();

        foreach (Resource resource in wasteProduction)
        {
            wasteProductionAtLevel.Add(new Resource(resource.Type, (resource.Amount + (wasteProductionIncrease * (level - 1))) * GetEfficiency()));
        }

        return wasteProductionAtLevel;
    }

    public List<Resource> CalculateUpgradeCost(int level)
    {
        List<Resource> upgradeCostAtLevel = new List<Resource>();

        foreach (Resource resource in upgradeCost)
        {
            upgradeCostAtLevel.Add(new Resource(resource.Type, resource.Amount + (upgradeCostIncrease * (level - 1))));
        }

        return upgradeCostAtLevel;
    }
    public float CalculateUpgradeTime(int level)
    {
        return TimeBase + (UpgradeTime * level) + (upgradeTimeIncrease * Mathf.Pow(level, 2));
    }

    public float GetEfficiency()
    {
        if (CrewCurrent > 0)
        {


            float crewEfficiency = (float)CrewCurrent / CrewRequirement;
            return crewEfficiency;
        }
        return 0;
    }

    public void AddCrew()
    {
        if (CrewCurrent < CrewRequirement)
        {
            CrewCurrent++;
            // Perform any additional logic for adding crew members
        }
    }

    public void RemoveCrew()
    {
        if (CrewCurrent > 0)
        {
            CrewCurrent--;
            // Perform any additional logic for removing crew members
        }
    }
}
