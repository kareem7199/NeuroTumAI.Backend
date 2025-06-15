using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NeuroTumAI.Service.Providers.Identity
{
	public class CustomEmailTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser> where TUser : class
	{
		private const string TokenProviderName = "EmailConfirmation";

		public async Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
		{
			// Use a cryptographically secure random number generator
			using (var rng = RandomNumberGenerator.Create())
			{
				var tokenBytes = new byte[4]; // 4 bytes = 32 bits
				rng.GetBytes(tokenBytes);
				var token = BitConverter.ToUInt32(tokenBytes, 0) % 1000000; // 6 digits

				// Store the token for later validation
				// The correct parameter order is: user, loginProvider, name, value
				await manager.SetAuthenticationTokenAsync(user, TokenProviderName, purpose, token.ToString("D6"));

				return token.ToString("D6"); // Pad with leading zeros
			}
		}

		public async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
		{
			// Correct parameter order: user, loginProvider, name
			var storedToken = await manager.GetAuthenticationTokenAsync(user, TokenProviderName, purpose);

			if (storedToken == null)
				return false;

			// Compare the provided token with the stored one
			var result = token == storedToken;

			// If validation succeeded, remove the token to prevent reuse
			if (result)
			{
				await manager.RemoveAuthenticationTokenAsync(user, TokenProviderName, purpose);
			}

			return result;
		}

		public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
		{
			return Task.FromResult(true);
		}
	}
}
