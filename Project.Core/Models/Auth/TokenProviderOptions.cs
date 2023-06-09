﻿using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.Core.Models.Auth
{
    public class TokenProviderOptions
    {
        /// <summary>
        /// The relative request path to listen on.
        /// </summary>
        /// <remarks>The default path is <c>/token</c>.</remarks>
        public string Path { get; set; } = "/token";

        /// <summary>
        ///  The Issuer (iss) claim for generated tokens.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// The Audience (aud) claim for the generated tokens.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// The expiration time for the generated tokens.
        /// </summary>
        /// <remarks>The default is five minutes (300 seconds).</remarks>
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(2);

        /// <summary>
        /// The signing key to use when generating tokens.
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

        /// <summary>
        /// Resolves a user identity given a apiKey.
        /// </summary>
        public Func<HttpContext, string, Task<ClaimsIdentity>> ApiKeyIdentityResolver { get; set; }

        /// <summary>
        /// Resolves a user identity given a username and password.
        /// </summary>
        public Func<HttpContext, string, string, Task<ClaimsIdentity>> IdentityResolver { get; set; }

        public string RefreshTokenSigningKey { get; set; }

    }
}
