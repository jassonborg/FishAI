using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace IdleClickerKit
{
	[RequireComponent (typeof(Text))]
	public class UpgradeDetails_Text : MonoBehaviour {
		
		[Tooltip ("Type of label to display.")]
		[SerializeField]
		protected UpgradeLabelType labelType;

		[Tooltip ("The upgrade component to get name from.")]
		[SerializeField]
		protected BaseUpgrade upgrade;

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
				upgrade = gameObject.GetComponentInParent<BaseUpgrade> ();
			if (upgrade == null) {
				Debug.LogWarning ("UpgradeDetails_Text couldn't find a upgrade to get the text from");
			} else {
				UpdateText ();
				RegisterListeners ();
			}
		}

		virtual protected void UpdateText() {
			switch (labelType) {
			case UpgradeLabelType.NAME:
				myText.text = upgrade.UpgradeName;
				break;
			case UpgradeLabelType.DESCRIPTION:
				myText.text = string.Format (upgrade.Description, upgrade.UpgradeName, ClickManager.GetInstance (upgrade.ClickName).ClickName, 
					upgrade.CustomInfo [0], upgrade.CustomInfo [1], upgrade.CustomInfo [2], upgrade.CustomInfo [3], upgrade.CustomInfo [4]);
				break;
			case UpgradeLabelType.ALT_DESCRIPTION:
				myText.text = string.Format (upgrade.AlternateDescription, upgrade.UpgradeName, ClickManager.GetInstance (upgrade.ClickName).ClickName, 
					upgrade.CustomInfo [0], upgrade.CustomInfo [1], upgrade.CustomInfo [2], upgrade.CustomInfo [3], upgrade.CustomInfo [4]);
				break;
			case UpgradeLabelType.MIXED_DESCRIPTION:
				if (upgrade.Count > 0) {
					myText.text = string.Format (upgrade.AlternateDescription, upgrade.UpgradeName, ClickManager.GetInstance (upgrade.ClickName).ClickName, 
						upgrade.CustomInfo [0], upgrade.CustomInfo [1], upgrade.CustomInfo [2], upgrade.CustomInfo [3], upgrade.CustomInfo [4]);
				} else {
					myText.text = string.Format (upgrade.Description, upgrade.UpgradeName, ClickManager.GetInstance (upgrade.ClickName).ClickName, 
						upgrade.CustomInfo [0], upgrade.CustomInfo [1], upgrade.CustomInfo [2], upgrade.CustomInfo [3], upgrade.CustomInfo [4]);
				}
				break;
			case UpgradeLabelType.COST:
				myText.text = upgrade.Cost.ToString ();
				break;
			case UpgradeLabelType.MAXED_NAME:
				myText.text = upgrade.UpgradeName + " (MAXED)";
				break;
			}
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

	public enum UpgradeLabelType {
		NAME, DESCRIPTION, ALT_DESCRIPTION, MIXED_DESCRIPTION, COST, MAXED_NAME
	}
}