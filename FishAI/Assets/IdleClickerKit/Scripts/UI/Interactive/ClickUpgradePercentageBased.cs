using UnityEngine;
using System.Collections;

namespace IdleClickerKit
{
		
	public class ClickUpgradePercentageBased : BaseUpgrade {

		[Header ("Click Data")]

		[Tooltip ("Percentage boost.")]
		[SerializeField]
		[Range(0.0f,1.0f)]
		protected float boostPercentage;

		/// <summary>
		/// The actual amount of clicks boosted. We could use a float here, but with any signficant number of clicks, the roudning should be negligble.
		/// </summary>
		protected int currentBoost;

		override protected void DoBuy() {
			int boostedAmount = (int)((float)ClickManager.GetInstance (rewardClickName).ClickIncrement * boostPercentage);
			currentBoost += boostedAmount;
			ClickManager.GetInstance(rewardClickName).IncreaseClickIncrement (boostedAmount);
		}

		/// <summary>
		/// Gets the custom info. Mappings: 
		/// {0} = clickAdded, {1} = current clicks added
		/// </summary>
		/// <value>The custom info.</value>
		override public string[] CustomInfo {
			get {
				return new string[]{ "" + string.Format("{0:P1}%", boostPercentage), "" + (string.Format("{0:P1}%", (float)currentCount * boostPercentage)), "", "", ""};
			}
		}

		void Awake() {
			Init ();
		}

#region Persistable

		/// <summary>
		/// Gets the unique save key.
		/// </summary>
		override public string UniqueSaveKey {
			get {
				return "Data_ClickUpgradePercentageBased_" + upgradeName;
			}
		}

		/// <summary>
		/// Gets the save data.
		/// </summary>
		/// <value>The save data.</value>
		override public object SaveData {
			get {
				return new object[] {currentCount, currentBoost};
			}
			set {
				if (value.GetType () == SavedObjectType) {
					currentCount = (int)((object[])value) [0];
					currentBoost = (int)((object[])value) [1];
				}
			}
		}

		/// <summary>
		/// Get the type of object this Persistable saves.
		/// </summary>
		override public System.Type SavedObjectType {
			get {
				return typeof(object[]);
			}
		}

		/// <summary>
		/// Things to do after load.
		/// </summary>
		override public void PostLoadAction() {
			ClickManager.GetInstance(rewardClickName).IncreaseClickIncrement (currentBoost);
		}

#endregion

	}
}