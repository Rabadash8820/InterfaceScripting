using System.Collections.Generic;
using UnityEngine;

namespace InterfaceScripting {

    public class ModelSelector : MonoBehaviour
    {
        private Color? _origColor;
        private MeshRenderer _renderer;

        public Camera Camera;
        [Tooltip("When selected, models will change to this color, then back when deselected.")]
        public Color SelectedColor = Color.white;

        private void Awake() {
            this.AssertAssociation(Camera, nameof(Camera));
        }

        public GameObject SelectedModel { get; private set; }

        public void TrySelectModel(Vector3 mousePosition)
        {
            bool hit = Physics.Raycast(Camera.ScreenPointToRay(mousePosition), out RaycastHit hitInfo);
            if (hit) {
                GameObject model = hitInfo.collider.gameObject;
                SelectModel(model);
            }
        }
        public void SelectModel(GameObject model)
        {
            if (SelectedModel != null && model != SelectedModel)
                DeselectModel();

            SelectedModel = model;

            _renderer = model.GetComponentInChildren<MeshRenderer>();
            _origColor = _renderer?.material?.color;
            if (_renderer != null && _renderer.material != null)
                _renderer.material.color = SelectedColor;

            this.LogModelSelected(model);
        }
        public void DeselectModel() {
            GameObject oldModel = SelectedModel;
            if (oldModel == null)
                return;

            if (_renderer != null && _origColor.HasValue)
                _renderer.material.color = _origColor.Value;

            SelectedModel = null;

            this.LogModelDeselected(oldModel);
        }

    }
}
