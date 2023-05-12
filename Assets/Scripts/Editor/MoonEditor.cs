using System.Numerics;
using UnityEditor;
using UnityEngine;
using static OutpostManager;

[CustomEditor(typeof(Moon))]
public class MoonEditor : CelestialObjectEditor
{
    

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AddSpaceStationSection();
        AddLocationSection();
    }

    

    
}
