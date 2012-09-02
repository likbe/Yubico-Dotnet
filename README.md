Yubico-Dotnet
=============

Require a Yubikey from Yubico (http://www.yubico.com)

Simple .NET/C# console application that validate a Yubikey's OTP (Yubico)

Usage : 
-------
Edit app.config to set your Yubico Client ID and Yubico Secret Key
Start a windows console 
Launch bin/Debug/Yubico.Console.exe YOUR_YUBIKEY_OTP (YOUR_YUBIKEY_OTP must be obtained by pressing on your Yubikey)

Result :
--------
Return 'Validated' if your OTP match Yubikey's server code and 'Rejected' otherwise.
