using System.Collections.Generic;
using System;
using UnityEngine;

public class UIRoot : MonoBehaviour
{
    private Dictionary<Type, PanelData> _panelsMap = new Dictionary<Type, PanelData>();
    private List<PanelData> _uiPanels;

    public void Init(Configs configs)
    {
        _uiPanels = new List<PanelData>();

        foreach (var panel in GetComponentsInChildren<UIPanel>(true))
        {
            PanelData newPanelData = new PanelData();
            newPanelData.Panel = panel;
            _uiPanels.Add(newPanelData);
        }

        for (int i = 0; i < _uiPanels.Count; i++)
        {
            _uiPanels[i].Panel.Init(configs);
            _panelsMap.Add(_uiPanels[i].Panel.GetType(), _uiPanels[i]);
        }

        EnablePanel<UpgradesPanel>();
    }

    public void EnablePanel<T>() where T : UIPanel
    {
        for (int i = 0; i < _uiPanels.Count; i++)
        {
            if (_uiPanels[i].Panel.GetType() == (typeof(T)))
            {
                _uiPanels[i].Panel.Enable();
            }
            else
            {
                _uiPanels[i].Panel.Disable();
            }
        }
    }

    public void EnablePanel<T, TPayload>(T type, TPayload payload) where T : UIPanel
    {
        for (int i = 0; i < _uiPanels.Count; i++)
        {
            if (_uiPanels[i].Panel == type)
            {
                _uiPanels[i].Panel.Enable(payload);
            }
            else
            {
                _uiPanels[i].Panel.Disable();
            }
        }
    }

    public void DeactivatePanels()
    {
        for (int i = 0; i < _uiPanels.Count; i++)
        {
            _uiPanels[i].Panel.Disable();
        }
    }

    public T GetPanel<T>() where T : UIPanel
    {
        return (T)_panelsMap[typeof(T)].Panel;
    }

    [Serializable]
    private class PanelData
    {
        public UIPanel Panel;
    }
}