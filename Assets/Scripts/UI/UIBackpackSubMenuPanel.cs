using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBackpackSubMenuPanel : MonoBehaviour, IController
{
    private UIBaseButton blend;
    private UIBaseButton drop;

    private Item item;
    private GameObject slot;

    private void Awake()
    {
        blend = transform.Find("Panel/Btn/BtnBlend").GetComponent<UIBaseButton>();
        drop = transform.Find("Panel/Btn/BtnDrop").GetComponent<UIBaseButton>();
    }

    private void Start()
    {
        blend.OnSubmitEvent += (e) =>
        {
        };

        drop.OnSubmitEvent += (e) =>
        {
            UIManager.Instance.OpenPanel(PanelID.SetNumberPanel, out var panel);
            panel.GetComponent<UISetNumberPanel>().Setup(item, slot);
            var panelTransform = panel.transform.Find("Panel").GetComponent<RectTransform>();
            var targetTransform = panel.transform.Find("BackpackPosition").GetComponent<RectTransform>();
            panelTransform.anchoredPosition = targetTransform.anchoredPosition;
            ClosePanel();
        };

        blend.OnCancelEvent += (e) => ClosePanel();
        drop.OnCancelEvent += (e) => ClosePanel();
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(blend.gameObject);
        blend.Reset();
        drop.Reset();
    }

    public void Setup(Item item, GameObject slot)
    {
        this.item = item;
        this.slot = slot;
    }

    private void ClosePanel()
    {
        UIManager.Instance.ClosePanel(PanelID.BackpackSubMenuPanel);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public IArchitecture GetArchitecture()
    {
        return App.Interface;
    }
}
