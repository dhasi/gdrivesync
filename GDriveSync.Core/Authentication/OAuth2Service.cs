using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Drive.v2;
using Google.Apis.Util;
using System;

namespace GDriveSync.Core
{
    public class OAuth2Service : IAuthenticationService
    {
        private const string CLIENT_ID = "1061429631265.apps.googleusercontent.com";
        private const string CLIENT_SECRET = "Z7-FRZtdLsPJXaQORtZvTsVi";

        private readonly NativeApplicationClient _oAuthProvider;

        public OAuth2Service()
        {
            _oAuthProvider = new NativeApplicationClient(GoogleAuthenticationServer.Description, CLIENT_ID, CLIENT_SECRET);
        }

        public IAuthenticator GetAuthenticator(string authorizationCode, string refreshToken)
        {
            var auth = new OAuth2Authenticator<NativeApplicationClient>(_oAuthProvider, x => GetAuthorization(authorizationCode, refreshToken));
            auth.LoadAccessToken();
            return auth;
        }

        private IAuthorizationState GetAuthorization(string authorizationCode, string refreshToken)
        {
            var state = new AuthorizationState(new[] { DriveService.Scopes.Drive.GetStringValue() });
            if (!string.IsNullOrEmpty(refreshToken))
            {
                state.RefreshToken = refreshToken;
                if (_oAuthProvider.RefreshToken(state))
                    return state;
            }
            state.Callback = new Uri(NativeApplicationClient.OutOfBandCallbackUrl);
            return _oAuthProvider.ProcessUserAuthorization(authorizationCode, state);
        }


        public Uri GetAuthorizationUri()
        {
            var state = new AuthorizationState(new[] { DriveService.Scopes.Drive.GetStringValue() });
            state.Callback = new Uri(NativeApplicationClient.OutOfBandCallbackUrl);
            var authUri = _oAuthProvider.RequestUserAuthorization(state);
            return authUri;
        }
    }
}
