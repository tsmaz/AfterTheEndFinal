using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equippable : MonoBehaviour
{
	public GameObject EquippedItemPrefab;
	public Button EquipButton;

	private void Start()
	{
		if ( EquippedItemPrefab == null )
		{
			Debug.LogError( "EquippedItemPrefab is not assigned in the inspector!" );
			return;
		}

		if ( EquipButton == null )
		{
			Debug.LogError( "EquipButton is not assigned in the inspector!" );
			return;
		}

		// Ensure the EquipmentManager instance is set
		if ( EquipmentManager.Instance == null )
		{
			Debug.LogError( "EquipmentManager instance is not set! Make sure it is initialized before using this script." );
			return;
		}

		EquipButton.onClick.AddListener( () => EquipmentManager.Instance.EquipItem( EquippedItemPrefab ) );
	}
}