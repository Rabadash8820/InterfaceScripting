using UnityEngine;
using UnityEngine.Assertions;

namespace InterfaceScripting {
    public class InputHandler : MonoBehaviour {

        public ModelManager ModelManager;
        public TransformMenuManager TransformMenuManager;

        private void Awake() {
            this.AssertAssociation(ModelManager, nameof(ModelManager));
            this.AssertAssociation(TransformMenuManager, nameof(TransformMenuManager));
        }
        private void Update() {
            bool primaryDown = Input.GetMouseButtonDown(0);
            bool primaryUp = Input.GetMouseButtonUp(0);
            Vector3 mousePos = Input.mousePosition;
            GameObject model = ModelManager.SelectedModel;

            if (model == null) {
                if (primaryDown)
                    ModelManager.TrySelectModel(mousePos, TransformMenuManager.TransformState);
            }

            else if (TransformMenuManager.TransformState == TransformState.Moving) {
                if (primaryDown) {
                    this.LogModelMoved(model, model.transform.position);
                    ModelManager.ReleaseModel();
                }
                else
                    ModelManager.MoveModel(mousePos);
            }

            else if (TransformMenuManager.TransformState == TransformState.Rotating) {

            }

            else if (TransformMenuManager.TransformState == TransformState.Scaling) {
                if (primaryDown) {
                    this.LogModelScaled(model, model.transform.localScale.x);
                    ModelManager.ReleaseModel();
                }
                else
                    ModelManager.ScaleModel(mousePos);
            }
        }
    }
}
