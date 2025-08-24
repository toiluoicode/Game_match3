using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public int rows;
    public int cols;
    public int Score = 0;
    public int MaxScore = 0;
    [SerializeField]
    private TextMeshProUGUI textScore;
    public GameObject piecePrefabs;
    public Piece[,] pieces;
    private Piece selectedPiece = null;
    private Color[] color = { Color.red, Color.green, Color.blue, Color.yellow };
    void Start()
    {
        pieces = new Piece[rows, cols];
        InitBoard();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.transform.position.z * -1; // khoảng cách tới plane z=0
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit.collider != null)
            {
                Piece piece = hit.collider.GetComponent<Piece>();
                if (piece != null)
                {
                    OnclickedPiece(piece);
                }
            }
        }
        textScore.text = "Score: " + Score; 
    }
    public void InitBoard()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Vector2 pos = new Vector2(c, -r);
                GameObject newOb = Instantiate(piecePrefabs, pos, Quaternion.identity, transform);
                newOb.name = $"{r},{c}";
                Piece newPiece = newOb.GetComponent<Piece>();
                Color randomColor = color[Random.Range(0, color.Length)];
                newPiece.InitPiece(r, c, randomColor);
                pieces[r, c] = newPiece;
            }
        }
    }
    public bool SameColor(Color color1, Color color2)
    {
        float threshold = 0.01f; // sai số chấp nhận
        return  Mathf.Abs(color1.r - color2.r) < threshold &&
                Mathf.Abs(color1.g - color2.g) < threshold &&
                Mathf.Abs(color1.b - color2.b) < threshold &&
                Mathf.Abs(color1.a - color2.a) < threshold;
    }
    public void SwapPiece(Piece pieceA, Piece pieceB)
    {
        pieces[pieceA.row, pieceA.col] = pieceB;
        pieces[pieceB.row, pieceB.col] = pieceA;
        int tempRow = pieceA.row;
        int tempCol = pieceA.col;
        pieceA.row = pieceB.row;
        pieceA.col = pieceB.col;
        pieceB.row = tempRow;
        pieceB.col = tempCol;
        // checkedMatch(pieceA, pieceB);
        Vector2 posA = pieceA.transform.position;
        Vector2 posB = pieceB.transform.position;
        float duration = 0.25f;
        LeanTween.move(pieceA.gameObject, posB, duration).setEase(LeanTweenType.easeOutQuad);
        LeanTween.move(pieceB.gameObject, posA, duration)
        .setEase(LeanTweenType.easeOutQuad)
        .setOnComplete(() =>
        {
            checkedMatch(pieceA, pieceB);
        });
    }
    public bool IsAdjacent(Piece a, Piece b)
    {
        int dx = Mathf.Abs(a.row - b.row);
        int dy = Mathf.Abs(a.col - b.col);
        return (dx + dy == 1);
    }
    public void CheckSwap(Piece a, Piece b)
    {
        if (IsAdjacent(a, b))
        {
            SwapPiece(a, b);
        }
    }
    public void OnclickedPiece(Piece ClickPiece)
    {
        if (selectedPiece == null)
        {
            selectedPiece = ClickPiece;
        }
        else
        {
            Piece pieceA = selectedPiece;
            Piece pieceB = ClickPiece;
            CheckSwap(pieceA, pieceB);
            selectedPiece = null;
        }
    }
    void OnDrawGizmos()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Camera.main.transform.position, worldPos);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit.collider != null)
            {

                Debug.Log("object: " + hit.collider.gameObject.name);
            }
        }
    }
    public void checkedMatch(Piece pieceA, Piece pieceB)
    {
        List<Piece> piecesDestroy = new List<Piece>();
        piecesDestroy.AddRange(GetMatch(pieceA));
        piecesDestroy.AddRange(GetMatch(pieceB));

        if (piecesDestroy.Count > 0)
        {
            Score += piecesDestroy.Count;
            foreach (Piece p in piecesDestroy)
            {
                pieces[p.row, p.col] = null;
                Destroy(p.gameObject);
            }
            DropPiece();
        }

    }
    public List<Piece> GetMatch(Piece piece)
    {
        List<Piece> result = new List<Piece>();
        List<Piece> horizontal = new List<Piece> { piece };
        List<Piece> vertical = new List<Piece> { piece };
        int r = piece.row;
        int c = piece.col;
        Color color = piece.color;
        for (int i = r - 1; i >= 0; i--)
        {
            if (pieces[i, c] != null && SameColor(pieces[i, c].color, color))
            {
                vertical.Add(pieces[i, c]);
            }
            else
            {
                break;
            }
        }
        for (int i = r + 1; i < pieces.GetLength(0); i++)
        {
            if (pieces[i, c] != null && SameColor(pieces[i, c].color, color))
            {
                vertical.Add(pieces[i, c]);
            }
            else
            {
                break;
            }
        }
        if (vertical.Count >= 3)
        {
            result.AddRange(vertical);
        }
        for (int i = c - 1; i >= 0; i--)
        {
            if (pieces[r, i] != null && SameColor(pieces[r, i].color, color))
            {
                horizontal.Add(pieces[r, i]);
            }
            else
            {
                break;
            }
        }
        for (int i = c + 1; i < pieces.GetLength(1); i++)
        {
            if (pieces[r, i] != null && SameColor(pieces[r, i].color, color))
            {
                horizontal.Add(pieces[r, i]);
            }
            else
            {
                break;
            }
        }
        if (horizontal.Count >= 3)
        {
            result.AddRange(horizontal);
        }

        return result;
    }
    public void DropPiece()
    {
        Debug.Log("DropPiece");
        for (int col = 0; col < pieces.GetLength(1); col++)
        {
            int writeRow = pieces.GetLength(0) - 1;
            for (int row = pieces.GetLength(0) - 1; row >= 0; row--)
            {
                if (pieces[row, col] != null)
                {
                    pieces[writeRow, col] = pieces[row, col];
                    pieces[writeRow, col].row = writeRow;
                    pieces[writeRow, col].col = col;
                    Vector2 targetPos = new Vector2(col, -writeRow);
                    float duration = 0.25f;
                    LeanTween.move(pieces[writeRow, col].gameObject, targetPos, duration).setEase(LeanTweenType.easeInQuad);
                    if (writeRow != row)
                    {
                        pieces[row, col] = null;
                    }
                    writeRow--;
                }

            }
        }
        SpawnPiece();
    }
    public void SpawnPiece()
    {
        for (int col = 0; col < pieces.GetLength(1); col++)
        {
            for (int row = 0; row < pieces.GetLength(0); row++)
            {
                if (pieces[row, col] == null)
                {
                    Vector2 spawnPos = new Vector2(col, 1);
                    Vector2 targetPos = new Vector2(col, -row);
                    GameObject newOb = Instantiate(piecePrefabs, spawnPos, Quaternion.identity, transform);
                    newOb.name = $"{row}, {col}";
                    Piece piece = newOb.GetComponent<Piece>();
                    Color color = this.color[Random.Range(0, this.color.Length)];
                    piece.InitPiece(row, col, color);
                    pieces[row, col] = piece;
                    LeanTween.move(newOb, targetPos, 0.25f).setEase(LeanTweenType.easeInQuad);
                }
            }
        }
    }
}
