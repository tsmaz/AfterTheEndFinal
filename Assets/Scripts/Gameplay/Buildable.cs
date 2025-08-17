using System;
using UnityEngine;
using UnityEngine.UI;

	public class Buildable : MonoBehaviour
	{
		public Button startConstructionButton;
		public GameObject structurePrefab;
		public GameObject ghostPrefab;

		private void Start()
		{
			startConstructionButton.onClick.AddListener( () => ConstructionManager.Instance.ActivateConstructionMode( gameObject ,structurePrefab, ghostPrefab ) );
		}
	}