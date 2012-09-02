//-----------------------------------------------------------------------
// <copyright file="YubicoApiResponseSection.cs" company="INSIDE-IAM">
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
    public class YubicoApiResponseSection : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the name of the signature parameter
        /// </summary>
        [ConfigurationProperty("Signature", IsRequired = true)]
        public string Signature
        {
            get { return (string)this["Signature"]; }
            set { this["Signature"] = value; }
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
        /// Gets or sets the name of the status parameter
        /// </summary>
        [ConfigurationProperty("Status", IsRequired = true)]
        public string Status
        {
            get { return (string)this["Status"]; }
            set { this["Status"] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the session counter parameter
        /// </summary>
        [ConfigurationProperty("SessionCounter", IsRequired = true)]
        public string SessionCounter
        {
            get { return (string)this["SessionCounter"]; }
            set { this["SessionCounter"] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the session use parameter
        /// </summary>
        [ConfigurationProperty("SessionUse", IsRequired = true)]
        public string SessionUse
        {
            get { return (string)this["SessionUse"]; }
            set { this["SessionUse"] = value; }
        }
    }
}
