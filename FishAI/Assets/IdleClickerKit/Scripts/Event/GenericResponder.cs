using UnityEngine;
#if !UNITY_4_6 && !UNITY_5_1 && !UNITY_5_2
using UnityEngine.SceneManagement;
#endif
using System.Collections;
using IdleClickerKit.Effects;

namespace IdleClickerKit
{
	/// <summary>
	/// Use this component to do something when an event occurs. This is a base class, and should be extended.
	/// </summary>
	public abstract class GenericResponder : MonoBehaviour
	{
		/// <summary>
		/// Handles the event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		virtual protected void DoAction(EventResponse action, System.EventArgs args)
		{
			if (action.delay == 0.0f) DoImmediateAction (action, args);
			else StartCoroutine(DoDelayedAction(action, args));
		}

		/// <summary>
		/// Do the action after a delay.
		/// </summary>
		/// <param name="args">Event arguments.</param>
		/// <param name="action">Action.</param>
		virtual protected IEnumerator DoDelayedAction(EventResponse action, System.EventArgs args)
		{
			float delayTimer = action.delay;
			while (delayTimer > 0.0f)
			{
				delayTimer -= Time.deltaTime;
				yield return true;
			}
			DoImmediateAction (action, args);
		}
		
		/// <summary>
		/// Do the action
		/// </summary>
		/// <param name="args">Event arguments.</param>
		/// <param name="action">Action.</param>
		virtual protected void DoImmediateAction(EventResponse action, System.EventArgs args)
		{
			Animator animator;
			Animation animation;
			switch(action.responseType)
			{
			case EventResponseType.DEBUG_LOG:
				Debug.Log (string.Format ("Got event, arguments: {0}", args != null ? args.ToString () : ""));
				break;
			case EventResponseType.ACTIVATE_GAMEOBJECT:
				action.targetGameObject.SetActive(true);	
				break;
			case EventResponseType.DEACTIVATE_GAMEOBJECT:
				action.targetGameObject.SetActive(false);	
				break;
			case EventResponseType.SEND_MESSSAGE:
				action.targetGameObject.SendMessage(action.message, SendMessageOptions.DontRequireReceiver);
				break;
			case EventResponseType.ENABLE_BEHAVIOUR:
				if (action.targetComponent is Behaviour) ((Behaviour)action.targetComponent).enabled = true;
				else if (action.targetComponent is Renderer) ((Renderer)action.targetComponent).enabled = true;
				break;
			case EventResponseType.DISABLE_BEHAVIOUR:
				if (action.targetComponent is Behaviour) ((Behaviour)action.targetComponent).enabled = false;
				else if (action.targetComponent is Renderer) ((Renderer)action.targetComponent).enabled = false;
				break;
			case EventResponseType.PLAY_PARTICLES:
				if (action.targetComponent is ParticleSystem) {
					((ParticleSystem)action.targetComponent).Play ();
				}
				break;
			case EventResponseType.PAUSE_PARTICLES:
				if (action.targetComponent is ParticleSystem) ((ParticleSystem)action.targetComponent).Pause();
				break;
			case EventResponseType.SWITCH_SPRITE:
				if (action.targetComponent is SpriteRenderer) ((SpriteRenderer)action.targetComponent).sprite = action.newSprite;
				break;
			case EventResponseType.LOAD_SCENE:
				#if !UNITY_4_6 && !UNITY_5_1 && !UNITY_5_2
				SceneManager.LoadScene(action.message);
				#else
				Application.LoadLevel (action.message);
				#endif
				break;
			
			case EventResponseType.START_EFFECT:
				if (action.targetComponent is FX_Base) 
				{
					if (action.targetGameObject != null && action.message != null && action.message != "") ((FX_Base)action.targetComponent).StartEffect(action.targetGameObject, action.message);
					else ((FX_Base)action.targetComponent).StartEffect();
				}
				else Debug.LogWarning("Trying to play an Effect that isn't derived from FX_Base.");
				break;
			
			case EventResponseType.PLAY_ANIMATION:
				animator = action.targetGameObject.GetComponent<Animator>();
				if (animator != null)
				{
					animator.Play (action.message);
				}
				else
				{
					animation = action.targetGameObject.GetComponent<Animation>();
					if (animation != null) 
					{
						animation.Play();
					}
					else
					{
						Debug.LogWarning("Couldn't find an Animation or Animatopr on the target GameObject");
					}
				}
				break;
			case EventResponseType.STOP_ANIMATION:
				animation = action.targetGameObject.GetComponent<Animation>();
				if (animation != null) 
				{
					animation.Stop();
				}
				else
				{
					Debug.LogWarning("Couldn't find an Animation or Animator on the target GameObject");
				}
				break;
			}

		}
	}
}