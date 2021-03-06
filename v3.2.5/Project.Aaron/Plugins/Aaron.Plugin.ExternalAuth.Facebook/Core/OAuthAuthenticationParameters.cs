//Contributor:  Nicholas Mayne

using System;
using System.Collections.Generic;
using Aaron.Core.Services.Authentication.External;

namespace Aaron.Plugin.ExternalAuth.Facebook.Core
{
    [Serializable]
    public class OAuthAuthenticationParameters : OpenAuthenticationParameters
    {
        private readonly string _providerSystemName;
        private IList<UserClaims> _claims;

        public OAuthAuthenticationParameters(string providerSystemName)
        {
            this._providerSystemName = providerSystemName;
        }

        public override IList<UserClaims> UserClaims
        {
            get
            {
                return _claims;
            }
        }

        public void AddClaim(UserClaims claim)
        {
            if (_claims == null)
                _claims = new List<UserClaims>();

            _claims.Add(claim);
        }

        public override string ProviderSystemName
        {
            get { return _providerSystemName; }
        }
    }
}