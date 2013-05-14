
namespace GDriveSync.Core
{
    public interface IDriveClientFactory
    {
        IDriveClient Create();
    }

    public class DriveClientFactory : IDriveClientFactory
    {
        public IDriveClient Create()
        {
            return new DriveClient(
                new OAuth2Service(),
                new CredentialsStore());
        }
    }
}
