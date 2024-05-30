using QFramework;
using UnityEngine;
using UnityEngine.UI;

public class UITestPanel : MonoBehaviour, IController
{
    private Button add1ToBackpack;
    private Button add2ToBackpack;
    private Button add1ToStorage;
    private Button add2ToStorage;
    private Button sub1FromBackpack;
    private Button sub2FromBackpack;
    private Button sub1FromStorage;
    private Button sub2FromStorage;

    private void Awake()
    {
        add1ToBackpack = transform.Find("Panel/Button/Add1ToBackpack").GetComponent<Button>();
        add2ToBackpack = transform.Find("Panel/Button/Add2ToBackpack").GetComponent<Button>();
        add1ToStorage = transform.Find("Panel/Button/Add1ToStorage").GetComponent<Button>();
        add2ToStorage = transform.Find("Panel/Button/Add2ToStorage").GetComponent<Button>();
        sub1FromBackpack = transform.Find("Panel/Button/Sub1FromBackpack").GetComponent<Button>();
        sub2FromBackpack = transform.Find("Panel/Button/Sub2FromBackpack").GetComponent<Button>();
        sub1FromStorage = transform.Find("Panel/Button/Sub1FromStorage").GetComponent<Button>();
        sub2FromStorage = transform.Find("Panel/Button/Sub2FromStorage").GetComponent<Button>();
    }

    private void Start()
    {
        add1ToBackpack.onClick.AddListener(() => this.SendCommand(new AddItemCommand(1, 1, true)));
        add2ToBackpack.onClick.AddListener(() => this.SendCommand(new AddItemCommand(2, 1, true)));
        add1ToStorage.onClick.AddListener(() => this.SendCommand(new AddItemCommand(1, 1, false)));
        add2ToStorage.onClick.AddListener(() => this.SendCommand(new AddItemCommand(2, 1, false)));
        sub1FromBackpack.onClick.AddListener(() => this.SendCommand(new SubItemCommand(1, 1, true)));
        sub2FromBackpack.onClick.AddListener(() => this.SendCommand(new SubItemCommand(2, 1, true)));
        sub1FromStorage.onClick.AddListener(() => this.SendCommand(new SubItemCommand(1, 1, false)));
        sub2FromStorage.onClick.AddListener(() => this.SendCommand(new SubItemCommand(2, 1, false)));
    }

    public IArchitecture GetArchitecture()
    {
        return App.Interface;
    }
}