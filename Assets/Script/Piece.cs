using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public int row;
    public int col;
    public Color color;
    public PieceColor pieceColor;
    void Awake()
    {
        pieceColor = GetComponentInChildren<PieceColor>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void InitPiece(int row, int col, Color color)
    {
        this.row = row;
        this.col = col;
        this.color = color;
        pieceColor.color = color;
    }

    
    
}
