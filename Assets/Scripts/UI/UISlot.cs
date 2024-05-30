using System;
using System.Collections;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlot : MonoBehaviour, IController, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    public Item item;
    public event Action<Item> OnSubmitEvent;
    public event Action<UISlot> OnDestroyEvent;
    public event Action<GameObject, Item> OnSelectEvent;

    private Image icon;
    private Image background;
    private TextMeshProUGUI count;

    private bool isSelected;

    private void Awake()
    {
        icon = transform.Find("Icon").GetComponent<Image>();
        background = transform.Find("Background").GetComponent<Image>();
        count = transform.Find("CountText").GetComponent<TextMeshProUGUI>();

        isSelected = true;
    }

    public void Refresh(Item item)
    {
        this.item = item;

        if (item != null && item.count > 0)
        {
            var spriteId = item.itemData.icon.Split('_');
            var handle = Addressables.LoadAssetAsync<Sprite[]>(spriteId[0]);
            icon.sprite = handle.WaitForCompletion()[int.Parse(spriteId[1])];
            icon.color = isSelected ? Color.white : Color.gray;
            count.text = item.count.ToString();
        }
        else
        {
            icon.sprite = null;
            icon.color = new Color(1, 1, 1, 0.005f);
            count.text = string.Empty;
        }
    }

    public void Selected(bool isSelected)
    {
        if (this.isSelected == isSelected) return;
        if (isSelected)
        {
            this.isSelected = true;
            if (item.count > 0) icon.color = Color.white;
            background.color = Color.white;
            count.color = Color.white;
        }
        else
        {
            this.isSelected = false;
            if (item.count > 0) icon.color = Color.gray;
            background.color = Color.gray;
            count.color = Color.gray;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(WaitForFrameEnd());
        IEnumerator WaitForFrameEnd()
        {
            yield return new WaitForEndOfFrame();
            OnSelectEvent?.Invoke(gameObject, item);
            background.color = Color.green;
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        background.color = Color.white;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        OnSubmitEvent?.Invoke(item);
    }

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke(this);
    }

    public IArchitecture GetArchitecture()
    {
        return App.Interface;
    }
}
