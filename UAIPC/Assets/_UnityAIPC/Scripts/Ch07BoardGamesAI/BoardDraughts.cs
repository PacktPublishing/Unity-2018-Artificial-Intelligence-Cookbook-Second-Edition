using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardDraughts : Board
{
    public int size = 8;
    public int numPieces = 12;
    public GameObject prefab;
    protected PieceDraughts[,] board;

    void Awake()
    {
        board = new PieceDraughts[size, size];
    }

    void Start()
    {
        // TODO
        // initialization and board set up
        // your implementation may vary
        PieceDraughts pd = prefab.GetComponent<PieceDraughts>();
        if (pd == null)
        {
            Debug.LogError("No PieceDraught component detected");
            return;
        }
        int i;
        int j;
        int piecesLeft = numPieces;
        for (i = 0; i < size; i++)
        {
            if (piecesLeft == 0)
                break;
            int init = 0;
            if (i % 2 != 0)
                init = 1;
            for (j = init; j < size; j+=2)
            {
                if (piecesLeft == 0)
                    break;
                PlacePiece(j, i, PieceColor.WHITE);
                piecesLeft--;
            }
        }

        piecesLeft = numPieces;
        for (i = size - 1; i >= 0; i--)
        {
            if (piecesLeft == 0)
                break;
            int init = 0;
            if (i % 2 != 0)
                init = 1;
            for (j = init; j < size; j+=2)
            {
                if (piecesLeft == 0)
                    break;
                PlacePiece(j, i, PieceColor.BLACK);
                piecesLeft--;
            }
        }
    }

    private void PlacePiece(int x, int y, PieceColor color)
    {
        // TODO
        // your own transformations
        // according to space placements
        Vector3 pos = new Vector3();
        pos.x = (float)x;
        pos.y = -(float)y;
        GameObject go = GameObject.Instantiate(prefab);
        go.transform.position = pos;
        PieceDraughts p = go.GetComponent<PieceDraughts>();
        p.Setup(x, y, color);
        board[y, x] = p;
    }

    public override float Evaluate()
    {
        PieceColor color = PieceColor.WHITE;
        if (player == 1)
            color = PieceColor.BLACK;
        return Evaluate(color);
    }

    public override float Evaluate(int player)
    {
        PieceColor color = PieceColor.WHITE;
        if (player == 1)
            color = PieceColor.BLACK;
        return Evaluate(color);
    }

    private float Evaluate(PieceColor color)
    {
        float eval = 1f;
        float pointSimple = 1f;
        float pointSuccess = 5f;
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        int i;
        int j;
        for (i = 0; i < rows; i++)
        {
            for (j = 0; j < cols; j++)
            {
                PieceDraughts p = board[i, j];
                if (p == null)
                    continue;
                if (p.color != color)
                    continue;
                Move[] moves = p.GetMoves(ref board);
                foreach (Move mv in moves)
                {
                    MoveDraughts m = (MoveDraughts)mv;
                    if (m.success)
                        eval += pointSuccess;
                    else
                        eval += pointSimple;
                }
            }
        }
        return eval;
    }

    public override Move[] GetMoves()
    {
        List<Move> moves = new List<Move>();
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        int i;
        int j;
        for (i = 0; i < rows; i++)
        {
            for (j = 0; i < cols; j++)
            {
                PieceDraughts p = board[i, j];
                if (p == null)
                    continue;
                moves.AddRange(p.GetMoves(ref board));
            }
        }
        return moves.ToArray();
    }
}
