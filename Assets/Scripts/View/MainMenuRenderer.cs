using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuRenderer : MonoBehaviour
{
    #region attributes
    [SerializeField]
    private GameObject flashcardButtonPrefab;
    [SerializeField]
    private GameObject lessonButtonPrefab;
    [SerializeField]
    private GameObject container;
    [SerializeField]
    private Button mainMenuReturnButton;

    private List<GameObject> menuListGameObjects;
    #endregion


    #region private methods

    private void Start()
    {
    }

    void EraseMenuList()
    {
        foreach (var item in menuListGameObjects)
        {
            Destroy(item.gameObject);
        }
        menuListGameObjects.Clear();
    }

    void UpdateLoadFilePath(string filepath)
    {
        AppData.LoadFilePath = filepath;
    }
    #endregion

    #region public methods
    public void CreateMenuList(List<MenuItem> menuItemList, MenuType type)
    {
        if(menuListGameObjects == null)
        {
            menuListGameObjects = new List<GameObject>();
        }
        else
        {
            EraseMenuList();
        }
        GridLayoutGroup containerGrid = container.GetComponent<GridLayoutGroup>();
        containerGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;

        switch (type)
        {
            case MenuType.Lesson:
                containerGrid.constraintCount = 1;
                containerGrid.cellSize = new Vector2(650, 100);
                mainMenuReturnButton.GetComponentInChildren<UISwitcher>().desiredUIType = UIType.MainMenu;
                foreach (MenuItem item in menuItemList)
                {
                    Debug.Log(item.itemFilePath);
                    GameObject itemButton = Instantiate(lessonButtonPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), container.transform);
                    itemButton.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>(item.itemImage);
                    itemButton.GetComponentInChildren<Text>().text = item.itemName;
                    Button button = itemButton.GetComponentInChildren<Button>();
                    button.onClick.AddListener(() => AppData.LoadFilePath = item.itemFilePath);
                    SceneSwitcher switcher = itemButton.GetComponent<SceneSwitcher>();
                    if (switcher == null)
                    {
                        itemButton.AddComponent<SceneSwitcher>();
                    }
                    itemButton.GetComponent<SceneSwitcher>().destSceneType = SceneType.Lesson;

                    menuListGameObjects.Add(itemButton);
                }
                break;
            case MenuType.Learn:
                containerGrid.constraintCount = 4;
                containerGrid.cellSize = new Vector2(150, 150);
                mainMenuReturnButton.GetComponentInChildren<UISwitcher>().desiredUIType = UIType.MainMenu2;
                foreach (MenuItem item in menuItemList)
                {
                    GameObject itemButton = Instantiate(flashcardButtonPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), container.transform);
                    itemButton.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>(item.itemImage);
                    itemButton.GetComponentInChildren<Text>().text = item.itemName;
                    Button button = itemButton.GetComponentInChildren<Button>();
                    button.onClick.AddListener(() => AppData.LoadFilePath = item.itemFilePath);
                    SceneSwitcher switcher = itemButton.GetComponent<SceneSwitcher>();
                    if (switcher == null)
                    {
                        itemButton.AddComponent<SceneSwitcher>();
                    }
                    itemButton.GetComponent<SceneSwitcher>().destSceneType = SceneType.Learn;

                    menuListGameObjects.Add(itemButton);
                }
                break;
            case MenuType.Quiz:
                containerGrid.constraintCount = 4;
                containerGrid.cellSize = new Vector2(150, 150);
                mainMenuReturnButton.GetComponentInChildren<UISwitcher>().desiredUIType = UIType.MainMenu2;
                foreach (MenuItem item in menuItemList)
                {
                    GameObject itemButton = Instantiate(flashcardButtonPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), container.transform);
                    itemButton.GetComponentsInChildren<Image>()[1].sprite = Resources.Load<Sprite>(item.itemImage);
                    itemButton.GetComponentInChildren<Text>().text = item.itemName;
                    Button button = itemButton.GetComponentInChildren<Button>();
                    button.onClick.AddListener(() => AppData.LoadFilePath = item.itemFilePath);
                    Debug.Log(AppData.LoadFilePath+ " " + item.itemFilePath);
                    SceneSwitcher switcher = itemButton.GetComponent<SceneSwitcher>();
                    if (switcher == null)
                    {
                        itemButton.AddComponent<SceneSwitcher>();
                    }
                    itemButton.GetComponent<SceneSwitcher>().destSceneType = SceneType.Quiz;

                    menuListGameObjects.Add(itemButton);
                }
                break;
        }

        Debug.Log(menuListGameObjects.Count);
        Debug.Log(menuItemList.Count);
    }
    #endregion
}
