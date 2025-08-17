using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MimicSpace
{
	public enum MimicAIState
	{
		Idle,
		Patrol,
		Chase
	}

	/// <summary>
	/// Hostile AI movement script for Mimic creatures
	/// Maintains compatibility with Mimic component velocity requirements
	/// </summary>
	public class HostileMovement : MonoBehaviour
	{
		[ Header( "Movement Settings" ) ] [ Tooltip( "Body Height from ground" ) ] [ Range( 0.5f, 5f ) ]
		public float height = 0.8f;

		public float patrolSpeed = 2f;
		public float chaseSpeed = 5f;
		public float velocityLerpCoef = 4f;

		[ Header( "Detection Settings" ) ] public float detectionRange = 5f;

		[ Header( "Timing Settings" ) ] public float walkTimeMin = 3f;
		public float walkTimeMax = 6f;
		public float waitTimeMin = 2f;
		public float waitTimeMax = 4f;

		private Vector3 velocity = Vector3.zero;
		private Mimic myMimic;
		private Transform playerTransform;
		private MimicAIState currentState = MimicAIState.Idle;

		private float walkTime;
		private float walkCounter;
		private float waitTime;
		private float waitCounter;
		private int walkDirection;
		private bool isWalking;

		private void Start()
		{
			myMimic = GetComponent<Mimic>();

			// Find the player using the established tag pattern
			GameObject player = GameObject.FindGameObjectWithTag( "Player" );
			if ( player != null )
			{
				playerTransform = player.transform;
			}

			// Initialize timing with randomized values
			walkTime = Random.Range( walkTimeMin, walkTimeMax );
			waitTime = Random.Range( waitTimeMin, waitTimeMax );

			waitCounter = waitTime;
			walkCounter = walkTime;

			ChooseDirection();
		}

		void Update()
		{
			CheckForPlayer();
			UpdateStateBehavior();
			UpdatePosition();
		}

		void CheckForPlayer()
		{
			if ( playerTransform == null ) return;

			float distanceToPlayer = Vector3.Distance( transform.position, playerTransform.position );

			// Simple distance-based detection
			if ( distanceToPlayer <= detectionRange )
			{
				currentState = MimicAIState.Chase;
			}
			else if ( currentState == MimicAIState.Chase )
			{
				currentState = MimicAIState.Patrol;
				ChooseDirection();
			}
		}

		void UpdateStateBehavior()
		{
			Vector3 targetVelocity = Vector3.zero;

			switch ( currentState )
			{
				case MimicAIState.Idle:
					targetVelocity = HandleIdleState();
					break;
				case MimicAIState.Patrol:
					targetVelocity = HandlePatrolState();
					break;
				case MimicAIState.Chase:
					targetVelocity = HandleChaseState();
					break;
			}

			// Lerp velocity smoothly like original Movement script
			velocity = Vector3.Lerp( velocity, targetVelocity, velocityLerpCoef * Time.deltaTime );

			// Assign velocity to the mimic for proper leg placement
			myMimic.velocity = velocity;
		}

		Vector3 HandleIdleState()
		{
			waitCounter -= Time.deltaTime;

			if ( waitCounter <= 0 )
			{
				currentState = MimicAIState.Patrol;
				ChooseDirection();
			}

			return Vector3.zero;
		}

		Vector3 HandlePatrolState()
		{
			if ( isWalking )
			{
				walkCounter -= Time.deltaTime;

				Vector3 direction = GetDirectionVector( walkDirection );

				if ( walkCounter <= 0 )
				{
					isWalking = false;
					currentState = MimicAIState.Idle;
					waitCounter = waitTime;
					return Vector3.zero;
				}

				return direction * patrolSpeed;
			}
			else
			{
				ChooseDirection();
				return Vector3.zero;
			}
		}

		Vector3 HandleChaseState()
		{
			if ( playerTransform == null ) return Vector3.zero;

			// Move toward the player
			Vector3 directionToPlayer = ( playerTransform.position - transform.position ).normalized;

			// Remove Y component to keep movement horizontal
			directionToPlayer.y = 0;
			directionToPlayer = directionToPlayer.normalized;

			return directionToPlayer * chaseSpeed;
		}

		void UpdatePosition()
		{
			// Move the transform (same logic as original Movement script)
			transform.position = transform.position + velocity * Time.deltaTime;

			// Ground height adjustment with raycast
			RaycastHit hit;
			Vector3 destHeight = transform.position;
			if ( Physics.Raycast( transform.position + Vector3.up * 5f, -Vector3.up, out hit ) )
				destHeight = new Vector3( transform.position.x, hit.point.y + height, transform.position.z );
			transform.position = Vector3.Lerp( transform.position, destHeight, velocityLerpCoef * Time.deltaTime );
		}

		void ChooseDirection()
		{
			walkDirection = Random.Range( 0, 4 );
			isWalking = true;
			walkCounter = walkTime;
		}

		Vector3 GetDirectionVector( int direction )
		{
			switch ( direction )
			{
				case 0: return Vector3.forward; // North
				case 1: return Vector3.right; // East
				case 2: return Vector3.left; // West
				case 3: return Vector3.back; // South
				default: return Vector3.zero;
			}
		}

		// Debug visualization for detection range
		void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere( transform.position, detectionRange );
		}
	}
}