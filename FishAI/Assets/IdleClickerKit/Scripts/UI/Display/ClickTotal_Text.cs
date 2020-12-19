using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace IdleClickerKit
{
	[RequireComponent (typeof(Text))]
	public class ClickTotal_Text : MonoBehaviour {
		
		[Tooltip ("Name of the clicker this total is for. Leave blank for default.")]
		[SerializeField]
		protected string clickName;

		/// <summary>
		/// Should we count up to our target value.
		/// </summary>
		[SerializeField]
		protected bool useCountUp = true;

		[SerializeField]
		protected bool useKSymbol = false;

		[SerializeField]
		protected bool useMSymbol = false;

		[SerializeField]
		protected bool useBSymbol = false;

		[SerializeField]
		protected bool useCommas = false;

		protected Text myText;

		protected long displayedTotal;

		/// <summary>
		/// Init.
		/// </summary>
		void Start() {
			PostInit ();
		}

		/// <summary>
		/// Update the click count.
		/// </summary>
		void Update () {
			if (!useCountUp) {
				// Each frame update number of clicks
				myText.text = "" + ClickManager.GetInstance(clickName).Clicks;
			}
		}

		/// <summary>
		/// Initialise instance. In this casel lookup text references.
		/// </summary>
		virtual protected void PostInit() {
			myText = GetComponent<Text>();
			if (useCountUp)
				StartCoroutine (ShowClickTotal ());
		}

		private IEnumerator ShowClickTotal()
		{
			displayedTotal = ClickManager.GetInstance(clickName).Clicks;
			myText.text = GetStringForValue (displayedTotal);

			while (true) {
				if (displayedTotal != ClickManager.GetInstance(clickName).Clicks) {
					float difference = displayedTotal - ClickManager.GetInstance(clickName).Clicks;
					      if (difference > 400000000) displayedTotal -= 135137313;
					else if (difference > 40000000) displayedTotal -= 1351371;
					else if (difference > 4000000) displayedTotal -= 1351371;
					else if (difference > 400000) displayedTotal -= 135173;
					else if (difference > 40000) displayedTotal -= 13517;
					else if (difference > 4000) displayedTotal -= 1351;
					else if (difference > 400) displayedTotal -= 133;
					else if (difference > 40) displayedTotal -= 13;
					else if (difference > 0) displayedTotal -= 1;

					else if (difference < -400000000) displayedTotal += 135137313;
					else if (difference < -40000000) displayedTotal += 1351371;
					else if (difference < -4000000) displayedTotal += 1351371;
					else if (difference < -400000) displayedTotal += 135137;
					else if (difference < -40000) displayedTotal += 13517;
					else if (difference < -4000) displayedTotal += 1351;
					else if (difference < -400) displayedTotal += 133;
					else if (difference < -40) displayedTotal += 13;
					else if (difference < 0) displayedTotal += 1;
					myText.text = GetStringForValue (displayedTotal);
				}
				yield return true;
			}
		}

		public string GetStringForValue(long value) {
			if (value >= 100000000000 && useBSymbol)
				return (useCommas ? string.Format ("{0:N0}B", value / 1000000000) : string.Format ("{0:D}B", value / 1000000000));
			else if (value >= 100000000 && useMSymbol)
				return (useCommas ? string.Format ("{0:N0}M", value / 1000000) : string.Format ("{0:D}M", value / 1000000));
			else if (value >= 100000 && useKSymbol) {
				return (useCommas ? string.Format ("{0:N0}K", value / 1000) : string.Format ("{0:D}K", value / 1000));
			}
			return (useCommas ?  string.Format("{0:N0}", value) :  string.Format("{0:D}", value));
		}
	}
}