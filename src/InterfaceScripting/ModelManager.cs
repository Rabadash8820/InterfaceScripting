using System.Collections.Generic;
using UnityEngine;

namespace InterfaceScripting {

    public class ModelManager : MonoBehaviour
    {
        private readonly IList<GameObject> _models = new List<GameObject>();
        private Vector3 _modelScreenPos;
        private float _origScaleDist = 0f;
        private float _origScale = 1f;

        public Camera Camera;

        [Header("Spawning")]
        [Tooltip("Every time a model is added, it will be spawned as a child of this Transform.")]
        public Transform SpawnRoot;
        [Tooltip("Whenever a new model is added, it will be spawned this many units in front of the " + nameof(Camera) + ".")]
        public float SpawnDistance;
        [Tooltip("Use '{0}' to represent the name of the model prefab. Use '{1}' to represent the index of the newly added model (0-based, across all model types).")]
        public string ModelNameFormatString;
        [Tooltip("While moving models with the mouse, they will be constrained to this rectangle of the screen. Measured in Viewport Coordinates (values 0-1). This Rect is unaffected by frustum of " + nameof(Camera) + ".")]
        public Rect ViewportRect;

        [Header("Transformations")]
        [Tooltip("While rotating models, the amount by which the model rotates will be this factor of the distance moved by the mouse.")]
        public float MouseRotateFactor;
        [Tooltip("While scaling models, this LineRender will be used as a visual aid for the user, connecting their cursor to the geometric center of the model.")]
        public LineRenderer ScaleLineRenderer;
        [Tooltip(nameof(ScaleLineRenderer) + "'s line will be rendered this many units in front of " + nameof(Camera) + ", so that it stays in front of the models as they expand, but is still visible in front of " + nameof(Camera) + ".")]
        public float ScaleLineDistance;

        private void Reset() {
            Camera = null;

            SpawnRoot = null;
            SpawnDistance = 10f;
            ModelNameFormatString = "{0}-{1}";
            ViewportRect = new Rect(Vector2.zero, Vector2.one);

            MouseRotateFactor = 1f;
            ScaleLineRenderer = null;
            ScaleLineDistance = 0.01f;
        }
        private void OnValidate() {
            ViewportRect.x = Mathf.Clamp(ViewportRect.x, 0f, 1f);
            ViewportRect.y = Mathf.Clamp(ViewportRect.y, 0f, 1f);
            ViewportRect.width = Mathf.Clamp(ViewportRect.width, 0f, 1f);
            ViewportRect.height = Mathf.Clamp(ViewportRect.height, 0f, 1f);
        }
        private void Awake() {
            this.AssertAssociation(SpawnRoot, nameof(SpawnRoot));
            this.AssertAssociation(Camera, nameof(Camera));

            ScaleLineRenderer.enabled = false;
        }

        public GameObject SelectedModel { get; private set; }

        public void AddModel(GameObject modelPrefab) {
            //Vector3 spawnPos = Camera.ScreenToWorldPoint(Input.mousePosition);
            GameObject model = Instantiate(modelPrefab, Vector3.zero, Quaternion.identity, SpawnRoot);
            model.name = string.Format(ModelNameFormatString, modelPrefab.name, _models.Count);
            _models.Add(model);

            this.LogModelAdded(modelPrefab.name);

            SelectModel(model, TransformState.Moving);
        }
        public void TrySelectModel(Vector3 mousePosition, TransformState transformState) {
            bool hit = Physics.Raycast(Camera.ScreenPointToRay(mousePosition), out RaycastHit hitInfo);
            if (hit) {
                GameObject model = hitInfo.collider.gameObject;
                SelectModel(model, transformState);
            }
        }
        public void SelectModel(GameObject model, TransformState transformState) {
            if (SelectedModel != null && model != SelectedModel)
                ReleaseModel();

            SelectedModel = model;
            _modelScreenPos = Camera.WorldToScreenPoint(SelectedModel.transform.position);
            _modelScreenPos.z = 0f;

            this.LogModelSelected(model, transformState);
        }
        public void MoveModel(Vector3 mousePosition) {
            if (SelectedModel == null)
                return;

            Vector3 mouseViewportPos = Camera.ScreenToViewportPoint(mousePosition);
            mouseViewportPos.x = Mathf.Clamp(mouseViewportPos.x, ViewportRect.xMin, ViewportRect.xMax);
            mouseViewportPos.y = Mathf.Clamp(mouseViewportPos.y, ViewportRect.yMin, ViewportRect.yMax);
            mouseViewportPos.z = SpawnDistance;
            SelectedModel.transform.position = Camera.ViewportToWorldPoint(mouseViewportPos);
        }
        public void RotateModel(Quaternion deltaRotation) {
            if (SelectedModel != null)
                SelectedModel.transform.localRotation *= deltaRotation;
        }
        public void ScaleModel(Vector3 mousePosition) {
            if (SelectedModel == null)
                return;

            if (_origScaleDist == 0f) {
                ScaleLineRenderer.enabled = true;
                Vector3 scaleLineStartPos = SelectedModel.transform.position;
                scaleLineStartPos.z = ScaleLineDistance - SpawnDistance;
                ScaleLineRenderer?.SetPosition(0, scaleLineStartPos);
                _origScaleDist = Vector3.Distance(_modelScreenPos, mousePosition);
                _origScale = SelectedModel.transform.localScale.x;
            }

            float currScaleDist = Vector3.Distance(_modelScreenPos, mousePosition);
            SelectedModel.transform.localScale = _origScale * currScaleDist / _origScaleDist * Vector3.one;
            Vector3 scaleLineEndPos = Camera.ScreenToWorldPoint(mousePosition);
            scaleLineEndPos.z = ScaleLineDistance - SpawnDistance;
            ScaleLineRenderer?.SetPosition(1, scaleLineEndPos);
        }
        public void ReleaseModel() {
            GameObject oldModel = SelectedModel;
            if (oldModel == null)
                return;

            SelectedModel = null;
            _origScaleDist = 0f;
            ScaleLineRenderer.enabled = false;

            this.LogModelDeselected(oldModel);
        }

    }
}
