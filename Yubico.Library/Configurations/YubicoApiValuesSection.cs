//-----------------------------------------------------------------------
// <copyright file="YubicoApiValuesSection.cs" company="INSIDE-IAM">
//     Copyright (c) Inside IAM. All rights reserved.
// </copyright>
// <author>Marc Morel</author>
//-----------------------------------------------------------------------
namespace InsideIAM.Yubico.Library.Configurations
{
    using System.Configuration;

    /// <summary>
    /// Yubico APIs default values configuration section reader
    /// </summary>
    public class YubicoApiValuesSection : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets if a timestamp should be returned from Yubico's OTP server
        /// </summary>
        [ConfigurationProperty("TimeStamp", IsRequired = false)]
        public int TimeStamp
        {
            get { return (int)this["TimeStamp"]; }
            set { this["TimeStamp"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the parameters of the queries should be signed
        /// </summary>
        [ConfigurationProperty("CheckSignature", IsRequired = false)]
        public bool CheckSignature
        {
            get { return (bool)this["CheckSignature"]; }
            set { this["CheckSignature"] = value; }
        }

        /// <summary>
        /// Gets or sets Yubico's API URL to verify the validity of OTPs
        /// </summary>
        [ConfigurationProperty("VerifyUrl", IsRequired = true)]
        public string VerifyUrl
        {
            get { return (string)this["VerifyUrl"]; }
            set { this["VerifyUrl"] = value; }
        }
    }
}
