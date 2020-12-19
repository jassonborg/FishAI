using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace IdleClickerKit {

	public class ContinuousClicker : CooldownClicker, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {

		/// <summary>
		/// Enable or disable the continuous clicking function.
		/// </summary>
		public bool Active;

		/// <summary>
		/// Respond to the pointer down event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnPointerDown (PointerEventData eventData)
		{
			Active = true;
		}

		/// <summary>
		/// Respond to the pointer up event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnPointerUp (PointerEventData eventData)
		{
			Active = false;
		}

		/// <summary>
		/// Respond to the pointer exit event.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnPointerExit (PointerEventData eventData)
		{
			Active = false;
		}

		/// <summary>
		/// Unity Update hook.
		/// </summary>
		void Update()
		{
			if (coolDownTimer > 0) {
				coolDownTimer -= Time.deltaTime;
				image.fillAmount = CoolDownPercentage;
			}
			if (Active && coolDownTimer <= 0) Click ();
		}
	}
}