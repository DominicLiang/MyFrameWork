using UnityEngine;
using UnityEngine.UI;

public class UITestBottomPanel : MonoBehaviour
{
    private bool isTestPanelOpen;

    private Button openBackpack;
    private Button openStore;
    private Button openGM;

    private void Awake()
    {
        openBackpack = transform.Find("Panel/Button/OpenBackpack").GetComponent<Button>();
        openStore = transform.Find("Panel/Button/OpenStore").GetComponent<Button>();
        openGM = transform.Find("Panel/Button/OpenGMPanel").GetComponent<Button>();
    }

    private void Start()
    {
        isTestPanelOpen = true;
        openBackpack.onClick.AddListener(() =>
        {
            UIManager.Instance.ClosePanel(PanelID.StoragePanel);
            UIManager.Instance.OpenPanel(PanelID.BackpackPanel, out var panel);
        });
        openStore.onClick.AddListener(() =>
        {
            UIManager.Instance.ClosePanel(PanelID.BackpackPanel);
            UIManager.Instance.OpenPanel(PanelID.StoragePanel, out var panel);
        });
        openGM.onClick.AddListener(() =>
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
        });
    }
}
