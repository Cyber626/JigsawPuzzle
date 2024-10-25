using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolver : MonoBehaviour
{
    [SerializeField] private List<Piece> pieceList;
    [Tooltip("Time after which another piece is placed")]
    [SerializeField] private float piecePlacingTimeOffset = 1f;
    [Tooltip("Time taken to move piece to origin")]
    [SerializeField] private float piecePlacingTime = 1f;
    private float timer = 0f;
    private void Update()
    {
        if (pieceList.Count > 0)
        {
            if (timer >= piecePlacingTimeOffset)
            {
                timer = 0;
                Piece selectedPiece = pieceList[Random.Range(0, pieceList.Count - 1)];
                pieceList.Remove(selectedPiece);
                selectedPiece.Selected();
                selectedPiece.MoveToOriginalPlace(piecePlacingTime);
                selectedPiece.Unselected();
            }
            timer += Time.deltaTime;
        }
    }
}
