using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
	private Slider slider;

	public GameObject playerStatus;

	private float currentHealth, maxHealth;

	void Awake()
	{
		slider = GetComponent<Slider>();
	}

	void Update()
	{
		currentHealth = playerStatus.GetComponent<PlayerStatus>().currentHealth;
		maxHealth = playerStatus.GetComponent<PlayerStatus>().maxHealth;

		float fillValue = currentHealth / maxHealth;
		slider.value = fillValue;
	}
}