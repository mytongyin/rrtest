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



    public void TileChangeHeight(BattleTileController battleTileController)
    {





    }

}