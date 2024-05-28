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

    public bool OpenPanel(PanelID id)
    {
        if (openedPanelDic.ContainsKey(id))
        {
            Debug.LogError($"界面已经打开: {id}");
            return false;
        }

        if (!cachedPanelDic.TryGetValue(id, out GameObject panelPrefab))
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(id.ToString());
            var panelObject = Object.Instantiate(handle.WaitForCompletion(), UIRoot, false);
            cachedPanelDic.Add(id, panelObject);
            openedPanelDic.Add(id, panelObject);
        }
        else
        {
            panelPrefab.SetActive(true);
            openedPanelDic.Add(id, panelPrefab);
        }

        return true;
    }

    public bool ClosePanel(PanelID id)
    {
        if (!openedPanelDic.TryGetValue(id, out GameObject openedPanel))
        {
            Debug.LogError($"界面未打开: {id}");
            return false;
        }

        openedPanel.SetActive(false);
        openedPanelDic.Remove(id);
        return true;
    }
}