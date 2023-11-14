using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

public class Humans
{
    public string Imya;
    public int Vozrast;
    public string Familiya;

}

public class FileManager
{
    private List<Humans> hum = new List<Humans>();
    private string filePath;

    public FileManager(string path)
    {
        filePath = path;
    }

    public void LoadFromFile()
    {
        if (File.Exists(filePath))
        {
            string fileExtension = Path.GetExtension(filePath).ToLower();

            if (fileExtension == ".txt")
            {
                hum.Clear();
                foreach (string line in File.ReadLines(filePath))
                {
                    string[] parts = line.Split(' ');
                    if (parts.Length == 3)
                    {
                        hum.Add(new Humans { Imya = parts[0], Vozrast = int.Parse(parts[1]), Familiya = parts[2] });
                    }
                }
            }
            else if (fileExtension == ".xml")
            {
                    XmlSerializer xml = new XmlSerializer(typeof(List<Humans>));
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    {
                        hum = (List<Humans>)xml.Deserialize(fs);
                    }
            }
            else if (fileExtension == ".json")
            {
                string json = File.ReadAllText(filePath);
                hum = JsonConvert.DeserializeObject<List<Humans>>(json);
            }
            else
            {
                Console.WriteLine("не тот формат файла");
            }
        }
        else
        {
            Console.WriteLine("Файл не существует");
        }
    }


    public void SaveToFile()
    {
        string fileExtension = Path.GetExtension(filePath).ToLower();

        if (fileExtension == ".txt")
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (var human in hum)
                {
                    sw.WriteLine($"{human.Imya} {human.Vozrast} {human.Familiya}");
                }
            }
        }
        else if (fileExtension == ".xml")
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<Humans>));
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                xml.Serialize(fs, hum);
            }
        }
        else if (fileExtension == ".json")
        {
            string json = JsonConvert.SerializeObject(hum, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        else
        {
            Console.WriteLine("не тот формат файла");
        }
    }
}
public class Program0
{
    static void Main()
    {
        Console.WriteLine("Введите путь к файлу");
        string path = Console.ReadLine();

        FileManager fileManager = new FileManager(path);
        fileManager.LoadFromFile();

        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey();
            if (key.Key == ConsoleKey.F1)
            {
                fileManager.SaveToFile();
                Console.WriteLine("Файл сохранен.");
            }
        } while (key.Key != ConsoleKey.Escape);

        Console.WriteLine("Программа завершена.");
    }
}
