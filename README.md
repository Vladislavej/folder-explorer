# Folder Information Application


This is a simple command-line application written in C# I made for a job interview, that takes a single input - a path to a folder. The application then reads information about the specified folder, including all files and nested folders, and saves this information in the following format:

- For each folder, it saves its name, a list of all files, and a list of all nested folders it contains.
- For each file, it saves its name and extension.

## Features

1. **List Unique File Extensions**: Displays all unique file extensions found within the specified folder and its subfolders.
2. **Serialize Folder Information to JSON**: Serializes the folder information into JSON format.
3. **Save JSON to File**: Saves the generated JSON data to a file.
4. **Deserialize JSON File**: Deserializes JSON data containing folder information.
