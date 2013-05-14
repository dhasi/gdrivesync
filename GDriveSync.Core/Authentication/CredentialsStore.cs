
namespace GDriveSync.Core
{
    public interface ICredentialsStore
    {
        string AuthorizationCode { get; set; }
        string RefreshToken { get; set; }
    }

    public class CredentialsStore : ICredentialsStore
    {
        public string AuthorizationCode { get; set; }

        public string RefreshToken { get; set; }

        public CredentialsStore()
        {
            AuthorizationCode = "4/qLzKlXJ9DcNnxt-uAtgmwWWQvAvR.8nDxmwY9kFkUOl05ti8ZT3ZvHr80fQI";
            RefreshToken = "1/djuwRyw7gf15eZiem5_T5yNQG1YSHf4gPm_cEGoGmkA";
        }
    }
}
