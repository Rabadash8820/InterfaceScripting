using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace InterfaceScripting {
    public class SignInManager : MonoBehaviour {

        private string _currEmail;
        private readonly IDictionary<string, byte[]> _pwdHashes = new Dictionary<string, byte[]>();

        public Logger Logger;
        public CredentialCollection ValidCredentials;
        public UnityEvent SignedIn = new UnityEvent();
        public UnityEvent NotSignedIn = new UnityEvent();

        private void Awake() {
            string typeName = GetType().Name;
            Assert.IsNotNull(Logger, $"{typeName} {name} must be associated with a {nameof(Logger)}");
            Assert.IsNotNull(ValidCredentials, $"{typeName} {name} must be associated with a {nameof(ValidCredentials)}");

            for (int c = 0; c < ValidCredentials.Credentials.Length; ++c) {
                Credential cred = ValidCredentials.Credentials[c];
                byte[] pwdHash = Convert.FromBase64String(cred.PasswordSha512Base64);
                _pwdHashes.Add(cred.Email, pwdHash);
            }
        }

        public void TrySignIn(string email, string password) {
            bool authenticated = false;

            // If this email isn't registered, or the password hash doesn't match the registered one, then we're not authenticated
            if (_pwdHashes.ContainsKey(email)) {
                using var hashAlgorithm = SHA512.Create();
                byte[] hash = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
                var match = _pwdHashes.SingleOrDefault(x => x.Key == email && x.Value.SequenceEqual(hash));
                if (!match.Equals(default(KeyValuePair<string, byte[]>)))
                    authenticated = true;
            }

            if (authenticated) {
                _currEmail = email;
                Logger.LogSignedIn(_currEmail);
            }

            (authenticated ? SignedIn : NotSignedIn).Invoke();
        }
        public void SignOut() {
            if (_currEmail == null)
                return;

            Logger.LogSignedOut(_currEmail);

            _currEmail = null;
        }

    }
}
