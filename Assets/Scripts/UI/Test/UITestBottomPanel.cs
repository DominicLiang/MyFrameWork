using UnityEngine;
using UnityEngine.EventSystems;

public class UITestBottomPanel : MonoBehaviour
{
    private bool isTestPanelOpen;

    private UIBaseButton openBackpack;
    private UIBaseButton openStore;
    private UIBaseButton openGM;

    private void Awake()
    {
        openBackpack = transform.Find("Panel/Button/OpenBackpack").GetComponent<UIBaseButton>();
        openStore = transform.Find("Panel/Button/OpenStore").GetComponent<UIBaseButton>();
        openGM = transform.Find("Panel/Button/OpenGMPanel").GetComponent<UIBaseButton>();
    }

    private void Start()
    {
        isTestPanelOpen = true;

        openBackpack.OnSubmitEvent += (e) =>
        {
            UIManager.Instance.ClosePanel(PanelID.StoragePanel);
            UIManager.Instance.OpenPanel(PanelID.BackpackPanel, out var panel);
            UIManager.Instance.ClosePanel(PanelID.TestBottomPanel);
        };

        openStore.OnSubmitEvent += (e) =>
        {
            UIManager.Instance.ClosePanel(PanelID.BackpackPanel);
            UIManager.Instance.OpenPanel(PanelID.StoragePanel, out var panel);
            UIManager.Instance.ClosePanel(PanelID.TestBottomPanel);
        };

        openGM.OnSubmitEvent += (e) =>
        {
            if (isTestPanelOpen)
            {
                UIManager.Instance.OpenPanel(PanelID.TestPanel, out var panel);
            }
            else
            {
                UIManager.Instance.ClosePanel(PanelID.TestPanel);
            }
            isTestPanelOpen = !isTestPanelOpen;
        };
    }

    private void OnEnable()
    {
        SetActiveButton();
    }

    private void SetActiveButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(openBackpack.gameObject);
    }
}
