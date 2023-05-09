using UnityEditor;
using UnityEngine;
using static OutpostManager;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    private Planet planet;
    private string newMoonName = "New Moon";
    private string newSpaceStationName = "New Space Station";
    private string newLocationName = "New Location";
    private float newMoonSize = 1f;
    private float newSpaceStationSize = 1f;
    private double newMoonDistance = 1.0;
    private double newSpaceStationDistance = 1.0;

    private void OnEnable()
    {
        planet = (Planet)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add New Moon");
        newMoonName = EditorGUILayout.TextField("Name", newMoonName);
        newMoonSize = EditorGUILayout.FloatField("Size", newMoonSize);
        newMoonDistance = EditorGUILayout.DoubleField("Distance From Planet", newMoonDistance);

        if (GUILayout.Button("Add Moon"))
        {
            AddMoon();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add New Space Station");
        newSpaceStationName = EditorGUILayout.TextField("Name", newSpaceStationName);
        newSpaceStationSize = EditorGUILayout.FloatField("Size", newSpaceStationSize);
        newSpaceStationDistance = EditorGUILayout.DoubleField("Distance From Orbit Object", newSpaceStationDistance);

        if (GUILayout.Button("Add Space Station"))
        {
            AddSpaceStation();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add New Location");
        newLocationName = EditorGUILayout.TextField("Name", newLocationName);

        if (GUILayout.Button("Add Location"))
        {
            AddLocation();
        }
    }

    private void AddMoon()
    {
        Moon newMoon = CreateInstance<Moon>();
        newMoon.name = newMoonName;
        newMoon.Size = newMoonSize;
        newMoon.DistanceFromPlanet = newMoonDistance;
        newMoon.Planet = planet;
        string assetPath = $"Assets//Scriptable objects/Starmap objects/Moons/{newMoonName}.asset";
        AssetDatabase.CreateAsset(newMoon, AssetDatabase.GenerateUniqueAssetPath(assetPath));
        planet.Moons.Add(newMoon);
        EditorUtility.SetDirty(planet);
    }

    private void AddSpaceStation()
    {
        SpaceStation newSpaceStation = CreateInstance<SpaceStation>();
        newSpaceStation.name = newSpaceStationName;
        newSpaceStation.Size = newSpaceStationSize;
        newSpaceStation.DistanceFromOrbitObject = newSpaceStationDistance;
        newSpaceStation.OrbitObject = planet;
        string assetPath = $"Assets//Scriptable objects/Starmap objects/SpaceStations/{newSpaceStationName}.asset";
        AssetDatabase.CreateAsset(newSpaceStation, AssetDatabase.GenerateUniqueAssetPath(assetPath));
        planet.stations.Add(newSpaceStation);
        EditorUtility.SetDirty(planet);
    }

    private void AddLocation()
    {
        Location newLocation = CreateInstance<Location>();
        newLocationName = planet.name + "_" + newLocationName;
        newLocation.name = newLocationName;
        newLocation.LocationObject = planet;
        string assetPath = $"Assets/Scriptable objects/Starmap objects/Locations/{newLocationName}.asset";
        AssetDatabase.CreateAsset(newLocation, AssetDatabase.GenerateUniqueAssetPath(assetPath));
        planet.locations.Add(newLocation);
        EditorUtility.SetDirty(planet);
    }
}
