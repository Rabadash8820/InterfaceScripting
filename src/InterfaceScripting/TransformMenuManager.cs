﻿using UnityEngine;
using UnityEngine.UI;

namespace InterfaceScripting {

    public enum TransformState {
        Moving,
        Rotating,
        Scaling
    }

    public class TransformMenuManager : MonoBehaviour {

        private bool _awoken = false;
        private Button _currBtn;

        public InputProvider InputProvider;
        public ModelSelector ModelSelector;

        [Space]
        public TransformState StartingState = TransformState.Moving;
        [Tooltip("Whenever a transformation button is toggled on, it will change to this color.")]
        public Color ActiveButtonColor = Color.yellow;

        [Header("Buttons")]
        public Button MoveButton;
        public Button RotateButton;
        public Button ScaleButton;

        private void Awake() {
            this.AssertAssociation(InputProvider, nameof(InputProvider));
            this.AssertAssociation(ModelSelector, nameof(ModelSelector));

            this.AssertAssociation(MoveButton, nameof(MoveButton));
            this.AssertAssociation(RotateButton, nameof(RotateButton));
            this.AssertAssociation(ScaleButton, nameof(ScaleButton));

            _currBtn = MoveButton;
            ToggleState(StartingState);

            MoveButton.onClick.AddListener(() => toggle(MoveButton, TransformState.Moving));
            RotateButton.onClick.AddListener(() => toggle(RotateButton, TransformState.Rotating));
            ScaleButton.onClick.AddListener(() => toggle(ScaleButton, TransformState.Scaling));

            _awoken = true;
        }

        public TransformState TransformState { get; private set; }

        public void ToggleMoving() => toggle(MoveButton, TransformState.Moving);
        public void ToggleRotating() => toggle(RotateButton, TransformState.Rotating);
        public void ToggleScaling() => toggle(ScaleButton, TransformState.Scaling);
        public void ToggleState(TransformState newState) {
            switch (newState) {
                case TransformState.Moving: toggle(MoveButton, newState); break;
                case TransformState.Rotating: toggle(RotateButton, newState); break;
                case TransformState.Scaling: toggle(ScaleButton, newState); break;
            }
        }
        private void toggle(Button button, TransformState newState) {
            if (newState == TransformState && _awoken)
                return;

            _currBtn.image.color = Color.white;
            _currBtn = button;
            _currBtn.image.color = ActiveButtonColor;
            TransformState = newState;

            this.LogTransformMenuStateChanged(newState);
        }

    }
}
