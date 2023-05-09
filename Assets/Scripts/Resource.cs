using System;
using System.Collections.Generic;
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
    public static string ListToString(List<Resource> resources)
    {
        string result = "";
        foreach (Resource resource in resources)
        {
            result += $"{resource.Type}: {resource.Amount}, ";
        }
        return result.TrimEnd(',', ' ');
    }
    public enum ResourceType
    {
        Metals,
        Minerals,
        Gas,
        Food,
        Power,
        AgriculturalSupplies,
        Alloys,
        ConsumerGoods,
        MedicalSupplies,
        Waste,
        Vice,
        Scrap,
        Habitation,
        Storage,
        HangarSpace,
        Money

    }
}