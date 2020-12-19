using UnityEngine;
using System.Collections;

namespace IdleClickerKit.Effects
{
	/// <summary>
	/// Simple FX class which adds a force to a rigidbody.
	/// </summary>
	/// </summary>
	[RequireComponent (typeof(Rigidbody2D))]
	public class FX_ImpartForce2D : FX_Base
	{
		/// <summary>
		/// How much force to apply.
		/// </summary>
		public Vector2 force;
		
		/// <summary>
		/// Force mode to use.
		/// </summary>
		public ForceMode2D forceMode;

		/// <summary>
		/// Do the effect.
		/// </summary>
		override protected void DoEffect()
		{
			GetComponent<Rigidbody2D>().AddForce (force, forceMode);
		}
	}
}