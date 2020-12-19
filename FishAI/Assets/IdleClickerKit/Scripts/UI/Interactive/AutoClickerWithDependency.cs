using UnityEngine;
using System.Collections;

namespace IdleClickerKit
{
	/// <summary>
	/// Works exactly like an auto clicker except can only be purchased if the parent of
	/// the pips is active. THis allows you to easily set up a dependency mechanism where 
	/// one object effectily extens or 'works in' another mecanism.
	/// </summary>
	public class AutoClickerWithDependency : AutoClicker {
		
		override protected bool CheckAffordable() {
			bool baseResult = base.CheckAffordable ();
			if (!baseResult) return false;

			// Now check pips
			if (pips.Length > (currentCount)) {
				if (!pips [currentCount].gameObject.transform.parent.gameObject.activeInHierarchy) {
					return false;
				}
			} else {
				return false;
			}
			return true;
		}

		override public void Buy() {
			if (pips.Length > (currentCount)) {
				if (pips [currentCount].gameObject.transform.parent.gameObject.activeInHierarchy) {
					base.Buy ();
				}
			}
		}

	}
}