//-----------------------------------------------------------------------
// <copyright file="DefaultIdConfigurationException.cs" company="INSIDE-IAM">
//     Copyright (c) Inside IAM. All rights reserved.
// </copyright>
// <author>Marc Morel</author>
//-----------------------------------------------------------------------
namespace InsideIAM.Yubico.Library
{
    using System;

    /// <summary>
    /// Exception indicating that the id value in the configuration file is not set
    /// </summary>
    [Serializable]
    public class DefaultIdConfigurationException : Exception
    {
        /// <summary>
        /// Gets the message of the current exception
        /// </summary>
        public override string Message
        {
            get
            {
                return "You forgot to change default configuration values. You must replace [INSERT_YOUR_YUBICO_ID_HERE] value with your own Yubico ID value.";
            }
        }
    }
}
