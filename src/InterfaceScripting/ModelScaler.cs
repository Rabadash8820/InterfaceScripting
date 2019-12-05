using UnityEngine;

namespace InterfaceScripting {

    public class ModelScaler : MonoBehaviour
    {
        private bool _scaling = false;
        private Vector3 _modelScreenPos;
        private float _origScaleDist;
        private float _origScale;

        public Camera Camera;
        public ModelSpawner ModelSpawner;
        public ModelSelector ModelSelector;
        public InputProvider InputProvider;

        [Tooltip("While scaling models, this LineRender will be used as a visual aid for the user, connecting their cursor to the geometric center of the model.")]
        public LineRenderer ScaleLineRenderer;
        [Tooltip(nameof(ScaleLineRenderer) + "'s line will be rendered this many units in front of " + nameof(Camera) + ", so that it stays in front of the models as they expand, but is still visible in front of " + nameof(Camera) + ".")]
        public float ScaleLineDistance;

        private void Reset() {
            ScaleLineDistance = 1f;
        }
        private void Awake() {
            this.AssertAssociation(Camera, nameof(Camera));
            this.AssertAssociation(ModelSpawner, nameof(ModelSpawner));
            this.AssertAssociation(ModelSelector, nameof(ModelSelector));
            this.AssertAssociation(InputProvider, nameof(InputProvider));

            if (ScaleLineRenderer != null)
                ScaleLineRenderer.enabled = false;
        }

        public void TryScale() {
            GameObject model = ModelSelector.SelectedModel;
            if (ModelSelector.SelectedModel == null)
                return;

            Vector3 mousePos = InputProvider.MousePosition;
            if (!_scaling) {
                beginScale(model, mousePos);
                _scaling = true;
            }

            // Scale the model
            float currScaleDist = Vector3.Distance(_modelScreenPos, mousePos);
            model.transform.localScale = _origScale * currScaleDist / _origScaleDist * Vector3.one;

            // Adjust the "scale line" visual aid
            Vector3 scaleLineEndPos = Camera.ScreenToWorldPoint(mousePos);
            scaleLineEndPos.z = ScaleLineDistance - ModelSpawner.SpawnDistance;
            ScaleLineRenderer?.SetPosition(1, scaleLineEndPos);
        }
        public void EndScale() {
            GameObject model = ModelSelector.SelectedModel;
            if (model == null)
                return;

            if (ScaleLineRenderer != null)
                ScaleLineRenderer.enabled = false;
            _scaling = false;

            this.LogModelScaled(model, model.transform.localScale.x);
        }

        private void beginScale(GameObject model, Vector3 mousePosition)
        {
            // Save some initial state about this scaling operation
            _modelScreenPos = Camera.WorldToScreenPoint(model.transform.position);
            _modelScreenPos.z = 0f;
            _origScaleDist = Vector3.Distance(_modelScreenPos, mousePosition);
            _origScale = model.transform.localScale.x;

            // Turn back on the "scale line" visual aid
            if (ScaleLineRenderer != null) {
                Vector3 scaleLineStartPos = model.transform.position;
                scaleLineStartPos.z = ScaleLineDistance - ModelSpawner.SpawnDistance;
                ScaleLineRenderer?.SetPosition(0, scaleLineStartPos);
                ScaleLineRenderer.enabled = true;
            }
        }

    }
}
