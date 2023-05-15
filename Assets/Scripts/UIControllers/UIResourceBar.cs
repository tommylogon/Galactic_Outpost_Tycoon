using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Resource;

public class UIResourceBar : MonoBehaviour
{
    public UIController uiController;

    private Label moneyLabel;
    private Label powerLabel;
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
    private Label foodLabel;
    private Label storageLabel;
    private Label hangarSpaceLabel;

    


    private void StartUpdatingLabels()
    {
        StartCoroutine(UpdateLabelsCoroutine());
    }
    private IEnumerator UpdateLabelsCoroutine()
    {
        while (true)
        {
            if(uiController.outpostResourceManager != null)
            {
                UpdateHabitationLabels();
                UpdateStorageLabels();
                UpdateResourceLabelsFromManager();

            }
            yield return new WaitForSeconds(1f);
        }
    }
    public void UpdateResourceLabelsFromManager()
    {
        Dictionary<string, int> resourceData = new Dictionary<string, int>();

        foreach (var resource in uiController.outpostResourceManager.resources)
        {
            resourceData[resource.Key.ToString()] = resource.Value;
        }

        UpdateResourceValues(resourceData);
    }
    public void InitializeLabels(VisualElement resourcePanel)
    {
        moneyLabel = resourcePanel.Q<Label>("moneyLabel");
        metalsLabel = resourcePanel.Q<Label>("metalsLabel");
        mineralsLabel = resourcePanel.Q<Label>("mineralsLabel");
        gasLabel = resourcePanel.Q<Label>("gasLabel");
        foodLabel = resourcePanel.Q<Label>("foodLabel");
        powerLabel = resourcePanel.Q<Label>("powerLabel");
        agriculturalSuppliesLabel = resourcePanel.Q<Label>("agriculturalSuppliesLabel");
        alloysLabel = resourcePanel.Q<Label>("alloysLabel");
        consumerGoodsLabel = resourcePanel.Q<Label>("consumerGoodsLabel");
        medicalSuppliesLabel = resourcePanel.Q<Label>("medicalSuppliesLabel");
        wasteLabel = resourcePanel.Q<Label>("wasteLabel");
        viceLabel = resourcePanel.Q<Label>("viceLabel");
        scrapLabel = resourcePanel.Q<Label>("scrapLabel");
        habitationLabel = resourcePanel.Q<Label>("habitationLabel");
        storageLabel = resourcePanel.Q<Label>("storageLabel");
        hangarSpaceLabel = resourcePanel.Q<Label>("hangarSpaceLabel");

        StartUpdatingLabels();
    }


    public void UpdateResourceValues(Dictionary<string, int> resourceData)
    {
        moneyLabel.text = $"Money: {resourceData["Money"]}";
        metalsLabel.text = $"Metals: {resourceData["Metals"]}";
        mineralsLabel.text = $"Minerals: {resourceData["Minerals"]}";
        gasLabel.text = $"Gas: {resourceData["Gas"]}";
        foodLabel.text = $"Food: {resourceData["Food"]}";
        powerLabel.text = $"Power: {resourceData["Power"]}";
        agriculturalSuppliesLabel.text = $"Agricultural Supplies: {resourceData["AgriculturalSupplies"]}";
        alloysLabel.text = $"Alloys: {resourceData["Alloys"]}";
        consumerGoodsLabel.text = $"Consumer Goods: {resourceData["ConsumerGoods"]}";
        medicalSuppliesLabel.text = $"Medical Supplies: {resourceData["MedicalSupplies"]}";
        wasteLabel.text = $"Waste: {resourceData["Waste"]}";
        viceLabel.text = $"Vice: {resourceData["Vice"]}";
        scrapLabel.text = $"Scrap: {resourceData["Scrap"]}";
        hangarSpaceLabel.text = $"Hangar Space: {resourceData["HangarSpace"]}";
    }

    public void UpdateStorageLabels()
    {
        var storageData = uiController.outpostResourceManager.GetStorageData();
        storageLabel.text = $"Storage: {storageData.totalCurrentStorage}/{storageData.totalMaxStorage}";

    }
    public void UpdateHabitationLabels()
    {
        var habitationData = uiController.outpostResourceManager.GetHabitationData();
        habitationLabel.text = $"Habitation: {habitationData.CrewIdle}/{habitationData.CrewWorking}/{habitationData.MaxPopulation}";
    }
}
