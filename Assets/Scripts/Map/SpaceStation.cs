using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpaceStation", menuName = "SolarSystem/SpaceStation")]
public class SpaceStation : ScriptableObject
{
    public string Name;
    public float Size;
    public double DistanceFromOrbitObject;
    public ScriptableObject OrbitObject; // Star, Planet, or Moon
    public List<Location> locations = new List<Location>();
}
