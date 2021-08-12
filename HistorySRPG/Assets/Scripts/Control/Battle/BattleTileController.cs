using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//战场沙盘上的格子的控制类
public class BattleTileController : MonoBehaviour
{

    [Header("行")]
    public int Row;

    [Header("列")]
    public int Column;

    [Header("高度")]
    public int Height;

    BattleSceneManager SceneManager;

    public void Init(int row, int col, int height)
    {
        Row = row;
        Column = col;
        Height = height;
        SceneManager = GameGrid.battleSceneManager;
        SceneManager.allTilesDic.Add(new Vector2(row, col), this);
    }


    public void ChangeHeight(int changeNum)
    {
        if (changeNum > 0)
        {

        }



    }

    public void Raise()
    {

    }

    public void Lower()
    {

    }


    //public void
}


