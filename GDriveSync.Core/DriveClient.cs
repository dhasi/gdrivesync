using AutoMapper;
using Google.Apis.Authentication;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GDriveSync.Core
{
    public interface IDriveClient : IDisposable
    {
    }

    public class DriveClient : IDriveClient
    {
        private readonly IAuthenticationService _authService;
        private readonly ICredentialsStore _credentials;
        private IAuthenticator _authenticator;
        private DriveService _driveService;

        public DriveClient(IAuthenticationService authService, ICredentialsStore credentials)
        {
            _authService = authService;
            _credentials = credentials;
        }

        private IAuthenticator GetAuthenticator()
        {
            _authenticator = _authenticator ?? _authService.GetAuthenticator(_credentials.AuthorizationCode, _credentials.RefreshToken);
            return _authenticator;
        }

        private DriveService GetDriveService()
        {
            _driveService = _driveService ?? new DriveService(new BaseClientService.Initializer { Authenticator = GetAuthenticator() });
            return _driveService;
        }

        public List<Item> GetFolders()
        {
            return GetFolders("root");
        }

        public List<Item> GetFolders(string parentId)
        {
            var service = GetDriveService();
            var fileResource = service.Files.List();
            fileResource.Q = string.Format("'{0}' in parents and trashed = false and mimeType = '{1}'", parentId, MimeTypes.Folder);
            fileResource.MaxResults = 500;
            var files = fileResource
                .Fetch()
                .Items;

            //var childrenRequest = service.Children.List("root");
            //childrenRequest.MaxResults = 500;
            //var children = childrenRequest.Fetch();


            var folders = files.Select(x => new Item
            {
                Id = x.Id,
                Title = x.Title,
                CreatedDate = DateTime.Parse(x.CreatedDate),
                ModifiedDate = DateTime.Parse(x.ModifiedDate),
                Md5Checksum = x.Md5Checksum,
                MimeType = x.MimeType,
                Parents = x.Parents.Select(p => p.Id).ToList()
            })
            .OrderBy(x => x.Title)
            .ToList();

            return folders;
        }

        public void Dispose()
        { }
    }
}
