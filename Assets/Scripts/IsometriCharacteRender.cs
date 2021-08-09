using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometriCharacteRender : MonoBehaviour
{
    public static readonly string[] staticDirections = { "Static N", "Static NE", "Static E", "Static SE", "Static S", "Static SW", "Static W", "Static NW" };
    public static readonly string[] runDirections = { "Run N", "Run NE","Run E", "Run SE", "Run S", "Run SW", "Run W", "Run NW"};
   public static readonly int[] angledirections = { 1, 1, 1, 1, 1, -1, -1, -1, -1 };
    Animator animator;
    int lastDirection;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void SetDirection(float angle)
    {

       string[] directionArray = runDirections;
        Debug.Log(" angle :" + angle);
        angle = angle % 360;
        lastDirection = (int)(angle / 45) ;

        if (angle % 45 > (22.5))
        {
            lastDirection += 1;

        }
        if (lastDirection >= 8)
        {
            lastDirection = lastDirection - 8;
        }
        else if (lastDirection < 0)
        {
            lastDirection = lastDirection+  8;

        }
        Debug.Log("lastDirection:" + directionArray[lastDirection]+"      "+ lastDirection);
        transform.localScale = new Vector3(angledirections[lastDirection], transform.localScale.y, 1);
        animator.Play(directionArray[lastDirection]);

    }


    public void SetStop() {
        transform.localScale = new Vector3(1, 1, 1);
        animator.Play(staticDirections[lastDirection]);

    }

}
