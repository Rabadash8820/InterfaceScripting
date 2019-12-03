using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace InterfaceScripting {

    public enum TransformMenuState {
        Moving,
        Rotating,
        Scaling
    }

    public class TransformMenuManager : MonoBehaviour {

        private Button _currBtn;

        public Button MoveButton;
        public Button RotateButton;
        public Button ScaleButton;
        public TransformMenuState StartingState = TransformMenuState.Moving;
        [Tooltip("Whenever a transformation button is toggled on, it will change to this color.")]
        public Color ActiveButtonColor = Color.yellow;

        private void Awake() {
            string typeName = GetType().Name;
            Assert.IsNotNull(MoveButton, $"{typeName} {name} must be associated with a {nameof(MoveButton)}");
            Assert.IsNotNull(RotateButton, $"{typeName} {name} must be associated with a {nameof(RotateButton)}");
            Assert.IsNotNull(ScaleButton, $"{typeName} {name} must be associated with a {nameof(ScaleButton)}");

            _currBtn = MoveButton;
            ToggleState(StartingState);

            MoveButton.onClick.AddListener(() => toggle(MoveButton, TransformMenuState.Moving));
            RotateButton.onClick.AddListener(() => toggle(RotateButton, TransformMenuState.Rotating));
            ScaleButton.onClick.AddListener(() => toggle(ScaleButton, TransformMenuState.Scaling));
        }

        public void ToggleMoving() => toggle(MoveButton, TransformMenuState.Moving);
        public void ToggleRotating() => toggle(RotateButton, TransformMenuState.Rotating);
        public void ToggleScaling() => toggle(ScaleButton, TransformMenuState.Scaling);
        public void ToggleState(TransformMenuState newState) {
            switch (newState) {
                case TransformMenuState.Moving: toggle(MoveButton, newState); break;
                case TransformMenuState.Rotating: toggle(RotateButton, newState); break;
                case TransformMenuState.Scaling: toggle(ScaleButton, newState); break;
            }
        }
        private void toggle(Button button, TransformMenuState newState) {
            _currBtn.image.color = Color.white;
            _currBtn = button;
            _currBtn.image.color = ActiveButtonColor;
            TransformMenuState = newState;
        }

        public TransformMenuState TransformMenuState { get; private set; } = TransformMenuState.Moving;

    }
}
