using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static OutpostManager;

public class UIController : MonoBehaviour
{

    private enum ViewType
    {
        Overview,
        Resources,
        Employees,
        OutpostModules,
        Missions,
        Shipyard,
        Defence,
        Fleet,
        Starmap
    }
    private ViewType currentView;
    public static UIController instance;

    public VisualTreeAsset overviewContentTemplate;
    public VisualTreeAsset locationSelectionContentTemplate;
    public VisualTreeAsset outpostModulesContentTemplate;

    public VisualTreeAsset OutpostModuleTemplate;

    private VisualElement root;
    private VisualElement gameContentArea;

    private Button overviewButton;
    private Button createOutpostButton;
    private Button resourcesButton;
    private Button outpostModulesButton;

    private SolarSystemManager solarSystemManager;
    private OutpostResourceManager outpostResourceManager;

    private Label moneyLabel;
    private Label metalsLabel;
    private Label mineralsLabel;
    private Label agriculturalSuppliesLabel;
    private Label alloysLabel;
    private Label consumerGoodsLabel;
    private Label gasLabel;
    private Label medicalSuppliesLabel;
    private Label scrapLabel;
    private Label viceLabel;
    private Label wasteLabel;
    private Label habitationLabel;
    private Label storageLabel;
    private Label hangarSpaceLabel;



    void Start()
    {
        instance = this;
        root = GetComponent<UIDocument>().rootVisualElement;
        gameContentArea = root.Q<VisualElement>("gameContentArea");
        overviewButton = root.Q<Button>("overviewButton");
        resourcesButton = root.Q<Button>("resourcesButton");
        createOutpostButton = root.Q<Button>("createOutpostButton");
        outpostModulesButton = root.Q<Button>("outpostModulesButton");

        moneyLabel = root.Q<Label>("moneyLabel");
        metalsLabel = root.Q<Label>("metalsLabel");
        mineralsLabel = root.Q<Label>("mineralsLabel");
        agriculturalSuppliesLabel = root.Q<Label>("agriculturalSuppliesLabel");
        alloysLabel = root.Q<Label>("alloysLabel");
        consumerGoodsLabel = root.Q<Label>("consumerGoodsLabel");
        gasLabel = root.Q<Label>("gasLabel");
        medicalSuppliesLabel = root.Q<Label>("medicalSuppliesLabel");
        scrapLabel = root.Q<Label>("scrapLabel");
        viceLabel = root.Q<Label>("viceLabel");
        wasteLabel = root.Q<Label>("wasteLabel");
        habitationLabel = root.Q<Label>("habitationLabel");
        storageLabel = root.Q<Label>("storageLabel");
        hangarSpaceLabel = root.Q<Label>("hangarSpaceLabel");

        solarSystemManager = FindObjectOfType<SolarSystemManager>();
        

        overviewButton.clicked += ShowOverview;
        resourcesButton.clicked += ShowResources;
        createOutpostButton.clicked += ShowLocationSelection;
        outpostModulesButton.clicked += ShowOutpostModules;
    }

    private void ShowOverview()
    {
        // Clear existing content
        gameContentArea.Clear();

        // Load the overview content from the UXML file and add it to the gameContentArea
        VisualElement overviewContent = overviewContentTemplate.CloneTree();
        gameContentArea.Add(overviewContent);
        currentView = ViewType.Overview;
    }
    private void ShowResources()
    {
        gameContentArea.Clear();
        

        // Load the resources content from the UXML file and add it to the gameContentArea
        VisualElement resourcesContent = outpostModulesContentTemplate.CloneTree();
        gameContentArea.Add(resourcesContent);

        GenerateOutpostModuleButtons(typeof(ResourceExtractor));
        currentView = ViewType.Resources;
    }
    public void UpdateResurces(OutpostModule module)
    {
        if (currentView == ViewType.Resources)
        {
            ShowResources();
            ShowModuleDetails(module);
        }
    }
    private void ShowOutpostModules()
    {
        gameContentArea.Clear();

        // Load the outpost modules content from the UXML file and add it to the gameContentArea
        VisualElement outpostModulesContent = outpostModulesContentTemplate.CloneTree();
        gameContentArea.Add(outpostModulesContent);

        GenerateOutpostModuleButtons(typeof(Habitation));
        GenerateOutpostModuleButtons(typeof(StorageSpace));
        currentView = ViewType.OutpostModules;
    }
    private void GenerateOutpostModuleButtons(System.Type moduleType)
    {
        VisualElement outpostModules = root.Q<VisualElement>("outpostModules");

        Outpost selectedOutpost = PlayerOutpostManager.selectedOutpost;

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


    private void QuickUpgradeModule(OutpostModule module)
    {
        UpgradeSelectedModule(module);
    }

    private void ShowLocationSelection()
    {
        // Clear existing content
        gameContentArea.Clear();

        // Load the location selection content from the UXML file and add it to the gameContentArea
        VisualElement locationSelectionContent = locationSelectionContentTemplate.CloneTree();
        gameContentArea.Add(locationSelectionContent);

        // Get the location list ScrollView
        ScrollView locationList = locationSelectionContent.Q<ScrollView>("locationList");

        // Iterate through available locations and create buttons for each
        foreach (Location location in solarSystemManager.Locations)
        {
            Button locationButton = new Button(() => CreateOutpost(location));
            locationButton.text = location.Name;
            locationList.Add(locationButton);
        }
        currentView = ViewType.Overview;
    }

    private void CreateOutpost(Location location)
    {
        // Implement outpost creation logic here
        Debug.Log("Creating an outpost at: " + location.Name);

        OutpostManager.PlayerOutpostManager.CreateOutpost(location);
        outpostResourceManager = OutpostManager.PlayerOutpostManager.selectedOutpost.ResourceManager;
    }

    private void ShowModuleDetails(OutpostModule module)
    {
        
        Label habitationLabel = root.Q<Label>("habitationLabel");
        Label storageLabel = root.Q<Label>("storageLabel");

        root.Q<Button>("selectedModuleAddCrewButton").clicked += () => AddCrewToModule(module);
        root.Q<Button>("selectedModuleRemoveCrewButton").clicked += () => RemoveCrewFromModule(module);
        // Update the current crew label
        UpdateModuleCrewLabel(module);

        string wasteProductionStr = string.Join(", ", module.wasteProduction.Select(res => $"{res.Type}: {res.Amount}"));
        string upgradeCostStr = string.Join(", ", module.upgradeCost.Select(res => $"{res.Type}: {res.Amount}"));

        root.Q<Label>("selectedModuleUpgradeCost").text = upgradeCostStr;

        if (module is ResourceExtractor extractor)
        {
            root.Q<Label>("selectedModuleResource").text = extractor.ResourceType.ToString();
            root.Q<Label>("selectedModuleExtractionRate").text = extractor.CalculateResourceProduction(extractor.level).ToString();
        }
        else if (module is StorageSpace storageModule)
        {
            storageLabel.style.display = DisplayStyle.Flex;
            storageLabel.text = $"Storage: {storageModule.CurrentResourceStorage.Count}/{storageModule.MaxStorage}";

            habitationLabel.style.display = DisplayStyle.None;
        }
        else if (module is Habitation habitationModule)
        {
            habitationLabel.style.display = DisplayStyle.Flex;
            habitationLabel.text = $"Habitation: {habitationModule.TotalPopulation}/{habitationModule.HabitableSpace}";

            storageLabel.style.display = DisplayStyle.None;
        }
        
        else
        {
            root.Q<Label>("selectedModuleResource").text = "N/A";
            root.Q<Label>("selectedModuleExtractionRate").text = "N/A";
            storageLabel.style.display = DisplayStyle.None;
            habitationLabel.style.display = DisplayStyle.None;
        }
        root.Q<Button>("selectedModuleUpgradeButton").clicked += () => UpgradeSelectedModule(module);
        root.Q<Label>("selectedModuleName").text = module.Name;
        root.Q<Label>("selectedModuleLevel").text = module.level.ToString();
        root.Q<Label>("selectedModuleEnergy").text = module.PowerRequirement.ToString();

        UpdateModuleCrewLabel(module);
        root.Q<Label>("selectedModuleWasteProduction").text = wasteProductionStr;
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
        UpdateResurces(module);

    }
    
    public void UpdateResourceLabels()
    {
        moneyLabel.text = $"Money: {outpostResourceManager.GetResourceAmount(ResourceType.Money)}";
        metalsLabel.text = $"Metals: {outpostResourceManager.GetResourceAmount(ResourceType.Metals)}";
        mineralsLabel.text = $"Minerals: {outpostResourceManager.GetResourceAmount(ResourceType.Minerals)}";
        agriculturalSuppliesLabel.text = $"Agricultural Supplies: {outpostResourceManager.GetResourceAmount(ResourceType.AgriculturalSupplies)}";
        alloysLabel.text = $"Alloys: {outpostResourceManager.GetResourceAmount(ResourceType.Alloys)}";
        consumerGoodsLabel.text = $"Consumer Goods: {outpostResourceManager.GetResourceAmount(ResourceType.ConsumerGoods)}";
        gasLabel.text = $"Gas: {outpostResourceManager.GetResourceAmount(ResourceType.Gas)}";
        medicalSuppliesLabel.text = $"Medical Supplies: {outpostResourceManager.GetResourceAmount(ResourceType.MedicalSupplies)}";
        scrapLabel.text = $"Scrap: {outpostResourceManager.GetResourceAmount(ResourceType.Scrap)}";
        viceLabel.text = $"Vice: {outpostResourceManager.GetResourceAmount(ResourceType.Vice)}";
        wasteLabel.text = $"Waste: {outpostResourceManager.GetResourceAmount(ResourceType.Waste)}";
    }
    public void UpdateStorageLabels(float currentStorage, float maxStorage)
    {
        storageLabel.text = $"Storage: {currentStorage}/{maxStorage}";
        
    }
    public void UpdateHabitationLabels(float crewidle, float crewWorking, float maxPopulation)
    {
        habitationLabel.text = $"Habitation: {crewidle}/{crewWorking}/{maxPopulation}";

    }
    private void AddCrewToModule(OutpostModule module)
    {
        PlayerOutpostManager.selectedOutpost.ResourceManager.AddCrewToModule(module);
    }

    private void RemoveCrewFromModule(OutpostModule module)
    {
        PlayerOutpostManager.selectedOutpost.ResourceManager.RemoveCrewFromModule(module);
    }

    public void UpdateModuleCrewLabel(OutpostModule module)
    {
        root.Q<Label>("selectedModuleCurrentCrew").text = module.CrewCurrent.ToString();
        root.Q<Label>("selectedModuleCrew").text = module.CrewRequirement.ToString();
    }

}
