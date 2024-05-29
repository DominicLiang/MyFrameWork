using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UITooltipPanel : MonoBehaviour, IController
{
    private Image itemIcon;
    private TextMeshProUGUI itemName;
    private TextMeshProUGUI description;
    private TextMeshProUGUI rare;
    private TextMeshProUGUI sellPrice;
    private TextMeshProUGUI storageText;
    private TextMeshProUGUI backpackText;

    private void Awake()
    {
        itemIcon = transform.Find("Panel/Icon").GetComponent<Image>();
        itemName = transform.Find("Panel/ItemName").GetComponent<TextMeshProUGUI>();
        description = transform.Find("Panel/Description").GetComponent<TextMeshProUGUI>();
        rare = transform.Find("Panel/Rare").GetComponent<TextMeshProUGUI>();
        sellPrice = transform.Find("Panel/SellPrice").GetComponent<TextMeshProUGUI>();
        storageText = transform.Find("Panel/StorageText").GetComponent<TextMeshProUGUI>();
        backpackText = transform.Find("Panel/BackpackText").GetComponent<TextMeshProUGUI>();
    }

    public void Refresh(Item item)
    {
        var spriteId = item.itemData.icon.Split('_');
        var handle = Addressables.LoadAssetAsync<Sprite[]>(spriteId[0]);
        itemIcon.sprite = handle.WaitForCompletion()[int.Parse(spriteId[1])];

        itemName.text = item.itemData.itemName;

        description.text = item.itemData.description;

        rare.text = $"RARE {item.itemData.rare}";

        sellPrice.text = item.itemData.sellPrice.ToString();

        var itemFromStorage = this.SendQuery(new GetItemByIdQuery(item.itemData.id, false));
        var numForStorage = itemFromStorage == null ? 0 : itemFromStorage.count;
        storageText.text = numForStorage.ToString();

        var itemFromBackpack = this.SendQuery(new GetItemByIdQuery(item.itemData.id, true));
        var numForBackpack = itemFromBackpack == null ? 0 : itemFromBackpack.count;
        backpackText.text = $"{numForBackpack}/{item.itemData.maxCarry}";
    }

    public IArchitecture GetArchitecture()
    {
        return App.Interface;
    }
}
