using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Pathfinding;
public enum PlayerState
{
    MOVE,
    IDLE,
   
}
public class PlayerMoveController : MonoBehaviour
{

    public float movementSpeed = 1f;
    IsometriCharacteRender isoRenderer;
    Rigidbody2D rbody;
    public GameObject collideEffect;

    public PlayerState STATE =  PlayerState.IDLE;


    int timefp;
    AIDestinationSetter aisetter;
    Vector3 direction;
    Vector3 lastpos; //上一帧的位置
    Vector3 targetpos;
    private void Awake()
    {
        direction = new Vector3();
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometriCharacteRender>();
        aisetter = GetComponent<AIDestinationSetter>();
        lastpos = transform.position;

    }

    void FixedUpdate()
    {
        Vector3 curenPosition = transform.position;
        timefp++;
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetpos = new Vector3(mousePosition.x, mousePosition.y,transform.position.z) ;
            UpdateDirection(targetpos);
            STATE = PlayerState.MOVE;
            timefp = 0;
            float angle = getAngle(curenPosition, targetpos);
            isoRenderer.SetDirection(angle);
        }

      switch(STATE)
        {
            case PlayerState.IDLE:
                isoRenderer.SetStop();
                break;
            case PlayerState.MOVE:
                Vector2 distV2 = targetpos - curenPosition;
                float speedPerMs = Time.deltaTime * 2 * 1f;
                bool updatePos = true;
                Vector3 nextPos = new Vector3();
              if(timefp%20 == 0)  UpdateDirection(targetpos);
                if (distV2.magnitude < speedPerMs)
                {
                    nextPos = targetpos;
                    STATE = PlayerState.IDLE;
                }
                else
                {
                    nextPos.x = curenPosition.x + speedPerMs * direction.x;
                    nextPos.y = curenPosition.y + speedPerMs * direction.y;
                }
                if (updatePos)
                {
                    transform.position = nextPos;
                }
                break;
        }
     
      

    }

     void UpdateDirection(Vector3 tPos)
    {
        tPos.z = transform.position.z;
        Vector2 off = tPos - transform.position;
        direction.x = off.normalized.x;
        direction.y = off.normalized.y;
    }


    public static float getAngle(Vector3 startPoint, Vector3 endPoint)
    {
        float dx = endPoint.x - startPoint.x;
        float dy = endPoint.y - startPoint.y;
        return getAngle1(dx, dy);
    }

    public static float getAngle1(float dx, float dy)
    {
        double angle = Mathf.Atan2(dx, dy);
        if (angle < 0) angle += 2 * Mathf.PI;
        double RADIAN_TO_DEGREE = 180.0 / Mathf.PI;
        angle *= RADIAN_TO_DEGREE;
        return (float)angle;
    }

    void  createClickEffect() {

    }
}
