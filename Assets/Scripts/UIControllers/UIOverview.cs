using UnityEngine;
using UnityEngine.UIElements;

public class UIOverview : MonoBehaviour
{
    public VisualTreeAsset overviewContentTemplate;

    public void Show(VisualElement gameContentArea)
    {
        VisualElement overviewContent = overviewContentTemplate.CloneTree();
        gameContentArea.Add(overviewContent);
    }

}