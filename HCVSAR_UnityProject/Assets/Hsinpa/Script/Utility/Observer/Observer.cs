using UnityEngine;
using System.Collections;

namespace ObserverPattern {
	public class Observer : MonoBehaviour {

		public virtual void OnNotify(string p_event, params object[] p_objects) {
			
		}
	}
}