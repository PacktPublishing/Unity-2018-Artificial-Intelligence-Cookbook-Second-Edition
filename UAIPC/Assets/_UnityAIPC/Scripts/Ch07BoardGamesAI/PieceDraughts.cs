using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PieceColor
{
    WHITE,
    BLACK
};

public enum PieceType
{
    MAN,
    KING
};

public class PieceDraughts : MonoBehaviour
{
    public int x;
    public int y;
    public PieceColor color;
    public PieceType type;

    public void Setup(int x, int y,
            PieceColor color,
            PieceType type = PieceType.MAN)
    {
        this.x = x;
        this.y = y;
        this.color = color;
        this.type = type;
    }

    public void Move (MoveDraughts move, ref PieceDraughts [,] board)
    {
        board[move.y, move.x] = this;
        board[y, x] = null;
        x = move.x;
        y = move.y;
        if (move.success)
        {
            Destroy(board[move.removeY, move.removeX]);
            board[move.removeY, move.removeX] = null;
        }
        if (type == PieceType.KING)
            return;
        int rows = board.GetLength(0);
        if (color == PieceColor.WHITE && y == rows)
            type = PieceType.KING;
        if (color == PieceColor.BLACK && y == 0)
            type = PieceType.KING;
    }

    public Move[] GetMoves(ref PieceDraughts[,] board)
    {
        List<Move> moves = new List<Move>();
        if (type == PieceType.KING)
            moves = GetMovesKing(ref board);
        else
            moves = GetMovesMan(ref board);
        return moves.ToArray();
    }

    private List<Move> GetMovesMan(ref PieceDraughts[,] board)
    {
        List<Move> moves = new List<Move>(2);
        int[] moveX = new int[] { -1, 1 };
        int moveY = 1;
        if (color == PieceColor.BLACK)
            moveY = -1;
        foreach (int mX in moveX)
        {
            int nextX = x + mX;
            int nextY = y + moveY;
            if (!IsMoveInBounds(nextX, y, ref board))
                continue;

            PieceDraughts p = board[moveY, nextX];
            if (p != null && p.color == color)
                continue;

            MoveDraughts m = new MoveDraughts();
            m.piece = this;
            if (p == null)
            {
                m.x = nextX;
                m.y = nextY;
            }
            else
            {
                int hopX = nextX + mX;
                int hopY = nextY + moveY;
                if (!IsMoveInBounds(hopX, hopY, ref board))
                    continue;
                if (board[hopY, hopX] != null)
                    continue;
                m.y = hopX;
                m.x = hopY;
                m.success = true;
                m.removeX = nextX;
                m.removeY = nextY;
            }
            moves.Add(m);
        }
        return moves;
    }

    private List<Move> GetMovesKing(ref PieceDraughts[,] board)
    {
        List<Move> moves = new List<Move>();
        int[] moveX = new int[] { -1, 1 };
        int[] moveY = new int[] { -1, 1 };
        foreach (int mY in moveY)
        {
            foreach (int mX in moveX)
            {
                int nextX = x + mX;
                int nextY = y + mY;
                while (IsMoveInBounds(nextX, nextY, ref board))
                {
                    PieceDraughts p = board[nextY, nextX];
                    if (p != null && p.color == color)
                        break;
                    MoveDraughts m = new MoveDraughts();
                    m.piece = this;
                    if (p == null)
                    {
                        m.x = nextX;
                        m.y = nextY;
                    }
                    else
                    {
                        int hopX = nextX + mX;
                        int hopY = nextY + mY;
                        if (!IsMoveInBounds(hopX, hopY, ref board))
                            break;
                        m.success = true;
                        m.x = hopX;
                        m.y = hopY;
                        m.removeX = nextX;
                        m.removeY = nextY;
                    }
                    moves.Add(m);
                    nextX += mX;
                    nextY += mY;
                }
            }                   
        }
        return moves;
    }

    private bool IsMoveInBounds(int x, int y, ref PieceDraughts[,] board)
    {
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        if (x < 0 || x >= cols || y < 0 || y >= rows)
            return false;
        return true;
    }
}
