using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;

public class PopupSpawner : MonoBehaviour
{
	public GameObject textPrefab; // Assign the prefab in the inspector
	public Canvas canvas;

	public static PopupSpawner Instance { get; private set; }

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

	public void SpawnPopup( Vector3 position, string text )
	{
		GameObject textPopup = Instantiate( textPrefab, new Vector3( position.x, position.y, position.z ), Quaternion.identity );
		textPopup.transform.SetParent( canvas.transform, false ); // Set the parent to the canvas
		textPopup.transform.localPosition = position; // Set the local position to the specified position
		textPopup.GetComponent<TextMeshProUGUI>().SetText( text );
	}
}