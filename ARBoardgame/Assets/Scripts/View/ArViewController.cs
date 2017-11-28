using System.Collections;
using System.Collections.Generic;
using GoogleARCore;
using GoogleARCore.HelloAR;
using UnityEngine;

public class ArViewController : MonoBehaviour
{

	public bool BoardPositioned { get; private set; }
	
	public GameObject boardPrefab;

	public GameObject planePrefab;
	
	private List<TrackedPlane> m_newPlanes = new List<TrackedPlane>();

	private List<TrackedPlane> m_allPlanes = new List<TrackedPlane>();

	private Color[] m_planeColors = new Color[] {
		new Color(1.0f, 1.0f, 1.0f),
		new Color(0.956f, 0.262f, 0.211f),
		new Color(0.913f, 0.117f, 0.388f),
		new Color(0.611f, 0.152f, 0.654f),
		new Color(0.403f, 0.227f, 0.717f),
		new Color(0.247f, 0.317f, 0.709f),
		new Color(0.129f, 0.588f, 0.952f),
		new Color(0.011f, 0.662f, 0.956f),
		new Color(0f, 0.737f, 0.831f),
		new Color(0f, 0.588f, 0.533f),
		new Color(0.298f, 0.686f, 0.313f),
		new Color(0.545f, 0.764f, 0.290f),
		new Color(0.803f, 0.862f, 0.223f),
		new Color(1.0f, 0.921f, 0.231f),
		new Color(1.0f, 0.756f, 0.027f)
	};
	
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

	public void Update()
	{
		// The tracking state must be FrameTrackingState.Tracking in order to access the Frame.
		if (Frame.TrackingState != FrameTrackingState.Tracking)
		{
			const int LOST_TRACKING_SLEEP_TIMEOUT = 15;
			Screen.sleepTimeout = LOST_TRACKING_SLEEP_TIMEOUT;
			return;
		}

		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		Frame.GetNewPlanes(ref m_newPlanes);

		// Iterate over planes found in this frame and instantiate corresponding GameObjects to visualize them.
		for (int i = 0; i < m_newPlanes.Count; i++)
		{
			// Instantiate a plane visualization prefab and set it to track the new plane. The transform is set to
			// the origin with an identity rotation since the mesh for our prefab is updated in Unity World
			// coordinates.
			GameObject planeObject = Instantiate(planePrefab, Vector3.zero, Quaternion.identity,
				transform);
			planeObject.GetComponent<TrackedPlaneVisualizer>().SetTrackedPlane(m_newPlanes[i]);
			foreach (var collider in planeObject.GetComponentsInChildren<Collider>())
				collider.gameObject.layer = 8;

			// Apply a random color and grid rotation.
			planeObject.GetComponent<Renderer>().material.SetColor("_GridColor", m_planeColors[Random.Range(0,
				m_planeColors.Length - 1)]);
			planeObject.GetComponent<Renderer>().material.SetFloat("_UvRotation", Random.Range(0.0f, 360.0f));
		}
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
		
		if (!BoardPositioned)
		{
			TrackableHitFlag raycastFilter = TrackableHitFlag.PlaneWithinBounds | TrackableHitFlag.PlaneWithinPolygon;
			TrackableHit trackHit;
			if (Session.Raycast(ray, raycastFilter, out trackHit))
			{
				BoardPositioned = true;
				Vector3 pos = trackHit.Point;
				GameObject board = Instantiate(boardPrefab, pos, Quaternion.identity);
				Vector3 lookPos = new Vector3(transform.position.x, pos.y, transform.position.z);
				board.transform.LookAt(lookPos);
				board.transform.Rotate(0, 90, 0, Space.World);
			}
			return;
		}
		else
		{
			int layerMask = 1 << LayerMask.NameToLayer("Default");
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, float.MaxValue, layerMask)) {
				
				this.PostNotification(SQUARE_LMB_CLICKED);
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
