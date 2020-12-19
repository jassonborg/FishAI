using UnityEngine;
using System.Collections;

namespace IdleClickerKit
{
		
	public class ClickUpgrade : BaseUpgrade {

		[Header ("Click Data")]

		[Tooltip ("Amount of clicks added per click.")]
		[SerializeField]
		protected int clicksAdded;

		override protected void DoBuy() {
			ClickManager.GetInstance(rewardClickName).IncreaseClickIncrement (clicksAdded);
		}

		/// <summary>
		/// Gets the custom info. Mappings: 
		/// {0} = clickAdded, {1} = current clicks added
		/// </summary>
		/// <value>The custom info.</value>
		override public string[] CustomInfo {
			get {
				return new string[]{ "" + clicksAdded, "" + (currentCount * clicksAdded), "", "", ""};
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
				return "Data_ClickUpgrade_" + upgradeName;
			}
		}

		/// <summary>
		/// Gets the save data.
		/// </summary>
		/// <value>The save data.</value>
		override public object SaveData {
			get {
				return new object[] {currentCount};
			}
			set {
				if (value.GetType () == SavedObjectType) {
					currentCount = (int)((object[])value) [0];
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
			ClickManager.GetInstance(rewardClickName).IncreaseClickIncrement (clicksAdded * currentCount);
		}

#endregion

	}
}