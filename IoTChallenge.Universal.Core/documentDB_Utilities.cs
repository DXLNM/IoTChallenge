using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using documentDB;
namespace IoTChallenge.Universal.Core
{
    public static partial class documentDBUtilities
    {
        internal static string accountID = "dxddb"; // Account ID host: dxddb.documents.azure.com:443/
        internal static string databaseID = "bBNBAA=="; //DatabaseName = iotchallenge
        internal static string collectionID = "bBNBAPFVBgA="; //CollectionName = kiosk
    }
}
