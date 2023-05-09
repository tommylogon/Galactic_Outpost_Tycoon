using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpaceStation))]
public class SpaceStationEditor : Editor
{
    private SpaceStation spaceStation;
    private string newLocationName = "New Location";

    private void OnEnable()
    {
        spaceStation = (SpaceStation)target;
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

    private void AddLocation()
    {
        Location newLocation = CreateInstance<Location>();
        newLocation.name = spaceStation.name +'_'+newLocationName;
        newLocation.LocationObject = spaceStation;
        string assetPath = $"Assets/Scriptable objects/Starmap objects/Locations/{newLocationName}.asset";
        AssetDatabase.CreateAsset(newLocation, AssetDatabase.GenerateUniqueAssetPath(assetPath));
        spaceStation.locations.Add(newLocation);
        EditorUtility.SetDirty(spaceStation);
    }
}
