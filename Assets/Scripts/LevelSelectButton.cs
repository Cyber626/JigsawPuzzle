using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private int levelIndex;
    [SerializeField] private GameObject levelCompletedNotifier;

    private void Start()
    {
        if (YandexGame.savesData.finishedLevels[SceneLoader.GetSelectedIndex() * 5 + levelIndex])
        {
            levelCompletedNotifier.SetActive(true);
        }
    }

    public void on24Pieces()
    {
        SceneLoader.SetLevelIndex(levelIndex);
        SceneManager.LoadScene("24GameScene");
    }

    public void on54Pieces()
    {
        SceneLoader.SetLevelIndex(levelIndex);
        SceneManager.LoadScene("54GameScene");
    }

    public void on96Pieces()
    {
        SceneLoader.SetLevelIndex(levelIndex);
        SceneManager.LoadScene("96GameScene");
    }

    public void on150Pieces()
    {
        SceneLoader.SetLevelIndex(levelIndex);
        SceneManager.LoadScene("150GameScene");
    }
    public void on384Pieces()
    {
        SceneLoader.SetLevelIndex(levelIndex);
        SceneManager.LoadScene("384GameScene");
    }
}
