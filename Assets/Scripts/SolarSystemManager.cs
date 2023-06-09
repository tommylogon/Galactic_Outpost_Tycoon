using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class SolarSystemManager : MonoBehaviour
{
    public Star Star;
    public List<Location> Locations;
    public MapManager mapManager;
    public float updateInterval = 60;


    private void Start()
    {
        // Get all locations from the Star hierarchy
        Locations = GetAllLocations();

        // Initialize resources for all locations
        InitializeResourcesForAllLocations();

        
    }


    private List<Location> GetAllLocations()
    {
        List<Location> allLocations = new List<Location>();

        // Add Star's locations
        if (Star.stations != null && Star.stations.Count > 0)
        {
            allLocations.AddRange(Star.stations.SelectMany(station => station.locations));
        }

        // Iterate through Planets
        if (Star.Planets != null)
        {
            foreach (Planet planet in Star.Planets)
            {
                // Add Planet's locations
                if (planet.locations != null)
                {
                    allLocations.AddRange(planet.locations);
                }

                if (planet.stations != null && planet.stations.Count > 0)
                {
                    allLocations.AddRange(planet.stations.SelectMany(station => station.locations));
                }

                // Iterate through Moons
                if (planet.Moons != null)
                {
                    foreach (Moon moon in planet.Moons)
                    {
                        // Add Moon's locations
                        if (moon.locations != null)
                        {
                            allLocations.AddRange(moon.locations);
                        }

                        if (moon.stations != null && moon.stations.Count > 0)
                        {
                            allLocations.AddRange(moon.stations.SelectMany(station => station.locations));
                        }
                    }
                }
            }
        }

        return allLocations;
    }


    private void InitializeResourcesForAllLocations()
    {
        foreach (Location location in Locations)
        {
            location.InitializeResources();
        }
    }
    private IEnumerator UpdatePositionsCoroutine()
    {
        while (true)
        {
            foreach (Planet planet in Star.Planets)
            {
                // Update the planet's rotation and call UpdateSolarSystemObjectPosition
                planet.Rotation += planet.RotationSpeed * Time.deltaTime;
                mapManager.UpdateSolarSystemObjectPosition(planet, planet.DistanceFromParent, planet.Rotation);
            }

            // Do the same for moons and stations

            yield return new WaitForSeconds(updateInterval);
        }
    }

}
