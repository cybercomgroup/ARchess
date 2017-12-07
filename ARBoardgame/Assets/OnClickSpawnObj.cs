using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickSpawnObj : MonoBehaviour {

	public void OnClickActon()
    {
        GameObject controller = GameObject.Find("GameController");
        if (controller == null) return;

        GameController ctrl = controller.GetComponent<GameController>();
        if (ctrl == null) return;

        ctrl.SpawnObj();

    }
}
