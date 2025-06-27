using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _To_Do_List
{
    internal class Program
    {
        static void ADD(ref Dictionary<string, (string Description, bool IsCompleted)> taskDictionary)
        {
            Console.WriteLine("Введите название задачи (ключ): ");
            string key = Console.ReadLine();

            Console.WriteLine("Введите описание задачи (значение): ");
            string value = Console.ReadLine();

            if (taskDictionary.ContainsKey(key))
            {
                Console.WriteLine("Задача с таким названием уже существует. Попробуйте другое название.");
            }
            else
            {
                taskDictionary.Add(key, (value, false));
                Console.WriteLine("Задача успешно добавлена.");
            }
        }

        static void CheckTasks(ref Dictionary<string, (string Description, bool IsCompleted)> taskDictionary)
        {
            if (taskDictionary.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            Console.WriteLine("Список задач:");
            int index = 1;
            foreach (var task in taskDictionary)
            {
                string status = task.Value.IsCompleted ? "Выполнено" : "Не выполнено";
                Console.WriteLine($"{index}. {task.Key} - {task.Value.Description} [{status}]");
                index++;
            }
        }

        static void MarkTaskAsCompleted(ref Dictionary<string, (string Description, bool IsCompleted)> taskDictionary)
        {
            if (taskDictionary.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            CheckTasks(ref taskDictionary);
            Console.WriteLine("Введите номер задачи, которую хотите отметить как выполненную: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumber))
            {
                if (taskNumber >= 1 && taskNumber <= taskDictionary.Count)
                {
                    var key = GetKeyByIndex(taskDictionary, taskNumber);
                    var task = taskDictionary[key];
                    taskDictionary[key] = (task.Description, true);
                    Console.WriteLine($"Задача '{key}' отмечена как выполненная.");
                }
                else
                {
                    Console.WriteLine("Некорректный номер задачи.");
                }
            }
            else
            {
                Console.WriteLine("Введено не число.");
            }
        }

        static void DeleteTask(ref Dictionary<string, (string Description, bool IsCompleted)> taskDictionary)
        {
            if (taskDictionary.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            CheckTasks(ref taskDictionary);
            Console.WriteLine("Введите номер задачи, которую хотите удалить:");
            if (int.TryParse(Console.ReadLine(), out int taskNumber))
            {
                if (taskNumber >= 1 && taskNumber <= taskDictionary.Count)
                {
                    var key = GetKeyByIndex(taskDictionary, taskNumber);
                    taskDictionary.Remove(key);
                    Console.WriteLine($"Задача '{key}' удалена.");
                }
                else
                {
                    Console.WriteLine("Некорректный номер задачи.");
                }
            }
            else
            {
                Console.WriteLine("Введено не число.");
            }
        }

        static void SaveTasksToFile(string filename, Dictionary<string, (string Description, bool IsCompleted)> taskDictionary)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (var task in taskDictionary)
                {
                    string line = $"{task.Key}|{task.Value.Description}|{task.Value.IsCompleted}";
                    writer.WriteLine(line);
                }
            }
            Console.WriteLine("Задачи сохранены в файл.");
        }

        static void LoadTasksFromFile(string filename, out Dictionary<string, (string Description, bool IsCompleted)> taskDictionary)
        {
            taskDictionary = new Dictionary<string, (string Description, bool IsCompleted)>();
            if (File.Exists(filename))
            {
                string[] lines = File.ReadAllLines(filename);
                foreach (var line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 3)
                    {
                        string key = parts[0];
                        string description = parts[1];
                        bool isCompleted = bool.Parse(parts[2]);
                        taskDictionary[key] = (description, isCompleted);
                    }
                }
            }
        }

        static string GetKeyByIndex(Dictionary<string, (string Description, bool IsCompleted)> dict, int index)
        {
            return dict.Keys.ElementAt(index - 1);
        }

        static void Main(string[] args)
        {
            string filename = "tasks.txt";
            LoadTasksFromFile(filename, out var Tasks);

            while (true)
            {
                Console.WriteLine("Welcome to To-Do List\nВыберите действие:\n1 - Добавление задачи\n2 - Просмотр всех задач\n3 - Отметка задачи как выполненной\n4 - удаление задачи\n5 - Сохранение задач в файл\n6 - Выход\nВвод: ");
               

                string input = Console.ReadLine();
                if (int.TryParse(input, out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            ADD(ref Tasks);
                            break;
                        case 2:
                            CheckTasks(ref Tasks);
                            break;
                        case 3:
                            MarkTaskAsCompleted(ref Tasks);
                            break;
                        case 4:
                            DeleteTask(ref Tasks);
                            break;
                        case 5:
                            SaveTasksToFile(filename, Tasks);
                            break;
                        case 6:
                            SaveTasksToFile(filename, Tasks);
                            Console.WriteLine("Выход из программы. Пока!");
                            return;
                        default:
                            Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Ввод должен быть числом. Попробуйте снова.");
                }

                Console.WriteLine("Нажмите любую кнопку для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
