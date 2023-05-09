using System;
using static OutpostManager;

[Serializable]
public class Resource
{
    public ResourceType Type;
    public float Amount;

    public Resource(ResourceType type, float amount)
    {
        Type = type;
        Amount = amount;
    }
}