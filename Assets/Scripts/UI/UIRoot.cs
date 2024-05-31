using UnityEngine;

public class UIRoot : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.OpenPanel(PanelID.TestBottomPanel, out var panel);
    }
}
