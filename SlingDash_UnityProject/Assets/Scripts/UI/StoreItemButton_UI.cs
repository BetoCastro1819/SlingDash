﻿using UnityEngine;
using UnityEngine.UI;

public class StoreItemButton_UI : MonoBehaviour 
{
	[Header("Item variables")]
	[SerializeField] string itemPath;
	[SerializeField] int price;

	[Header("UI elements")]
	[SerializeField] Text priceText;
	[SerializeField] Text itemAlreadyOwnedMessage;

	void Start()
	{
		// Check against persisten data to see if player already owns this product
		// If he owns it, disable the button and display message

		priceText.text = price.ToString() + " x";
	}
}