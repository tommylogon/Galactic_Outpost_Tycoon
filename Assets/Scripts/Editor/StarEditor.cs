using System.Numerics;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Star))]
public class StarEditor : Editor
{
    private Star star;
    private string newPlanetName = "New Planet";
    private string newSpaceStationName = "New Space Station";
    private float newPlanetSize = 1f;
    private float newSpaceStationSize = 1f;
    private double newPlanetDistance = 1.0;
    private double newSpaceStationDistance = 1.0;

    private void OnEnable()
    {
        star = (Star)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add New Planet");
        newPlanetName = EditorGUILayout.TextField("Name", newPlanetName);
        newPlanetSize = EditorGUILayout.FloatField("Size", newPlanetSize);
        newPlanetDistance = EditorGUILayout.DoubleField("Distance From Star", newPlanetDistance);

        if (GUILayout.Button("Add Planet"))
        {
            AddPlanet();
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
    }

    private void AddPlanet()
    {
        Planet newPlanet = CreateInstance<Planet>();
        newPlanet.name = newPlanetName;
        newPlanet.Size = newPlanetSize;
        newPlanet.DistanceFromStar = newPlanetDistance;
        newPlanet.Star = star;
        string assetPath = $"Assets/Scriptable objects/Starmap objects/Planets/{newPlanetName}.asset";
        AssetDatabase.CreateAsset(newPlanet, AssetDatabase.GenerateUniqueAssetPath(assetPath));
        star.Planets.Add(newPlanet);
        EditorUtility.SetDirty(star);
    }

    private void AddSpaceStation()
    {
        SpaceStation newSpaceStation = CreateInstance<SpaceStation>();
        newSpaceStation.name = newSpaceStationName;
        newSpaceStation.Size = newSpaceStationSize;
        newSpaceStation.DistanceFromOrbitObject = newSpaceStationDistance;
        newSpaceStation.OrbitObject = star;
        string assetPath = $"Assets/Scriptable objects/Starmap objects/SpaceStations/{newSpaceStationName}.asset";
        AssetDatabase.CreateAsset(newSpaceStation, AssetDatabase.GenerateUniqueAssetPath(assetPath));
        star.stations.Add(newSpaceStation);
        EditorUtility.SetDirty(star);
    }


}
