using UnityEngine;
using UnityEngine.UI;

public class Consumable : MonoBehaviour
	{
		public Button consumeButton;
		public int hungerRestored;
		public int thirstRestored;
		public int healthRestored;
		
		private void Start()
		{
			if ( consumeButton == null )
			{
				Debug.LogError( "ConsumeButton is not assigned in the inspector!" );
				return;
			}

			consumeButton.onClick.AddListener( () => PlayerStatus.Instance.ConsumeItem(this) );
		}
	}
	