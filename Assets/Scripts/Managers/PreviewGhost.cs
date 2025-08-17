using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

	public class PreviewGhost : MonoBehaviour
	{
		public float maxPlacementDistance;
		private MeshRenderer meshRenderer;

		private void Start()
		{
			if ( gameObject.TryGetComponent<MeshRenderer>( out meshRenderer ) )
			{
				Debug.Log("Found renderer on main object"  );
			}
			else
			{
				Debug.Log("Could not find renderer. Looking in children..."  );
			}

			for ( int i = 0; i < gameObject.transform.childCount; i++ )
			{
				gameObject.transform.GetChild( i ).gameObject.TryGetComponent<MeshRenderer>( out meshRenderer );
			}
		}

		private void Update()
		{

			if ( Input.GetKeyDown( KeyCode.Q ) )
			{
				// Rotate counter-clockwise smoothly
				transform.Rotate( Vector3.up, -90f );
			}

			if ( Input.GetKeyDown( KeyCode.E ) )
			{
				transform.Rotate( Vector3.up, 90f );
			}
			
			Debug.Log( "Ghost position: " + transform.position );
			if ( Input.GetMouseButtonDown( 0 ) )
			{
				if ( ConstructionManager.Instance.isValidPlacement )
				{
					ConstructionManager.Instance.ConfirmConstruction(transform.position, transform.rotation);
				}
				else
				{
					Debug.Log( "Invalid placement" );
				}
			}

			if ( Input.GetMouseButtonDown( 1 ) || Input.GetKeyDown( KeyCode.Escape ) || InventorySystem.Instance.isOpen || CraftingManager.Instance.mainMenuIsOpen )
			{
				ConstructionManager.Instance.ExitConstructionMode();
				return;
			}

			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			if ( Physics.Raycast( ray, out RaycastHit hit ) )
			{
				transform.position = hit.point;
			}

			float distanceToPlayer = Vector3.Distance( transform.position, Camera.main.transform.position );

			if ( !PlacementPositionIsValid(distanceToPlayer, hit) )
			{
				ConstructionManager.Instance.isValidPlacement = false;

				for ( int i = 0; i < meshRenderer.materials.Length; i++)
				{
					meshRenderer.materials[i] = ConstructionManager.Instance.ghostInvalidMaterial;
				}
		
			}
			else
			{
				ConstructionManager.Instance.isValidPlacement = true;
				for ( int i = 0; i < meshRenderer.materials.Length; i++)
				{
					meshRenderer.materials[i] = ConstructionManager.Instance.ghostInvalidMaterial;
				}
			}

			
		}

		public void SnapToNearestSocket()
		{
			
		}
		
		bool PlacementPositionIsValid(float distanceToPlayer, RaycastHit hit)
		{
			// Condition 1: Distance
			if ( distanceToPlayer > maxPlacementDistance )
			{
				return false;
			}

			// Condition 2: No colliders in the way, ignoring the ghost itself and ground
			Collider[] colliders = Physics.OverlapSphere( transform.position, 0.5f );
			foreach ( Collider collider in colliders )
			{
				if ( collider.gameObject != gameObject && collider.gameObject.layer != LayerMask.NameToLayer( "Ground" ) )
				{
					return false; 
				}
			}
				
			// Condition 3: Object must be terrain
			if ( hit.collider is not null )
			{
				bool isTerrain = hit.collider.gameObject.CompareTag( "Terrain" );
					
				if (!isTerrain) 
				{
					Debug.Log("Object is not terrain"  );
					return false;
				}
			}
				
			return true;
		}
	}

