using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    // Some testing
    public const string CLICK = "Click";
    private int count;



    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            count++;
            this.PostNotification(CLICK, count);
        }
    }

    /*
    public class LMBClickArgs : EventArgs
    {
        public string Test { get; set; }
    }


    protected virtual void OnLMBCLick(EventArgs e)
    {
        EventHandler handler = ThresholdReached;

        if (handler != null)
        {
            handler(this, e);
        }
    }
    */

}
