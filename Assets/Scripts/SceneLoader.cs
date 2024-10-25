using UnityEngine;

public class SceneLoader
{
    private static Sprite selectedSprite;
    private static int selectedIndex, levelIndex;

    public static void SetSelectedIndex(int index) { selectedIndex = index; }
    public static int GetSelectedIndex() { return selectedIndex; }
    public static void SetLevelIndex(int index) { levelIndex = index; }
    public static int GetLevelIndex() { return levelIndex; }
    public static void SetSelectedSprite(Sprite sprite) { selectedSprite = sprite; }
    public static Sprite GetSelectedSprite() { return selectedSprite; }
}
