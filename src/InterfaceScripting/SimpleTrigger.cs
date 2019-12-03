using UnityEngine;
using UnityEngine.Events;

namespace InterfaceScripting
{
    public class SimpleTrigger : MonoBehaviour
    {
        public UnityEvent Triggered = new UnityEvent();

        public void Trigger() => Triggered.Invoke();
    }
}
