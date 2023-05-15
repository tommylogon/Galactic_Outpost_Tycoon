using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static OutpostManager;
using static Resource;

public class UIController : MonoBehaviour
{

    public enum ViewType
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
    public ViewType CurrentView;
    public static UIController instance;

    public VisualTreeAsset overviewContentTemplate;
    public VisualTreeAsset locationSelectionContentTemplate;
    //
    public VisualTreeAsset starmapContentTemplate;


    public VisualElement root;
    public VisualElement GameContentArea;

    private Button overviewButton;
    private Button createOutpostButton;
    private Button resourcesButton;
    private Button outpostModulesButton;
    private Button starmapButton;

    public UIOverview uiOverview;
    public UIResourceBar resourceBar;
   // public UIResources uiResources;
    public UIOutpostModules uiOutpostModules;
    public UILocationSelection uiLocationSelection;
    public UIStarmap uiStarmap;

    
    private SolarSystemManager solarSystemManager;
    public OutpostResourceManager outpostResourceManager;
    public MapManager mapManager;



    


    void Start()
    {
        instance = this;
        root = GetComponent<UIDocument>().rootVisualElement;
        GameContentArea = root.Q<VisualElement>("gameContentArea");
        overviewButton = root.Q<Button>("overviewButton");
        resourcesButton = root.Q<Button>("resourcesButton");
        createOutpostButton = root.Q<Button>("createOutpostButton");
        outpostModulesButton = root.Q<Button>("outpostModulesButton");
        uiOutpostModules.uiController = this;
        resourceBar.uiController = this;




        solarSystemManager = FindObjectOfType<SolarSystemManager>();

        starmapButton = root.Q<Button>("starmapButton");
        starmapButton.clicked += ShowStarmap;
        overviewButton.clicked += ShowOverview;
        resourcesButton.clicked += ShowResources;
        createOutpostButton.clicked += ShowLocationSelection;
        outpostModulesButton.clicked += ShowOutpostModules;
    }

    private void ShowOverview()
    {

        GameContentArea.Clear();
        uiOverview.Show(GameContentArea);
        CurrentView = ViewType.Overview;
    }
    private void ShowResources()
    {
        GameContentArea.Clear();

        uiOutpostModules.Show(GameContentArea);
        // Load the resources content from the UXML file and add it to the gameContentArea
        uiOutpostModules.GenerateOutpostModuleButtons(typeof(ResourceExtractor));
        CurrentView = ViewType.Resources;



    }
    public void UpdateResurces(OutpostModule module)
    {
        if (CurrentView == ViewType.Resources)
        {
            ShowResources();
            uiOutpostModules.ShowModuleDetails(module);
        }
    }
    private void ShowOutpostModules()
    {
        GameContentArea.Clear();
        uiOutpostModules.Show(GameContentArea);
        uiOutpostModules.GenerateOutpostModuleButtons(typeof(Habitation));
        uiOutpostModules.GenerateOutpostModuleButtons(typeof(StorageSpace));
        CurrentView = ViewType.OutpostModules;
    }
    

    private void ShowLocationSelection()
    {
        GameContentArea.Clear();

        VisualElement locationSelectionContent = locationSelectionContentTemplate.CloneTree();
        GameContentArea.Add(locationSelectionContent);
        ScrollView locationList = locationSelectionContent.Q<ScrollView>("locationList");

        foreach (Location location in solarSystemManager.Locations)
        {
            Button locationButton = new Button(() => CreateOutpost(location));
            locationButton.text = location.Name;
            locationList.Add(locationButton);
        }
        CurrentView = ViewType.Overview;
    }

    private void CreateOutpost(Location location)
    {
        // Implement outpost creation logic here
        Debug.Log("Creating an outpost at: " + location.Name);

        PlayerOutpostManager.CreateOutpost(location);
        outpostResourceManager = PlayerOutpostManager.selectedOutpost.ResourceManager;
        resourceBar.InitializeLabels(root);



    }

    
    private void ShowStarmap()
    {
        // Clear existing content
        GameContentArea.Clear();

        // Load the starmap content from the UXML file and add it to the gameContentArea
        VisualElement starmapContent = starmapContentTemplate.CloneTree();
        GameContentArea.Add(starmapContent);
        CurrentView = ViewType.Starmap;
        mapManager.OpenMap();
        mapManager.ShowStarMap();
    }
}
