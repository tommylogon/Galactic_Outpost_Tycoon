using UnityEngine;

[CreateAssetMenu(fileName = "Habitation", menuName = "Outpost/Habitation")]
public class Habitation : OutpostModule
{
    public float FoodConsumptionPerPerson;
    public int BaseHabitableSpace;
    public int HabitableSpaceIncrease;
    public int HabitableSpace;
    public int IdlePopulation;
    public int WorkingPopulation;

    public int TotalPopulation => IdlePopulation + WorkingPopulation;
    public float FoodConsumption => FoodConsumptionPerPerson * TotalPopulation;

    // Add a method to add idle crew members to the habitation
    public void AddIdleCrew(int count)
    {
        
            IdlePopulation += count;
           
        
        
    }

    // Add a method to remove idle crew members from the habitation
    public void RemoveIdleCrew(int count)
    {
        if (IdlePopulation - count >= 0)
        {
            IdlePopulation -= count;
            
        }
        
    }

    // Add a method to add working crew members to the habitation
    public int CalculateWorkingCrew(Outpost outpost)
    {
        int totalWorkingCrew = 0;

        foreach (OutpostModule module in outpost.Modules)
        {
            if (!(module is Habitation))
            {
                totalWorkingCrew += module.CrewCurrent;
            }
        }

        return totalWorkingCrew;
    }
    public int CalculateHabitableSpace(int level)
    {
        return BaseHabitableSpace + (HabitableSpaceIncrease * (level - 1));
    }
}
