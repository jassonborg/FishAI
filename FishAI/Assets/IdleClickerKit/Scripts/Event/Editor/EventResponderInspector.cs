#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IdleClickerKit.Effects;

namespace  IdleClickerKit
{
	[CustomEditor(typeof(EventResponder), true)]
	public class EventResponderInspector : Editor
	{
		/// <summary>
		/// Cached and typed target reference.
		/// </summary>
		protected EventResponder myTarget;

		/// <summary>
		/// Cached types for target.
		/// </summary>
		protected string[] types;

		/// <summary>
		/// Cached events for type.
		/// </summary>
		protected string[] events;

		protected System.Type type;
		protected System.Reflection.EventInfo eventInfo;
		protected System.Type parameterType;

		/// <summary>
		/// Draw the GUI.
		/// </summary>
		public override void OnInspectorGUI()
		{
			myTarget = (EventResponder)target;

			GameObject sender = (GameObject) EditorGUILayout.ObjectField(new GUIContent("Sender", "Add the Game Object that holds the target component."), myTarget.sender, typeof(GameObject), true);

			if (sender != myTarget.sender) 
			{
				myTarget.sender = sender;
				if (myTarget.sender != null) types = GetComponentsOnGameObject(myTarget.sender);
			}

			if (myTarget.sender == null) myTarget.sender = myTarget.gameObject;

			if (myTarget.sender != null)
			{
				string typeName = null;
				if (types == null) types = GetComponentsOnGameObject(myTarget.sender);
				int typeIndex = System.Array.IndexOf(types, myTarget.typeName);
				if (typeIndex == -1 || typeIndex >= types.Length) typeIndex = 0;
				if (types != null && types.Length > 0)
				{	
					typeName = types[EditorGUILayout.Popup("Component", typeIndex, types)];
				}
				else
				{
					EditorGUILayout.HelpBox("No components found on this GameObject.", MessageType.Info);
				}
				if (typeName != myTarget.typeName && typeName != null) 
				{
					myTarget.typeName = typeName;
					if (myTarget.typeName != null) events = GetEventNamesForType(myTarget.typeName);
				}
				if (myTarget.typeName != null && myTarget.typeName.Length > 0)
				{
					if (events == null) events = GetEventNamesForType(myTarget.typeName);
					int eventIndex = System.Array.IndexOf(events, myTarget.eventName);
					if (eventIndex == -1 || eventIndex >= events.Length) eventIndex = 0;
					if (events != null && events.Length > 0)
					{
						string name = events[EditorGUILayout.Popup("Event", eventIndex, events)];
						if (name != myTarget.eventName || parameterType == null)
						{
							myTarget.eventName = name;
							//try
							//{
								type = myTarget.GetType().Assembly.GetType(this.GetType().Namespace + "." + typeName);
								eventInfo = type.GetEvent(myTarget.eventName);
								parameterType = eventInfo.EventHandlerType.GetMethod("Invoke").GetParameters()[1].ParameterType;
							//}
							//catch (System.Exception ex) {
							//	Debug.Log (ex.Message);
							//}
						}

						// TODO Add filters
					}
					else
					{
						EditorGUILayout.HelpBox("No events found on this component.", MessageType.Info);
					}
				}
			}

			if (myTarget.actions != null)
			{
				for (int i = 0; i < myTarget.actions.Length; i++)
				{
					RenderAction(myTarget, myTarget, myTarget.actions[i]);
				}
			}

			GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			// Add new actions
			if (GUILayout.Button("Add Action"))
			{
				if (myTarget.actions == null)
				{
					myTarget.actions = new EventResponse[1];
				}
				else
				{
					// Copy and grow array
					EventResponse[] tmpActions = myTarget.actions;
					myTarget.actions = new EventResponse[tmpActions.Length + 1];
					System.Array.Copy(tmpActions, myTarget.actions, tmpActions.Length);
				}
			}
			EditorGUILayout.EndHorizontal();

		}
	
		/// <summary>
		/// Draws an event response action in the inspector.
		/// </summary>
		/// <param name="action">Action.</param>
		public static void RenderAction(object target, object repsonder, EventResponse action)
		{
			if (!(target is EventResponder))
			{
				Debug.LogWarning ("Unexpected type passed to RenderAction()");
				return;
			}

			GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
			if (action == null) action = new EventResponse();
			EventResponse originalAction = new EventResponse (action);
			action.responseType = (EventResponseType) EditorGUILayout.EnumPopup( new GUIContent("Action Type", "The type of action to do when this event occurs."), action.responseType);

			// Delay
			action.delay = EditorGUILayout.FloatField( new GUIContent("Action Delay", "how long to wait before doing the action."), action.delay);
			if (action.delay < 0.0f) action.delay = 0.0f;
			else if (action.delay > 0.0f) EditorGUILayout.HelpBox("If you use many events with delay you may notice some garbage collection issues on mobile devices", MessageType.Info);

			// Game Object
			if (action.responseType == EventResponseType.ACTIVATE_GAMEOBJECT ||
			    action.responseType == EventResponseType.DEACTIVATE_GAMEOBJECT ||
			    action.responseType == EventResponseType.SEND_MESSSAGE)
			{
				action.targetGameObject = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Game Object", "The game object that will be acted on"), action.targetGameObject , typeof(GameObject), true);
			}

			// Component
			if (action.responseType == EventResponseType.ENABLE_BEHAVIOUR ||
			    action.responseType == EventResponseType.DISABLE_BEHAVIOUR)
			{
				action.targetComponent = (Component)EditorGUILayout.ObjectField(new GUIContent("Behaviour", "The behaviour will be acted on"), action.targetComponent , typeof(Component), true);
			}

			// Particle system
			if (action.responseType == EventResponseType.PLAY_PARTICLES ||
			    action.responseType == EventResponseType.PAUSE_PARTICLES)
			{
				action.targetComponent = (Component)EditorGUILayout.ObjectField(new GUIContent("Particle System", "The particle system that will be acted on"), action.targetComponent , typeof(ParticleSystem), true);
			}

			// Send message
			if (action.responseType == EventResponseType.SEND_MESSSAGE) action.message = EditorGUILayout.TextField(new GUIContent("Message", "The message to send via send message"), action.message);

			// Sprite
			if (action.responseType == EventResponseType.SWITCH_SPRITE)
			{
				action.targetComponent = (Component)EditorGUILayout.ObjectField(new GUIContent("Sprite Renderer", "SpriteRenderer to update."), action.targetComponent , typeof(SpriteRenderer), true);
				action.newSprite = (Sprite)EditorGUILayout.ObjectField(new GUIContent("New Sprite", "Sprite to switch in."), action.newSprite , typeof(Sprite), true);
			}

		
			// Load level
			if (action.responseType == EventResponseType.LOAD_SCENE) {
				action.message = EditorGUILayout.TextField(new GUIContent("Scene Name", "The name of the scene to load (make sure its added to the build settings)."), action.message);
			}

			// Effects
			if (action.responseType == EventResponseType.START_EFFECT)
			{
				action.targetComponent = (Component)EditorGUILayout.ObjectField(new GUIContent("Effect", "The effect that will be played."), action.targetComponent , typeof(FX_Base), true);
				action.targetGameObject = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Callback Object", "The game object that will be called back when the effect is finished"), action.targetGameObject , typeof(GameObject), true);
				if (action.targetComponent != null && action.targetGameObject != null)
				{
					action.message = EditorGUILayout.TextField(new GUIContent("Callback Message", "The name message to send on call back."), action.message);
					EditorGUILayout.HelpBox("Note that many effects do not support call backs.", MessageType.Info);
				}
			}

			// Animations
			if (action.responseType == EventResponseType.PLAY_ANIMATION ||
			    action.responseType == EventResponseType.STOP_ANIMATION) {
				action.targetGameObject = (GameObject) EditorGUILayout.ObjectField(new GUIContent("Animator", "GameObject holding an Animation of Animator to play or stop."), action.targetGameObject, typeof(GameObject), true);
			}

			// Animation state
			if (action.responseType == EventResponseType.PLAY_ANIMATION && action.targetGameObject != null) {
				Animator animator = action.targetGameObject.GetComponent<Animator>();
				if (animator != null)
				{
					action.message = EditorGUILayout.TextField(new GUIContent("Animation State", "Name of the Animation State to play."), action.message);
				}
			}

			// Animation state
			if (action.responseType == EventResponseType.STOP_ANIMATION) {
				Animator animator = action.targetGameObject.GetComponent<Animator>();
				if (animator != null)
				{
					EditorGUILayout.HelpBox("You cannot stop an Animator only an Animation. Instead use PLAY_ANIMATION and provide an IDLE or DEFAULT state", MessageType.Warning);
				}
			}

			if (!action.Equals(originalAction)) 
			{
				if (target is EventResponder) EditorUtility.SetDirty((EventResponder)target);
			}

			// Remove
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Remove Action"))
			{
				if (target is EventResponder) ((EventResponder)target).actions = ((EventResponder)target).actions.Where (a=>a != action).ToArray();
			}
			EditorGUILayout.EndHorizontal();
		}

		/// <summary>
		/// Get the names of all events for a given type.
		/// </summary>
		/// <returns>The event names for type.</returns>
		/// <param name="type">Type.</param>
		protected string[] GetEventNamesForType(string typeName)
		{
			System.Type type = typeof(BaseUpgrade).Assembly.GetType(this.GetType().Namespace + "." + typeName);
			if (type == null) type = typeof(BoxCollider2D).Assembly.GetType(this.GetType().Namespace + "." + typeName);
			if (type == null) return new string[0];
			System.Reflection.EventInfo[] events = type.GetEvents (System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
			if (events == null) return new string[0];
			return type.GetEvents(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Select(e=>e.Name).OrderBy(s=>s).ToArray();
		}

		/// <summary>
		/// Gets the type names of all components on a game object.
		/// </summary>
		/// <returns>The Components on game object.</returns>
		/// <param name="go">Go.</param>
		protected string[] GetComponentsOnGameObject(GameObject go)
		{
			return go.GetComponents(typeof(Component)).Select (c=>c.GetType().Name).OrderBy(s=>s).ToArray();
		}
	}

}