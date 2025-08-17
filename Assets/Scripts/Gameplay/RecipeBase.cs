using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjects
{
	[ CreateAssetMenu( fileName = "RecipeBase", menuName = "Recipes", order = 0 ) ]
	public class RecipeBase : ScriptableObject
	{
		public List<String> requiredItems;
		public List<int> requiredQuantities;
		public String resultingItem;
		public int resultingItemQuantity;
	}
}