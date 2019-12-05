using UnityEngine;

namespace InterfaceScripting {

    public class ModelMover : MonoBehaviour
    {

        public Camera Camera;
        public ModelSpawner ModelSpawner;
        public ModelSelector ModelSelector;
        public InputProvider InputProvider;

        [Tooltip("While moving models with the mouse, they will be constrained to this rectangle of the screen. Measured in Viewport Coordinates (values 0-1). This Rect is unaffected by frustum of " + nameof(Camera) + ".")]
        public Rect ViewportRect;

        private void Reset() {
            ViewportRect = new Rect(Vector2.zero, Vector2.one);
        }
        private void OnValidate() {
            ViewportRect.x = Mathf.Clamp(ViewportRect.x, 0f, 1f);
            ViewportRect.y = Mathf.Clamp(ViewportRect.y, 0f, 1f);
            ViewportRect.width = Mathf.Clamp(ViewportRect.width, 0f, 1f);
            ViewportRect.height = Mathf.Clamp(ViewportRect.height, 0f, 1f);
        }
        private void Awake() {
            this.AssertAssociation(Camera, nameof(Camera));
            this.AssertAssociation(ModelSpawner, nameof(ModelSpawner));
            this.AssertAssociation(ModelSelector, nameof(ModelSelector));
            this.AssertAssociation(InputProvider, nameof(InputProvider));
        }

        public void TryMove() {
            GameObject model = ModelSelector.SelectedModel;
            if (model == null)
                return;

            Vector3 mouseViewportPos = Camera.ScreenToViewportPoint(InputProvider.MousePosition);

            mouseViewportPos.x = Mathf.Clamp(mouseViewportPos.x, ViewportRect.xMin, ViewportRect.xMax);
            mouseViewportPos.y = Mathf.Clamp(mouseViewportPos.y, ViewportRect.yMin, ViewportRect.yMax);
            mouseViewportPos.z = ModelSpawner.SpawnDistance;

            model.transform.position = Camera.ViewportToWorldPoint(mouseViewportPos);
        }
        public void EndMove() {
            GameObject model = ModelSelector.SelectedModel;
            if (model == null)
                return;

            this.LogModelMoved(model, model.transform.position);
        }

    }
}
