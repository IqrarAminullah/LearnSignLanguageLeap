using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum UIType
{
    MainMenu,
    PauseScreen,
    EndScreen,
    transitionScreen
}

public class UIManager : MonoBehaviour
{
    #region private attributes
    protected List<UIController> controllerList { get; set; }
    private UIController activeUI { get; set; }
    [SerializeField]
    private UIType bootMenu;

    #endregion
    #region public methods
    protected void Awake()
    {
        controllerList = GetComponentsInChildren<UIController>().ToList();
        controllerList.ForEach(x => x.gameObject.SetActive(false));
        controllerList.ForEach(x => Debug.Log(x.type));
        SwitchUI(bootMenu);
    }

    public void SwitchUI(UIType targetUI)
    {
        if(activeUI != null)
        {
            activeUI.gameObject.SetActive(false);
        }
        UIController nextUI = controllerList.Find(x => x.type == targetUI);
        if(nextUI != null)
        {
            Debug.Log(nextUI);
            nextUI.gameObject.SetActive(true);
            activeUI = nextUI;
        }
        else
        {
            Debug.LogError("UI destination not found" + targetUI);
        }
    }
    #endregion
}
