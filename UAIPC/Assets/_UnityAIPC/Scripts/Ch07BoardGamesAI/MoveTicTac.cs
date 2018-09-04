using UnityEngine;
using System.Collections;

public class MoveTicTac : Move
{
    public int x;
    public int y;
    public int player;

    public MoveTicTac(int x, int y, int player)
    {
        this.x = x;
        this.y = y;
        this.player = player;
    }
}
