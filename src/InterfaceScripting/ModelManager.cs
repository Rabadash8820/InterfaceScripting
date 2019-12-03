using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace InterfaceScripting {

    public class ModelManager : MonoBehaviour
    {
        private readonly IList<GameObject> _models = new List<GameObject>();

        public Logger Logger;
        public Camera Camera;
        [Tooltip("Every time a model is added, it will be spawned as a child of this Transform.")]
        public Transform SpawnRoot;
        [Tooltip("Whenever a new model is added, it will be spawned this many units in front of the " + nameof(Camera) + ".")]
        public float SpawnDistance = 10f;
        [Tooltip("Use '{0}' to represent the name of the model prefab. Use '{1}' to represent the index of the newly added model (0-based, across all model types).")]
        public string ModelNameFormatString = "{0}-{1}";
        [Tooltip("While moving models with the mouse, they will be constrained to this rectangle of the screen. Measured in Viewport Coordinates (values 0-1). This Rect is unaffected by frustum of " + nameof(Camera) + ".")]
        public Rect ViewportRect = new Rect(Vector2.zero, Vector2.one);

        private void Reset() {

        }
        private void OnValidate() {
            ViewportRect.x = Mathf.Clamp(ViewportRect.x, 0f, 1f);
            ViewportRect.y = Mathf.Clamp(ViewportRect.y, 0f, 1f);
            ViewportRect.width = Mathf.Clamp(ViewportRect.width, 0f, 1f);
            ViewportRect.height = Mathf.Clamp(ViewportRect.height, 0f, 1f);
        }
        private void Awake() {
            string typeName = GetType().Name;
            Assert.IsNotNull(Logger, $"{typeName} {name} must be associated with a {nameof(Logger)}");
            Assert.IsNotNull(SpawnRoot, $"{typeName} {name} must be associated with a {nameof(SpawnRoot)}");
            Assert.IsNotNull(Camera, $"{typeName} {name} must be associated with a {nameof(Camera)}");
        }

        public GameObject SelectedModel { get; private set; }

        public void AddModel(GameObject modelPrefab) {
            //Vector3 spawnPos = Camera.ScreenToWorldPoint(Input.mousePosition);
            GameObject model = Instantiate(modelPrefab, Vector3.zero, Quaternion.identity, SpawnRoot);
            model.name = string.Format(ModelNameFormatString, modelPrefab.name, _models.Count);
            _models.Add(model);

            Logger.LogModelAdded(modelPrefab.name);

            SelectModel(model);
        }
        public void TrySelectModel(Vector3 mousePosition) {
            bool hit = Physics.Raycast(Camera.ScreenPointToRay(mousePosition), out RaycastHit hitInfo);
            if (hit) {
                GameObject model = hitInfo.collider.gameObject;
                SelectModel(model);
            }
        }
        public void SelectModel(GameObject model) {
            if (SelectedModel != null && model != SelectedModel)
                ReleaseModel();

            SelectedModel = model;
            Logger.LogModelSelected(model);
        }
        public void MoveModel(Vector3 mousePosition) {
            if (SelectedModel != null) {
                Vector3 mouseViewportPos = Camera.ScreenToViewportPoint(mousePosition);
                mouseViewportPos.x = Mathf.Clamp(mouseViewportPos.x, ViewportRect.xMin, ViewportRect.xMax);
                mouseViewportPos.y = Mathf.Clamp(mouseViewportPos.y, ViewportRect.yMin, ViewportRect.yMax);
                mouseViewportPos.z = SpawnDistance;
                SelectedModel.transform.position = Camera.ViewportToWorldPoint(mouseViewportPos);
            }
        }
        public void RotateModel(Quaternion deltaRotation) {
            if (SelectedModel != null)
                SelectedModel.transform.localRotation *= deltaRotation;
        }
        public void ScaleModel(float scaleFactor) {
            if (SelectedModel != null)
                SelectedModel.transform.localScale *= scaleFactor;
        }
        public void ReleaseModel() {
            GameObject oldModel = SelectedModel;
            if (oldModel != null) {
                SelectedModel = null;
                Logger.LogModelDeselected(oldModel);
            }
        }

    }
}
