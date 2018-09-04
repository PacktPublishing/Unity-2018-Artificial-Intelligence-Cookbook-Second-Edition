using UnityEngine;
using System.Collections;

public class BoardAI
{

    public static float Minimax(
            Board board,
            int player,
            int maxDepth,
            int currentDepth,
            ref Move bestMove)
    {
        if (board.IsGameOver() || currentDepth == maxDepth)
            return board.Evaluate(player);

        bestMove = null;
        float bestScore = Mathf.Infinity;
        if (board.GetCurrentPlayer() == player)
            bestScore = Mathf.NegativeInfinity;

        foreach (Move m in board.GetMoves())
        {
            Board b = board.MakeMove(m);
            float currentScore;
            Move currentMove = null;

            currentScore = Minimax(b, player, maxDepth, currentDepth + 1, ref currentMove);

            if (board.GetCurrentPlayer() == player)
            {
                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestMove = currentMove;
                }
            }
            else
            {
                if (currentScore < bestScore)
                {
                    bestScore = currentScore;
                    bestMove = currentMove;
                }
            }
        }

        return bestScore;
    }

    public static float Negamax(
            Board board,
            int maxDepth,
            int currentDepth,
            ref Move bestMove)
    {
        if (board.IsGameOver() || currentDepth == maxDepth)
            return board.Evaluate();

        bestMove = null;
        float bestScore = Mathf.NegativeInfinity;

        foreach (Move m in board.GetMoves())
        {
            Board b = board.MakeMove(m);
            float recursedScore;
            Move currentMove = null;
            recursedScore = Negamax(b, maxDepth, currentDepth + 1, ref currentMove);
            float currentScore = -recursedScore;

            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestMove = m;
            }
        }
        
        return bestScore;
    }

    public static float ABNegamax(
            Board board,
            int player,
            int maxDepth,
            int currentDepth,
            ref Move bestMove,
            float alpha,
            float beta)
    {
        if (board.IsGameOver() || currentDepth == maxDepth)
            return board.Evaluate(player);

        bestMove = null;
        float bestScore = Mathf.NegativeInfinity;
        foreach (Move m in board.GetMoves())
        {
            Board b = board.MakeMove(m);
            float recursedScore;
            Move currentMove = null;
            int cd = currentDepth + 1;
            float max = Mathf.Max(alpha, bestScore);
            recursedScore = ABNegamax(b, player, maxDepth, cd, ref currentMove, -beta, max);

            float currentScore = -recursedScore;

            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestMove = m;

                if (bestScore >= beta)
                    return bestScore;
            }

        }

        return bestScore;
    }


    public static float ABNegascout (
            Board board,
            int player,
            int maxDepth,
            int currentDepth,
            ref Move bestMove,
            float alpha,
            float beta)
    {
        if (board.IsGameOver() || currentDepth == maxDepth)
            return board.Evaluate(player);

        bestMove = null;
        float bestScore = Mathf.NegativeInfinity;

        float adaptiveBeta = beta;
        
        foreach (Move m in board.GetMoves())
        {
            Board b = board.MakeMove(m);

            Move currentMove = null;
            float recursedScore;
            int depth = currentDepth + 1;
            float max = Mathf.Max(alpha, bestScore);
            
            recursedScore = ABNegamax(b, player, maxDepth, depth, ref currentMove, -adaptiveBeta, max);
            float currentScore = -recursedScore;

            if (currentScore > bestScore)
            {
                if (adaptiveBeta == beta || currentDepth >= maxDepth - 2)
                {
                    bestScore = currentScore;
                    bestMove = currentMove;
                }
                else
                {
                    float negativeBest;
                    negativeBest = ABNegascout(b, player, maxDepth, depth, ref bestMove, -beta, -currentScore);
                    bestScore = -negativeBest;
                }

                if (bestScore >= beta)
                    return bestScore;

                adaptiveBeta = Mathf.Max(alpha, bestScore) + 1f;
            }

        } // end foreach

        return bestScore;
    }


}
