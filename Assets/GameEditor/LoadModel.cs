using System;
using System.IO;
using Model;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace GameEditor.Editor
{
    public class LoadModel : UnityEditor.Editor
    {
        private const string SHEET_NAME = "1lMDrAV8RbK0EaMmLNgDRTB4vt__yPoYxj6K6fLZJ-Ig";
        
        [MenuItem("Editor/UpdateModel", false, 1)]
        private static void SaveModelIntoResources()
        {
            foreach (var staticName in (ModelName[]) Enum.GetValues(typeof(ModelName)))
            {
                var nextName = staticName.ToString();
                SaveModel(nextName, SHEET_NAME);
            }
        }

        private static void SaveModel(string name, string sheetName)
        {
            SendRequest($"https://opensheet.elk.sh/{sheetName}/{name}", text => SaveJsonAsset(name, text));
        }

        private static void SendRequest(string url, Action<string> callback)
        {
            var request = new UnityWebRequest
            {
                url = url,
                method = UnityWebRequest.kHttpVerbPOST,
                downloadHandler = new DownloadHandlerBuffer(),
            };

            request.SendWebRequest().completed += operation =>
            {
                callback?.Invoke(request.downloadHandler.text);
            };
        }

        private static void SaveJsonAsset(string name, string text)
        {
            File.WriteAllText($"{Application.dataPath}/Resources/Model/{name}.json", text);
            Debug.Log($"{name}.json saved!");
        }
    }
}