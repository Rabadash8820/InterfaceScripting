using UnityEngine;

namespace InterfaceScripting {
    public class Logger : MonoBehaviour {
        public void LogModelAdded(string prefabName) => Debug.Log($"Added model using prefab '{prefabName}'");
        public void LogModelSelected(GameObject model) => Debug.Log($"Selected model '{model.name}'");
        public void LogModelDeselected(GameObject model) => Debug.Log($"Deselected model '{model.name}'");
    }
}
