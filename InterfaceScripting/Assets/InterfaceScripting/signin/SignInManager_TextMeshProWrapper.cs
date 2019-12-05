using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace InterfaceScripting
{
    public class SignInManager_TextMeshProWrapper : MonoBehaviour
    {
        public SignInManager SignInManager;
        public TMP_InputField EmailInput;
        public TMP_InputField PasswordInput;

        private void Awake()
        {
            Assert.IsNotNull(SignInManager, $"{GetType().Name} {name} must be associated with a {nameof(SignInManager)}");
        }

        public void TrySignIn()
        {
            SignInManager.TrySignIn(EmailInput.text, PasswordInput.text);
        }

    }
}
