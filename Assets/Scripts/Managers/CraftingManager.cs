using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

public class CraftingManager : MonoBehaviour
{
	public GameObject craftingScreenUI;
	public GameObject craftingToolsSubmenuUI;
	public GameObject craftingStructuresSubmenuUI;
	[ FormerlySerializedAs( "isOpen" ) ] public bool mainMenuIsOpen;
	public bool subMenuIsOpen;
	// Start is called before the first frame update


	public static CraftingManager Instance { get; set; }

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
		craftingScreenUI.SetActive( false );
		craftingToolsSubmenuUI.SetActive( false );
		craftingStructuresSubmenuUI.SetActive( false );
		mainMenuIsOpen = false;
		subMenuIsOpen = false;
	}

	// Update is called once per frame
	void Update()
	{
		if ( Input.GetKeyDown( KeyCode.C ) && !mainMenuIsOpen )
		{
			Debug.Log( "c is pressed" );
			craftingScreenUI.SetActive( true );
			craftingToolsSubmenuUI.SetActive( false );
			craftingStructuresSubmenuUI.SetActive( false );
			mainMenuIsOpen = true;
			subMenuIsOpen = false;
		}
		else if ( Input.GetKeyDown( KeyCode.C ) && mainMenuIsOpen )
		{
			craftingScreenUI.SetActive( false );
			mainMenuIsOpen = false;
			craftingToolsSubmenuUI.SetActive( false );
			craftingStructuresSubmenuUI.SetActive( false );
			subMenuIsOpen = false;
		}
	}

	public void CloseAllMenus()
	{
		craftingScreenUI.SetActive( false );
		craftingToolsSubmenuUI.SetActive( false );
		craftingStructuresSubmenuUI.SetActive( false );
		mainMenuIsOpen = false;
		subMenuIsOpen = false;
	}

	public void TryCraftRecipe( string recipeName )
	{
		Debug.Log( "Trying to craft recipe: " + recipeName );

		InventorySystem playerInventory = FindObjectOfType<InventorySystem>();
		RecipeBase recipe = Resources.Load<RecipeBase>( "CraftingRecipes/" + recipeName );
		if ( recipe == null )
		{
			Debug.LogError( "Recipe not found: " + recipeName );
			PopupSpawner.Instance.SpawnPopup( new Vector3( 0, 0, 0 ), "Craft failed: Recipe not found." );
			return;
		}


		if ( playerInventory.isFull )
		{
			PopupSpawner.Instance.SpawnPopup( new Vector3( 0, 0, 0 ), "Craft failed: inventory is full." );
			return;
		}

		if ( playerInventory.HasRequiredItems( recipe ) == true )
		{
			// Lopp through required items and quantities
			for ( int i = 0; i < recipe.requiredItems.Count; i++ )
			{
				if ( InventorySystem.Instance.RemoveFromInventory( recipe.requiredItems[ i ], recipe.requiredQuantities[ i ] ) )
				{
					Debug.Log( recipe.requiredItems[ i ] + " is removed from inventory. Count: " + recipe.requiredQuantities[ i ] );
				}
				else
				{
					Debug.Log( "Failed to remove item: " + recipe.requiredItems[ i ] + " in quanity: " + recipe.requiredQuantities[ i ] );
				}
			}

			playerInventory.AddToInventory( recipe.resultingItem, recipe.resultingItemQuantity );
			PopupSpawner.Instance.SpawnPopup( new Vector3( 0, 0, 0 ), "Crafting successful: " + recipe.resultingItem );
		}
		else
		{
			PopupSpawner.Instance.SpawnPopup( new Vector3( 0, 0, 0 ), "Craft failed: Missing required items." );
		}
	}

	public void OpenCraftingToolsSubmenu()
	{
		craftingToolsSubmenuUI.SetActive( true );
		craftingScreenUI.SetActive( false );
		mainMenuIsOpen = false;
		subMenuIsOpen = true;
	}

	public void OpenCraftingStructuresSubmenu()
	{
		craftingStructuresSubmenuUI.SetActive( true );
		craftingScreenUI.SetActive( false );
		mainMenuIsOpen = false;
		subMenuIsOpen = true;
	}
}