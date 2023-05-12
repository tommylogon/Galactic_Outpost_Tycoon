using System.Numerics;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Star))]
public class StarEditor : CelestialObjectEditor
{
    private string newPlanetName = "New Planet";
    private float newPlanetSize = 1f;
    
    private double newPlanetDistance = 1.0;

  

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

        AddSpaceStationSection();
    }

    private void AddPlanet()
    {
        Planet newPlanet = CreateInstance<Planet>();
        newPlanet.name = newPlanetName;
        newPlanet.Size = newPlanetSize;
        newPlanet.DistanceFromParent = newPlanetDistance;
        newPlanet.Star = (Star)celestialObject;
        string assetPath = $"Assets/Scriptable objects/Starmap objects/Planets/{newPlanetName}.asset";
        AssetDatabase.CreateAsset(newPlanet, AssetDatabase.GenerateUniqueAssetPath(assetPath));
        ((Star)celestialObject).Planets.Add(newPlanet);
        EditorUtility.SetDirty((Star)celestialObject);
    }

    


}
