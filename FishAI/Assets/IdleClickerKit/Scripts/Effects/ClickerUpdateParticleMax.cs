using UnityEngine;
using System.Collections;

namespace IdleClickerKit
{
	
	public class ClickerUpdateParticleMax : MonoBehaviour {

		[Tooltip ("Name of the click type to use")]
		[SerializeField]
		protected string clickName;

		[Tooltip ("Max allowed particles.")]
		[SerializeField]
		protected int max;

		ParticleSystem particles;

		void Start() {
			particles = GetComponent<ParticleSystem> ();
			if (particles == null) {
				Debug.LogError ("ClickerUpdateParticleMax couldn't find a particle system.");
				Destroy (this);
			}
		}

		void Update() {
			int total = ClickManager.GetInstance (clickName).ClickIncrement;
			if (total > max) total = max;
            ParticleSystem.MainModule tmp = particles.main;
            tmp.maxParticles = total;
		}

	}
}