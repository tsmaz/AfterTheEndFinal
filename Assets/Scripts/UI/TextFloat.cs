using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFloat : MonoBehaviour
{
	private float fadeSpeed = 0.5f; // Adjust the fade speed as needed
	private float translationSpeed = 20.0f;
	private float horizontalOffset = 0.5f; // Adjust this value to control the horizontal offset

	private float verticalOffset = 1.0f; // Adjust this value to control the vertical offset

	// Start is called before the first frame update
	void Start() { }

	// Update is called once per frame
	void Update()
	{
		RectTransform transform = gameObject.GetComponent<RectTransform>();

		// Move the text up and to the right
		transform.Translate( horizontalOffset * translationSpeed * Time.deltaTime,
			verticalOffset * translationSpeed * Time.deltaTime,
			0 );

		// Gradually reduce opacity of the text
		Color color = gameObject.GetComponent<TextMeshProUGUI>().color;
		color.a -= 1f * fadeSpeed * Time.deltaTime; // Adjust the value to control the fade speed
		gameObject.GetComponent<TextMeshProUGUI>().color = color;

		if ( color.a <= 0 )
		{
			Destroy( gameObject );
		}
	}
}