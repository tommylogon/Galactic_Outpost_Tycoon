using System.Collections.Generic;
using UnityEngine;
using static Resource;

[CreateAssetMenu(fileName = "ResourceCombiner", menuName = "Outpost/ResourceCombiner")]
public class ResourceCombiner : OutpostModule
{
    public List<ResourceType> InputResourceTypes;
    public ResourceType OutputResourceType;
}
