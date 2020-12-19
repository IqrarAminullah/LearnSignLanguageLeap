using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum UITYpe
{
    MainMenu,
    Lesson,
    Learn,
    Train
}

public class UIManager : Singleton<UIManager>
{
    #region private attributes
    private List<UIController> controllerList { get; set; }
    private UIController activeUI { get; set; }
    [SerializeField]
    private UITYpe bootMenu;
    #endregion

    #region public methods
    protected override void Awake()
    {
        base.Awake();
        controllerList = GetComponentsInChildren<UIController>().ToList();
        controllerList.ForEach(x => x.gameObject.SetActive(false));
        SwitchUI(bootMenu);
    }

    public void SwitchUI(UITYpe targetUI)
    {
        if(activeUI != null)
        {
            activeUI.gameObject.SetActive(false);
        }
        UIController nextUI = controllerList.Find(x => x.type == targetUI);
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
