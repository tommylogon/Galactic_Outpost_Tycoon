using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UIOutpostModules : MonoBehaviour
{
    public UIController uiController;

    public OutpostModule selectedModule;

    private Label selectedModuleName;
    private Label upgradeTimer;
    private Label selectedModuleLevel;

    private Label selectedModuleCurrentCrew;
    private Label selectedModuleRequiredCrew;
    private Button addCrewButton;
    private Button removeCrewButton;

    private Label selectedModuleResource;
    private Label selectedModuleEnergy;
    private Label selectedModuleExtractionRate;
    private Label selectedModuleEfficiency;
    private Label selectedModuleWasteProduction;
    private Label selectedModuleStorageLabel;
    private Label selectedModuleHabitationLabel;



    private Label nextLevelTimer;
    private Label selectedModuleNextLevelCrewRequirement;
    private Label selectedModuleNextLevelResourceProduction;
    private Label selectedModuleNextLevelPowerConsumption;
    private Label selectedModuleNextLevelWasteProduction;
    private Label selectedModuleNextLevelUpgradeCost;
    private Label selectedModuleNextLevelUpgradeTimer;

    private Button selectedModuleUpgradeButton;


    public VisualTreeAsset outpostModulesContentTemplate;
    public VisualTreeAsset OutpostModuleTemplate;

    VisualElement OutpostModulesContent;

    public void Show(VisualElement gameContentArea)
    {
        if (addCrewButton != null)
        {
            addCrewButton.clicked -= () => AddCrewToModule(selectedModule);

        }
        if (removeCrewButton != null)
        {
            removeCrewButton.clicked -= () => RemoveCrewFromModule(selectedModule);
        }
        if (selectedModuleUpgradeButton != null)
        {
            selectedModuleUpgradeButton.clicked -= () => UpgradeSelectedModule(selectedModule);

        }
        OutpostModulesContent = outpostModulesContentTemplate.CloneTree();
        gameContentArea.Add(OutpostModulesContent);
    }
    public void GenerateOutpostModuleButtons(System.Type moduleType)
    {
        VisualElement outpostModules = OutpostModulesContent.Q<VisualElement>("outpostModules");

        Outpost selectedOutpost = OutpostManager.PlayerOutpostManager.selectedOutpost;

        if (selectedOutpost == null)
        {
            Debug.LogWarning("No outpost selected");
            return;
        }

        foreach (OutpostModule module in selectedOutpost.Modules.Where(m => m.GetType() == moduleType))
        {
            // Instantiate the template and set the properties
            VisualElement outpostModule = OutpostModuleTemplate.CloneTree();
            outpostModule.Q<Button>("outpostModuleButton").clicked += () => ShowModuleDetails(module);
            outpostModule.Q<Image>("outpostModuleImage").sprite = module.image;
            outpostModule.Q<Label>("outpostModuleLevel").text = "Level: " + module.level.ToString();
            outpostModule.Q<Label>("selectedModuleName").text = module.Name;
            outpostModule.Q<Button>("outpostModuleQuickUpgradeButton").clicked += () => QuickUpgradeModule(module);

            // Add the module button to the bottom row
            outpostModules.Add(outpostModule);
        }
    }


    public void QuickUpgradeModule(OutpostModule module)
    {
        UpgradeSelectedModule(module);
    }
    public void ShowModuleDetails(OutpostModule module)
    {
        selectedModule = module;

        selectedModuleHabitationLabel = uiController.root.Q<Label>("selectedModuleHabitationLabel");
        selectedModuleStorageLabel = uiController.root.Q<Label>("selectedModuleStorageLabel");
        selectedModuleName = uiController.root.Q<Label>("selectedModuleName");
        selectedModuleLevel = uiController.root.Q<Label>("selectedModuleLevel");
        selectedModuleEnergy = uiController.root.Q<Label>("selectedModuleEnergy");

        upgradeTimer = uiController.root.Q<Label>("nextLevelTimer");
        selectedModuleNextLevelCrewRequirement = uiController.root.Q<Label>("nextLevelCrewRequirement");
        selectedModuleNextLevelResourceProduction = uiController.root.Q<Label>("nextLevelResourceProduction");
        selectedModuleNextLevelPowerConsumption = uiController.root.Q<Label>("nextLevelPowerConsumption");
        selectedModuleNextLevelWasteProduction = uiController.root.Q<Label>("nextLevelWasteProduction");
        selectedModuleNextLevelUpgradeTimer = uiController.root.Q<Label>("moduleUpgradeLable");
        selectedModuleNextLevelUpgradeCost = uiController.root.Q<Label>("selectedModuleUpgradeCost");

        

        addCrewButton = uiController.root.Q<Button>("selectedModuleAddCrewButton");
        removeCrewButton = uiController.root.Q<Button>("selectedModuleRemoveCrewButton");
        selectedModuleUpgradeButton = uiController.root.Q<Button>("selectedModuleUpgradeButton");


        

        // Add new event listeners
        addCrewButton.clicked += () => AddCrewToModule(module);
        removeCrewButton.clicked += () => RemoveCrewFromModule(module);
        selectedModuleUpgradeButton.clicked += () => UpgradeSelectedModule(module);

        // Update the current crew label
        UpdateModuleCrewLabel(module);

        string wasteProductionStr = Resource.ListToString(module.wasteProduction);
        string upgradeCostStr = Resource.ListToString(module.upgradeCost);


        uiController.root.Q<Label>("selectedModuleUpgradeCost").text =$"Upgrade cost:  {upgradeCostStr}";

        if (module is ResourceExtractor extractor)
        {
            uiController.root.Q<Label>("selectedModuleResource").text = $"Resources extracted: {extractor.ResourceType.ToString()}";
            uiController.root.Q<Label>("selectedModuleExtractionRate").text =$"Rate:  {extractor.CalculateResourceProduction(extractor.level).ToString()}";
        }
        else if (module is StorageSpace storageModule)
        {
            selectedModuleStorageLabel.style.display = DisplayStyle.Flex;
            selectedModuleStorageLabel.text = $"Storage: {storageModule.CurrentResourceStorage.Count}/{storageModule.MaxStorage}";

            selectedModuleHabitationLabel.style.display = DisplayStyle.None;
        }
        else if (module is Habitation habitationModule)
        {
            selectedModuleHabitationLabel.style.display = DisplayStyle.Flex;
            selectedModuleHabitationLabel.text = $"Habitation: {habitationModule.TotalPopulation}/{habitationModule.HabitableSpace}";

            selectedModuleStorageLabel.style.display = DisplayStyle.None;
        }

        else
        {
            uiController.root.Q<Label>("selectedModuleResource").style.display = DisplayStyle.None;
            uiController.root.Q<Label>("selectedModuleExtractionRate").style.display = DisplayStyle.None;
            selectedModuleStorageLabel.style.display = DisplayStyle.None;
            selectedModuleHabitationLabel.style.display = DisplayStyle.None;
        }

        selectedModuleName.text = module.Name;
        selectedModuleLevel.text =$"Level: { module.level.ToString()}";
        module.PowerRequirement.ToString();

        UpdateModuleCrewLabel(module);
        uiController.root.Q<Label>("selectedModuleWasteProduction").text =$"Waste produced: {wasteProductionStr}";

        UpdateNextLevelStats(module);
    }

    public void UpdateNextLevelStats(OutpostModule module)
    {
        int nextLevel = module.level + 1;
        selectedModuleNextLevelCrewRequirement.text = $"Next Level Crew Requirement: {module.GetCrewRequirementForLevel(nextLevel)}";
        selectedModuleNextLevelResourceProduction.text = $"Next Level Resource Production: {module.CalculateResourceProduction(nextLevel)}";
        selectedModuleNextLevelWasteProduction.text = $"Next Level Waste Production: {Resource.ListToString(module.CalculateWasteProduction(nextLevel))}";
        selectedModuleNextLevelPowerConsumption.text = $"Next Level Power Consumption: {module.CalculatePowerRequirement(nextLevel)}";
        selectedModuleNextLevelUpgradeTimer.text = $"Upgrade Timer: {module.CalculateUpgradeTime(nextLevel)}";
        selectedModuleNextLevelUpgradeCost.text = $"Next Level Upgrade Cost: {Resource.ListToString(module.upgradeCost)}";
    }

    private void UpgradeSelectedModule(OutpostModule module)
    {
        Outpost selectedOutpost = OutpostManager.PlayerOutpostManager.selectedOutpost;
        if (selectedOutpost == null)
        {
            Debug.LogWarning("No outpost selected");
            return;
        }

        selectedOutpost.UpgradeModule(module);

        // Refresh the module details UI
        uiController.UpdateResurces(module);

    }


    private void AddCrewToModule(OutpostModule module)
    {
        OutpostManager.PlayerOutpostManager.selectedOutpost.ResourceManager.AddCrewToModule(module);
    }

    private void RemoveCrewFromModule(OutpostModule module)
    {
        OutpostManager.PlayerOutpostManager.selectedOutpost.ResourceManager.RemoveCrewFromModule(module);
    }

    public void UpdateModuleCrewLabel(OutpostModule module)
    {
        uiController.root.Q<Label>("selectedModuleCurrentCrew").text =$"Crew {module.CrewCurrent.ToString()}";
        uiController.root.Q<Label>("selectedModuleCrew").text =$"Required crew: {module.CrewRequirement.ToString()}";
    }
}
