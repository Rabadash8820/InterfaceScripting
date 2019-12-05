using UnityEngine;

namespace InterfaceScripting {

    public class ModelTransformer : MonoBehaviour {

        public InputProvider InputProvider;
        public ModelSelector ModelSelector;
        public TransformMenuManager TransformMenuManager;

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

            if (ModelSelector.SelectedModel == null && primaryDown) {
                ModelSelector.TrySelectModel(InputProvider.MousePosition);
                return;
            }

            if (ModelSelector.SelectedModel != null && primaryDown) {
                switch (TransformMenuManager.TransformState) {
                    case TransformState.Moving: ModelMover.EndMove(); break;
                    case TransformState.Rotating: ModelRotater.EndRotate(); break;
                    case TransformState.Scaling: ModelScaler.EndScale(); break;
                }
                ModelSelector.DeselectModel();
                return;
            }

            switch (TransformMenuManager.TransformState) {
                case TransformState.Moving: ModelMover.TryMove(); break;
                case TransformState.Rotating: ModelRotater.TryRotate(); break;
                case TransformState.Scaling: ModelScaler.TryScale(); break;
            }
        }

    }
}
