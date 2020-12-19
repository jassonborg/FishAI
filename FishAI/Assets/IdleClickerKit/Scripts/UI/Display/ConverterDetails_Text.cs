using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace IdleClickerKit
{
	[RequireComponent (typeof(Text))]
	public class ConverterDetails_Text : MonoBehaviour {
		
		[Tooltip ("Type of label to display.")]
		[SerializeField]
		protected ConverterLabelType labelType;

		[Tooltip ("The click converter component to get name from.")]
		[SerializeField]
		protected ClickConverter converter;

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
			if (converter == null)
				converter = gameObject.GetComponentInParent<ClickConverter> ();
			if (converter == null) {
				Debug.LogWarning ("ConverterDetails_Text couldn't find a converter to get the text from");
			} else {
				UpdateText ();
				RegisterListeners ();
			}
		}

		virtual protected void UpdateText() {
			switch (labelType) {
			case ConverterLabelType.COST_NAME:
				myText.text = converter.CostName;
				break;
			case ConverterLabelType.COST_AMOUNT:
				myText.text = converter.CostAmount.ToString();
				break;
			case ConverterLabelType.REWARD_NAME:
				myText.text = converter.RewardName;
				break;
			case ConverterLabelType.REWARD_AMOUNT:
				myText.text = converter.RewardAmount.ToString ();;
				break;
			
			}
		}
			
		void RegisterListeners () {
//			converter.Converted += ConvertEventHandler;
		}
//
		void DeregisterListeners () {
//			if (converter != null) converter.Convert -= ConvertEventHandler;
		}

//		void ConvertEventHandler (object sender, ConvertEventArgs e)
//		{
//			UpdateText ();
//		}

	}

	public enum ConverterLabelType {
		COST_NAME, COST_AMOUNT, REWARD_NAME, REWARD_AMOUNT
	}
}