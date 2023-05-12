using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Planet", menuName = "SolarSystem/Planet")]
public class Planet : CelestialObject
{
    public Star Star;
    public List<Moon> Moons = new List<Moon>();
    
}