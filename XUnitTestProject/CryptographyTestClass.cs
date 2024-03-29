﻿using System.Text;
using System;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Crypto;
using Microsoft.Scripting.Utils;
using Xunit;

namespace XUnitTestProject;

/// <summary>
/// https://chat.openai.com/chat
/// https://github.com/keygen-sh/example-csharp-cryptographic-verification/blob/master/Program.cs
/// </summary>
public class CryptographyTestClass
{
    public const string KEYGEN_LICENSE_KEY = "key/eyJhY2NvdW50Ijp7ImlkIjoiMWZkZGNlYzgtOGRkMy00ZDhkLTliMTYtMjE1Y2FjMGY5YjUyIn0sInByb2R1Y3QiOnsiaWQiOiI2YjViYjVhMy0wZTMyLTQyMjYtODhmZC04NmE5N2VjZDA2NzgifSwicG9saWN5Ijp7ImlkIjoiY2IyN2E1ZTQtYjAxYS00OTFkLTg5MzctNWY2MmJjOGIzMmVkIiwiZHVyYXRpb24iOm51bGx9LCJ1c2VyIjpudWxsLCJsaWNlbnNlIjp7ImlkIjoiYWRhNmUwYTgtZDIyOS00MjY4LWE3OGMtMWFkZGJiMDg2MzIwIiwiY3JlYXRlZCI6IjIwMjEtMDMtMjJUMTM6MzA6MTcuODkyWiIsImV4cGlyeSI6bnVsbH19.PtRHZAiiF3lInyMjyudoHfCRmMYnfMk82xG-_6v68LIx7EyjqW1gDeVlKOMXHb6HnV8HJJiV5yoW9Vc5UKa_l3Z_rS3qQdTloKE5hNW7mzuYkV2ReZ_6Q8QGItMZbpZkGr-GHHsVJrkQoFolFM0c9Nlz9eoxibtIK8VOsnxbOkfoZSH3uTpDqGpKBaa0jNvivCtL0211PKsXtGgCz32qnNvEGSHRs4XEZ1ZLEJ5cyyeBdSqwkkTl0nUepMd9nCSYT3b0RhpIIgv9ybJPPrwTa4fNt7MbL8aDizfrDewvNdrEN8n3vEXdg_vZ0uKZxamoLa02fdo5rTA654Q-9gEd-g==";
    public const string KEYGEN_PUBLIC_KEY = "LS0tLS1CRUdJTiBQVUJMSUMgS0VZLS0tLS0KTUlJQklqQU5CZ2txaGtpRzl3MEJBUUVGQUFPQ0FROEFNSUlCQ2dLQ0FRRUF6UEFzZURZdXBLNzhaVWFTYkd3NwpZeVVDQ2VLby8xWHFUQUNPY21UVEhIR2dlSGFjTEsyajlVcmJUbGhXNWg4VnlvMGlVRUhyWTFLZ2Y0d3dpR2dGCmgwWWMrb0RXRGhxMWJJZXJ0STAzQUU0MjBMYnBVZjZPVGlvWCtuWTBFSW54WEYzSjdhQWR4L1IvbllnUkpyTFoKOUFUV2FRVlNnZjN2dHhDdEN3VWVLeEtaSTQxR0EvOUtIVGNDbWQzQnJ5QVExcGlZUHIrcXJFR2YyTkRKZ3IzVwp2VnJNdG5qZW9vcmRBYUNUeVlLdGZtNTZXR1hlWHI0M2RmZGVqQnVJa0k1a3FTendWeW94aG5qRS9SajZ4a3M4CmZmSCtka0FQTndtMElweFhKZXJ5YmptUFd5djdpeVhFVU44Q0tHKzY0MzBEN05vWUhwL2M5OTFaSFFCVXM1OWcKdndJREFRQUIKLS0tLS1FTkQgUFVCTElDIEtFWS0tLS0tCg==";
    
    public void Vertification()
    {
        //PEM stands for Privacy Enhanced Mail. It is a file format used to store cryptographic keys, such as public keys. PEM files typically contain text-based data that is encoded using Base64, and they often have a .pem file extension.
        string pemPublicKey = Encoding.UTF8.GetString(Convert.FromBase64String(KEYGEN_PUBLIC_KEY));

        // Parse and convert the base64 PEM public key to ASN1 format
        string encodedAns1PublicKey = pemPublicKey
        .Replace("-----BEGIN PUBLIC KEY-----", string.Empty)
        .Replace("-----END PUBLIC KEY-----", string.Empty)
        .Trim();

        //ASN.1 (Abstract Syntax Notation One) is a standardized, abstract syntax for describing data structures that are used in telecommunications and computer networking.
        //It is often used in conjunction with other protocols, such as X.509, to encode and transmit information.
        byte[] asn1PublicKey = Convert.FromBase64String(encodedAns1PublicKey);

        // Import the public key
        var publicKey = PublicKeyFactory.CreateKey(asn1PublicKey);

        // Calculate the "max" salt length
        var keyLength = (int)Math.Ceiling((2048 - 1) / 8.0);
        IDigest digest = new Sha256Digest();
        var saltLength = keyLength - digest.GetDigestSize() - 2;

        // Initialize RSA
        IAsymmetricBlockCipher engine = new RsaEngine();
        //PSS (Probabilistic Signature Scheme) is a digital signature scheme that is based on the RSA cryptosystem.
        //It is considered to be more secure than other signature schemes, such as the older RSA-based signature scheme known as PKCS#1 v1.5.
        var pss = new PssSigner(engine, digest, saltLength);

        ICipherParameters cipher = publicKey;
        pss.Init(false, cipher);

        string licenseKey = KEYGEN_LICENSE_KEY;
        string[] keyParts = licenseKey.Split('.');
        string signingData = keyParts[0];
        string[] dataParts = signingData.Split('/');
        string signingPrefix = dataParts[0];
        
        Xunit.Assert.True(signingPrefix == "key");
        
        string encodedData = ConvertBase64UrlString(dataParts[1]);
        byte[] signingDataBytes = Encoding.UTF8.GetBytes($"key/{encodedData}");
        string encodedSignature = ConvertBase64UrlString(keyParts[1]);
        byte[] signatureBytes = Convert.FromBase64String(encodedSignature);

        // Verify the license key signature
        //A block update algorithm is a type of cryptographic algorithm that is used to update the internal state of a cryptographic hash function or a message authentication code (MAC) in a block-wise manner.
        //In other words, the algorithm processes the input data in fixed-sized blocks, rather than processing the entire input at once. This allows the algorithm to operate on large amounts of data more efficiently,
        //without requiring a large amount of memory to store the entire input.
        pss.BlockUpdate(signingDataBytes, 0, signingDataBytes.Length);

        bool vertified = pss.VerifySignature(signatureBytes);
        Xunit.Assert.True(vertified);

        byte[] decodedDataBytes = Convert.FromBase64String(encodedData);
        string decodedData = Encoding.UTF8.GetString(decodedDataBytes);
        Console.WriteLine("[INFO] License key is cryptographically valid: key={0} dataset={1}", licenseKey, decodedData);
    }

    // Cryptographic keys use base64url encoding: https://keygen.sh/docs/api/#license-signatures
    private static string ConvertBase64UrlString(string s)
    {
        return s.Replace("-", "+").Replace("_", "/");
    }
}
