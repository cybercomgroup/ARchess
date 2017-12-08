using System.Collections;
using System.Collections.Generic;
using UnityARInterface;
using UnityEngine;

public class ArInteractionController : ARBase
{
    public const string AR_CLICK = "AR click";
    public const string ARCAMERA_UPDATE = "ArCamera Update";

    //private Vector3 lastFwd;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }

        if (Input.anyKeyDown)
        {
            HandleKeyPress();
        }
        /*
        else if(Input.GetMouseButtonDown(1))
        {
            HandleRMBClick();
        }
        */
        HandleCameraUpdate();
    }

    public void HandleCameraUpdate()
    {
        Camera camera = GetCamera();
        /*Vector3 fwd = camera.transform.forward;
        if (fwd != lastFwd)
        {
            lastFwd = fwd;*/
            this.PostNotification(ARCAMERA_UPDATE, camera.transform);
        //}
    }

    public void HandleClick() {
        Camera camera = GetCamera();
        Transform pos = camera.transform;
        this.PostNotification(AR_CLICK, pos);
    }

    /*
    public void HandleRMBClick() {
        //Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //this.PostNotification(ARRMB_CLICK, cursorPos);
    }
    */

    // NOTE: Temp for selecting a piece until menu is in place
    public void HandleKeyPress()
    {
        // Should later come from list in menu
        string selPi;

        if (Input.GetKeyDown("1"))
            selPi = "W_Pawn";
        else if (Input.GetKeyDown("2"))
            selPi = "B_Queen";
        else
            return;

        this.PostNotification(GameController.PIECE_SELECTED_IN_MENU, selPi);
    }
}
