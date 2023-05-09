using System.Collections.Generic;
using UnityEngine;
using static OutpostManager;

[CreateAssetMenu(fileName = "Location", menuName = "SolarSystem/Location")]
public class Location : ScriptableObject
{
    public string Name;
    public ScriptableObject LocationObject; // Star, Planet, Moon, or SpaceStation
    public List<ResourceType> AvailableResources;
    [Range(0, 1)]
    public List<float> ResourceChances;
    public List<int> ResourceAmount;
    public List<Resource> InitializedResources;

    [Range(0, 100)]
    public int HabitabilityScore; // Range from 0 to 100 (0 being extremely dangerous, 100 being completely safe)

    public int CurrentSpaceUsed;
    public int MaxAvailableSpace;


    public void InitializeResources()
    {
        InitializedResources = new List<Resource>();

        for (int i = 0; i < AvailableResources.Count; i++)
        {
            ResourceType resourceType = AvailableResources[i];
            float resourceChance = ResourceChances[i];

            // Generate a random float between 0 and 1
            float randomValue = Random.Range(0f, 1f);

            // Check if the random value is less than or equal to the resource chance
            if (randomValue <= resourceChance)
            {
                // Initialize the resource with a starting amount (customize this value as needed)
                float startingAmount = ResourceAmount[i];
                Resource resource = new Resource(resourceType, startingAmount);
                InitializedResources.Add(resource);
            }
        }
    }

}
