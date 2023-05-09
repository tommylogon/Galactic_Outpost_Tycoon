using UnityEditor;
using UnityEngine;
using static OutpostManager;

[CustomEditor(typeof(Location))]
public class LocationEditor : Editor
{
    private Location location;

    private ResourceType newResourceType = ResourceType.Metals;
    private float newResourceChance = 0.5f;

    private void OnEnable()
    {
        location = (Location)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Add New Resource");

        newResourceType = (ResourceType)EditorGUILayout.EnumPopup("Resource Type", newResourceType);
        newResourceChance = EditorGUILayout.Slider("Resource Chance", newResourceChance, 0f, 1f);

        if (GUILayout.Button("Add Resource"))
        {
            AddResource();
        }

        if (GUILayout.Button("Initialize Resources"))
        {
            location.InitializeResources();
            EditorUtility.SetDirty(location);
        }
    }

    private void AddResource()
    {
        location.AvailableResources.Add(newResourceType);
        location.ResourceChances.Add(newResourceChance);
        EditorUtility.SetDirty(location);
    }
}
