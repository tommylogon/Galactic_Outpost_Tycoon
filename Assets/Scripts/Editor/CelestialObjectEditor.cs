using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CelestialObject))]
public abstract  class CelestialObjectEditor : Editor
{
    protected CelestialObject celestialObject;
    protected string newSpaceStationName = "New Space Station";
    protected string newLocationName = "New Location";
    protected float newSpaceStationSize = 1f;
    protected double newSpaceStationDistance = 1.0;
    protected  void OnEnable()
    {
        celestialObject = (CelestialObject)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add New Location");
        newLocationName = EditorGUILayout.TextField("Name", newLocationName);

        if (GUILayout.Button("Add Location"))
        {
            AddLocation();
        }
    }

    protected virtual void AddLocationSection()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add New Location");
        newLocationName = EditorGUILayout.TextField("Name", newLocationName);

        if (GUILayout.Button("Add Location"))
        {
            AddLocation();
        }
    }
    protected virtual void AddSpaceStationSection()
    {
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
    protected  void AddSpaceStation()
    {
        CelestialObject newSpaceStation = CreateInstance<CelestialObject>();
        newSpaceStation.name = newSpaceStationName;
        newSpaceStation.Size = newSpaceStationSize;
        newSpaceStation.DistanceFromParent = newSpaceStationDistance;
        newSpaceStation.OrbitObject = celestialObject.OrbitObject;
        string assetPath = $"Assets/Scriptable objects/Starmap objects/SpaceStations/{newSpaceStationName}.asset";
        AssetDatabase.CreateAsset(newSpaceStation, AssetDatabase.GenerateUniqueAssetPath(assetPath));
        celestialObject.OrbitObject.stations.Add(newSpaceStation);
        EditorUtility.SetDirty(celestialObject.OrbitObject);
    }

    protected  void AddLocation()
    {
        Location newLocation = CreateInstance<Location>();
        newLocation.name = celestialObject.OrbitObject.name + '_' + newLocationName;
        newLocation.LocationObject = celestialObject.OrbitObject;
        string assetPath = $"Assets/Scriptable objects/Starmap objects/Locations/{newLocationName}.asset";

        AssetDatabase.CreateAsset(newLocation, AssetDatabase.GenerateUniqueAssetPath(assetPath));
        celestialObject.OrbitObject.locations.Add(newLocation);
        EditorUtility.SetDirty(celestialObject.OrbitObject);
    }
}
