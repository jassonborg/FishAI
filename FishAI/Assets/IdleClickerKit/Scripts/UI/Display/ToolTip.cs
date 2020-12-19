using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace IdleClickerKit
{
	public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

		[Tooltip ("UI components to show or hide.")]
		[SerializeField]
		protected GameObject toolTipComponent;

		[Tooltip ("If true toolTipComponent will be moved to mouse position.")]
		[SerializeField]
		protected bool moveTipToMousePosition;

		protected Vector2 lastPosition;

		const float ToolTipDelay = 1.25f;

		const float MouseMoveLeeway = 10.0f;

		public void OnPointerEnter(PointerEventData eventData)
		{
			StopAllCoroutines ();
			StartCoroutine (CheckForHover ());
		}   

		public void OnPointerExit(PointerEventData eventData)
		{
			StopAllCoroutines ();
			Hide();
		}

		public void Show () {
			if (moveTipToMousePosition) {
				MoveToolTipToMousePosition ();
			}
			toolTipComponent.SetActive (true);
			StartCoroutine (CheckForCancel());
		}

		public void Hide () {
			toolTipComponent.SetActive (false);
		}

		protected void MoveToolTipToMousePosition() {
			// Note this works for screen overlay only
			Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			pos = new Vector3 ((float)(((int)(pos.x * 5)) / 5.0f), (float)(((int)(pos.y * 5)) / 5.0f), 0);
			toolTipComponent.transform.position = pos;
		}

		protected IEnumerator CheckForHover () {
			float timeStill = 0.0f;
			lastPosition = Input.mousePosition;
			yield return true;
			while (timeStill < ToolTipDelay) {
				if (Vector2.Distance(lastPosition, Input.mousePosition) > MouseMoveLeeway) {
					timeStill = 0;
				} else {
					timeStill += Time.deltaTime;
				}
				lastPosition = Input.mousePosition;
				yield return true;
			}
			Show ();
		}

		protected IEnumerator CheckForCancel () {
			lastPosition = Input.mousePosition;
			yield return true;
			while (Vector2.Distance(lastPosition, Input.mousePosition) < MouseMoveLeeway) {
				yield return true;
			}
			Hide ();
		}
	}
}