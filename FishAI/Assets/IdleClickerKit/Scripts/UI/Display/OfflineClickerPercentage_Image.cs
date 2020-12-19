using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace IdleClickerKit
{
	

	/// <summary>
	/// Shows the percentage full for an offline clicker by revealing an image.
	/// </summary>
	[RequireComponent (typeof(Image))]
	public class OfflineClickerPercentage_Image : MonoBehaviour {

		[Tooltip ("The offline clicker to show the percentage for.")]
		[SerializeField]
		protected OfflineClicker clicker;

		protected Image myImage;

		void Awake() {
			myImage = GetComponent<Image>();
			if (clicker == null) clicker = GetComponentInParent<OfflineClicker> ();
			if (clicker == null) Debug.LogError ("OfflineClickerPercentage_Image couldn't find an OfflineClicker and one has not been assigned.");
		}

		void Update() {
			if (clicker != null) {
				myImage.fillAmount = clicker.CurrentStoreAsPercentage;
			}
		}
	}
}
