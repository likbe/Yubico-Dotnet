//-----------------------------------------------------------------------
// <copyright file="DefaultPrivateKeyConfigurationException.cs" company="INSIDE-IAM">
//     Copyright (c) Inside IAM. All rights reserved.
// </copyright>
// <author>Marc Morel</author>
//-----------------------------------------------------------------------
namespace InsideIAM.Yubico.Library
{
    using System;

    /// <summary>
    /// Exception indicating that the private key value in the configuration file is not set
    /// </summary>
    [Serializable]
    public class DefaultPrivateKeyConfigurationException : Exception
    {
        /// <summary>
        /// Gets the message of the current exception
        /// </summary>
        public override string Message
        {
            get
            {
                return "You forgot to change default configuration values. You must replace [INSERT_YOUR_YUBICO_PRIVATE_KEY_HERE] value with your own Yubico private key value.";
            }
        }
    }
}
