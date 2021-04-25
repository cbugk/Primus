using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Primus.Sample.ModTool.BeaconEditor.IO
{
    public static class StateIO
    {
        private static string directoryPath;
        private static string filePath;

        public static void Write()
        {
            ExportManager.GenerateExport();
            var mapEditorState = ExportManager.BeaconEditorState;

            FileStream fileStream = null;
            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                if (mapEditorState.Name == null || mapEditorState.Name == "")
                {
                    mapEditorState.Name = "UnnamedState";
                }

                directoryPath = @Application.dataPath + "/StateSaves/" + mapEditorState.Name;
                System.IO.Directory.CreateDirectory(directoryPath);

                int saveCount = Directory.GetFiles(directoryPath, "*.bin", SearchOption.TopDirectoryOnly).Length;
                filePath = directoryPath + "/Save" + (saveCount + 1).ToString("D2") + ".bin";

                fileStream = File.Create(filePath);
                binaryFormatter.Serialize(fileStream, mapEditorState);
                Debug.Log("Written to: " + directoryPath);
            }
            catch (System.Exception e)
            {
                if (e != null) { Debug.LogError(e); }
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }

        public static void Read(string fileName)
        {
            FileStream fileStream = null;
            try
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                directoryPath = @Application.dataPath + "/StateSaves/UnnamedState/";
                filePath = directoryPath + fileName;

                fileStream = File.Open(filePath, FileMode.Open);
                ImportManager.Load((BeaconEditorState)binaryFormatter.Deserialize(fileStream));
                Debug.Log("Loaded from: " + directoryPath);
            }
            catch (System.Exception e) { if (e != null) { Debug.LogError(e); } }
            finally
            {
                if (fileStream != null) { fileStream.Close(); }
            }
        }
    }
}