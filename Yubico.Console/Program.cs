//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="INSIDE-IAM">
//     Copyright (c) Inside IAM. All rights reserved.
// </copyright>
// <author>Marc Morel</author>
//-----------------------------------------------------------------------
namespace InsideIAM.Yubico.Console
{
    using System;
    using System.Globalization;
    using InsideIAM.Yubico.Library;

    /// <summary>
    /// Console Yubico Client
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main of the current console program
        /// </summary>
        /// <param name="args">Arguments of the current execution</param>
        /// <returns>A negative value if an error occured, 0 otherwise</returns>
        public static int Main(string[] args)
        {
            try
            {
                int result = 0;
                if (args == null || args.Length == 0)
                {
                    Console.WriteLine("Usage : Yubico.Console 'OTP'. You must press on your YubiKey to get the 'OTP' value. Do NOT type the quotes ''.");
                    result = -1;
                }
                else
                {
                    YubicoRequest request = new YubicoRequest();
                    string otp = args[0];
                    YubicoAnswer answer = request.Validate(otp);
                    Console.WriteLine(String.Format(CultureInfo.InvariantCulture, "Validation status is : {0}", answer.Status.ToString()));
                }

                return result;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return -2;
            }
        }
    }
}
