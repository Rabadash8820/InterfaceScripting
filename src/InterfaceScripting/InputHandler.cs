using UnityEngine;
using UnityEngine.Assertions;

namespace InterfaceScripting {
    public class InputHandler : MonoBehaviour {

        public ModelManager ModelManager;
        public TransformMenuManager TransformMenuManager;

        private void Awake() {
            string typeName = GetType().Name;
            Assert.IsNotNull(ModelManager, $"{typeName} {name} must be associated with a {nameof(ModelManager)}");
            Assert.IsNotNull(TransformMenuManager, $"{typeName} {name} must be associated with a {nameof(TransformMenuManager)}");
        }
        private void Update() {
            bool primary = Input.GetMouseButtonDown(0);
            Vector3 mousePos = Input.mousePosition;

            if (ModelManager.SelectedModel == null) {
                if (primary)
                    ModelManager.TrySelectModel(mousePos);
            }
            else {
                if (primary)
                    ModelManager.ReleaseModel();
                else
                    ModelManager.MoveModel(mousePos);
            }

            bool secondary = Input.GetMouseButtonDown(1);
            bool middle = Input.GetMouseButtonDown(2);

        }
    }
}
