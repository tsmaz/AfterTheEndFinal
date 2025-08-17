using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
	public static EquipmentManager Instance { get; set; }

	public GameObject equipVisualSlot;
	public GameObject equippedItem;

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

	// Start is called before the first frame update
	void Start()
	{
		equippedItem = null;
	}

	// Update is called once per frame
	void Update()
	{
		if ( Input.GetKeyDown( KeyCode.R ) )
		{
			UnequipItem();
		}
	}

	public void EquipItem( GameObject itemToEquip )
	{

		if (equippedItem is null )
		{
			// Instantiate the equipped item at the visualslot's position and rotation
			if ( itemToEquip is not null && equipVisualSlot != null )
			{
				if ( ConstructionManager.Instance.inConstructionMode )
				{
					ConstructionManager.Instance.ExitConstructionMode();
				}
				
				equippedItem = Instantiate( itemToEquip, equipVisualSlot.transform );
			}
			else
			{
				Debug.LogWarning( "Missing EquippedItemPrefab or EquipVisualSlot reference!" );
			}
		}
		else if (equippedItem is not null)
		{
			Debug.Log("An item is already equipped");
			PopupSpawner.Instance.SpawnPopup( Vector3.zero, "Already equipping an item. Press R to Unequip" );
		}
	}

	public void UnequipItem()
	{
		if ( equippedItem is not null )
		{
			Destroy( equippedItem );
			equippedItem = null;
			Debug.Log( "Item unequipped" );
		}
	}
	
	
}