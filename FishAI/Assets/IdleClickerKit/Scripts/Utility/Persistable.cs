using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;


namespace IdleClickerKit
{
	/// <summary>
	/// Base class for things that can be saved.
	/// </summary>
	public abstract class Persistable : MonoBehaviour {

		/// <summary>
		/// Gets the unique save key.
		/// </summary>
		abstract public string UniqueSaveKey {
			get;
		}

		/// <summary>
		/// Gets the save data.
		/// </summary>
		/// <value>The save data.</value>
		abstract public object SaveData {
			get; set;
		}

		/// <summary>
		/// Get the type of object this Persistable saves.
		/// </summary>
		abstract public System.Type SavedObjectType {
			get;
		}

		/// <summary>
		/// Things to do after load.
		/// </summary>
		virtual public void PostLoadAction() {
		}

		/// <summary>
		/// Things to do after reset.
		/// </summary>
		virtual public void PostResetAction() {
		}

		/// <summary>
		/// Save the given persistable.
		/// </summary>
		/// <param name="p">Persistable to save.</param>
		public static void Save(Persistable p)
		{
			object saveData = p.SaveData;
			// We assume all saveData is annotated with [Serializable] we could add a condition for ISeriazable too.
			if (saveData.GetType() == p.SavedObjectType && p.SavedObjectType.IsSerializable )
			{
				using(StringWriter writer = new StringWriter())
				{
					XmlSerializer serializer = new XmlSerializer(p.SavedObjectType);
					serializer.Serialize(writer, saveData);
					PlayerPrefs.SetString(p.UniqueSaveKey, writer.ToString());
				}
			}
			else
			{
				Debug.LogError("Save data for " + p + " is not serializable or of the wrong type.");
			}
		}

		/// <summary>
		/// Load the given persistable.
		/// </summary>
		/// <param name="p">Persistable to laod.</param>
		public static void Load(Persistable p)
		{
			string data = PlayerPrefs.GetString(p.UniqueSaveKey, "");
			if (data.Length > 0)
			{
				using (StringReader reader = new StringReader(data)){
					XmlSerializer serializer = new XmlSerializer(p.SavedObjectType);
					object savedObject = serializer.Deserialize(reader);
					p.SaveData = savedObject;
					p.PostLoadAction();
				}
			}
			else
			{
				p.PostResetAction();
			}
		}

		/// <summary>
		/// Resets the given persistable.
		/// </summary>
		/// <param name="p">Persistable to reset.</param>
		public static void Reset(Persistable p)
		{
			PlayerPrefs.DeleteKey (p.UniqueSaveKey);
			p.PostResetAction();
		}

	}
}
