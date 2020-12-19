using UnityEngine;
using System;
using System.Collections;

namespace IdleClickerKit
{

	public class OfflineClicker : AutoClicker {

		[Tooltip ("Does this offline clicker have a limit to how many clicks it can generate. Use 0 for no limit.")]
		[SerializeField]
		protected int storage;

		[Tooltip ("If storage is non-zero how will we collect things form storage?")]
		[SerializeField]
		protected OfflineClickCollectionType collectionType;

		[Tooltip ("Does storage increases with upgrades, or do upgrades only fill storage quicker?")]
		[SerializeField]
		protected bool storageIncreasesWithUpgrades;

		[Tooltip ("If true we will auto collect on start.")]
		[SerializeField]
		protected bool autoCollectOnStart;

		/// <summary>
		/// Number of clicks currently stored.
		/// </summary>
		protected int currentStore;

		/// <summary>
		/// When was the store last updated? Used for offline calculation.
		/// </summary>
		protected long lastUpdateDate;

		public int CurrentStore {
			get {
				return currentStore;
			}
		}

		/// <summary>
		/// Returns the current store as a perecentage of the total store.
		///  With 0.0 = 0%, and 1.0 = 100%. If storage = 0, returns 1.0f.
		/// </summary>
		/// <value>The current store as percentage (from 0 to 1).</value>
		public float CurrentStoreAsPercentage {
			get {
				if (storage <= 0) return 1.0f;
				return Mathf.Min(1.0f, ((float)currentStore / (float)TotalStorage));
			}
		}

		/// <summary>
		/// Gets the total amount storable.
		/// </summary>
		/// <value>The total storage.</value>
		public int TotalStorage {
			get {
				if (storageIncreasesWithUpgrades) return storage * currentCount;
				return storage;
			}
		}

		/// <summary>
		/// Gets the custom info. Mappings: 
		/// {0} = clickAmount, {1} = timeInterval, {2} = storage, 
		/// {3} = current clicks generated, {4} = current storage
		/// </summary>
		/// <value>The custom info.</value>
		override public string[] CustomInfo {
			get {
				return new string[]{ "" + clickAmount, "" + timeInterval, "" + storage, ""+ (currentCount * clickAmount), "" + (storageIncreasesWithUpgrades ? (storage * currentCount): storage)};
			}
		}

		public void Collect() {
			if ((collectionType == OfflineClickCollectionType.MANUAL_WHEN_FULL && currentStore >= TotalStorage) ||
			    collectionType == OfflineClickCollectionType.MANUAL_ANYTIME) {
				ClickManager.GetInstance(rewardClickName).AddClicks (currentStore);
				currentStore = 0;
				Save (this);
			}
		}

		void Awake() {
			Init ();
		}

		override protected IEnumerator AutoClick() {
			while (true) {				
				// Although tecnically this isn't 100% accurate its good enough
				yield return new WaitForSeconds(timeInterval);

				// If storage = 0 this is just like an auto clicker
				if (storage == 0) {
					ClickManager.Instance.AddClicks (clickAmount * currentCount);
					OnClicksGenerated (clickAmount * currentCount);
				} else {
					currentStore += clickAmount * currentCount;
					if (currentStore >= TotalStorage) {
						currentStore = TotalStorage;
						if (collectionType == OfflineClickCollectionType.AUTO_WHEN_FULL) {
							ClickManager.Instance.AddClicks (currentStore);
							OnClicksGenerated (currentStore);
							currentStore = 0;
						}
					}
					lastUpdateDate = DateTime.Now.ToBinary();
					Save (this);
				}
			}
		}

		#region Persistable

		/// <summary>
		/// Gets the unique save key.
		/// </summary>
		override public string UniqueSaveKey {
			get {
				return "Data_OfflineClicker_" + upgradeName;
			}
		}

		/// <summary>
		/// Gets the save data.
		/// </summary>
		/// <value>The save data.</value>
		override public object SaveData {
			get {
				if (lastUpdateDate == 0) lastUpdateDate = DateTime.Now.ToBinary();
				return new object[] {currentCount, currentStore, lastUpdateDate};
			}
			set {
				if (value.GetType () == SavedObjectType) {
					currentCount = (int)((object[])value) [0];
					currentStore = (int)((object[])value) [1];
					lastUpdateDate = (long)((object[])value) [2];
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
			
			// Calculate how many we generated
			TimeSpan t = DateTime.Now - DateTime.FromBinary (lastUpdateDate);
			int generated = (int) (((float)t.TotalSeconds / timeInterval) * ((float)clickAmount * (float)currentCount));

			// Techncially this isn't 100% accurate but it ensures that the number generated fits the clickAmount data
			// Uncomment if you want this to hold
			// int remainder = generated % clickAmount;
			// generated -= remainder;

			// Update store/click manager
			currentStore += generated;
			if (storage == 0) {
				ClickManager.Instance.AddClicks (currentStore);
				OnClicksGenerated (currentStore);
				currentStore = 0;
			} else if (currentStore >= TotalStorage) {
				currentStore = TotalStorage;
			}

			if (autoCollectOnStart) Collect ();

			// Update generation time and save
			lastUpdateDate = DateTime.Now.ToBinary();
			Save (this);
			StartClickers ();
		}

		#endregion

	}

	public enum OfflineClickCollectionType {
		AUTO_WHEN_FULL,
		MANUAL_WHEN_FULL,
		MANUAL_ANYTIME
	}
}