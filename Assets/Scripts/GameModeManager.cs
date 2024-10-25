using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameModeManager : MonoBehaviour
{

    [SerializeField] private Image image;
    [SerializeField] private GameObject musicButton;

    private void Start()
    {
        image.sprite = SceneLoader.GetSelectedSprite();
        BackgroundMusicManager.Instance.ChangeMusicButton(musicButton);
    }

    public static void OnMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
