using System;
using System.Text;
using System.Security.Cryptography;

namespace Skyline.DataMiner.Library.Common
{
	/// <summary>
	/// Class used to Encrypt data in DataMiner.
	/// </summary>
	internal class RSA
	{
		private static RSAParameters publicKey;

		/// <summary>
		/// Get or Sets the Public Key.
		/// </summary>
		internal static RSAParameters PublicKey
		{
			get
			{
				return publicKey;
			}

			set
			{
				publicKey = value;
			}
		}

		/// <summary>
		/// Encrypt a string value using the PublicKey.
		/// </summary>
		/// <param name="plainData">The string to encrypt.</param>
		/// <returns>Encrypted string value.</returns>
		internal static string Encrypt(string plainData)
		{
			if (plainData == null)
			{
				throw new ArgumentNullException("plainData");
			}

			if (publicKey.Modulus == null)
			{
				throw new IncorrectDataException("publicKey.Modulus is null");
			}

			if (publicKey.Exponent == null)
			{
				throw new IncorrectDataException("publicKey.Exponent is null");
			}

			plainData = plainData ?? "";

			RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(); //NOTE: OAEP padding is used.
			rsa.ImportParameters(publicKey);

			//Encrypt data
			byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(plainData), true);

			return BitConverter.ToString(encryptedData).Replace("-", "");
		}
	}
}
