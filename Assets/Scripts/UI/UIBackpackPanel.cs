using QFramework;
using UnityEngine;

public class UIBackpackPanel : UIStorageBasePanel, IController
{
    protected override void Start()
    {
        base.Start();
        SetSelectedGameObject(slots[0]);
    }
}