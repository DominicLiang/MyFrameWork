using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBackpackSubMenuPanel : MonoBehaviour, IController
{
    private UIBaseButton btnBlend;
    private UIBaseButton btnDrop;

    private Item item;
    private GameObject slot;

    private void Awake()
    {
        btnBlend = transform.Find("Panel/Btn/BtnBlend").GetComponent<UIBaseButton>();
        btnDrop = transform.Find("Panel/Btn/BtnDrop").GetComponent<UIBaseButton>();
    }

    private void Start()
    {
        btnBlend.OnSubmitEvent += (e) =>
        {
        };

        btnDrop.OnSubmitEvent += (e) =>
        {
            this.SendCommand(new SubItemCommand(item.itemData.id, item.count, true));
            ClosePanel();
        };

        btnBlend.OnCancelEvent += (e) => ClosePanel();
        btnDrop.OnCancelEvent += (e) => ClosePanel();
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(btnBlend.gameObject);
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
        EventSystem.current.SetSelectedGameObject(slot);
    }

    public IArchitecture GetArchitecture()
    {
        return App.Interface;
    }
}
