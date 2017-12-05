using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerController : NetworkBehaviour 
{
    public const string Started = "PlayerController.Start";
    public const string StartedLocal = "PlayerController.StartedLocal";
    public const string Destroyed = "PlayerController.Destroyed";
    public const string DO_SOMETHING = "Do something";

	public override void OnStartClient ()
	{
		base.OnStartClient ();
        this.PostNotification(Started);
    }

	public override void OnStartLocalPlayer ()
	{
		base.OnStartLocalPlayer ();
		this.PostNotification(StartedLocal);
	}

	void OnDestroy ()
	{
		this.PostNotification(Destroyed);
	}

	[Command]
	public void CmdDoSomething(string someStr)
	{
        RpcDoSomething(someStr);
	}

	[ClientRpc]
	void RpcDoSomething(string someStr)
	{
		this.PostNotification(DO_SOMETHING, someStr);
	}

    //Temp solution with JSON
    [Command]
    public void CmdPiecePut(string piecePutJSON)
    {
        RpcPiecePut(piecePutJSON);
    }

    [ClientRpc]
    void RpcPiecePut(string piecePutJSON)
    {
        this.PostNotification(GameController.PIECE_PUT_MULTI, piecePutJSON);
    }

    [Command]
    public void CmdPieceMoved(string moveJSON)
    {
        RpcPieceMoved(moveJSON);
    }

    [ClientRpc]
    void RpcPieceMoved(string moveJSON)
    {
        this.PostNotification(GameController.PIECE_MOVED_MULTI, moveJSON);
    }
}