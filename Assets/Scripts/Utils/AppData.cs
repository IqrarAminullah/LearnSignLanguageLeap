using System.Collections;
using System.Collections.Generic;

public enum ActivityType
{
    Menu,
    Lesson,
    Learn,
    Quiz
}
public static class AppData
{
    #region attributes
    public static string LoadFilePath { get; set; }
    public static ActivityType currentActivity { get; set; }
    #endregion
}
