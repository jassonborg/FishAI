using UnityEngine;
using System.Collections;

namespace IdleClickerKit
{
		
	public class ShowObjectUpgrade : BaseUpgrade {


		void Awake() {
			Init ();
		}

		override protected void DoBuy() {
			// Do nothing, all we do is show the pips
		}


#region Persistable

		/// <summary>
		/// Gets the unique save key.
		/// </summary>
		override public string UniqueSaveKey {
			get {
				return "Data_ShowUpgrade_" + upgradeName;
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

#endregion

	}
}