using UnityEngine;
using System.Collections;

namespace IdleClickerKit
{
	public class ClickEventArgs : System.EventArgs {

		string clickName;

		int clickAmount;

		public string ClickName {
			get {
				return clickName;
			}
		}

		public int ClickAmount {
			get {
				return clickAmount;
			}
		}

		public ClickEventArgs(string clickName, int clickAmount) {
			this.clickName = clickName;
			this.clickAmount = clickAmount;
		}
	}
}