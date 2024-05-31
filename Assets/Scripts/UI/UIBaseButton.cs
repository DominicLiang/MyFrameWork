using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBaseButton : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, ICancelHandler
{
    public bool isDefaultSelect;

    public event Action<BaseEventData> OnSelectEvent;
    public event Action<BaseEventData> OnDeselectEvent;
    public event Action<BaseEventData> OnSubmitEvent;
    public event Action<BaseEventData> OnCancelEvent;
    public event Action<GameObject> OnDestroyEvent;

    private Image btnImage;

    private void Awake()
    {
        btnImage = GetComponent<Image>();
    }

    private void Start()
    {
        if (!isDefaultSelect) btnImage.color = Color.clear;
    }

    public void OnSelect(BaseEventData eventData)
    {
        btnImage.color = Color.white;
        OnSelectEvent?.Invoke(eventData);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        btnImage.color = Color.clear;
        OnDeselectEvent?.Invoke(eventData);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        OnSubmitEvent?.Invoke(eventData);
    }

    public void OnCancel(BaseEventData eventData)
    {
        OnCancelEvent?.Invoke(eventData);
    }

    void OnDestroy()
    {
        OnDestroyEvent?.Invoke(gameObject);
        OnSelectEvent = null;
        OnDeselectEvent = null;
        OnSubmitEvent = null;
        OnCancelEvent = null;
        OnDestroyEvent = null;
    }
}
