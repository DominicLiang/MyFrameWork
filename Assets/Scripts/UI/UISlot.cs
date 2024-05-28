using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image icon;
    private TextMeshProUGUI count;

    private Item item;
    private UIInventoryPanel parent;

    private void Awake()
    {
        icon = transform.Find("Icon").GetComponent<Image>();
        count = transform.Find("CountText").GetComponent<TextMeshProUGUI>();
        parent = GetComponentInParent<UIInventoryPanel>();
    }

    public void Refresh(Item item)
    {
        this.item = item;

        if (item != null)
        {
            var spriteId = item.itemData.icon.Split('_');
            var handle = Addressables.LoadAssetAsync<Sprite[]>(spriteId[0]);
            icon.sprite = handle.WaitForCompletion()[int.Parse(spriteId[1])];
            icon.color = Color.white;
            count.text = item.count.ToString();
        }
        else
        {
            icon.sprite = null;
            icon.color = new Color(1, 1, 1, 0.005f);
            count.text = string.Empty;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        parent.SelectedItem = item;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        parent.SelectedItem = null;
    }
}
