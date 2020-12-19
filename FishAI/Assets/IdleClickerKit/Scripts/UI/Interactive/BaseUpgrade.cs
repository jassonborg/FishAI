using UnityEngine;
using System.Collections;

namespace IdleClickerKit
{
	/// <summary>
	/// Base class which upgrades inherit from.
	/// </summary>
	public abstract class BaseUpgrade : Persistable {

		[Header ("Human Readable")]

		[Tooltip ("Name of the object.")]
		[SerializeField]
		protected string upgradeName;

		[Tooltip ("Description of the component you can use the variables: {0} for name, {1} for click name, and {2} to {6} for component specific variables.")]
		[SerializeField]
		protected string description;

		[Tooltip ("Alternate description of the component. Use for example to provide different descriptions for active/inactive. Has the same variables available as description.")]
		[SerializeField]
		protected string alternateDescriptionText;

		[Header ("Costs")]
			
		[Tooltip ("Cost to buy one of these.")]
		[SerializeField]
		protected int cost;

		
		[Tooltip ("By what percentage does the cost increase. 0 means not at all, 1 means 100% (doubles each time you buy)")]
		[SerializeField]
		[Range (0.0f, 2.0f)]
		protected float costIncrease;

		[Tooltip ("Number of (available) clicks required for this upgrade to be made visible.")]
		[SerializeField]
		protected int visibleCost;


		[Tooltip ("Name of the clicks used to buy this upgrade. Leave blank for default.")]
		[SerializeField]
		protected string costClickName;

		[Header ("Upgrades")]

		[Tooltip ("Number of upgrades to start with, in case you want to start with mroe than 0.")]
		[SerializeField]
		protected int startingCount = 0;

		[Tooltip ("Maxiumum number of upgrades that can be purchased.")]
		[SerializeField]
		protected int maxUpgradeCount = 9;

		[Tooltip ("Name of the clicker this upgrade acts on. Leave blank for default.")]
		[SerializeField]
		protected string rewardClickName;


		[Header ("UI Components")]

		[Tooltip ("GameObject to show when this upgrade becomes visible.")]
		[SerializeField]
		protected GameObject visibleContent;

		[Tooltip ("GameObject to show when this upgrade can't be afforded.")]
		[SerializeField]
		protected GameObject cantAffordContent;

		[Tooltip ("GameObject to show when this upgrade cannot be purcahsed any more becasue it is maxed out.")]
		[SerializeField]
		protected GameObject maxedOutContent;

		[Tooltip ("When we show one content item (e.g Cant Afford Content) should we disbale the other content item.")]
		[SerializeField]
		protected bool deactivateComponents;

		[Tooltip ("Array of GameObjects to activate as upgrades are purchased (typically images).")]
		[SerializeField]
		protected GameObject[] pips;

		/// <summary>
		/// Number of upgrades purchased.
		/// </summary>
		protected int currentCount;

		/// <summary>
		/// Is this upgrade currently visible?
		/// </summary>
		protected bool isVisible;

		#region events

		public event System.EventHandler <ClickEventArgs> ClicksGenerated;

		public event System.EventHandler <UpgradeEventArgs> Upgraded;

		/// <summary>
		/// Raises the clicks generated event.
		/// </summary>
		/// <param name="amount">Amount of clicks generated.</param>
		virtual protected void OnClicksGenerated(int amount) {
			if (ClicksGenerated != null) {
				ClicksGenerated(this, new ClickEventArgs(rewardClickName, amount));
			}
		}

		/// <summary>
		/// Raises the upgraded event.
		/// </summary>
		virtual protected void OnUpgraded() {
			if (Upgraded != null) {
				Upgraded (this, new UpgradeEventArgs (upgradeName, currentCount));
			}
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets the count (number purchased).
		/// </summary>
		/// <value>The count.</value>
		virtual public int Count {
			get {
				return currentCount;
			}
		}
		
		/// <summary>
		/// Gets or sets the cost.
		/// </summary>
		/// <value>The cost.</value>
		virtual public int Cost {
			get {
				if (costIncrease == 0) return cost;
				return  (int)((float)cost * Mathf.Pow((1.0f + costIncrease), (float)currentCount));
			}
		}

		virtual public string UpgradeName {
			get {
				return upgradeName;
			}
		}

		virtual public string Description {
			get {
				return description;
			}
		}

		virtual public string AlternateDescription {
			get {
				return alternateDescriptionText;
			}
		}

		virtual public string ClickName {
			get {
				return costClickName;
			}
		}
			
		virtual public string[] CustomInfo {
			get {
				return new string[]{ "", "", "", "", ""};
			}
		}

		#endregion

		// Update is called once per frame
		void Update () {
			UpdateVisibleState ();
		}

		virtual protected void Init() {
			Load (this);
			if (visibleCost > Cost) visibleCost = Cost;
			if (visibleCost > 0) Hide();
			CheckMaxedOut ();
			UpdatePips ();
		}

		virtual protected void UpdateVisibleState() {
			
			if (!deactivateComponents) {
				if (CheckVisible ()) {
					Show ();
				} else {
					Hide ();
				}
				if (CheckAffordable ()) {
					HideNotAffordable ();
				} else {
					ShowNotAffordable ();

				}
				if (CheckMaxedOut ()) {
					ShowMaxedOut ();
				} else {
					HideMaxedOut ();
				}
			} else {
				CheckVisible ();

				if (CheckMaxedOut ()) {
					ShowMaxedOut ();
					Hide ();
					HideNotAffordable ();
					return;
				}
				if (CheckAffordable ()) {
					Show();
					HideNotAffordable ();
					HideMaxedOut ();
					return;
				} else {
					ShowNotAffordable ();
					Hide ();
					HideMaxedOut ();
					return;
				}
			}
		}

		virtual protected bool CheckVisible() {
			if (isVisible) return true;
			if (ClickManager.GetInstance(costClickName).Clicks >= visibleCost || currentCount > 0) {
				isVisible = true;
				return true;
			}
			return false;
		}

		virtual protected bool CheckAffordable() {
			// No need to change if this is maxed or not visible
			if (currentCount < maxUpgradeCount && isVisible) {
				if (ClickManager.GetInstance(costClickName).Clicks < Cost) {
					return false;
				}
			}
			return true;
		}

		virtual protected bool CheckMaxedOut() {
			if (currentCount >= maxUpgradeCount) {
				return true;
			}
			return false;
		}

		virtual protected void UpdatePips() {
			for (int i = 0; i < pips.Length; i++) {
				if (currentCount > i) {
					pips [i].SetActive (true);
				} else {
					pips [i].SetActive (false);
				}
			}
		}

		virtual protected void Show() {
			if (visibleContent != null) visibleContent.SetActive (true);
		}

		virtual protected void Hide() {
			isVisible = false;
			if (visibleContent != null) visibleContent.SetActive (false);
		}

		virtual protected void ShowNotAffordable() {
			if (cantAffordContent != null) cantAffordContent.SetActive (true);
		}

		virtual protected void HideNotAffordable() {
			if (cantAffordContent != null) cantAffordContent.SetActive (false);
		}

		virtual protected void ShowMaxedOut() {
			if (maxedOutContent != null) maxedOutContent.SetActive (true);
		}

		virtual protected void HideMaxedOut() {
			if (maxedOutContent != null) maxedOutContent.SetActive (false);
		}

		virtual public void Buy() {
			if (isVisible && currentCount < maxUpgradeCount && ClickManager.GetInstance(costClickName).Purchase (Cost)) {
				currentCount++;
				DoBuy();
				// UpdateVisibleState ();
				UpdatePips ();
				Save (this);
				OnUpgraded ();
			}
		}

		/// <summary>
		/// Things to do after load.
		/// </summary>
		override public void PostResetAction() {
			currentCount = startingCount;
		}

		abstract protected void DoBuy();

	}
}