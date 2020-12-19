using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace IdleClickerKit
{
	[RequireComponent (typeof(Text))]
	public class MultiCostUpgradeCost_Text : MonoBehaviour {
		
		[Tooltip ("Which click to get the cost for or empty for default")]
		[SerializeField]
		protected string clickName;

		[Tooltip ("The upgrade component to get name from.")]
		[SerializeField]
		protected MultiClickUpgrade upgrade;

		protected Text myText;

		/// <summary>
		/// Init.
		/// </summary>
		void Start() {
			PostInit ();
		}

		void OnDestroy() {
			DeregisterListeners ();
		}

		/// <summary>
		/// Initialise instance. In this casel lookup text references.
		/// </summary>
		virtual protected void PostInit() {
			myText = GetComponent<Text> ();
			if (upgrade == null)
				upgrade = gameObject.GetComponentInParent<MultiClickUpgrade> ();
			if (upgrade == null) {
				Debug.LogWarning ("MultiCostUpgradeCost_Text couldn't find a MultiClickUpgrade to get the cost from");
			} else {
				UpdateText ();
				RegisterListeners ();
			}
		}

		virtual protected void UpdateText() {
			myText.text = upgrade.GetCostForClickName (clickName).ToString ();
		}

		void RegisterListeners () {
			upgrade.Upgraded += UpgradedEventHandler;
		}

		void DeregisterListeners () {
			if (upgrade != null) upgrade.Upgraded -= UpgradedEventHandler;
		}

		void UpgradedEventHandler (object sender, UpgradeEventArgs e)
		{
			UpdateText ();
		}

	}

}