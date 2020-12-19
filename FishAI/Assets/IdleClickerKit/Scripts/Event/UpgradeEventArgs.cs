using UnityEngine;
using System.Collections;

public class UpgradeEventArgs : System.EventArgs {

	string upgradeName;

	int newCount;

	public string UpgradeName {
		get {
			return upgradeName;
		}
	}

	public int NewCount {
		get {
			return newCount;
		}
	}

	public UpgradeEventArgs(string upgradeName, int newCount) {
		this.upgradeName = upgradeName;
		this.newCount = newCount;
	}

}
