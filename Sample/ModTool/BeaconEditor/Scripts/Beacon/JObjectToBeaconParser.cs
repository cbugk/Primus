/*
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Bson;

namespace MapEditor.Beacon
{
    public class JObjectToBeaconParser : MonoBehaviour
    {
        private string _configFileName = "background.dat";
        Vector3 positionCache = Vector3.zero;


        internal void Export(BaseBeacon beacon)
        {
            if (!beacon)
            {
                Debug.Log("Null beacon.");
                return;
            }

            var filePath = Path.Combine(Application.persistentDataPath, _configFileName);
            using (var fs = File.Open(filePath, FileMode.Create))
            {
                using (var writer = new BsonWriter(fs))
                {
                    var serializer = new JsonSerializer();
                    var jo = new JObject();
                    jo.Add("name", beacon.name);
                    jo.Add("transform.position.x", beacon.transform.position.x);
                    jo.Add("transform.position.y", beacon.transform.position.y);
                    jo.Add("transform.position.z", beacon.transform.position.z);
                    jo.Add("BeaconType", beacon.Title.ToString());

                    serializer.Serialize(writer, jo);
                }
            }
        }
        internal void Import()
        {
            var filePath = Path.Combine(Application.persistentDataPath, _configFileName);
            using (var fs = File.OpenRead(filePath))
            {
                using (var reader = new BsonReader(fs))
                {
                    var serializer = new JsonSerializer();
                    var jo = serializer.Deserialize<JObject>(reader);
                    Parse(jo);
                }
            }
        }

        public void Parse(JObject jo)
        {
            //Debug.Log(jo.ToString());
            string beaconName = jo["name"].Value<string>();
            string beaconTypeString = jo["BeaconType"].Value<string>();
            positionCache.x = float.Parse(jo["transform.position.x"].Value<string>());
            positionCache.y = float.Parse(jo["transform.position.y"].Value<string>());
            positionCache.z = float.Parse(jo["transform.position.z"].Value<string>());

            BeaconType beaconType = (BeaconType)System.Enum.Parse(typeof(BeaconType), beaconTypeString);
            GameObject beaconInstance = MapEditorManager.Instance.Bibliotheca.CheckOut(beaconType);
            if (beaconInstance)
            {
                beaconInstance.name = beaconName;
                beaconInstance.transform.position = positionCache;
                MapEditorManager.Instance.BeaconInstances.Add(beaconInstance);
            }
        }
    }
}
*/