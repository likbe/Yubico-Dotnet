//-----------------------------------------------------------------------
// <copyright file="SignatureManager.cs" company="INSIDE-IAM">
//     Copyright (c) Inside IAM. All rights reserved.
// </copyright>
// <author>Marc Morel</author>
//-----------------------------------------------------------------------
namespace InsideIAM.Yubico.Library
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Cryptography helper to sign and verify signatures
    /// </summary>
    public sealed class SignatureManager : IDisposable
    {
        /// <summary>
        /// Create a new mutex 
        /// </summary>
        private static Mutex mutex = new Mutex(true, "Yubico.Library.SignatureManager.Instance");

        /// <summary>
        /// Dictionary of signature manager instances
        /// </summary>
        private static Dictionary<string, SignatureManager> instances;

        /// <summary>
        /// Algorithm to use to sign and verify signatures
        /// </summary>
        private HMAC algorithm;

        /// <summary>
        /// Initializes a new instance of the SignatureManager class
        /// </summary>
        /// <param name="privateKey">Yubico's private key to use to sign and verify signatures</param>
        private SignatureManager(string privateKey)
        {
            byte[] key = Convert.FromBase64String(privateKey);
            this.algorithm = new HMACSHA1();
            this.algorithm.Key = key;
        }

        /// <summary>
        /// Retrieve a SignatureManager instance from a given key
        /// </summary>
        /// <param name="key">Key of the instance to create or return</param>
        /// <returns>The instance that corresponds to a given key or creates a new one if necessary</returns>
        public static SignatureManager GetInstance(string key)
        {
            if (instances == null) 
            {
                instances = new Dictionary<string, SignatureManager>(); 
            }
            
            if (!instances.ContainsKey(key))
            {
                mutex.WaitOne();
                if (!instances.ContainsKey(key)) 
                { 
                    instances.Add(key, new SignatureManager(key)); 
                }

                mutex.ReleaseMutex();
            }

            return instances[key];
        }

        /// <summary>
        /// Check a given message's signature
        /// </summary>
        /// <param name="message">Message to verify</param>
        /// <param name="givenHash">Given signature/hash</param>
        /// <returns>True if the signature is valid, false otherwise</returns>
        public bool CheckSignature(string message, string givenHash)
        {
            bool result = false;
            byte[] resultHash = this.algorithm.ComputeHash(ASCIIEncoding.UTF8.GetBytes(message));
            if (String.Compare(Convert.ToBase64String(resultHash), givenHash, StringComparison.InvariantCulture) == 0)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Sign a given message
        /// </summary>
        /// <param name="message">Message to sign</param>
        /// <returns>The signature of the given message</returns>
        public string Sign(string message)
        {
            string result = String.Empty;
            byte[] hash = this.algorithm.ComputeHash(ASCIIEncoding.UTF8.GetBytes(message));
            result = System.Convert.ToBase64String(hash);
            return result;
        }

        /// <summary>
        /// Dispose the cryptographic algorithm instance
        /// </summary>
        public void Dispose()
        {
            this.algorithm.Dispose();
        }
    }
}