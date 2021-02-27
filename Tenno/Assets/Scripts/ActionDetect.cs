using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionDetect : MonoBehaviour
{


    public GameObject DetectedObject;
    private Collider CorrectCllider;

    public bool CollisonHappen;


    void OnTriggerStay(Collider col)
    {

        //触发器检测：未发生碰撞且标签为墙壁则返回检测物体给检测器
        if (!CollisonHappen)
        {
            if (col.tag == "Wall")
            {

                if (col != null && !col.isTrigger) // checks if the object has the right tag
                {

                    CollisonHappen = true;
                    DetectedObject = col.gameObject;
                    CorrectCllider = col;

                }

            }

        }

        //否则仅是否碰撞为真
        if (!CollisonHappen)
        {

            if (col != null && !col.isTrigger)
            {

                CollisonHappen = true;
                CorrectCllider = col;

            }

        }

    }


    private void Update()
    {

        //触发器判断是否发生碰撞
        if (DetectedObject == null || !CorrectCllider.enabled)
        {

            CollisonHappen = false;

        }
        if (DetectedObject != null)
        {
            if (!DetectedObject.activeInHierarchy)
            {

                CollisonHappen = false;

            }

        }

    }


    void OnTriggerExit(Collider col)
    {

        //触发器离开碰撞体
        if (col == CorrectCllider)
        {

            CollisonHappen = false;

        }

    }

}
