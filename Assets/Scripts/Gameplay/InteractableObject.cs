using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
	public bool playerInRange;
	public string itemName;
	public bool canBePickedUp = true;
	public bool canBeDeconstructed = false;

	public string GetItemName()
	{
		return itemName;
	}

	private void OnTriggerEnter( Collider other )
	{
		if ( other.CompareTag( "Player" ) )
		{
			playerInRange = true;
		}
	}

	private void OnTriggerExit( Collider other )
	{
		if ( other.CompareTag( "Player" ) )
		{
			playerInRange = false;
		}
	}

	public void Update()
	{
		if ( Input.GetKeyDown( KeyCode.E ) && playerInRange && SelectionManager.Instance.currentTarget == this )
		{
			// print debug string and destroy gameobject

			if ( canBePickedUp )
			{
				InventorySystem.Instance.AddToInventory( itemName, 1 );
				Destroy( gameObject );
			}
		}

		if ( Input.GetMouseButton( 0 ) && playerInRange && SelectionManager.Instance.currentTarget == this )
		{
			if ( EquipmentManager.Instance.equippedItem == null )
			{
				return;
			}

			if ( gameObject.CompareTag( "Tree" ) && EquipmentManager.Instance.equippedItem.CompareTag( "Axe" ) )
			{
				InventorySystem.Instance.AddToInventory( "Wood", 2 );
				PopupSpawner.Instance.SpawnPopup( Vector3.zero, "Gathered 2 Wood" );
				Destroy( gameObject );
			}
		}
	}
}