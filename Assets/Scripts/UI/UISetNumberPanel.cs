using System.Collections;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UISetNumberPanel : MonoBehaviour, IController
{
    public int number;

    private Item item;
    private GameObject slot;

    private TextMeshProUGUI numberText;

    private UIInputAction input;
    private InputAction move;
    private InputAction submit;
    private InputAction cancel;

    private void Awake()
    {
        numberText = transform.Find("Panel/Number").GetComponent<TextMeshProUGUI>();

        input = new UIInputAction();
        move = input.FindAction("Move");
        submit = input.FindAction("Submit");
        cancel = input.FindAction("Cancel");
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Start()
    {
        number = 0;
        numberText.text = number.ToString();

        move.performed += (e) =>
        {
            var upDown = e.ReadValue<Vector2>().y;
            if (upDown > 0)
            {
                number++;
            }
            else if (upDown < 0)
            {
                number--;
            }

            if (number < 0) number = item.count;
            if (number > item.count) number = 0;

            numberText.color = number == item.count ? Color.red : Color.white;

            numberText.text = number.ToString();
        };

        submit.performed += (e) =>
        {
            StartCoroutine(Wait());
            IEnumerator Wait()
            {
                yield return new WaitForEndOfFrame();
                this.SendCommand(new SubItemCommand(item.itemData.id, number, true, true));
                UIManager.Instance.ClosePanel(PanelID.SetNumberPanel);
                EventSystem.current.SetSelectedGameObject(slot);
            }
        };

        cancel.performed += (e) =>
        {
            StartCoroutine(Wait());
            IEnumerator Wait()
            {
                yield return new WaitForEndOfFrame();
                UIManager.Instance.ClosePanel(PanelID.SetNumberPanel);
                EventSystem.current.SetSelectedGameObject(slot);
            }
        };
    }

    public void Setup(Item item, GameObject slot)
    {
        this.item = item;
        this.slot = slot;
    }

    public IArchitecture GetArchitecture()
    {
        return App.Interface;
    }
}