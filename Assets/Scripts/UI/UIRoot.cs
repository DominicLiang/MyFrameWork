using UnityEngine;

public class UIRoot : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UIManager.Instance.OpenPanel(PanelID.InventoryPanel);
        }
    }
}
