using UnityEngine;
using System.IO;

public static class NocabDiskUtility {
    /**
     * A static class for reading and writing strings to the disk
     */


    private static string cleanFileName(string relativeDirectory) {
        // The output of this function is the true directory location on the file system
        // Do a little cleaning and add in the Unity special persistent Data Path
        
        if (relativeDirectory.Length > 0) {
            if (relativeDirectory[0] == '\\' ||
                relativeDirectory[0] == '/') {
                // Strip the leading special character
                Debug.Log("Warning: Provided relativePathName contains a leading problem character. Please remove the first character to increase efficency. Problem string: " + relativeDirectory);
                relativeDirectory.Remove(0, 1); // Start at element 0, remove 1 character
            }
        }
        return Path.Combine(Application.persistentDataPath, relativeDirectory);
    }

    public static void WriteStringToFile(string relativeDirectory, string fileName, string textToWrite) {
        string fullDirectory = cleanFileName(relativeDirectory);
        Directory.CreateDirectory(fullDirectory); // No op if it already exists
        string fileLocation = Path.Combine(fullDirectory, fileName);
        File.WriteAllText(fileLocation, textToWrite);
    }

    public static string ReadStringFromFile(string relativeDirectory, string filename) {
        string fullDirectory = cleanFileName(relativeDirectory);
        string fileLocation = Path.Combine(fullDirectory, filename);
        return File.ReadAllText(fileLocation);
    }
}
