using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace IdleClickerKit
{
	[RequireComponent (typeof(Text))]
	public class PerClick_Text : MonoBehaviour {

		[Tooltip ("Name of the clicker this per click text is for. Leave blank for default.")]
		[SerializeField]
		protected string clickName;

		[SerializeField]
		[Tooltip ("Text to show. The variable {0} will be replaced with the click increment, and {1} with the click name.")]
		protected string textString;

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
			UpdateText ();
		}
			
		/// <summary>
		/// Initialise instance. In this casel lookup text references.
		/// </summary>
		virtual protected void PostInit() {
			myText = GetComponent<Text>();
		}

		/// <summary>
		/// Initialise instance. In this casel lookup text references.
		/// </summary>
		virtual protected void UpdateText() {
			myText.text = string.Format(textString, ClickManager.GetInstance(clickName).ClickIncrement, ClickManager.GetInstance(clickName).ClickName);
		}

	}
}