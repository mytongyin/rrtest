
using UnityEngine;

/// <summary>
/// 相机边缘移动
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraScreenEdgeMove : MonoBehaviour
{


    [Header("使用边缘移动")]
    public bool isUseMoveOnScreenEdge = true;

    /// <summary>
    /// 打开调试
    /// </summary>
    public bool isDebugScreenEdge = true;

    //移动速度
    public float moveSpeed = 1f;

    /// <summary>
    /// 距离屏幕边缘多远就开始移动相机
    /// </summary>
    public int ScreenEdgeSize = 20;

    private bool MoveUp;
    private bool MoveDown;
    private bool MoveRight;
    private bool MoveLeft;

    private Rect RigthRect;
    private Rect UpRect;
    private Rect DownRect;
    private Rect LeftRect;

    private Material mat;
    private Vector3 dir = Vector3.zero;

    private void Start()
    {
        CreateLineMaterial();
    }

    private void Update()
    {
        if (isUseMoveOnScreenEdge)
        {
            UpRect = new Rect(1f, Screen.height - ScreenEdgeSize, Screen.width, ScreenEdgeSize);
            DownRect = new Rect(1f, 1f, Screen.width, ScreenEdgeSize);

            LeftRect = new Rect(1f, 1f, ScreenEdgeSize, Screen.height);
            RigthRect = new Rect(Screen.width - ScreenEdgeSize, 1f, ScreenEdgeSize, Screen.height);


            MoveUp = (UpRect.Contains(Input.mousePosition));
            MoveDown = (DownRect.Contains(Input.mousePosition));

            MoveLeft = (LeftRect.Contains(Input.mousePosition));
            MoveRight = (RigthRect.Contains(Input.mousePosition));

            dir.y = MoveUp ? 1 : MoveDown ? -1 : 0;
            dir.x = MoveLeft ? -1 : MoveRight ? 1 : 0;

            transform.position = Vector3.Lerp(transform.position, transform.position + dir * moveSpeed, Time.deltaTime);

        }
    }


    void CreateLineMaterial()
    {
        if (!mat)
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            mat = new Material(shader);
            mat.hideFlags = HideFlags.HideAndDontSave;
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            mat.SetInt("_ZWrite", 0);
        }
    }

    void OnPostRender()
    {
        if (isUseMoveOnScreenEdge && isDebugScreenEdge)
        {
            DrawRect(UpRect, MoveUp, Color.cyan, Color.red);
            DrawRect(DownRect, MoveDown, Color.green, Color.red);
            DrawRect(LeftRect, MoveLeft, Color.yellow, Color.red);
            DrawRect(RigthRect, MoveRight, Color.blue, Color.red);
        }
    }

    private void DrawRect(Rect rect, bool isMouseEnter, Color normalColor, Color HeighLightColor)
    {
        if (isMouseEnter)
        {
            DrawScreenRect(rect, HeighLightColor);
        }
        else
        {
            DrawScreenRect(rect, normalColor);
        }
    }

    private void DrawScreenRect(Rect rect, Color color)
    {
        GL.LoadOrtho();
        GL.Begin(GL.LINES);
        {
            mat.SetPass(0);
            GL.Color(color);
            GL.Vertex3(rect.xMin / Screen.width, rect.yMin / Screen.height, 0);
            GL.Vertex3(rect.xMin / Screen.width, rect.yMax / Screen.height, 0);

            GL.Vertex3(rect.xMin / Screen.width, rect.yMax / Screen.height, 0);
            GL.Vertex3(rect.xMax / Screen.width, rect.yMax / Screen.height, 0);

            GL.Vertex3(rect.xMax / Screen.width, rect.yMax / Screen.height, 0);
            GL.Vertex3(rect.xMax / Screen.width, rect.yMin / Screen.height, 0);

            GL.Vertex3(rect.xMax / Screen.width, rect.yMin / Screen.height, 0);
            GL.Vertex3(rect.xMin / Screen.width, rect.yMin / Screen.height, 0);

        }
        GL.End();
    }
}