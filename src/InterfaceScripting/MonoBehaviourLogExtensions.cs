using UnityEngine;
using UnityEngine.Assertions;

namespace InterfaceScripting {
    public static class MonoBehaviourLogExtensions {
        public static void AssertAssociation<T>(this MonoBehaviour component, T property, string propertyName) where T : class =>
            Assert.IsNotNull(property, $"{component.GetType().Name} '{component.name}' must be associated with a {nameof(propertyName)}");

        public static void LogSignedIn(this MonoBehaviour component, string email) =>
            log(component, $"User with email '{email}' signed in");
        public static void LogSignedOut(this MonoBehaviour component, string email) =>
            log(component, $"User with email '{email}' signed out");
        public static void LogTransformMenuStateChanged(this MonoBehaviour component, TransformState oldState, TransformState newState) =>
            log(component, $"Transform menu state changed from {oldState} to {newState}");
        public static void LogModelAdded(this MonoBehaviour component, string prefabName) =>
            log(component, $"Added model using prefab '{prefabName}'");
        public static void LogModelSelected(this MonoBehaviour component, GameObject model, TransformState transformState) =>
            log(component, $"Selected model '{model.name}' for {transformState}");
        public static void LogModelDeselected(this MonoBehaviour component, GameObject model) =>
            log(component, $"Deselected model '{model.name}'");
        public static void LogModelMoved(this MonoBehaviour component, GameObject model, Vector3 newPosition) =>
            log(component, $"Moved model '{model.name}' to {newPosition}");
        public static void LogModelScaled(this MonoBehaviour component, GameObject model, float newScale) =>
            log(component, $"Scaled model '{model.name}' to {newScale}");


        private static void log(MonoBehaviour component, string message) => Debug.Log($"Frame {Time.frameCount} | {component.GetType().Name} '{component.name}' | {message}");
    }
}
