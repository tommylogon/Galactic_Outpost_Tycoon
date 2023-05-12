using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "CelestialObject", menuName = "SolarSystem/CelestialObject")]
public class CelestialObject : ScriptableObject
{
    public enum ObjectType
    {
        Star,
        Planet,
        Moon,
        SpaceStation
    }
    public Vector2Int Position;
    public ObjectType Type;
    public string Name;
    public float Size;
    public double DistanceFromParent;
    public double Rotation;
    public double RotationSpeed;
    public StyleBackground icon;
    public Color color;
    public CelestialObject OrbitObject; // Star, Planet, or Moon
    public List<Location> locations = new List<Location>();
    public List<CelestialObject> stations = new List<CelestialObject>();
}