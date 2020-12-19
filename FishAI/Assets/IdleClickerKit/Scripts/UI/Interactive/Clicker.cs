using UnityEngine;
using System.Collections;

namespace IdleClickerKit {

	/// <summary>
	/// A thing that is clicked.
	/// </summary>
	public class Clicker : Persistable {

		/// <summary>
		/// The click type to get or blank for default.
		/// </summary>
		[Tooltip ("The click type to get or blank for default.")]
		[SerializeField]
		protected string clickName;

		/// <summary>
		/// Attach the UI click event to this to do a click.
		/// </summary>
		virtual public void Click() {
			ClickManager.GetInstance(clickName).AddClicks (ClickManager.GetInstance(clickName).ClickIncrement);
		}

		// Note although clickers don't save anything we extend persistable so that extensions class like the cool down 
		// clicker can persiste something

		#region Persistable

		/// <summary>
		/// Gets the unique save key.
		/// </summary>
		override public string UniqueSaveKey {
			get {
				return "Data_Clicker_" + clickName + "_" + gameObject.name;
			}
		}

		/// <summary>
		/// Gets the save data.
		/// </summary>
		/// <value>The save data.</value>
		override public object SaveData {
			get {
				return new object[] {0};
			}
			set {
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
