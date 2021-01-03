using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum UIType
{
    MainMenu,
    MainMenu2,
    MenuList,
    PauseScreen,
    EndScreen
}

public class UIManager : MonoBehaviour
{
    #region private attributes
    private List<UIController> controllerList { get; set; }
    private UIController activeUI { get; set; }
    [SerializeField]
    private UIType bootMenu;

    #endregion
    #region public methods
    void Awake()
    {
        controllerList = GetComponentsInChildren<UIController>().ToList();
        controllerList.ForEach(x => x.gameObject.SetActive(false));
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
