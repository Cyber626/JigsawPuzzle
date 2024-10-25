using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance { get; private set; }
    private bool isOn = true;
    [SerializeField] private AudioClip[] backgroundMusics;
    [SerializeField] private float audioStartDelay = 1;
    public Sprite onSprite, offSprite;
    private int currentMusicIndex;
    private AudioSource audioSource;
    private GameObject musicButton;
    private Image musicButtonImage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (isOn)
        {
            currentMusicIndex = Random.Range(0, backgroundMusics.Length);
            audioSource.clip = backgroundMusics[currentMusicIndex];
            StartCoroutine(StartWithDelay(audioStartDelay));
        }
        else
        {
            currentMusicIndex = backgroundMusics.Length;
        }


    }

    private IEnumerator StartWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Play();
    }

    private void Update()
    {
        if (isOn && !audioSource.isPlaying)
        {
            int index;
            do
            {
                index = Random.Range(0, backgroundMusics.Length);
            }
            while (index == currentMusicIndex);

            currentMusicIndex = index;
            audioSource.clip = backgroundMusics[currentMusicIndex];
        }
    }

    public void ChangeMusicButton(GameObject newMusicButton)
    {
        musicButton = newMusicButton;
        musicButtonImage = newMusicButton.GetComponent<Image>();
        musicButton.GetComponent<Button>().onClick.AddListener(ToggleOnOff);
        if (isOn)
        {
            musicButtonImage.sprite = onSprite;
        }
        else
        {
            musicButtonImage.sprite = offSprite;
        }
    }

    public void ToggleOnOff()
    {
        isOn = !isOn;
        if (isOn)
        {
            audioSource.Play();
            musicButtonImage.sprite = onSprite;
        }
        else
        {
            audioSource.Pause();
            musicButtonImage.sprite = offSprite;
        }
    }
}
