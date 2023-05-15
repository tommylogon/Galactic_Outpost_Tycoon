//using System.Linq;
//using UnityEngine;
//using UnityEngine.UIElements;
//using static OutpostManager;

//public class UIResources : MonoBehaviour
//{
//    private UIController uiController;
//    public VisualTreeAsset outpostModulesContentTemplate;
//    public VisualTreeAsset OutpostModuleTemplate;

//    private void Start()
//    {
//        uiController = GetComponent<UIController>();
//    }

//    public void ShowResources()
//    {
//        uiController.GameContentArea.Clear();

//        // Load the resources content from the UXML file and add it to the gameContentArea
//        VisualElement resourcesContent = outpostModulesContentTemplate.CloneTree();
//        uiController.GameContentArea.Add(resourcesContent);

//        GenerateOutpostModuleButtons(typeof(ResourceExtractor));
//        uiController.CurrentView = UIController.ViewType.Resources;
//    }

//    public void UpdateResources(OutpostModule module)
//    {
//        if (uiController.CurrentView == UIController.ViewType.Resources)
//        {
//            ShowResources();
//            uiController.ShowModuleDetails(module);
//        }
//    }

//    private void GenerateOutpostModuleButtons(System.Type moduleType)
//    {
//        VisualElement outpostModules = uiController.root.Q<VisualElement>("outpostModules");

//        Outpost selectedOutpost = PlayerOutpostManager.selectedOutpost;

//        if (selectedOutpost == null)
//        {
//            Debug.LogWarning("No outpost selected");
//            return;
//        }

//        foreach (OutpostModule module in selectedOutpost.Modules.Where(m => m.GetType() == moduleType))
//        {
//            // Instantiate the template and set the properties
//            VisualElement outpostModule = OutpostModuleTemplate.CloneTree();
//            outpostModule.Q<Button>("outpostModuleButton").clicked += () => uiController.ShowModuleDetails(module);
//            outpostModule.Q<Image>("outpostModuleImage").sprite = module.image;
//            outpostModule.Q<Label>("outpostModuleLevel").text = "Level: " + module.level.ToString();
//            outpostModule.Q<Label>("selectedModuleName").text = module.Name;
//            outpostModule.Q<Button>("outpostModuleQuickUpgradeButton").clicked += () => uiController.QuickUpgradeModule(module);

//            // Add the module button to the bottom row
//            outpostModules.Add(outpostModule);
//        }
//    }
//}
