using System.Numerics;
using UnityEditor;
using UnityEngine;
using static OutpostManager;

[CustomEditor(typeof(Moon))]
public class MoonEditor : Editor
{
    private Moon moon;
    private string newSpaceStationName = "New Space Station";
    private string newLocationName = "New Location";
    private float newSpaceStationSize = 1f;
    private double newSpaceStationDistance = 1.0;

    private void OnEnable()
    {
        moon = (Moon)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

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

    private void AddSpaceStation()
    {
        SpaceStation newSpaceStation = CreateInstance<SpaceStation>();
        newSpaceStation.name = newSpaceStationName;
        newSpaceStation.Size = newSpaceStationSize;
        newSpaceStation.DistanceFromOrbitObject = newSpaceStationDistance;
        newSpaceStation.OrbitObject = moon;
        string assetPath = $"Assets/Scriptable objects/Starmap objects/SpaceStations/{newSpaceStationName}.asset";
        AssetDatabase.CreateAsset(newSpaceStation, AssetDatabase.GenerateUniqueAssetPath(assetPath));
        moon.stations.Add(newSpaceStation);
        EditorUtility.SetDirty(moon);
    }

    private void AddLocation()
    {
        Location newLocation = CreateInstance<Location>();
        newLocation.name = moon.name +'_'+ newLocationName;
        newLocation.LocationObject = moon;
        string assetPath = $"Assets/Scriptable objects/Starmap objects/Locations/{newLocationName}.asset";
        
        AssetDatabase.CreateAsset(newLocation, AssetDatabase.GenerateUniqueAssetPath(assetPath));
        moon.locations.Add(newLocation);
        EditorUtility.SetDirty(moon);
    }
}
