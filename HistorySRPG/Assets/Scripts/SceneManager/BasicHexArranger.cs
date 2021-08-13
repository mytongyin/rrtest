
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class BasicHexArranger : MonoBehaviour
{
    [Header("行")]
    public int Row = 16;

    [Header("列")]
    public int Column = 16;

    [Header("六边形最上面的面")]
    public Sprite[] tile_cell;

    [Header("六边形到它右边六边形的X距离差")]
    public float tile_width_2 = 1.93f;

    [Header("六边形到它右上六边形的Y高度差")]
    public float tile_height_3 = 1.17f;

    [Header("六边形到它右上六边形的X距离差")]
    public float tile_width_3 = 1.275f;

    [Header("六边形到它左上六边形的X距离差")]
    public float tile_width_4 = -0.659f;

    [Header("六边形左边的1高度接面的位置差")]
    public Vector3 h1_step_left_v3;

    [Header("六边形左边的1高度接面")]
    public Sprite h1_step_left;

    [Header("六边形下方的1高度接面的位置差")]
    public Vector3 h1_step_middle_v3;

    [Header("六边形下方的1高度接面")]
    public Sprite h1_step_middle;

    [Header("六边形右边的1高度接面的位置差")]
    public Vector3 h1_step_right_v3;

    [Header("六边形右边的1高度接面")]
    public Sprite h1_step_right;

    [Header("六边形左上的1高度接面的位置差")]
    public Vector3 h1_top_left_elevated_v3;

    [Header("六边形左上的1高度接面")]
    public Sprite h1_top_left_elevated;

    [Header("六边形中上方的1高度接面的位置差")]
    public Vector3 h1_top_middle_elevated_v3;

    [Header("六边形中上方的1高度接面")]
    public Sprite h1_top_middle_elevated;

    [Header("六边形右上的1高度接面的位置差")]
    public Vector3 h1_top_right_elevated_v3;

    [Header("六边形右上的1高度接面")]
    public Sprite h1_top_right_elevated;

    void Start()
    {
        //X和Y用来记录当前行的第一个
        float X = 0;
        float Y = 0;
        for (int i = 0; i < Row; i++)
        {
            if (i > 0)
            {
                if (i % 2 == 0)
                {
                    X = X + tile_width_4;
                }
                else
                {
                    X = X + tile_width_3;
                }
            }
            for (int j = 0; j < Column; j++)
            {
                float CurX = X + j * tile_width_2;

                UnityEngine.Object TilePrefab = PrefabDataManager.getPrefab("Tile");
                GameObject TileObject = UnityEngine.Object.Instantiate(TilePrefab) as GameObject;


                TileObject.name = "X:" + i + "Y:" + j;
                TileObject.transform.position = new Vector3(CurX, Y, 0f);

                Transform Button = TileObject.transform.Find("Canvas").transform.Find("Button");
                Button.GetComponent<Image>().sprite = tile_cell[Random.Range(0, tile_cell.Length)];

                BattleTileController battleTileController = TileObject.AddComponent<BattleTileController>();
                battleTileController.Init(i, j, 0, Row);

                Button.GetComponent<Button>().onClick.AddListener(delegate ()
                {
                    Debug.Log("!!!!!");
                    battleTileController.ChangeHeight(1);
                });

                //GameObject g = new GameObject("X:" + i + "Y:" + j);
                //g.AddComponent(typeof(SpriteRenderer));
                //g.GetComponent<SpriteRenderer>().sprite = tile_cell[Random.Range(0, tile_cell.Length)];
                //g.GetComponent<SpriteRenderer>().sortingOrder = Row - i;
                //g.transform.position = new Vector3(CurX, Y, 0f);

                //g.AddComponent<Button>().onClick.AddListener(delegate ()
                //{
                //    Debug.Log("!!!!!");
                //});

            }
            Y = Y + tile_height_3;
        }

        // start in upper-left corner, ish.
        //float anchor_x = Camera.main.pixelWidth * 0.01f * -0.5f - tile_width * 0.5f;
        //float anchor_y = Camera.main.pixelHeight * 0.01f * 0.5f - tile_height * 0.5f;
        //float x = 0f;
        //float y = 0f;
        //int row = 1;
        //int decor = 8;

        //foreach (Sprite s in tile_dump) {

        //	// tile sprite
        //	GameObject g = new GameObject (s.name);
        //	g.AddComponent (typeof(SpriteRenderer));
        //	g.GetComponent<SpriteRenderer> ().sprite = s;
        //	g.GetComponent<SpriteRenderer> ().sortingOrder = row;

        //	// underground sprite
        //	GameObject dirt = new GameObject (s.name + "_underground");
        //	dirt.AddComponent (typeof(SpriteRenderer));

        //	// assign proper under"ground" sprite
        //	if (s.name.Contains("Void")) {
        //		dirt.GetComponent<SpriteRenderer> ().sprite = undervoid[Random.Range(0,3)];
        //	}
        //	else if (s.name.Contains("Ocean")) {
        //		dirt.GetComponent<SpriteRenderer> ().sprite = underwater;
        //	} else {
        //		dirt.GetComponent<SpriteRenderer> ().sprite = underground;
        //	}

        //	dirt.GetComponent<SpriteRenderer> ().sortingOrder = row;

        //	float pos_x = anchor_x + x * tile_width;
        //	float pos_y = anchor_y + y * tile_height * 0.75f;

        //	if (row % 2 == 0) {
        //		// offset alternate rows
        //		pos_x += tile_width * 0.5f;
        //	}

        //	g.transform.position = new Vector3 (pos_x,pos_y,0f);

        //	dirt.transform.SetParent (g.transform);
        //	dirt.transform.position = new Vector3 (pos_x,pos_y + tile_under_height * 0.5f ,1f);


        //	if (decor > 0) {

        //		// place some random decor sprites in the first four hexes, just for fun.
        //		Sprite thing_sprite = random_decor[ Random.Range(0,random_decor.Length)];
        //		GameObject thing = new GameObject (thing_sprite.name + decor.ToString());
        //		thing.AddComponent (typeof(SpriteRenderer));
        //		thing.GetComponent<SpriteRenderer> ().sprite = thing_sprite;
        //		thing.GetComponent<SpriteRenderer> ().sortingOrder = row+1;

        //		thing.transform.SetParent (g.transform);
        //		thing.transform.position = new Vector3 (	Random.Range(pos_x - 0.64f, pos_x + 0.64f),
        //													Random.Range(pos_y + tile_height * 0.25f, pos_y + tile_height * 0.75f),
        //													1f);
        //		decor--;
        //	}

        //	x += 1f;

        //	// make rows of eight tiles across
        //	if (x >= 8f) {
        //		x -= 8f;
        //		y--;
        //		row++;
        //	}

        //}
    }
}