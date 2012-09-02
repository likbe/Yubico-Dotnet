//-----------------------------------------------------------------------
// <copyright file="YubicoAnswer.cs" company="INSIDE-IAM">
//     Copyright (c) Inside IAM. All rights reserved.
// </copyright>
// <author>Marc Morel</author>
//-----------------------------------------------------------------------
namespace InsideIAM.Yubico.Library
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using InsideIAM.Yubico.Library.Configurations;

    /// <summary>
    /// Yubico's OTP server answer
    /// </summary>
    public class YubicoAnswer
    {
        /// <summary>
        /// Instance of Yubico's answer configuration reader
        /// </summary>
        private YubicoApiResponseSection responseConfiguration = (YubicoApiResponseSection)ConfigurationManager.GetSection("YubicoApiResponse");

        /// <summary>
        /// Instance of the credentials configuration reader
        /// </summary>
        private YubicoCredentialsSection credentialsConfiguration = (YubicoCredentialsSection)ConfigurationManager.GetSection("YubicoCredentials");

        /// <summary>
        /// Initializes a new instance of the YubicoAnswer class
        /// </summary>
        public YubicoAnswer()
        {
            this.Status = AnswerStatus.Unknown;
            this.Parameters = new SortedDictionary<string, string>();
        }

        /// <summary>
        /// Answer status
        /// </summary>
        public enum AnswerStatus
        {
            /// <summary>
            /// No answer was received 
            /// </summary>
            Unknown,

            /// <summary>
            /// OTP is valid
            /// </summary>
            Validated,

            /// <summary>
            /// OTP was rejected by the Yubico's OTP server
            /// </summary>
            Rejected,

            /// <summary>
            /// The signature of your parameters does not match. Check the value of the PrivateKey entry in YubicoCredentials configuration section into your configuration file.
            /// </summary>
            InvalidSignature,

            /// <summary>
            /// The OTP receipt by Yubico's server was already used by a previous authentication
            /// </summary>
            Replayed,

            /// <summary>
            /// Yubico's server was not able to find your ID. Check the value of the ID entry in YubicoCredentials configuration section into your configuration file.
            /// </summary>
            NoSuchClient,

            /// <summary>
            /// One or several required parameters were missing
            /// </summary>
            MissingParameter,

            /// <summary>
            /// Yubico's OTP server sent an internal technical error
            /// </summary>
            TechnicalIssue
        }

        /// <summary>
        /// Gets or sets the status of the current answer
        /// </summary>
        public AnswerStatus Status { get; set; }

        /// <summary>
        /// Gets a value indicating whether the response was validated by Yubico's OTP server and the signature was successfully verified
        /// </summary>
        public bool IsValid
        {
            get { return this.Status == AnswerStatus.Validated && this.IsSignatureValid; }
        }

        /// <summary>
        /// Gets or sets the parameters of the current answer
        /// </summary>
        public SortedDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the signature was validated or not
        /// </summary>
        public bool IsSignatureValid { get; set; }

        /// <summary>
        /// Validate the current answer : control the signature if present and parse the Yubico code answer
        /// </summary>
        public void Validate()
        {
            if (this.Parameters.ContainsKey(this.responseConfiguration.Signature))
            {
                string concat = String.Empty;
                foreach (string key in this.Parameters.Keys)
                {
                    if (key != this.responseConfiguration.Signature) 
                    { 
                        concat += String.Format("{0}={1}&", key, this.Parameters[key]); 
                    }
                }

                this.IsSignatureValid = SignatureManager.GetInstance(this.credentialsConfiguration.PrivateKey).CheckSignature(
                    concat.Substring(0, concat.Length - 1), 
                    this.Parameters[this.responseConfiguration.Signature]);
            }

            if (this.Parameters.ContainsKey(this.responseConfiguration.Status))
            {
                this.Status = this.ParseStatus(this.Parameters[this.responseConfiguration.Status]);
            }
        }

        /// <summary>
        /// Parse the status of the return code of the current answer
        /// </summary>
        /// <param name="status">Status to parse</param>
        /// <returns>AnswerStatus value that correspond to the given Yubico's status</returns>
        private YubicoAnswer.AnswerStatus ParseStatus(string status)
        {
            YubicoAnswer.AnswerStatus result = YubicoAnswer.AnswerStatus.Unknown;
            if (status.StartsWith("OK")) 
            { 
                result = YubicoAnswer.AnswerStatus.Validated; 
            }
            else if (status.StartsWith("BAD_OTP")) 
            { 
                result = YubicoAnswer.AnswerStatus.Rejected;
            }
            else if (status.StartsWith("REPLAYED_OTP")) 
            { 
                result = YubicoAnswer.AnswerStatus.Replayed; 
            }
            else if (status.StartsWith("BAD_SIGNATURE"))
            { 
                result = YubicoAnswer.AnswerStatus.InvalidSignature; 
            }
            else if (status.StartsWith("MISSING_PARAMETER")) 
            { 
                result = YubicoAnswer.AnswerStatus.MissingParameter; 
            }
            else if (status.StartsWith("NO_SUCH_CLIENT")) 
            { 
                result = YubicoAnswer.AnswerStatus.NoSuchClient;
            }
            else if (status.StartsWith("BACKEND_ERROR")) 
            { 
                result = YubicoAnswer.AnswerStatus.TechnicalIssue;
            }

            return result;
        }
    }
}
