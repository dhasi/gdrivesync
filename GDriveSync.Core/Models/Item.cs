using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GDriveSync.Core
{
    [DebuggerDisplay("Folder: {Title} ({Id})")]
    public class Item
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Md5Checksum { get; set; }

        public string MimeType { get; set; }

        public bool IsFolder { get { return string.Compare(MimeType, MimeTypes.Folder, StringComparison.InvariantCultureIgnoreCase) == 0; } }

        public bool IsFile { get { return string.Compare(MimeType, MimeTypes.File, StringComparison.InvariantCultureIgnoreCase) == 0; } }

        public List<string> Parents { get; set; }

        public bool IsRoot { get { return Parents.Count == 0; } }
    }
}
