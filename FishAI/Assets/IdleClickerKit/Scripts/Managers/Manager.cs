using UnityEngine;
using System.Collections;

namespace IdleClickerKit {
 
	/// <summary>
	/// Abstract class extended by all "manager" type objects.
	/// Managers have only one instance in a scene and this class helps to enforce that.
	/// </summary>
	public abstract class Manager <T> : Persistable where T : Manager <T> {

		/// <summary>
		/// Static reference to this manager.
		/// </summary>
		protected static T manager;

		/// <summary>
		/// Has this manager been initialised.
		/// </summary>
		protected bool initialised = false;

		/// <summary>
		/// Get the instance of the manager class or create if one has not yet been created.
		/// </summary>
		/// <returns>An instance of the manager class.</returns>
		public static T Instance {
			get {
				if (manager == null) Create ();
				return manager;
			}
		}

		/// <summary>
		/// Create a new game object and attach an instance of the manager.
		/// </summary>
		protected static void Create() {
			T[] managers = FindObjectsOfType<T> ();
			foreach (T m in managers) {
				if (m.IsValidManager) {
					if (manager != null) Debug.LogError (string.Format("More than one main manager found for type {0}", typeof(T)));
					manager = m;
				}
			}
			if (manager == null) {
				GameObject go = new GameObject ();
				go.name = typeof(T).Name;
				manager = (T)go.AddComponent (typeof(T));
			}
		}

		/// <summary>
		/// If there is already a manager destroy self, else initialise and assign to the static reference.
		/// </summary>
		void Awake(){
			if (manager == null) {
				if (!initialised) Init ();
				manager = (T) this;
			} else if (manager != this) {
				Destroy (gameObject);	
			} else if (!initialised) {
				Init ();
			}
		}

		/// <summary>
		/// Init this instance.
		/// </summary>
		virtual protected void Init() {
			initialised = true;
		}

		/// <summary>
		/// Determines whether this instance is valid manager to be returned by a call to Instance.
		/// </summary>
		/// <returns><c>true</c> if this instance is valid manager; otherwise, <c>false</c>.</returns>
		virtual protected bool IsValidManager {
			get {
				return true;
			}
		}
	}
}

