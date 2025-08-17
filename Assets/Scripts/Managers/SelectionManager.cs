using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
	public bool onTarget;
	public GameObject interaction_Info_UI;
	Text interaction_text;
	public InteractableObject currentTarget;
	public static SelectionManager Instance { get; set; }

	private void Start()
	{
		interaction_text = interaction_Info_UI.GetComponent<Text>();
		onTarget = false;
	}

	private void Awake()
	{
		if ( Instance != null && Instance != this )
		{
			Destroy( gameObject );
		}
		else
		{
			Instance = this;
		}
	}

	void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit hit;
		if ( Physics.Raycast( ray, out hit ) )
		{
			var selectionTransform = hit.transform;
			var interactableObject = selectionTransform.GetComponent<InteractableObject>();

			if ( interactableObject && interactableObject.playerInRange)
			{
				interaction_text.text = interactableObject.GetItemName();
				interaction_Info_UI.SetActive( true );
				currentTarget = interactableObject;
				onTarget = true;
			}
			else
			{
				interaction_Info_UI.SetActive( false );
				currentTarget = null;
				onTarget = false;
			}
		}
		else
		{
			interaction_Info_UI.SetActive( false );
			currentTarget = null;
			onTarget = false;
		}
	}
}