using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HydrationBar : MonoBehaviour
{
	private Slider slider;

	public GameObject playerStatus;

	private float currentHydration, maxHydration;

	void Awake()
	{
		slider = GetComponent<Slider>();
	}

	void Update()
	{
		currentHydration = playerStatus.GetComponent<PlayerStatus>().currentHydrationLevel;
		maxHydration = playerStatus.GetComponent<PlayerStatus>().maxHydrationLevel;

		float fillValue = currentHydration / maxHydration;
		slider.value = fillValue;
	}
}