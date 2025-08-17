using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InventorySystem : MonoBehaviour
{
	public static InventorySystem Instance { get; set; }

	public GameObject HotbarGrid;
	public GameObject inventoryScreenUI;
	public bool isOpen;
	public List<GameObject> slotList = new List<GameObject>();
	public List<ItemData> itemList = new List<ItemData>();
	private GameObject itemToAdd;
	private GameObject slotToEquip;
	public bool isFull;

	public UnityEvent<int> OnItemQuantityChanged;

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
	
	void Start()
	{
		isOpen = false;
		inventoryScreenUI.SetActive( false );

		PopulateSlotList();
	}


	void Update()
	{
		if ( Input.GetKeyDown( KeyCode.I ) && !isOpen )
		{
			Debug.Log( "i is pressed" );
			OpenInventory();
		}
		else if ( Input.GetKeyDown( KeyCode.I ) && isOpen )
		{
			inventoryScreenUI.SetActive( false );
			CloseInventory();
		}
		else if ( Input.GetKeyDown( KeyCode.Alpha1 ) )
		{
			SelectHotbarSlot( 1 );
		}
		else if ( Input.GetKeyDown( KeyCode.Alpha2 ) )
		{
			SelectHotbarSlot( 2 );
		}
		else if ( Input.GetKeyDown( KeyCode.Alpha3 ) )
		{
			SelectHotbarSlot( 3 );
		}
		else if ( Input.GetKeyDown( KeyCode.Alpha4 ) )
		{
			SelectHotbarSlot( 4 );
		}
		else if ( Input.GetKeyDown( KeyCode.Alpha5 ) )
		{
			SelectHotbarSlot( 5 );
		}
		else if ( Input.GetKeyDown( KeyCode.Alpha6 ) )
		{
			SelectHotbarSlot( 6 );
		}
		else if ( Input.GetKeyDown( KeyCode.Alpha7 ) )
		{
			SelectHotbarSlot( 7 );
		}
	}

	public void OpenInventory()
	{
		inventoryScreenUI.SetActive( true );
		isOpen = true;
	}

	public void CloseInventory()
	{
		inventoryScreenUI.SetActive( false );
		isOpen = false;
	}

	private void PopulateSlotList()
	{
		foreach ( Transform child in inventoryScreenUI.transform )
		{
			if ( child.CompareTag( "Slot" ) )
			{
				slotList.Add( child.gameObject );
			}
		}
	}

	public void AddToInventory( string itemName, int quantity )
	{
		// If Inventory full, return and debug log
		if ( checkIfFull() )
		{
			Debug.Log( "Inventory is full" );
			return;
		}

		// Check if item already exists in inventory
		GameObject existingItem = TryFindSlotWithItem( itemName );
		ItemSlot itemSlot = existingItem?.GetComponent<ItemSlot>();

		if ( existingItem != null )
		{
			// If item exists, increase its quantity
			ItemData itemData = itemList.Find( item => item.Name == itemName );
			itemData.Quantity += quantity;
			Text quantityText = existingItem.GetComponentInChildren<Text>();
			quantityText.text = itemData.Quantity.ToString();
		}
		else
		{
			// If item does not exist, create a new item and add it to the inventory
			slotToEquip = findNextEmptySlot();
			if ( slotToEquip != null )
			{
				itemToAdd = ( GameObject )Instantiate( Resources.Load<GameObject>( "InventoryItems/" + itemName ),
					slotToEquip.transform.position, slotToEquip.transform.rotation, slotToEquip.transform );
				itemList.Add( new ItemData( itemName, quantity ) );
				Text quantityText = itemToAdd.GetComponentInChildren<Text>();
				quantityText.text = quantity.ToString();
			}
			else
			{
				Debug.Log( "Failed to find an empty slot" );
			}
		}
	}

	
	
	public bool RemoveFromInventory( string itemName, int quantity )
	{
		// Check if item already exists in inventory
		GameObject existingItem = TryFindSlotWithItem( itemName );
		// Access the ItemSlot component
		ItemSlot itemSlot = existingItem?.GetComponent<ItemSlot>();

		if ( existingItem != null )
		{
			if ( itemList.Exists( item => item.Name == itemName ) )
			{
				ItemData itemData = itemList.Find( item => item.Name == itemName );

				if ( itemData.Quantity >= quantity )
				{
					itemData.Quantity -= quantity;
					Text quantityText = existingItem.GetComponentInChildren<Text>();
					quantityText.text = itemData.Quantity.ToString();

					if ( itemData.Quantity <= 0 )
					{
						itemList.Remove( itemData );
						Destroy( existingItem.gameObject );
					}

					return true;
				}
				else
				{
					Debug.Log( "Not enough items to remove" );
					return false;
				}
			}
			else
			{
				Debug.Log( "Item not found in inventory" );
				return false;
			}
		}

		return false;
	}

	private GameObject TryFindSlotWithItem( string itemName )
	{
		foreach ( GameObject slot in slotList )
		{
			if ( slot.transform.childCount > 0 )
			{
				Transform item = slot.transform.GetChild( 0 );

				if ( item.name == itemName || item.name == itemName + "(Clone)" )
				{
					return item.gameObject;
				}
			}
		}

		// Item not found
		return null;
	}

	public bool HasRequiredItems( ScriptableObjects.RecipeBase recipe )
	{
		if ( recipe == null )
		{
			Debug.Log( "recipe is null" );
		}

		for ( int i = 0; i < recipe.requiredItems.Count; i++ )
		{
			string requiredItem = recipe.requiredItems[ i ];
			int requiredQuantity = recipe.requiredQuantities[ i ];

			GameObject existingItem = TryFindSlotWithItem( requiredItem );

			if ( existingItem == null || itemList.Find( item => item.Name == requiredItem ).Quantity < requiredQuantity )
			{
				Debug.Log( "Missing required items for crafting" );
				return false;
			}
		}

		return true;
	}

	public void DropItem( GameObject item ) { }

	private GameObject findNextEmptySlot()
	{
		foreach ( GameObject slot in slotList )
		{
			if ( slot.transform.childCount == 0 )
			{
				return slot;
			}
		}

		Debug.Log( "No Empty slot" );
		return null;
	}

	private bool checkIfFull()
	{
		int counter = 0;

		foreach ( GameObject slot in slotList )
		{
			if ( slot.transform.childCount > 0 )
			{
				counter++;
			}
		}

		if ( counter == 55 )
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void SelectHotbarSlot( int slotIndex )
	{
		var selectedSlot = HotbarGrid.transform.GetChild( slotIndex - 1 ).gameObject;
		GameObject itemInSlot = null;
		
		if ( selectedSlot.transform.childCount > 0 )
		{ 
			itemInSlot = selectedSlot.transform.GetChild( 0 ).gameObject;
			Debug.Log( "Selected item: " + itemInSlot.name );
		}
		else if ( selectedSlot.transform.childCount == 0 )
		{
			Debug.Log( "No item in selected hotbar slot" );
			return;
		}
		
		if ( itemInSlot is not null )
		{
			GameObject equipPrefab = itemInSlot.GetComponent<Equippable>()?.EquippedItemPrefab;
			if ( equipPrefab is not null )
			{
				EquipmentManager.Instance.EquipItem( equipPrefab );
			}
		}
		else
		{
			Debug.Log( "Failed to equip item" );
		}
	}
}