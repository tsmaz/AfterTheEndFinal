using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HungerBar : MonoBehaviour
{
	private Slider slider;

	public GameObject playerStatus;

	private float currentHunger, maxHunger;

	void Awake()
	{
		slider = GetComponent<Slider>();
	}

	void Update()
	{
		currentHunger = playerStatus.GetComponent<PlayerStatus>().currentHunger;
		maxHunger = playerStatus.GetComponent<PlayerStatus>().maxHunger;

		float fillValue = currentHunger / maxHunger;
		slider.value = fillValue;
	}
}