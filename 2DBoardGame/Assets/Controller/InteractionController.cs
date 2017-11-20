using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public bool BoardPositioned { get; private set; }

    public const string LMB_CLICK = "LMB click";
    public const string RMB_CLICK = "RMB click";
    public const string BOARD_POSITIONED = "Board position selected";
    private int count;


    // Use this for initialization
    void Start()
    {
        BoardPositioned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleLMBClick();
        }
        else if(Input.GetMouseButtonDown(1))
        {
            HandleRMBClick();
        }
    }

    public void HandleLMBClick()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (BoardPositioned == false)
        {
            this.PostNotification(BOARD_POSITIONED, cursorPos);

            BoardPositioned = true;
        }
        else
        {
            this.PostNotification(LMB_CLICK, cursorPos);
        }

    }

    public void HandleRMBClick()
    {
        if (BoardPositioned == true)
        {
            Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            this.PostNotification(RMB_CLICK, cursorPos);
        }
    }
}
