using System;
using System.IO;

public class FileSystemManager
{
    public static void Main(string[] args)
    {
        string groupNumber = "IP-24";
        string lastName = "Petrenko";
        string rootPath = @"D:\OOP_lab08";

        try
        {
            Directory.CreateDirectory(rootPath);
            Console.WriteLine($" 1. Створено каталог: {rootPath}");

            string groupPath = Path.Combine(rootPath, groupNumber);
            string lastNamePath = Path.Combine(rootPath, lastName);
            string sourcesPath = Path.Combine(rootPath, "Sources");
            string reportsPath = Path.Combine(rootPath, "Reports");
            string textsPath = Path.Combine(rootPath, "Texts");

            Directory.CreateDirectory(groupPath);
            Directory.CreateDirectory(lastNamePath);
            Directory.CreateDirectory(sourcesPath);
            Directory.CreateDirectory(reportsPath);
            Directory.CreateDirectory(textsPath);
            Console.WriteLine(" 2. Створено каталоги: Group, LastName, Sources, Reports, Texts.");

            CopyDirectory(textsPath, Path.Combine(lastNamePath, "Texts"));
            CopyDirectory(sourcesPath, Path.Combine(lastNamePath, "Sources"));
            CopyDirectory(reportsPath, Path.Combine(lastNamePath, "Reports"));
            Console.WriteLine($" 3. Каталоги Texts, Sources, Reports скопійовано до {lastNamePath}");

            string newLastNamePath = Path.Combine(groupPath, lastName);
            if (Directory.Exists(newLastNamePath))
            {
                Directory.Delete(newLastNamePath, true);
            }
            Directory.Move(lastNamePath, newLastNamePath);
            Console.WriteLine($" 4. Каталог {lastNamePath} переміщено до {groupPath}");

            string dirInfoFilePath = Path.Combine(textsPath, "dirinfo.txt");

            DirectoryInfo textsDirInfo = new DirectoryInfo(textsPath);
            using (StreamWriter writer = new StreamWriter(dirInfoFilePath))
            {
                writer.WriteLine("--- Інформація про каталог ---");
                writer.WriteLine($"Назва: {textsDirInfo.Name}");
                writer.WriteLine($"Повний шлях: {textsDirInfo.FullName}");
                writer.WriteLine($"Дата створення: {textsDirInfo.CreationTime}");
                writer.WriteLine($"Кореневий диск: {textsDirInfo.Root}");

                writer.WriteLine("\n--- Вкладені файли ---");
                foreach (var file in textsDirInfo.GetFiles())
                {
                    writer.WriteLine(file.Name);
                }

                writer.WriteLine("\n--- Вкладені каталоги ---");
                foreach (var subDir in textsDirInfo.GetDirectories())
                {
                    writer.WriteLine(subDir.Name);
                }
            }
            Console.WriteLine($" 5. Файл {dirInfoFilePath} створено та заповнено інформацією.");

            Console.WriteLine("\n Усі операції успішно виконано!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Сталася помилка: {ex.Message}");
        }
    }

    public static void CopyDirectory(string sourceDir, string destinationDir)
    {
        var dir = new DirectoryInfo(sourceDir);

        if (!dir.Exists)
            throw new DirectoryNotFoundException($"Каталог-джерело не знайдено: {dir.FullName}");

        Directory.CreateDirectory(destinationDir);

        foreach (FileInfo file in dir.GetFiles())
        {
            string targetFilePath = Path.Combine(destinationDir, file.Name);
            file.CopyTo(targetFilePath);
        }

        foreach (DirectoryInfo subDir in dir.GetDirectories())
        {
            string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
            CopyDirectory(subDir.FullName, newDestinationDir);
        }
    }
}
