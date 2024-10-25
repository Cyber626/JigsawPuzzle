using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float animationTime = 1f, pieceOffsetDetection = .5f;
    public Vector2 pieceStartPositionTopLeft, pieceStartPositionBottomRight, fieldBoundaryTopLeft, fieldBoundaryBottomRight;
    [HideInInspector] public Sprite chosenSprite;
    [SerializeField] private GameObject musicButton;
    [SerializeField] private int piecesInLevel;
    [SerializeField] private AudioSource levelFinishedAudio;
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        chosenSprite = SceneLoader.GetSelectedSprite();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        BackgroundMusicManager.Instance.ChangeMusicButton(musicButton);
    }

    public static void ReturnMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void PiecePlaced()
    {
        audioSource.Play();
        piecesInLevel--;
        if (piecesInLevel == 0)
        {
            levelFinishedAudio.Play();
            YandexGame.savesData.finishedLevels[SceneLoader.GetSelectedIndex() * 5 + SceneLoader.GetLevelIndex()] = true;
            YandexGame.SaveProgress();
        }
    }
}
