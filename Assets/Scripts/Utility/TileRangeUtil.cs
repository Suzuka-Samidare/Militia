using System;
using UnityEngine;

public static class TileRangeUtil
{
    public static void ForEachSquareRange(int centerY, int centerX, int range, Action<int, int> action)
    {
        for (int y = centerY - range; y <= centerY + range; y++)
        {
            for (int x = centerX - range; x <= centerX + range; x++)
            {
                // 座標を引数として設定して外部からDoSomething
                action(y, x);
            }
        }
    }

    public static void ForEachManhattanRange(int centerY, int centerX, int range, Action<int, int> action)
    {
        for (int y = centerY - range; y <= centerY + range; y++)
        {
            // Y軸方向の距離を計算
            int distY = Mathf.Abs(y - centerY);
            // 残りの移動可能距離（range - distY）がX軸に割ける最大幅になる
            int xRemaining = range - distY;

            for (int x = centerX - xRemaining; x <= centerX + xRemaining; x++)
            {
                // 座標を引数として設定して外部からDoSomething
                action(y, x);
            }
        }
    }
}
