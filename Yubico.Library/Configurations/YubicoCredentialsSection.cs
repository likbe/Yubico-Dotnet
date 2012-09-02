//-----------------------------------------------------------------------
// <copyright file="YubicoCredentialsSection.cs" company="INSIDE-IAM">
//     Copyright (c) Inside IAM. All rights reserved.
// </copyright>
// <author>Marc Morel</author>
//-----------------------------------------------------------------------
namespace InsideIAM.Yubico.Library.Configurations
{
    using System.Configuration;

    /// <summary>
    /// Yubico credentials configuration section reader
    /// </summary>
    public class YubicoCredentialsSection : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the Yubico client ID
        /// </summary>
        [ConfigurationProperty("Id", IsRequired = true)]
        public string Id
        {
            get { return (string)this["Id"]; }
            set { this["Id"] = value; }
        }

        /// <summary>
        /// Gets or sets the Yubico private key to sign and verify query parameters
        /// </summary>
        [ConfigurationProperty("PrivateKey", IsRequired = true)]
        public string PrivateKey
        {
            get { return (string)this["PrivateKey"]; }
            set { this["PrivateKey"] = value; }
        }
    }
}
