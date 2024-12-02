using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private List<SortingGroup> allPiecesSorting;
    private GameObject selectedPiece;
    private int orderNumber = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0f);
            if (hit[0].transform != null)
            {
                if (hit[0].transform.CompareTag("Piece") && !hit[0].transform.GetComponent<Piece>().isPositioned)
                {
                    selectedPiece = hit[0].transform.gameObject;
                    selectedPiece.GetComponent<Piece>().Selected();
                    orderNumber++;
                    selectedPiece.GetComponent<SortingGroup>().sortingOrder = orderNumber;
                    Vector3 pos = selectedPiece.transform.position;
                    pos.z = -orderNumber / 1000f;
                    selectedPiece.transform.position = pos;
                }
                else if (hit[0].transform.name == "Background")
                {
                    GetComponent<CameraSystem>().FollowMouse(true);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectedPiece != null)
            {
                Piece pieceScript = selectedPiece.GetComponent<Piece>();
                pieceScript.CheckPosition();
                pieceScript.Unselected();
                selectedPiece = null;
            }
            else
            {
                GetComponent<CameraSystem>().FollowMouse(false);
            }
        }
        if (selectedPiece != null)
        {
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePoint.x < GameManager.Instance.fieldBoundaryTopLeft.x)
            {
                mousePoint.x = GameManager.Instance.fieldBoundaryTopLeft.x;
            }
            else if (mousePoint.x > GameManager.Instance.fieldBoundaryBottomRight.x)
            {
                mousePoint.x = GameManager.Instance.fieldBoundaryBottomRight.x;
            }

            if (mousePoint.y > GameManager.Instance.fieldBoundaryTopLeft.y)
            {
                mousePoint.y = GameManager.Instance.fieldBoundaryTopLeft.y;
            }
            else if (mousePoint.y < GameManager.Instance.fieldBoundaryBottomRight.y)
            {
                mousePoint.y = GameManager.Instance.fieldBoundaryBottomRight.y;
            }
            selectedPiece.transform.position = new Vector3(mousePoint.x, mousePoint.y, selectedPiece.transform.position.z);
        }

        if (orderNumber > 10000)
        {
            OptimizeOrders();
        }
    }

    private void OptimizeOrders()
    {
        orderNumber = allPiecesSorting.Count - 1;
        List<SortingGroup> allSortingCopy = allPiecesSorting;
        for (int i = 0; i < allPiecesSorting.Count; i++)
        {
            SortingGroup smallest = allPiecesSorting[i];
            for (int j = 0; j < allSortingCopy.Count; j++)
            {
                if (smallest.sortingOrder > allSortingCopy[j].sortingOrder)
                {
                    smallest = allSortingCopy[j];
                }
            }
            smallest.sortingOrder = i;
            Vector3 pos = smallest.transform.position;
            pos.z = -i / 1000f;
            smallest.transform.position = pos;
        }
    }
}
