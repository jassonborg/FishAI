using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace IdleClickerKit
{
	/// <summary>
	/// Shows the name of the default click type.
	/// </summary>
	[RequireComponent (typeof(Text))]
	public class ClickLabel_Text : MonoBehaviour {

		protected Text myText;

		/// <summary>
		/// Init.
		/// </summary>
		void Start() {
			PostInit ();
		}
			
		/// <summary>
		/// Initialise instance. In this casel lookup text references.
		/// </summary>
		virtual protected void PostInit() {
			myText = GetComponent<Text>();
			myText.text =  ClickManager.Instance.ClickName;
		}

	}
}