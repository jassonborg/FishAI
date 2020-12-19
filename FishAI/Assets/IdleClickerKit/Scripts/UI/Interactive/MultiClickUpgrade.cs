using UnityEngine;
using System.Collections;

namespace IdleClickerKit
{
		
	public class MultiClickUpgrade : ClickUpgrade {

		[Tooltip ("Other clicks to reward.")]
		[SerializeField]
		protected string[] additionalRewardClickNames;

		[Header ("Additional Costs")]

		[Tooltip ("Additional resource cost.")]
		[SerializeField]
		protected AdditionalClickCost[] additionalCosts;

		override protected void DoBuy() {
			ClickManager.GetInstance(rewardClickName).IncreaseClickIncrement (clicksAdded);
			foreach (string c in additionalRewardClickNames) {
				ClickManager.GetInstance (c).IncreaseClickIncrement (clicksAdded);
			}
		}
			
		void Awake() {
			Init ();
		}

		/// <summary>
		/// Gets the cost for the click with the given name.
		/// </summary>
		/// <returns>The cost for the click name or 0 if clickName is not found.</returns>
		/// <param name="clickName">Click name.</param>
		virtual public int GetCostForClickName(string clickName) {
			if (clickName == null || clickName == "") return Cost;
			if (clickName == costClickName) return Cost;
			foreach (AdditionalClickCost acc in additionalCosts) {
				if (acc.ClickName == clickName) return acc.Cost;
			}
			return 0;
		}

		/// <summary>
		/// Checks if this is affordable and updates UI to match. Here we need to also check the additional costs.
		/// </summary>
		override protected bool CheckAffordable() {
			// No need to change if this is maxed or not visible
			if (currentCount < maxUpgradeCount && isVisible) {
				if (ClickManager.GetInstance (costClickName).Clicks < Cost) {
					return false;
				}
				else {
					foreach (AdditionalClickCost acc in additionalCosts) {
						if (ClickManager.GetInstance (acc.ClickName).Clicks < acc.Cost) {
							return false;
						}
					}
				}
			}
			return true;
		}

#region Persistable

		/// <summary>
		/// Gets the unique save key.
		/// </summary>
		override public string UniqueSaveKey {
			get {
				return "Data_MultiClickUpgrade_" + upgradeName;
			}
		}

		/// <summary>
		/// Things to do after load.
		/// </summary>
		override public void PostLoadAction() {
			ClickManager.GetInstance(rewardClickName).IncreaseClickIncrement (clicksAdded * currentCount);
			foreach (string c in additionalRewardClickNames) {
				ClickManager.GetInstance (c).IncreaseClickIncrement (clicksAdded * currentCount);
			}
		}

		/// <summary>
		/// Things to do after reset.
		/// </summary>
		override public void PostResetAction() {
			currentCount = 0;
		}

#endregion

	}

	[System.Serializable]
	public class AdditionalClickCost {
		
		[Tooltip ("Cost click name.")]
		[SerializeField]
		string clickName;

		[Tooltip ("Cost amount.")]
		[SerializeField]
		int cost;

		public string ClickName {
			get { return clickName; }
            protected set { clickName = value; }
        }

		public int Cost {
			get { return cost; }
            protected set { cost = value; }
        }

	}
}