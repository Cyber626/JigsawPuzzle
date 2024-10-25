using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private GameObject imageHoldersPrefab, container, musicButton;
    [Tooltip("Positions are Local")]
    [SerializeField] private Vector3 selectedImagePos, prevImagePos, nextImagePos;
    [SerializeField] private float selectedImageScale = 2f, animationTime = 0.5f;
    private int selected = 0;
    private GameObject selectedImage, prevImage, nextImage, prevPrevImage;
    private Vector3 normalScale, prevPrevImagePos, nextNextImagePos;

    private void Start()
    {
        selected = SceneLoader.GetSelectedIndex();
        prevPrevImagePos = 2 * prevImagePos - selectedImagePos;
        nextNextImagePos = 2 * nextImagePos - selectedImagePos;

        selectedImage = CreateNewImage(selectedImagePos, selected);
        normalScale = selectedImage.transform.localScale;
        selectedImage.transform.localPosition = selectedImagePos;
        selectedImage.transform.localScale = normalScale * selectedImageScale;
        selectedImage.AddComponent<Button>().onClick.AddListener(OnImageSelect);

        if (selected + 1 != sprites.Length)
        {
            nextImage = CreateNewImage(nextImagePos, selected + 1);
        }
        if (selected != 0)
        {
            prevImage = CreateNewImage(prevImagePos, selected - 1);
        }

        BackgroundMusicManager.Instance.ChangeMusicButton(musicButton);
    }

    public void OnNext()
    {
        selected++;
        if (selected < sprites.Length)
        {
            if (prevImage != null)
            {
                prevImage.transform.LeanMoveLocal(prevPrevImagePos, animationTime);
                prevImage.transform.LeanScale(Vector2.zero, animationTime).setOnComplete(DestroyGivenObject, prevImage);
            }

            Destroy(selectedImage.GetComponent<Button>());
            prevImage = selectedImage;
            MoveAndScaleAnimated(prevImage, prevImagePos, normalScale);

            selectedImage = nextImage;
            MoveAndScaleAnimated(selectedImage, selectedImagePos, normalScale * selectedImageScale);
            selectedImage.AddComponent<Button>().onClick.AddListener(OnImageSelect);

            if (selected + 1 != sprites.Length)
            {
                nextImage = CreateNewImage(nextNextImagePos, selected + 1);
                nextImage.transform.localScale = Vector3.zero;
                MoveAndScaleAnimated(nextImage, nextImagePos, normalScale);
            }
            else
            {
                nextImage = null;
            }
        }
        else
        {
            selected--;
        }
    }
    public void OnPrev()
    {
        if (selected > 0)
        {
            selected--;
            if (nextImage != null)
            {
                nextImage.transform.LeanMoveLocal(nextNextImagePos, animationTime);
                nextImage.transform.LeanScale(Vector2.zero, animationTime).setOnCompleteParam(nextImage).setOnComplete(DestroyGivenObject);
            }

            Destroy(selectedImage.GetComponent<Button>());
            nextImage = selectedImage;
            MoveAndScaleAnimated(nextImage, nextImagePos, normalScale);

            selectedImage = prevImage;
            MoveAndScaleAnimated(selectedImage, selectedImagePos, normalScale * selectedImageScale);
            selectedImage.AddComponent<Button>().onClick.AddListener(OnImageSelect);

            if (selected > 0)
            {
                prevImage = CreateNewImage(prevPrevImagePos, selected - 1);
                prevImage.transform.localScale = Vector3.zero;
                MoveAndScaleAnimated(prevImage, prevImagePos, normalScale);
            }
            else
            {
                prevImage = null;
            }
        }
    }
    private GameObject CreateNewImage(Vector3 position, int spriteIndex)
    {
        GameObject refObject = Instantiate(imageHoldersPrefab, container.transform);
        refObject.transform.localPosition = position;
        refObject.transform.Find("Image").GetComponent<Image>().sprite = sprites[spriteIndex];
        int numberCompletedLevel = 0;
        for (int i = 0; i < 5; i++)
        {
            if (YandexGame.savesData.finishedLevels[spriteIndex * 5 + i])
            {
                numberCompletedLevel++;
            }
        }
        refObject.GetComponentInChildren<ProgressBarCircle>().BarValue = 20 * numberCompletedLevel;
        return refObject;
    }

    private void MoveAndScaleAnimated(GameObject gameObject, Vector3 position, Vector3 scale)
    {
        gameObject.transform.LeanMoveLocal(position, animationTime);
        gameObject.transform.LeanScale(scale, animationTime);
    }

    private void OnImageSelect()
    {
        SceneLoader.SetSelectedSprite(sprites[selected]);
        SceneLoader.SetSelectedIndex(selected);
        SceneManager.LoadScene("GameModeScene");
    }

    private static void DestroyGivenObject(object obj)
    {
        GameObject objectToDestroy = obj as GameObject;
        Destroy(objectToDestroy);
    }
}
