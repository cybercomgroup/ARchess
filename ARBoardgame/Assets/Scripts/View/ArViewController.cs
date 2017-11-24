using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArViewController : MonoBehaviour
{

	public bool BoardPositioned { get; private set; }
	
	public GameObject boardPrefab;
	
	public const string SQUARE_RMB_CLICKED = "Square RMB clicked";
	public const string SQUARE_LMB_CLICKED = "Square LMB clicked";
	public const string OUTSIDE_LMB_CLICKED = "Outside board LMB clicked";
	
	// NOTE: Should each class handle its notification subcsriptions? Handle subscriptions in GameController maybe?

	// Handling subscribing and unsubscribing to the notifications in OnEnable and OnDisable is a "better practice" method to prevent
	// mem leakage, subscribed no longer existing objects
	void OnEnable()
	{
		this.AddObserver(OnLMBClick, ArInteractionController.ARLMB_CLICK);
		this.AddObserver(OnCameraUpdate, ArInteractionController.ARCAMERA_UPDATE);
		//this.AddObserver(OnPiecePut, GameController.PIECE_PUT);
		//this.AddObserver(OnPieceMoved, GameController.PIECE_MOVED);
		//this.AddObserver(OnPieceRemoved, GameController.PIECE_REMOVED);
	}

	void OnDisable()
	{
		this.RemoveObserver(OnLMBClick, ArInteractionController.ARLMB_CLICK);
		this.RemoveObserver(OnCameraUpdate, ArInteractionController.ARCAMERA_UPDATE);
		//this.RemoveObserver(OnPiecePut, GameController.PIECE_PUT);
		//this.RemoveObserver(OnPieceMoved, GameController.PIECE_MOVED);
		//this.RemoveObserver(OnPieceRemoved, GameController.PIECE_REMOVED);
	}


	public void OnCameraUpdate(object sender, object args)
	{
		Transform transform = (Transform)args;
		Ray ray = new Ray(transform.position, transform.forward);
		int layerMask = 1 << LayerMask.NameToLayer("Default");
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, float.MaxValue, layerMask))
		{
			if(hit.collider.gameObject.CompareTag("Highlightable"))
			{
				hit.collider.gameObject.GetComponent<HighlightView>().Highlighted = true;
			}
		}
	}
	
	public void OnLMBClick(object sender, object args)
	{
		Transform transform = (Transform)args;
		
		Ray ray = new Ray(transform.position, transform.forward);
		int layerMask = 1 << (BoardPositioned ? LayerMask.NameToLayer("Default"): LayerMask.NameToLayer("ARGameObject"));
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, float.MaxValue, layerMask)) {
			if (!BoardPositioned)
			{
				BoardPositioned = true;
				Vector3 pos = hit.point;
				GameObject board = Instantiate(boardPrefab, pos, Quaternion.identity);
				Vector3 lookPos = new Vector3(transform.position.x, pos.y, transform.position.z);
				board.transform.LookAt(lookPos);
				board.transform.Rotate(0, 90, 0, Space.World);
				return;
			}
			else
			{
				
				return;
			}
		}

		// Debug.Log("ViewController - position left clicked: " + pos + "\n");

		/*SquarePos squarePos = GetSquarePosOfVectPos(pos);

		if (squarePos != null)
		{
			// Debug.Log("ViewController - square (" + squarePos.Col + "," + squarePos.Row + ") left clicked");

			this.PostNotification(SQUARE_LMB_CLICKED, squarePos);
		}
		else
		{
			
		}*/
		this.PostNotification(OUTSIDE_LMB_CLICKED);
	}
}
