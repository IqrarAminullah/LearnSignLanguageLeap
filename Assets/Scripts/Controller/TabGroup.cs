using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public List<TabController> tabController;
    public TabButton activeTab;

    private void Start()
    {
        tabController = GetComponentsInChildren<TabController>().ToList();
        foreach(TabController tab in tabController)
        {
            tab.gameObject.SetActive(false);
        }
    }

    public void Subscribe(TabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if(activeTab != button || activeTab == null)
        {
            button.background.color = Color.green;
        }
    }
    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        activeTab = button;
        ResetTabs();
        button.background.color = Color.gray;
        foreach(TabController tab in tabController)
        {
            if(tab.tabIndex == button.desiredTab)
            {
                tab.gameObject.SetActive(true);
            }
            else
            {
                tab.gameObject.SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if(button == activeTab && activeTab != null) { continue; }
            button.background.color = Color.white;
        }
    }
}
