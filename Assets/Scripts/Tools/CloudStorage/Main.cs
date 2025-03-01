using System;
using System.Collections;

namespace Tools.CloudStorage
{
    public partial class CloudStorageHandler
    {
        public IEnumerator GenerateDownloadURL(string path) => _GenerateDownloadURL(path);
        
        public event Action<string> OnErrorOccured;
    }
}