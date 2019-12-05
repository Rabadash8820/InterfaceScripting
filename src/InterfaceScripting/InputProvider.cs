using UnityEngine;

namespace InterfaceScripting {
    public class InputProvider : MonoBehaviour {

        private void Update() {
            MousePrimaryButtonDown = Input.GetMouseButtonDown(0);
            MousePosition = Input.mousePosition;
        }

        public bool MousePrimaryButtonDown { get; private set; }
        public Vector3 MousePosition { get; private set; }

    }
}
