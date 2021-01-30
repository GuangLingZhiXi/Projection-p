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

        if (col == CorrectCllider)
        {

            CollisonHappen = false;

        }

    }

}
