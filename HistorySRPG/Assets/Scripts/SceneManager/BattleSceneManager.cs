using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BattleTileDirection
{
    left,
    middle,
    right,
    leftElevated,
    middleElevated,
    rightElevated
}


public class BattleSceneManager : MonoBehaviour
{
    public Dictionary<Vector2, BattleTileController> allTilesDic = new Dictionary<Vector2, BattleTileController>();
    public float Height1_Y = 0.2f;




    public void Awake()
    {
        GameGrid.battleSceneManager = this;
    }

    public void TileChangeHeight(BattleTileController battleTileController)
    {

    }


    public BattleTileController TileGetNeighbor(BattleTileController battleTileController, BattleTileDirection direction)
    {
        Vector2 v2 = new Vector2(battleTileController.Row, battleTileController.Column);
        if (v2.x % 2 == 1)
        {
            switch (direction)
            {
                case BattleTileDirection.left:
                    v2.x -= 1;
                    break;
                case BattleTileDirection.middle:
                    v2.x -= 1;
                    v2.y += 1;
                    break;
                case BattleTileDirection.right:
                    v2.y += 1;
                    break;
                case BattleTileDirection.leftElevated:
                    v2.y -= 1;
                    break;
                case BattleTileDirection.middleElevated:
                    v2.x += 1;
                    break;
                case BattleTileDirection.rightElevated:
                    v2.x += 1;
                    v2.y += 1;
                    break;
            }
        }
        else
        {
            switch (direction)
            {
                case BattleTileDirection.left:
                    v2.y -= 1;
                    v2.x -= 1;
                    break;
                case BattleTileDirection.middle:
                    v2.x -= 1;
                    break;
                case BattleTileDirection.right:
                    v2.y += 1;
                    break;
                case BattleTileDirection.leftElevated:
                    v2.y -= 1;
                    break;
                case BattleTileDirection.middleElevated:
                    v2.x += 1;
                    v2.y -= 1;
                    break;
                case BattleTileDirection.rightElevated:
                    v2.x += 1;
                    break;
            }
        }

        if (allTilesDic.ContainsKey(v2))
        {
            return allTilesDic[v2];
        }
        else
        {
            return null;
        }
    }
}