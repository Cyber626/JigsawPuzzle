using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using YG;

public class Piece : MonoBehaviour
{
    [HideInInspector] public bool isPositioned = false;
    [SerializeField] private GameObject shadowObject, imageObject;
    private Vector3 originalPosition;

    private void Start()
    {
        imageObject.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.chosenSprite;
        imageObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        originalPosition = new(transform.position.x, transform.position.y, 0);
        Vector2 newPos = new Vector2(Random.Range(GameManager.Instance.pieceStartPositionTopLeft.x, GameManager.Instance.pieceStartPositionBottomRight.x),
            Random.Range(GameManager.Instance.pieceStartPositionTopLeft.y, GameManager.Instance.pieceStartPositionBottomRight.y));
        if (YandexGame.EnvironmentData.isDesktop)
        {
            transform.LeanMoveLocal(newPos, GameManager.Instance.animationTime);
        }
        else
        {
            transform.position = newPos;
        }
    }

    public void Selected()
    {
        shadowObject.transform.position = transform.position + new Vector3(0.1f, -0.1f);
    }

    public void Unselected()
    {
        shadowObject.transform.localPosition = Vector3.zero;
    }

    public void CheckPosition()
    {
        Vector3 temp = new(transform.position.x, transform.position.y, 0);
        if (Vector3.Distance(temp, originalPosition) < GameManager.Instance.pieceOffsetDetection)
        {
            MoveToOriginalPlace(GameManager.Instance.animationTime);
        }
    }

    public void MoveToOriginalPlace(float animationTime)
    {
        GetComponent<SortingGroup>().sortingOrder = 0;
        transform.LeanMoveLocal(originalPosition, animationTime);
        GameManager.Instance.PiecePlaced();
        isPositioned = true;
        Destroy(transform.GetComponent<BoxCollider2D>());
    }

}
