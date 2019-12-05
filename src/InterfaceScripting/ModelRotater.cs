using UnityEngine;

namespace InterfaceScripting {

    public class ModelRotater : MonoBehaviour
    {
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
            //if (model != null)
            //    model.transform.localRotation *= deltaRotation;
        }
        public void EndRotate() {
            GameObject model = ModelSelector.SelectedModel;
            if (model == null)
                return;

            //this.LogModelRotated(model, );   // TODO
        }

    }
}
