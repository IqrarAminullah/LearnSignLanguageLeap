using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class UIManager : Singleton<UIManager>
{
    #region private attributes
    private List<UIController> controllerList { get; set; }
    private UIController activeUI { get; set; }
    #endregion

    #region public methods
    protected override void Awake()
    {
        base.Awake();
        controllerList = GetComponentsInChildren<UIController>().ToList();
        controllerList.ForEach(x => x.gameObject.SetActive(false));
        SwitchUI(0);
    }

    public void SwitchUI(int UIIdx)
    {
        if(activeUI != null)
        {
            activeUI.gameObject.SetActive(false);
        }
        UIController nextUI = controllerList.Find(x => x.type == UIIdx);
        if(nextUI != null)
        {
            nextUI.gameObject.SetActive(true);
            activeUI = nextUI;
        }
        else
        {
            Debug.LogError("UI destination not found");
        }
    }
    #endregion
}
