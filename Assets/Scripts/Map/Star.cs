using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Star", menuName = "SolarSystem/Star")]
public class Star : CelestialObject
{
    public List<Planet> Planets = new List<Planet>();
    
}