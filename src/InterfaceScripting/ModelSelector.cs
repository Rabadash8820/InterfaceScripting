using System.Collections.Generic;
using UnityEngine;

namespace InterfaceScripting {

    public class ModelSelector : MonoBehaviour
    {
        private readonly IList<GameObject> _models = new List<GameObject>();

        public Camera Camera;

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
                ReleaseModel();

            SelectedModel = model;

            this.LogModelSelected(model);
        }
        public void ReleaseModel() {
            GameObject oldModel = SelectedModel;
            if (oldModel == null)
                return;

            SelectedModel = null;

            this.LogModelDeselected(oldModel);
        }

    }
}
