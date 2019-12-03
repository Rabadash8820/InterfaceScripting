using System;
using UnityEngine;

namespace InterfaceScripting {

    [CreateAssetMenu(menuName = nameof(InterfaceScripting) + "/" + nameof(CredentialCollection), fileName = "credentials")]
    public class CredentialCollection : ScriptableObject {
        public Credential[] Credentials;
    }

    [Serializable]
    public class Credential {
        public string Email;
        public string PasswordSha512Base64;
    }

}
