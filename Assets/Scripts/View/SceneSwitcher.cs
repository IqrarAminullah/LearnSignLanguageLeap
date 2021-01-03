using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneType
{
    MainMenu,
    Learn,
    Lesson,
    Quiz
}
public class SceneSwitcher : MonoBehaviour
{
    #region attributes
    [SerializeField]
    public SceneType destSceneType;

    Button button;
    #endregion

    #region private methods
    private void Start()
    {
        button = GetComponent<Button>();
        if(button != null)
        {
            button.onClick.AddListener(OnButtonClicked);
        }
    }

    private void OnButtonClicked()
    {
        SwitchScene();
    }
    #endregion
    #region public methods
    public void SwitchScene()
    {
        switch (destSceneType)
        {
            case SceneType.MainMenu:
                SceneManager.LoadScene("Main");
                break;
            case SceneType.Lesson:
                AppData.currentActivity = ActivityType.Lesson;
                SceneManager.LoadScene("Lesson");
                break;
            case SceneType.Learn:
                AppData.currentActivity = ActivityType.Learn;
                SceneManager.LoadScene("Learn");
                break;
            case SceneType.Quiz:
                AppData.currentActivity = ActivityType.Quiz;
                SceneManager.LoadScene("Quiz");
                break;
        }
    }
    #endregion
}