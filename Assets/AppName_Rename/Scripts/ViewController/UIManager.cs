using System.Collections;
using System.Collections.Generic;
using AppName_Rename;
using AppName_Rename.Core;
using QFramework;
using UnityEngine;

public class UIManager : MonoBehaviour,IController
{
    void Start()
    {
        var popup = GameObject.FindGameObjectWithTag("PopupRoot");
        if (popup == null)
        {
            Debug.LogError("PopupRoot not found in the scene.");
        }
        var panel = GameObject.FindGameObjectWithTag("PanelRoot");
        if (panel == null)
        {
            Debug.LogError("PanelRoot not found in the scene.");
        }
        this.SendEvent<IOnUIRootSetEvent>(new OnUIRootSetEvent(popup.transform, panel.transform));
    }
    public IArchitecture GetArchitecture()
    {
        return AppArchitecture_Rename.Interface;
    }
}
