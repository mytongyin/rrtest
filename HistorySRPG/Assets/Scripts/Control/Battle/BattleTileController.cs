using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

//战场沙盘上的格子的控制类
public class BattleTileController : MonoBehaviour
{

    [Header("行")]
    public int Row;

    [Header("列")]
    public int Column;

    [Header("高度")]
    public int Height;

    Vector3 basePosition;
    Transform Canvas;
    Transform Up;
    Transform Down_1h;
    Transform Down;

    BattleSceneManager SceneManager;

    public void Init(int row, int col, int height, int totalRow)
    {
        Row = row;
        Column = col;
        Height = height;
        basePosition = this.transform.position;
        Canvas = this.transform.Find("Canvas");
        Up = this.transform.Find("Up");
        Down_1h = this.transform.Find("Down_1h");
        Down = this.transform.Find("Down");
        SceneManager = GameGrid.battleSceneManager;
        SceneManager.allTilesDic.Add(new Vector2(row, col), this);


        int sortOrderMiddle = (totalRow - row) * 1000;
        Canvas.GetComponent<Canvas>().sortingOrder = sortOrderMiddle;
        Up.GetComponent<SortingGroup>().sortingOrder = sortOrderMiddle + 1;
        Down_1h.GetComponent<SortingGroup>().sortingOrder = sortOrderMiddle - 1;
        Down.GetComponent<SortingGroup>().sortingOrder = sortOrderMiddle - 2;
        this.transform.Find("1h-step-left").GetComponent<SpriteRenderer>().sortingOrder = sortOrderMiddle + 1000;
        this.transform.Find("1h-step-middle").GetComponent<SpriteRenderer>().sortingOrder = sortOrderMiddle + 1000;
        ChangeSortOrderByHeight(height);
    }


    public void ChangeSortOrderByHeight(int heightChangeNum)
    {
        this.transform.Find("Canvas").GetComponent<Canvas>().sortingOrder += (heightChangeNum * 10);
        this.transform.Find("Up").GetComponent<SortingGroup>().sortingOrder += (heightChangeNum * 10);
        this.transform.Find("Down").GetComponent<SortingGroup>().sortingOrder += (heightChangeNum * 10);
        this.transform.Find("Down_1h").GetComponent<SortingGroup>().sortingOrder += (heightChangeNum * 10);
        this.transform.Find("1h-step-left").GetComponent<SpriteRenderer>().sortingOrder += (heightChangeNum * 10);
        this.transform.Find("1h-step-middle").GetComponent<SpriteRenderer>().sortingOrder += (heightChangeNum * 10);
    }

    public void ClearView()
    {
        foreach (Transform child in Up)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in Down_1h)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in Down)
        {
            child.gameObject.SetActive(false);
        }
        this.transform.Find("1h-step-left").gameObject.SetActive(false);
        this.transform.Find("1h-step-middle").gameObject.SetActive(false);
        this.transform.position = basePosition;
    }


    public void ChangeViewByHeightChange(bool noRecursion)
    {
        ClearView();
        BattleTileController leftBattleTileController = SceneManager.TileGetNeighbor(this, BattleTileDirection.left);
        if (leftBattleTileController && !noRecursion)
        {
            leftBattleTileController.NeighborChangeHeight(this, BattleTileDirection.rightElevated);
        }

        BattleTileController middleBattleTileController = SceneManager.TileGetNeighbor(this, BattleTileDirection.middle);
        if (middleBattleTileController && !noRecursion)
        {
            middleBattleTileController.NeighborChangeHeight(this, BattleTileDirection.middleElevated);
        }

        BattleTileController rightBattleTileController = SceneManager.TileGetNeighbor(this, BattleTileDirection.right);
        if (rightBattleTileController && !noRecursion)
        {
            rightBattleTileController.NeighborChangeHeight(this, BattleTileDirection.leftElevated);
        }

        BattleTileController leftElevatedBattleTileController = SceneManager.TileGetNeighbor(this, BattleTileDirection.leftElevated);
        if (leftElevatedBattleTileController && !noRecursion)
        {
            leftElevatedBattleTileController.NeighborChangeHeight(this, BattleTileDirection.right);
        }

        BattleTileController middleElevatedBattleTileController = SceneManager.TileGetNeighbor(this, BattleTileDirection.middleElevated);
        if (middleElevatedBattleTileController && !noRecursion)
        {
            middleElevatedBattleTileController.NeighborChangeHeight(this, BattleTileDirection.middle);
        }

        BattleTileController rightElevatedBattleTileController = SceneManager.TileGetNeighbor(this, BattleTileDirection.rightElevated);
        if (rightElevatedBattleTileController && !noRecursion)
        {
            rightElevatedBattleTileController.NeighborChangeHeight(this, BattleTileDirection.left);
        }

        if (Height > 0)
        {
            this.transform.position += new Vector3(0, GameGrid.battleSceneManager.Height1_Y * Height, 0);
        }
        if (Height >= 1)
        {
            //if (Height == 2)
            //{
            //    Down.Find("vertical2").gameObject.SetActive(true);
            //    Down.Find("vertical1").gameObject.SetActive(true);
            //}
            //else
            //{
            //    //todo
            //}
            GameObject vertical = Down.Find("vertical1").gameObject;
            for (int i = 0; i < Height; i++)
            {
                GameObject curVertical = GameObject.Instantiate(vertical, Down.transform.position + new Vector3(0, -i * GameGrid.battleSceneManager.Height1_Y, 0), Quaternion.identity) as GameObject;
                curVertical.transform.SetParent(Down);
                curVertical.SetActive(true);
            }
        }
        if (leftBattleTileController && Height - leftBattleTileController.Height == 1)
        {
            this.transform.Find("1h-step-left").gameObject.SetActive(true);
        }
        if (middleBattleTileController && Height - middleBattleTileController.Height == 1)
        {
            this.transform.Find("1h-step-middle").gameObject.SetActive(true);
        }
        if (rightBattleTileController && Height - rightBattleTileController.Height == 1)
        {
            Down_1h.Find("1h-step-right").gameObject.SetActive(true);
        }
        if (leftElevatedBattleTileController && Height - leftElevatedBattleTileController.Height == 1)
        {
            Down_1h.Find("top left elevated").gameObject.SetActive(true);
        }
        if (middleElevatedBattleTileController && Height - middleElevatedBattleTileController.Height == 1)
        {
            Down_1h.Find("top middle elevated").gameObject.SetActive(true);
        }
        if (rightElevatedBattleTileController && Height - rightElevatedBattleTileController.Height == 1)
        {
            Down_1h.Find("top right elevated").gameObject.SetActive(true);
        }
    }


    public void ChangeHeight(int changeNum)
    {
        ChangeSortOrderByHeight(changeNum);

        if (changeNum > 0)
        {
            for (int i = 0; i < changeNum; i++)
            {
                Raise();
            }
        }
        if (changeNum < 0)
        {
            for (int i = changeNum; i < 0; i++)
            {
                Lower();
            }
        }
    }

    public void NeighborChangeHeight(BattleTileController battleTileController, BattleTileDirection direction)
    {
        ChangeViewByHeightChange(true);

    }

    public void Raise()
    {
        Height++;
        ChangeViewByHeightChange(false);
    }

    public void Lower()
    {
        if (Height > 0)
        {
            Height--;
            ChangeViewByHeightChange(false);
        }
    }


    //public void
}


