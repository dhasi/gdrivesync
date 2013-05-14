using Google.Apis.Authentication;
using System;

namespace GDriveSync.Core
{
    public interface IAuthenticationService
    {
        IAuthenticator GetAuthenticator(string authorizationCode, string refreshToken);
        
        Uri GetAuthorizationUri();
    }
}
