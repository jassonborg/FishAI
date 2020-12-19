using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace IdleClickerKit
{
	[RequireComponent (typeof(Text))]
	public class PerSecond_Text : MonoBehaviour {

		[Tooltip ("Name of the clicker this per click text is for. Leave blank for default.")]
		[SerializeField]
		protected string clickName;

		[Tooltip ("List of clickers to consider in the calculation.")]
		[SerializeField]
		protected AutoClicker[] autoClickers;

		[SerializeField]
		[Tooltip ("Text to show. The variable {0} will be replaced with the number of clicks per second, and {1} with the click name.")]
		protected string textString = "{0:0.0} {1} per second.";

		[SerializeField]
		[Tooltip ("How often do we update this value. Use 0 for every frame.")]
		protected float updateInterval;

		protected Text myText;

		/// <summary>
		/// Init.
		/// </summary>
		void Start() {
			PostInit ();
		}


		/// <summary>
		/// Init.
		/// </summary>
		void Update() {
			if (updateInterval == 0) UpdateText ();
		}
			
		/// <summary>
		/// Initialise instance. In this casel lookup text references.
		/// </summary>
		virtual protected void PostInit() {
			myText = GetComponent<Text>();
			if (updateInterval > 0) InvokeRepeating ("UdpateText", 0.0f, updateInterval);
		}

		/// <summary>
		/// Initialise instance. In this casel lookup text references.
		/// </summary>
		virtual protected void UpdateText() {
			float total = 0;
			for (int i = 0; i < autoClickers.Length; i++) {
				total += autoClickers[i].CurrentClicksPerSecond;
			}
			myText.text = string.Format(textString, total, ClickManager.GetInstance(clickName).ClickName);
		}

	}
}