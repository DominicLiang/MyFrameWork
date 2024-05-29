using UnityEngine;

public class UIRoot : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UIManager.Instance.OpenPanel(PanelID.StoragePanel, out GameObject storagePanel);
            UIManager.Instance.OpenPanel(PanelID.BackpackPanel, out GameObject backpackPanel);
            UIManager.Instance.OpenPanel(PanelID.TestPanel, out GameObject testPanel);
            storagePanel.GetComponent<UIStoragePanel>().otherStoragePanel = backpackPanel.GetComponent<UIStoragePanel>();
            backpackPanel.GetComponent<UIStoragePanel>().otherStoragePanel = storagePanel.GetComponent<UIStoragePanel>();
        }
    }
}
