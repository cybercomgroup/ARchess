using System.Collections;
using System.Collections.Generic;
using UnityARInterface;
using UnityEngine;

public class ArInteractionController : ARBase
{
    public const string ARLMB_CLICK = "ArLMB click";
    public const string ARRMB_CLICK = "ArRMB click";
    public const string ARCAMERA_UPDATE = "ArCamera Update";

    //private Vector3 lastFwd;


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

    public void HandleLMBClick() {
        Camera camera = GetCamera();
        Transform pos = camera.transform;
        this.PostNotification(ARLMB_CLICK, pos);
    }

    public void HandleRMBClick() {
        //Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //this.PostNotification(ARRMB_CLICK, cursorPos);
    }
}
