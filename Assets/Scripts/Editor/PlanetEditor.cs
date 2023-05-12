using UnityEditor;
using UnityEngine;
using static OutpostManager;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : CelestialObjectEditor
{

    private string newMoonName = "New Moon";

    private float newMoonSize = 1f;
    private double newMoonDistance = 1.0;



    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        AddMoonSection();
        AddSpaceStationSection();
        AddLocationSection();
    }

    private void AddMoon()
    {
        Moon newMoon = CreateInstance<Moon>();
        newMoon.name = newMoonName;
        newMoon.Size = newMoonSize;
        newMoon.DistanceFromParent = newMoonDistance;
        newMoon.Planet = (Planet)celestialObject;
        string assetPath = $"Assets//Scriptable objects/Starmap objects/Moons/{newMoonName}.asset";
        AssetDatabase.CreateAsset(newMoon, AssetDatabase.GenerateUniqueAssetPath(assetPath));
        ((Planet)celestialObject).Moons.Add(newMoon);
        EditorUtility.SetDirty(celestialObject);
    }

    private void AddMoonSection()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add New Moon");
        newMoonName = EditorGUILayout.TextField("Name", newMoonName);
        newMoonSize = EditorGUILayout.FloatField("Size", newMoonSize);
        newMoonDistance = EditorGUILayout.DoubleField("Distance From Planet", newMoonDistance);

        if (GUILayout.Button("Add Moon"))
        {
            AddMoon();
        }
    }
   
}
