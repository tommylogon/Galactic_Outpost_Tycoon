using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OutpostManager : MonoBehaviour
{
    
    public static OutpostManager PlayerOutpostManager;
    public List<Outpost> Outposts;
    public List<OutpostModule> AvailableModules; // List of ScriptableObjects
    public Outpost selectedOutpost;
    [SerializeField]
    private List<Resource> startingResources;

    private void Start()
    {
        
        Outposts = new List<Outpost>();
        if(gameObject.tag == "PlayerOutpostManager")
        {
            PlayerOutpostManager = this;
        }
        InvokeRepeating(nameof(UpdateOutposts), 1f, 1f);

    }
    private void UpdateOutposts()
    {
        foreach (Outpost outpost in Outposts)
        {
            outpost.ExtractResources();
            ManageOngoingUpgrades(outpost);
        }
    }
    public void CreateOutpost(Location location)
    {
        Outpost newOutpost = new Outpost(location, this, startingResources);
        newOutpost.Modules = new List<OutpostModule>();
        

        foreach (OutpostModule moduleTemplate in AvailableModules)
        {
            OutpostModule newModule = InstantiateOutpostModule(moduleTemplate);
            newOutpost.Modules.Add(newModule);
        }

        // Set the extraction rates based on the available resources at the location
        foreach (ResourceExtractor extractor in newOutpost.Modules.OfType<ResourceExtractor>())
        {
            int resourceIndex = location.AvailableResources.IndexOf(extractor.ResourceType);
            if (resourceIndex != -1)
            {
                //extractor.ExtractionRate *= location.ResourceChances[resourceIndex];
            }
        }
        newOutpost.InitializeStartingResources();
        Outposts.Add(newOutpost);
        selectedOutpost = newOutpost;
    }

    private OutpostModule InstantiateOutpostModule(OutpostModule template)
    {
        OutpostModule newModule = Instantiate(template);
        newModule.name = template.name;
        return newModule;
    }
    public struct OngoingUpgrade
    {
        public OutpostModule Module;
        public float StartTime;
    }
    private void ManageOngoingUpgrades(Outpost outpost)
    {
        for (int i = outpost.OngoingUpgrades.Count - 1; i >= 0; i--)
        {
            OngoingUpgrade upgrade = outpost.OngoingUpgrades[i];
            if (Time.time >= upgrade.StartTime + upgrade.Module.UpgradeTime)
            {
                upgrade.Module.level++;
                // CrewRequirement, PowerRequirement, WasteProduction, and UpgradeTime will be updated automatically
                upgrade.Module.IsUpgrading = false;
                outpost.OngoingUpgrades.RemoveAt(i);
            }
        }
    }

    
}
