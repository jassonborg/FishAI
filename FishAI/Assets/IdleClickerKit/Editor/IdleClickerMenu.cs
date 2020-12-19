using UnityEditor;
using UnityEngine;
using System.Collections;

namespace IdleClickerKit
{
	public class IdleClickMenu  {

		[MenuItem ("Assets/Idle Clicker kit/Reset All Player Prefs")]
		public static void ResetAllPlayerPrefs()
		{
			PlayerPrefs.DeleteAll ();
		}
	}
}
