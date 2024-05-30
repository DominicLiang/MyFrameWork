using UnityEngine;
using UnityEngine.InputSystem;

public class UIRoot : MonoBehaviour
{
    private InputAction start;

    private void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();
        start = playerInput.actions.FindAction("Start");
    }

    private void Update()
    {
        if (start.triggered)
        {
            UIManager.Instance.OpenPanel(PanelID.TestBottomPanel, out var panel);
        }
    }
}
