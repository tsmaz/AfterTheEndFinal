using UnityEngine;

	public class ConstructionManager : MonoBehaviour
	{
		public static ConstructionManager Instance { get; set; }

		public GameObject itemToBeConstructed;	
		public GameObject constructionPrefab;
		public GameObject selectedGhost;
		
		public bool inConstructionMode = false;
		public bool isValidPlacement;

		public Material ghostValidMaterial;
		public Material ghostInvalidMaterial;



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

		private void Update()
		{
			if (!inConstructionMode)
			{
				return;
			}
			if ( Input.GetKeyDown( KeyCode.Escape ) && inConstructionMode )
			{
				ExitConstructionMode();
			}
		}

		public void ActivateConstructionMode(GameObject inventoryItem, GameObject itemToConstruct, GameObject previewGhost )
		{
			if ( InventorySystem.Instance.isOpen )
			{
				InventorySystem.Instance.CloseInventory();
			}
			
			if (CraftingManager.Instance.mainMenuIsOpen || CraftingManager.Instance.subMenuIsOpen)
			{
				CraftingManager.Instance.CloseAllMenus();
			}
			
			Debug.Log("Entering Construction Mode"  );
			EquipmentManager.Instance.UnequipItem();
			inConstructionMode = true;
			constructionPrefab = itemToConstruct;
			itemToBeConstructed = inventoryItem;
			selectedGhost = Instantiate( previewGhost );
			
		}
		
		public void ExitConstructionMode()
		{
			Debug.Log( "Exiting Construction Mode" );
			if ( selectedGhost != null )
			{
				Destroy( selectedGhost.gameObject);
				selectedGhost = null;
			}
			inConstructionMode = false;
			itemToBeConstructed = null;
			constructionPrefab = null;
		}

		public void ConfirmConstruction(Vector3 position, Quaternion rotation)
		{
			InventorySystem.Instance.RemoveFromInventory( itemToBeConstructed.tag.ToString(), 1);
			itemToBeConstructed = null;
			var newlyConstructedObject = Instantiate( constructionPrefab, selectedGhost.transform.position, selectedGhost.transform.rotation );
			ExitConstructionMode();
		}

	}
