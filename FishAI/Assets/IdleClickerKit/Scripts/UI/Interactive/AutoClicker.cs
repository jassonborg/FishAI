using UnityEngine;
using System.Collections;

namespace IdleClickerKit
{
	
	public class AutoClicker : BaseUpgrade {

		[Header ("Click Data")]

		[Tooltip ("Amount of clicks added per time internal.")]
		[SerializeField]
		protected int clickAmount;

		[Tooltip ("How often does this auto clicker, click?")]
		[SerializeField]
		protected float timeInterval;

		/// <summary>
		/// Gets the custom info. Mappings: 
		/// {0} = clickAmount, {1} = timeInterval, {2} = storage, 
		/// {3} = current clicks generated
		/// </summary>
		/// <value>The custom info.</value>
		override public string[] CustomInfo {
			get {
				return new string[]{ "" + clickAmount, "" + timeInterval, "" + rewardClickName, "" + (currentCount * clickAmount), ""};
			}
		}

		public float CurrentClicksPerSecond {
			get {
				return ((float)currentCount * (float)clickAmount) / timeInterval;
			}
		}

		protected bool hasStarted;

		void Awake() {
			Init ();
		}

		override protected void DoBuy() {
			StartClickers();
		}

		virtual protected void StartClickers() {
			if (!hasStarted) {
				StartCoroutine (AutoClick());
				hasStarted = true;
			}
		}

		virtual protected void StopClickers() {
			StopCoroutine (AutoClick());
			hasStarted = false;
		}

		virtual protected IEnumerator AutoClick() {
			while (true) {				
				// Although tecnically this isn't 100% accurate its good enough
				yield return new WaitForSeconds(timeInterval);
				// Update clicks
				ClickManager.GetInstance(rewardClickName).AddClicks (clickAmount * currentCount);
				// Send event
				OnClicksGenerated (clickAmount * currentCount);
			}
		}

#region Persistable

		/// <summary>
		/// Gets the unique save key.
		/// </summary>
		override public string UniqueSaveKey {
			get {
				return "Data_AutoClicker_" + upgradeName;
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
			StartClickers ();
		}

		/// <summary>
		/// Things to do after reset.
		/// </summary>
		override public void PostResetAction() {
			base.PostResetAction ();
			if (currentCount > 0) StartClickers ();
		}


#endregion

	}
}