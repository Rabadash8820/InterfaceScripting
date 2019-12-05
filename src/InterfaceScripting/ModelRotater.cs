using UnityEngine;

namespace InterfaceScripting {

    public class ModelRotater : MonoBehaviour
    {
        private bool _rotating = false;
        private Vector3 _prevMousePos;

        public Camera Camera;
        public ModelSelector ModelSelector;
        public InputProvider InputProvider;

        [Tooltip("Models will be rotated this many degrees for every pixel that the cursor moves in the x- or y-direction.")]
        public float RotateAnglesPerPixel;

        private void Reset() {
            RotateAnglesPerPixel = 1f;
        }
        private void Awake() {
            this.AssertAssociation(Camera, nameof(Camera));
            this.AssertAssociation(ModelSelector, nameof(ModelSelector));
            this.AssertAssociation(InputProvider, nameof(InputProvider));
        }

        public void TryRotate() {
            GameObject model = ModelSelector.SelectedModel;
            if (model == null)
                return;

            Vector3 mousePos = InputProvider.MousePosition;
            if (!_rotating) {
                beginRotate(model, mousePos);
                _rotating = true;
            }

            float xDegrees = RotateAnglesPerPixel * (mousePos.x - _prevMousePos.x);
            float yDegrees = RotateAnglesPerPixel * (mousePos.y - _prevMousePos.y);
            model.transform.localRotation *=
                Quaternion.AngleAxis(-xDegrees, Camera.transform.up) *
                Quaternion.AngleAxis(yDegrees, Camera.transform.right);

            _prevMousePos = mousePos;
        }
        public void EndRotate() {
            GameObject model = ModelSelector.SelectedModel;
            if (model == null)
                return;

            _rotating = false;

            this.LogModelRotated(model, model.transform.localRotation);
        }

        private void beginRotate(GameObject model, Vector3 mousePosition) {
            _prevMousePos = mousePosition;
        }

    }
}
