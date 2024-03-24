using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

class FolderInfo
{
    public string Name { get; set; }
    public List<string> Files { get; set; }
    public List<FolderInfo> SubFolders { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        string path;
        while (true)
        {
            Console.WriteLine("\nPlease provide the folder path or a .json (type 'exit' to quit)");

            path = Console.ReadLine();

            if (path.Equals("exit", StringComparison.OrdinalIgnoreCase))
                break;

            if (File.Exists(path) && Path.GetExtension(path).Equals(".json", StringComparison.OrdinalIgnoreCase))
            {
                List<string> extensionsFromJson = LoadExtensionsFromJson(path);

                Console.Write("Extensions found in JSON file: ");
                foreach (string extension in extensionsFromJson)
                {
                    Console.Write(extension + " ");
                }
                continue;
            }
            else if (!Directory.Exists(path))
            {
                Console.WriteLine("Invalid folder path or JSON file not found!");
                continue;
            }

            FolderInfo rootFolder = GetFolderInfo(path);
            List<string> uniqueExtensions = GetUniqueExtensions(rootFolder);

            Console.Write("Extensions found in folder: ");
            foreach (string extension in uniqueExtensions)
            {
                Console.Write(extension + " ");
            }
            Console.WriteLine("\nDo you want to save this information to a JSON file? (yes/no)");
            var response = Console.ReadLine();

            if (response.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Please provide a location to save the JSON file");
                var jsonPath = Console.ReadLine();
                SaveToJson(rootFolder, jsonPath);
            }
        }
    }
    static FolderInfo GetFolderInfo(string path)
    {
        DirectoryInfo directory = new(path);
        FolderInfo folderInfo = new()
        {
            Name = directory.Name,
            Files = new List<string>(),
            SubFolders = new List<FolderInfo>()
        };

        foreach (FileInfo file in directory.GetFiles())
        {
            folderInfo.Files.Add(file.Name);
        }

        foreach (DirectoryInfo subDir in directory.GetDirectories())
        {
            folderInfo.SubFolders.Add(GetFolderInfo(subDir.FullName));
        }

        return folderInfo;
    }

    static List<string> GetUniqueExtensions(FolderInfo folder)
    {
        HashSet<string> extensions = new();

        foreach (string fileName in folder.Files)
        {
            string extension = Path.GetExtension(fileName);
            extensions.Add(extension);
        }

        foreach (FolderInfo subFolder in folder.SubFolders)
        {
            extensions.UnionWith(GetUniqueExtensions(subFolder));
        }

        return new List<string>(extensions);
    }

    static void SaveToJson(FolderInfo folder, string path)
    {
        try
        {
            string json = JsonConvert.SerializeObject(folder, Formatting.Indented);
            File.WriteAllText(path, json);
            Console.WriteLine("Data saved to JSON successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving to JSON: {ex.Message}");
        }
    }

    static List<string> LoadExtensionsFromJson(string path)
    {
        try
        {
            string json = File.ReadAllText(path);
            FolderInfo loadedFolder = JsonConvert.DeserializeObject<FolderInfo>(json);
            return GetUniqueExtensions(loadedFolder);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading from JSON: {ex.Message}");
            return new List<string>();
        }
    }
}