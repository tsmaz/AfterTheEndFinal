using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
	public class PlayerDetection : MonoBehaviour
	{
		float strongDetectionRange = 20f;
		private List<GameObject> enemiesInRange = new List<GameObject>();
		private void OnTriggerEnter(Collider other)
		{
			Debug.Log(other.gameObject.name);
			if (other.CompareTag("Detectable"))
			{
				enemiesInRange.Add(other.gameObject);
				Debug.Log("Enemy detected: " + other.gameObject.name);
			}
		}

		private void OnTriggerExit( Collider other )
		{
			Debug.Log(other.gameObject.name);
			if ( other.CompareTag( "Detectable" ) )
			{
				enemiesInRange.Remove(other.gameObject);
				Debug.Log("Enemy lost: " + other.gameObject.name);
				if (enemiesInRange.Count == 0)
				{
					Debug.Log("No enemies in range.");
					DetectingDeviceManager.Instance.signalNoDetection();
				}
			}
		}

		private void Update()
		{
			if (enemiesInRange.Count != 0)
			{
				foreach ( var enemy in enemiesInRange )
				{
					float distanceToPlayer = Vector3.Distance(enemy.transform.position, transform.position);

					if ( distanceToPlayer < strongDetectionRange )
					{
						DetectingDeviceManager.Instance.signalStrongDetection();
						return;
					} 

				}
				
				DetectingDeviceManager.Instance.signalWeakDetection();
			}
		}
	}
}