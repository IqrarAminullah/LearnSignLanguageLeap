using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UISwitcher : MonoBehaviour
{
    public UIType desiredUIType;

    UIManager UIManager;
    Button menuButton;

    private void Start()
    {
        menuButton = GetComponent<Button>();
        menuButton.onClick.AddListener(OnButtonClicked);
        UIManager = FindObjectOfType<UIManager>();
    }

    void OnButtonClicked()
    {
        SwitchMenu();
    }

    public void SwitchMenu()
    {
        Debug.Log(AppData.currentActivity + " go to " + desiredUIType);
        UIManager.SwitchUI(desiredUIType);
    }
}