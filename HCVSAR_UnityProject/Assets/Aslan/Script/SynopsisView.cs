using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.View;

public class SynopsisView : BaseView
{
    public MainBaseVIew mainBaseVIew;
    public Button close;
    
    public void OpenSynopsisView()
    {
        this.Show(true);
        mainBaseVIew.PanelController(true);
        SwitchPanelController();
    }

    private void SwitchPanelController()
    {
        close.onClick.AddListener(() => {
            this.Show(false);
            mainBaseVIew.PanelController(false);
        });
    }
}
