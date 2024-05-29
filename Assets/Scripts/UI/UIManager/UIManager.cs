using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine;

public class UIManager
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            instance ??= new UIManager();
            return instance;
        }
    }

    private Transform uiRoot;

    public Transform UIRoot
    {
        get
        {
            if (uiRoot == null)
            {
                var go = GameObject.Find("UIRoot");
                if (go == null)
                {
                    Debug.LogError("场景中必须有一个名为UIRoot的空物体!");
                    return null;
                }
                uiRoot = go.transform;
            }
            return uiRoot;
        }
    }

    private readonly Dictionary<PanelID, GameObject> cachedPanelDic;
    private readonly Dictionary<PanelID, GameObject> openedPanelDic;

    public UIManager()
    {
        cachedPanelDic = new Dictionary<PanelID, GameObject>();
        openedPanelDic = new Dictionary<PanelID, GameObject>();
    }

    public bool OpenPanel(PanelID id, out GameObject panel)
    {
        if (openedPanelDic.TryGetValue(id, out panel)) return false;

        if (!cachedPanelDic.TryGetValue(id, out panel))
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(id.ToString());

            panel = Object.Instantiate(handle.WaitForCompletion(), UIRoot);

            cachedPanelDic.Add(id, panel);
            openedPanelDic.Add(id, panel);
        }
        else
        {
            panel.SetActive(true);
            openedPanelDic.Add(id, panel);
        }

        return true;
    }

    public bool ClosePanel(PanelID id)
    {
        if (!openedPanelDic.TryGetValue(id, out GameObject openedPanel)) return false;

        openedPanel.SetActive(false);
        openedPanelDic.Remove(id);
        return true;
    }
}