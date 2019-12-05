using UnityEngine;
using UnityEngine.Events;

namespace InterfaceScripting {

    public class ModelTransformer : MonoBehaviour {

        private bool _hovering = false;

        public InputProvider InputProvider;
        public ModelSelector ModelSelector;
        public TransformMenuManager TransformMenuManager;

        [Header("Hover events")]
        public UnityEvent ModelHoverEnter = new UnityEvent();
        public UnityEvent ModelHoverExit = new UnityEvent();

        [Header("Transform Managers")]
        public ModelMover ModelMover;
        public ModelRotater ModelRotater;
        public ModelScaler ModelScaler;

        private void Awake() {
            this.AssertAssociation(InputProvider, nameof(InputProvider));
            this.AssertAssociation(ModelSelector, nameof(ModelSelector));
            this.AssertAssociation(TransformMenuManager, nameof(TransformMenuManager));

            this.AssertAssociation(ModelMover, nameof(ModelMover));
            this.AssertAssociation(ModelRotater, nameof(ModelRotater));
            this.AssertAssociation(ModelScaler, nameof(ModelScaler));
        }
        private void Update() {
            bool primaryDown = InputProvider.MousePrimaryButtonDown;
            Vector3 mousePos = InputProvider.MousePosition;

            GameObject modelUnderMouse = null;
            if (ModelSelector.SelectedModel == null) {
                // Raise events for when we enter/exit the state of hovering over a model
                modelUnderMouse = ModelSelector.GetModelUnderMouse(mousePos);
                if (modelUnderMouse != null && !_hovering) {
                    _hovering = true;
                    ModelHoverEnter.Invoke();
                }
                else if (modelUnderMouse == null && _hovering) {
                    _hovering = false;
                    ModelHoverExit.Invoke();
                }

                // If the hovered-over model is clicked, then select it
                if (_hovering && primaryDown) {
                    ModelSelector.SelectModel(modelUnderMouse);
                    return;
                }
            }

            // If user clicks while a model is selected, then end any transformations and deselect it
            if (ModelSelector.SelectedModel != null && primaryDown) {
                switch (TransformMenuManager.TransformState) {
                    case TransformState.Moving: ModelMover.EndMove(); break;
                    case TransformState.Rotating: ModelRotater.EndRotate(); break;
                    case TransformState.Scaling: ModelScaler.EndScale(); break;
                }
                ModelSelector.DeselectModel();
                return;
            }

            // Otherwise, continue updating the transformation
            switch (TransformMenuManager.TransformState) {
                case TransformState.Moving: ModelMover.TryMove(); break;
                case TransformState.Rotating: ModelRotater.TryRotate(); break;
                case TransformState.Scaling: ModelScaler.TryScale(); break;
            }
        }

    }
}
