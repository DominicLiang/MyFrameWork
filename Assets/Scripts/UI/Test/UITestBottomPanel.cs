using UnityEngine;
using UnityEngine.EventSystems;

public class UITestBottomPanel : MonoBehaviour
{
    private bool isTestPanelOpen;

    private UIBaseButton openBackpack;
    private UIBaseButton openStore;
    private UIBaseButton openGM;

    private GameObject currentSelect;

    private void Awake()
    {
        openBackpack = transform.Find("Panel/Button/OpenBackpack").GetComponent<UIBaseButton>();
        openStore = transform.Find("Panel/Button/OpenStore").GetComponent<UIBaseButton>();
        openGM = transform.Find("Panel/Button/OpenGMPanel").GetComponent<UIBaseButton>();

        currentSelect = openBackpack.gameObject;
    }

    private void Start()
    {
        isTestPanelOpen = true;

        openBackpack.OnSubmitEvent += (e) =>
        {
            UIManager.Instance.OpenPanel(PanelID.BackpackPanel, out var panel);
            UIManager.Instance.ClosePanel(PanelID.StoragePanel);
            UIManager.Instance.ClosePanel(PanelID.TestBottomPanel);

            openBackpack.isDefaultSelect = true;
            openStore.isDefaultSelect = false;
            openGM.isDefaultSelect = false;
            currentSelect = openBackpack.gameObject;
        };

        openStore.OnSubmitEvent += (e) =>
        {
            UIManager.Instance.OpenPanel(PanelID.StoragePanel, out var panel);
            UIManager.Instance.ClosePanel(PanelID.BackpackPanel);
            UIManager.Instance.ClosePanel(PanelID.TestBottomPanel);

            openBackpack.isDefaultSelect = false;
            openStore.isDefaultSelect = true;
            openGM.isDefaultSelect = false;
            currentSelect = openStore.gameObject;
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

            openBackpack.isDefaultSelect = false;
            openStore.isDefaultSelect = false;
            openGM.isDefaultSelect = true;
            currentSelect = openGM.gameObject;
        };
    }

    private void OnEnable()
    {
        SetActiveButton();
        openBackpack.Reset();
        openStore.Reset();
        openGM.Reset();
    }

    private void SetActiveButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(currentSelect);
    }
}
