﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using azurecopy.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace azurecopy.Utils
{
    public static class AzureHelper
    {

        public static string AzureStorageConnectionString { get; set; }

        static string AzureDetection = "windows.net";
        static string DevAzureDetection = "127.0.0.1";
        

        // look up connection string from app.config
        public static CloudBlobClient GetCloudBlobClient()
        {
            AzureStorageConnectionString = ConfigHelper.AzureConnectionString;

            return GetCloudBlobClient(AzureStorageConnectionString);
        }

        public static CloudBlobClient GetCloudBlobClient(string azureStorageConnectionString)
        {
            CloudStorageAccount azureStorageAccount = CloudStorageAccount.Parse(azureStorageConnectionString);
            return azureStorageAccount.CreateCloudBlobClient();

        }


        // container lives in different part of url depending on dev or real.
        // if url ends in / then we assume its 
        public static string GetContainerFromUrl(string blobUrl)
        {
            var url = new Uri( blobUrl );
            string container = "";  // there may be no container.

            if (blobUrl.EndsWith("/"))
            {
                container = url.Segments[url.Segments.Length - 1];
            }
            else
            {

                // container will be second last segment of url.
                // length == 4 means BASE + ACCOUNT + CONTAINER + BLOB
                // length == 3 means BASE + CONTAINER + BLOB

                container = url.Segments[url.Segments.Length - 2];
            }

            container = container.TrimEnd('/');
            return container;
        }

        public static string GetBlobFromUrl(string blobUrl)
        {
            var url = new Uri(blobUrl);
            var blobName = "";

            blobName = url.Segments[url.Segments.Length - 1];

            return blobName;
        }

        public static bool MatchHandler(string url)
        {
            return url.Contains(AzureDetection) || url.Contains(DevAzureDetection);
        }


    }
}
