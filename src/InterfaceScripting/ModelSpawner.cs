using System.Collections.Generic;
using UnityEngine;

namespace InterfaceScripting {

    public class ModelSpawner : MonoBehaviour
    {
        private readonly IList<GameObject> _models = new List<GameObject>();

        public Camera Camera;
        public ModelSelector ModelSelector;
        public InputProvider InputProvider;

        [Tooltip("Every time a model is added, it will be spawned as a child of this Transform.")]
        public Transform SpawnRoot;
        [Tooltip("Whenever a new model is added, it will be spawned this many units in front of the " + nameof(Camera) + ".")]
        public float SpawnDistance;
        [Tooltip("Use '{0}' to represent the name of the model prefab. Use '{1}' to represent the index of the newly added model (0-based, across all model types).")]
        public string ModelNameFormatString;

        private void Reset() {
            SpawnDistance = 10f;
            ModelNameFormatString = "{0}-{1}";
        }
        private void Awake() {
            this.AssertAssociation(Camera, nameof(Camera));
            this.AssertAssociation(ModelSelector, nameof(ModelSelector));
            this.AssertAssociation(InputProvider, nameof(InputProvider));
            this.AssertAssociation(SpawnRoot, nameof(SpawnRoot));
        }

        public void Spawn(GameObject modelPrefab) {
            Vector3 spawnViewportPos = Camera.ScreenToViewportPoint(InputProvider.MousePosition);
            spawnViewportPos.z = SpawnDistance;
            Vector3 spawnWorldPos = Camera.ViewportToWorldPoint(spawnViewportPos);

            GameObject model = Instantiate(modelPrefab, spawnWorldPos, Quaternion.identity, SpawnRoot);
            model.name = string.Format(ModelNameFormatString, modelPrefab.name, _models.Count);
            _models.Add(model);

            this.LogModelAdded(modelPrefab.name);

            ModelSelector.SelectModel(model);
        }

    }
}
