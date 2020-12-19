using UnityEngine;
using System.Collections;

namespace IdleClickerKit {
	
	public class ClickConverter : MonoBehaviour {

		[Tooltip ("Click name of the resource that is consumed.")]
		[SerializeField]
		protected string costClickName;

		[Tooltip ("Number of resources the process costs.")]
		[SerializeField]
		protected int costClickAmount;

		[Tooltip ("Click name of the resource that is rewarded.")]
		[SerializeField]
		protected string rewardClickName;

		[Tooltip ("Number of resources the process rewards.")]
		[SerializeField]
		protected int rewardClickAmount;

		public string CostName {
			get { return costClickName; }
		}

		public int CostAmount {
			get { return costClickAmount; }
		}

		public string RewardName {
			get { return rewardClickName; }
		}

		public int RewardAmount {
			get { return rewardClickAmount; }
		}

		public void Click() {
			if (ClickManager.GetInstance (costClickName).Clicks >= costClickAmount) {
				if (ClickManager.GetInstance (costClickName).Purchase (costClickAmount)) {
					ClickManager.GetInstance (rewardClickName).AddClicks (rewardClickAmount);
				}
			}
		}
	}
}
