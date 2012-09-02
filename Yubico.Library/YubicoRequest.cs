//-----------------------------------------------------------------------
// <copyright file="YubicoRequest.cs" company="INSIDE-IAM">
//     Copyright (c) Inside IAM. All rights reserved.
// </copyright>
// <author>Marc Morel</author>
//-----------------------------------------------------------------------
namespace InsideIAM.Yubico.Library
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using InsideIAM.Yubico.Library.Configurations;

    /// <summary>
    /// Request to yubico service
    /// </summary>
    public class YubicoRequest
    {
        /// <summary>
        /// Default configuration value of Yubico ID
        /// </summary>
        private const string YubicoDefaultConfigurationIdValue = "[INSERT_YOUR_YUBICO_ID_HERE]";

        /// <summary>
        /// Default configuration value of Yubico Private Key 
        /// </summary>
        private const string YubicoDefaultConfigurationPrivateKeyValue = "[INSERT_YOUR_YUBICO_PRIVATE_KEY_HERE]";

        /// <summary>
        /// Instance of the credentials configuration reader
        /// </summary>
        private YubicoCredentialsSection credentialsConfiguration = (YubicoCredentialsSection)ConfigurationManager.GetSection("YubicoCredentials");

        /// <summary>
        /// Instance of the default values of Yubico's API configuration reader
        /// </summary>
        private YubicoApiValuesSection apiValuesConfiguration = (YubicoApiValuesSection)ConfigurationManager.GetSection("YubicoApiValues");

        /// <summary>
        /// Instance of the name parameters of Yubico's requests configuration reader
        /// </summary>
        private YubicoApiRequestSection parametersConfiguration = (YubicoApiRequestSection)ConfigurationManager.GetSection("YubicoApiRequest");

        /// <summary>
        /// Gets the parameters of the current request
        /// </summary>
        public string Parameters { get; private set; }

        /// <summary>
        /// Gets the signature of the parameters of the current request
        /// </summary>
        public string ParametersSignature { get; private set; }

        /// <summary>
        /// Validate a given OTP
        /// </summary>
        /// <param name="otp">OTP from pressure on the YubiKey button</param>
        /// <returns>The validation result of the given OTP value</returns>
        public YubicoAnswer Validate(string otp)
        {
            YubicoAnswer result = new YubicoAnswer();
            this.InitializeParameters(otp);
            this.SignParameters();

            StreamReader reader = new StreamReader(this.GetAnswer());
            string currentLine = String.Empty;
            while (!reader.EndOfStream)
            {
                currentLine = reader.ReadLine();
                if (!String.IsNullOrEmpty(currentLine))
                {
                    int indexOfEqual = currentLine.IndexOf('=');
                    result.Parameters.Add(currentLine.Substring(0, indexOfEqual), currentLine.Substring(indexOfEqual + 1, currentLine.Length - (indexOfEqual + 1)));
                }
            }

            result.Validate();
            return result;
        }

        /// <summary>
        /// Initialize request parameters from the given OTP and configuration files
        /// </summary>
        /// <param name="otp">OTP from pressure on the YubiKey button</param>
        private void InitializeParameters(string otp)
        {
            try
            {
                string id = this.credentialsConfiguration.Id;
                if (String.Compare(this.credentialsConfiguration.Id, YubicoDefaultConfigurationIdValue, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    throw new DefaultIdConfigurationException();
                }

                string privateKey = this.credentialsConfiguration.PrivateKey;
                if (String.Compare(this.credentialsConfiguration.PrivateKey, YubicoDefaultConfigurationPrivateKeyValue, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    throw new DefaultPrivateKeyConfigurationException();
                }

                int timestamp = this.apiValuesConfiguration.TimeStamp;

                this.Parameters = String.Format(
                    "{0}={1}&{2}={3}&{4}={5}",
                    this.parametersConfiguration.Id,
                    id,
                    this.parametersConfiguration.OTP,
                    otp,
                    this.parametersConfiguration.TimeStamp,
                    timestamp);
            }
            catch (Exception exception)
            {
                Trace.WriteLine(String.Format(CultureInfo.InvariantCulture, "The following exception occured : {0}", exception.Message));
                throw;
            }
        }

        /// <summary>
        /// Create a signature for parameters
        /// </summary>
        private void SignParameters()
        {
            if (this.apiValuesConfiguration.CheckSignature)
            {
                this.ParametersSignature = SignatureManager.GetInstance(this.credentialsConfiguration.PrivateKey).Sign(this.Parameters);
            }
        }

        /// <summary>
        /// Retrieve the answer of the current request
        /// </summary>
        /// <returns>A stream containing the answer of the current request</returns>
        private Stream GetAnswer()
        {
            string url = String.Format("{0}{1}", this.apiValuesConfiguration.VerifyUrl, this.Parameters);
            if (this.apiValuesConfiguration.CheckSignature)
            {
                url += String.Format("&{0}={1}", this.parametersConfiguration.Signature, this.ParametersSignature);
            }

            Uri uri = new Uri(url);
            WebRequest request = HttpWebRequest.Create(uri);
            WebResponse response = request.GetResponse();
            return response.GetResponseStream();
        }
    }
}
