using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Moon", menuName = "SolarSystem/Moon")]
public class Moon : ScriptableObject
{
    public string Name;
    public float Size;
    public double DistanceFromPlanet;
    public Planet Planet;

    public List<SpaceStation> stations = new List<SpaceStation>();
    public List<Location> locations = new List<Location>();
}