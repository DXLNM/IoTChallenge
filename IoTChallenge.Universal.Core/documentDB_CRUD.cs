using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using documentDB;
using Newtonsoft.Json;
using IoTChallenge.Universal.Core.Classes;
namespace IoTChallenge.Universal.Core
{
    public static partial class documentDBUtilities
    {
        public static async Task<CameraSensorDocument> getCameraDocument(string documentID)
        {
            string jsonResult = await CRUD.getDocument(accountID, databaseID, collectionID, documentID);
            return JsonConvert.DeserializeObject<CameraSensorDocument>(jsonResult);
        }

        public static async Task<MotionSensorDocument> getMotionDocument(string documentID)
        {
            string jsonResult = await CRUD.getDocument(accountID, databaseID, collectionID, documentID);
            return JsonConvert.DeserializeObject<MotionSensorDocument>(jsonResult);
        }


        public static async Task<string> updateCameraDocument(string documentID, CameraSensorDocument document)
        {
            string jsonFile = JsonConvert.SerializeObject(document);
            return await CRUD.updateDocument(accountID, databaseID, collectionID, documentID, jsonFile);
        }

        public static async Task<string> updateMotionDocument(string documentID, MotionSensorDocument document)
        {
            string jsonFile = JsonConvert.SerializeObject(document);
            return await CRUD.updateDocument(accountID, databaseID, collectionID, documentID, jsonFile);
        }

        public static async Task<string> createCameraDocument(CameraSensorDocument document)
        {
            string jsonFile = JsonConvert.SerializeObject(document);
            return await CRUD.createDocument(accountID, databaseID, collectionID, jsonFile);
        }

        public static async Task<string> createMotionDocument(MotionSensorDocument document)
        {
            string jsonFile = JsonConvert.SerializeObject(document);
            return await CRUD.createDocument(accountID, databaseID, collectionID, jsonFile);
        }

        public static async Task<string> deleteDocument(string documentID)
        {
            return await CRUD.deleteDocument(accountID, databaseID, collectionID, documentID);
        }
    }
}
