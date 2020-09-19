using System.Collections;
using System.Collections.Generic;
using thelab.core;
using UnityEngine;
using UnityEngine.Networking;
using UnityScript.Steps;

namespace Hsinpa.AR
{
    public class ARDataSync
    {
        private string GoogleSheetURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQAyCfqm9J5kMSTL8wYon8LWLx2kO6YeyEpTogq2GF6YH6-MrgNDK-BmECPQ5UIeaNYXQgpf5I5ZITJ/pub?gid=0&single=true&output=csv";
        private int _version = 0;
        private Dictionary<string, ARData> _arDataList = new Dictionary<string, ARData>();

        public IEnumerator WebSyncARData(System.Action callback)
        {
            UnityWebRequest.ClearCookieCache();

            using (UnityWebRequest webRequest = UnityWebRequest.Get(GoogleSheetURL))
            {
                //8 Seconds timeout
                webRequest.timeout = 6;
                webRequest.useHttpContinue = false;

                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError)
                {
                    Debug.Log("WebUpdateSheet Error: " + webRequest.error);
                }
                else
                {
                    CSVFile csvFile = new CSVFile(webRequest.downloadHandler.text);

                    int version = csvFile.Get<int>(0, GeneralFlag.GoogleDocVar.Version);
                    Debug.Log("Version " + version);

                    if (version > _version) {
                        _version = version;
                        _arDataList = ParseCSV(csvFile);
                        callback();
                    }
                }
            }
        }

        public ARData FindArData(string name) {
            if (_arDataList.TryGetValue(name, out ARData aRData))
                return aRData;

            return default(ARData);
        }

        private Dictionary<string, ARData> ParseCSV(CSVFile csvFile) {
            Dictionary<string, ARData> arList = new Dictionary<string, ARData>();
            int csvCount = csvFile.length;

            for (int i = 0; i < csvCount; i++) {
                ARData aRData = new ARData();

                string name = csvFile.Get<string>(i, "Name", "");
                string position_str = csvFile.Get<string>(i, "Position", "");
                string rotation_str = csvFile.Get<string>(i, "Rotation", "");
                float scale = float.Parse(csvFile.Get<string>(i, "Scale", "0.1"));

                aRData.name = name;
                aRData.position = StrToVector(position_str);
                aRData.rotation = Quaternion.Euler(StrToVector(rotation_str));
                aRData.scale = new Vector3(scale, scale, scale);

                Debug.Log("name " + name);
                Debug.Log("position_str " + position_str);
                Debug.Log("rotation_str " + rotation_str);
                Debug.Log("scale " + scale);


                if (arList.ContainsKey(name))
                    arList[name] = aRData;
                else
                    arList.Add(name, aRData);
            }

            return arList;
        }

        private Vector3 StrToVector(string vector_str) {
            Vector3 vector = Vector3.one;
            try {
                string[] strPair = vector_str.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);

                if (strPair.Length == 3)
                {
                    return new Vector3(float.Parse(strPair[0]), float.Parse(strPair[1]), float.Parse(strPair[2]));
                }
            }
            catch {
                Debug.LogError("StrToVector Error " + vector_str);
            }

            return vector;
        }


        public struct ARData {
            public string name;
            public Vector3 position;
            public Quaternion rotation;
            public Vector3 scale;

            public bool isValid => !string.IsNullOrEmpty(name);
        }

    }
}