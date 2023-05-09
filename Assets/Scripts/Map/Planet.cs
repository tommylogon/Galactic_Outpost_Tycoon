using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Planet", menuName = "SolarSystem/Planet")]
public class Planet : ScriptableObject
{
    public string Name;
    public float Size;
    public double DistanceFromStar;
    public Star Star;
    public List<Moon> Moons = new List<Moon>();
    public List<SpaceStation> stations = new List<SpaceStation>();
    public List<Location> locations = new List<Location>();
}