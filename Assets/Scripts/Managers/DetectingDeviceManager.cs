using System.Collections.Generic;
using UnityEngine;

	public class DetectingDeviceManager : MonoBehaviour
	{
		public Material noSignalMaterial;
		public Material weakSignalMaterial;
		public Material strongSignalMaterial;
		
		private enum SignalState
		{
			NoSignal,
			WeakSignal,
			StrongSignal
		}
		
		public static DetectingDeviceManager Instance { get; set; }

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

		public void signalWeakDetection()
		{
			if ( EquipmentManager.Instance.equippedItem is null ) return;
			
			if ( EquipmentManager.Instance.equippedItem.CompareTag( "Detector" ) )
			{
				setDisplayState( SignalState.WeakSignal );
			}
		}
		
		public void signalStrongDetection()
		{
			if ( EquipmentManager.Instance.equippedItem is null ) return;
			setDisplayState( SignalState.StrongSignal );
		}
		
		public void signalNoDetection()
		{
			if ( EquipmentManager.Instance.equippedItem is null ) return;
			setDisplayState( SignalState.NoSignal );
		}

		private void setDisplayState( SignalState state )
		{
			
			if (EquipmentManager.Instance.equippedItem is null) return;
			
			if ( EquipmentManager.Instance.equippedItem.CompareTag( "Detector" ) )
			{
				var detectorDisplay = EquipmentManager.Instance.equippedItem.transform.GetChild( 1 ).GetComponent<MeshRenderer>();
				
				switch ( state )
				{
					case SignalState.NoSignal:
						detectorDisplay.material = noSignalMaterial;
						break;
					case SignalState.WeakSignal:
						detectorDisplay.material = weakSignalMaterial;
						break;
					case SignalState.StrongSignal:
						detectorDisplay.material = strongSignalMaterial;
						break;
				}
			}
			else
			{
				Debug.Log("Currently Held Item is not a detector");
			}

		}
	}