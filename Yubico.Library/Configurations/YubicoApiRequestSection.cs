//-----------------------------------------------------------------------
// <copyright file="YubicoApiRequestSection.cs" company="INSIDE-IAM">
//     Copyright (c) Inside IAM. All rights reserved.
// </copyright>
// <author>Marc Morel</author>
//-----------------------------------------------------------------------
namespace InsideIAM.Yubico.Library.Configurations
{
    using System.Configuration;

    /// <summary>
    /// Yubico APIs parameters names configuration section reader
    /// </summary>
    public class YubicoApiRequestSection : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the name of the id parameter
        /// </summary>
        [ConfigurationProperty("Id", IsRequired = true)]
        public string Id
        {
            get { return (string)this["Id"]; }
            set { this["Id"] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the OTP parameter
        /// </summary>
        [ConfigurationProperty("OTP", IsRequired = true)]
        public string OTP
        {
            get { return (string)this["OTP"]; }
            set { this["OTP"] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the timestamp parameter
        /// </summary>
        [ConfigurationProperty("TimeStamp", IsRequired = true)]
        public string TimeStamp
        {
            get { return (string)this["TimeStamp"]; }
            set { this["TimeStamp"] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the signature parameter
        /// </summary>
        [ConfigurationProperty("Signature", IsRequired = true)]
        public string Signature
        {
            get { return (string)this["Signature"]; }
            set { this["Signature"] = value; }
        }
    }
}
