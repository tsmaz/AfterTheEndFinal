using UnityEngine;

public enum AIState
{
	Idle,
	Patrol,
	Chase
}

public class HostileAI : MonoBehaviour
{
	[ Header( "Movement Settings" ) ] public float moveSpeed = 0.2f;
	public float chaseSpeed = 0.4f;

	[ Header( "Detection Settings" ) ] public float detectionRange = 5f;

	[ Header( "Timing Settings" ) ] public float walkTimeMin = 3f;
	public float walkTimeMax = 6f;
	public float waitTimeMin = 5f;
	public float waitTimeMax = 7f;

	private Animator animator;
	private Transform playerTransform;
	private AIState currentState = AIState.Idle;

	private Vector3 stopPosition;
	private float walkTime;
	private float walkCounter;
	private float waitTime;
	private float waitCounter;
	private int walkDirection;
	private bool isWalking;

	void Start()
	{
		animator = GetComponent<Animator>();

		GameObject player = GameObject.FindGameObjectWithTag( "Player" );
		if ( player != null )
		{
			playerTransform = player.transform;
		}

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
	}

	void CheckForPlayer()
	{
		if ( playerTransform == null ) return;

		float distanceToPlayer = Vector3.Distance( transform.position, playerTransform.position );

		if ( distanceToPlayer <= detectionRange )
		{
			currentState = AIState.Chase;
		}
		else if ( currentState == AIState.Chase )
		{
			currentState = AIState.Patrol;
			ChooseDirection();
		}
	}

	void UpdateStateBehavior()
	{
		switch ( currentState )
		{
			case AIState.Idle:
				HandleIdleState();
				break;
			case AIState.Patrol:
				HandlePatrolState();
				break;
			case AIState.Chase:
				HandleChaseState();
				break;
		}
	}

	void HandleIdleState()
	{
		animator.SetBool( "isRunning", false );
		waitCounter -= Time.deltaTime;

		if ( waitCounter <= 0 )
		{
			currentState = AIState.Patrol;
			ChooseDirection();
		}
	}

	void HandlePatrolState()
	{
		if ( isWalking )
		{
			animator.SetBool( "isRunning", true );
			walkCounter -= Time.deltaTime;

			switch ( walkDirection )
			{
				case 0:
					transform.localRotation = Quaternion.Euler( 0f, 0f, 0f );
					transform.position += transform.forward * moveSpeed * Time.deltaTime;
					break;
				case 1:
					transform.localRotation = Quaternion.Euler( 0f, 90, 0f );
					transform.position += transform.forward * moveSpeed * Time.deltaTime;
					break;
				case 2:
					transform.localRotation = Quaternion.Euler( 0f, -90, 0f );
					transform.position += transform.forward * moveSpeed * Time.deltaTime;
					break;
				case 3:
					transform.localRotation = Quaternion.Euler( 0f, 180, 0f );
					transform.position += transform.forward * moveSpeed * Time.deltaTime;
					break;
			}

			if ( walkCounter <= 0 )
			{
				stopPosition = transform.position;
				isWalking = false;
				animator.SetBool( "isRunning", false );
				currentState = AIState.Idle;
				waitCounter = waitTime;
			}
		}
		else
		{
			ChooseDirection();
		}
	}

	void HandleChaseState()
	{
		if ( playerTransform == null ) return;

		animator.SetBool( "isRunning", true );

		Vector3 directionToPlayer = ( playerTransform.position - transform.position ).normalized;
		transform.rotation = Quaternion.LookRotation( directionToPlayer );
		transform.position += directionToPlayer * chaseSpeed * Time.deltaTime;
	}

	void ChooseDirection()
	{
		walkDirection = Random.Range( 0, 4 );
		isWalking = true;
		walkCounter = walkTime;
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere( transform.position, detectionRange );
	}
}