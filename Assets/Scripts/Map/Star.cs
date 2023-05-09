using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Star", menuName = "SolarSystem/Star")]
public class Star : ScriptableObject
{
    public string Name;
    public float Size;
    public List<Planet> Planets = new List<Planet>();
    public List<SpaceStation> stations = new List<SpaceStation>();
}