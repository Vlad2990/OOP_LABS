using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;
using Application.Service;
using Domain.Entity;
using Presentation.Commands;
using System.ComponentModel;
using System.Xml.Linq;
using System.Diagnostics;

namespace Presentation
{
    internal class View
    {
        private readonly StudentService _studentService;
        private readonly QuoteService _quoteService;
        public List<StudentDTO> students = [];
        public int Selected = 0;
        Command Command;
        string space = new string('=', 50);
        string menu = "Add student";
        public View() 
        {
            space = '\n' + space + '\n';
            _studentService = new StudentService();
            _quoteService = new QuoteService();
            Command = new ViewCommand(_studentService);
        }
        public async Task Start()
        {
            Console.CursorVisible = false;
            await Run();
            while(true)
            {
                Console.Clear();
                ListAll();
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        if (Selected == 5) Selected -= 4;

                        else if (Selected > 0) Selected--;
                        break;

                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        if (Selected == 1 && students.Count != 0) Selected += 4;
                        
                        else if (Selected != 1 && Selected - 5 < students.Count-1) Selected++;

                        break;

                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        if (Selected == 0)
                        {
                            await Adding();
                        }
                        else if (Selected == 1)
                        {
                            await Ending();
                        }
                        else
                        {
                            await Viewing();
                        }
                        break;
                }
            }
        }
        public void ListAll()
        {
            if (Selected == 0) Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(menu);
            Console.ForegroundColor = ConsoleColor.White;
            if (Selected == 1) Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Exit");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(space);

            Console.WriteLine("Students:");
            Console.WriteLine();
            for (int i = 0; i < students.Count; i++) 
            {
                if (i == Selected - 5)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(students[i].name);
                    Console.ForegroundColor = ConsoleColor.White;
                    continue;
                }
                Console.WriteLine(students[i].name);
            }
        }
        private async Task Adding()
        {
            Console.Clear();
            Console.WriteLine("Input name:");
            string name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Wrong input. Press any button");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Input grade(0 <= grade <= 10):");
            string inp = Console.ReadLine();
            if (!int.TryParse(inp, out int grade) || grade < 0 || grade > 10)
            {
                Console.WriteLine("Wrong input. Press any button");
                Console.ReadKey();
                return;
            }
            StudentDTO student = new StudentDTO(name, grade);
            Command = new AddCommand(_studentService, student);
            await Run();
            Console.Clear();
            Console.WriteLine($"Student name: {students[^1].name}\nGrade: {students[^1].grade}");
            Console.WriteLine(space);
            var quoteTask = _quoteService.GetQuoteAsync();
            await Loading(quoteTask);
            var quote = await quoteTask;
            Console.WriteLine($"{quote.Content}\n\t- {quote.Author}");
            Console.ReadKey(true);
        }
        private async Task Loading(Task<Quote> quoteTask)
        {
            var loadingSymbols = new[] { '|', '/', '-', '\\' };
            int i = 0;

            while (!quoteTask.IsCompleted)
            {
                Console.Write($"\rWait... {loadingSymbols[i++ % loadingSymbols.Length]}");
                await Task.Delay(200); 
            }
            Console.WriteLine("\r                     \r");
        }
        private async Task Viewing()
        {
            int pos = 0;

            while (true)
            {
                ListViewing(pos);
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        if (pos > 0) --pos;
                        break;

                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        if (pos < 1) ++pos;
                        break;

                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        if (pos == 0)
                        {
                            Console.WriteLine("Input new name:");
                            string name = Console.ReadLine();
                            if (string.IsNullOrEmpty(name))
                            {
                                Console.WriteLine("Wrong input. Press any button");
                                Console.ReadKey();
                                break;
                            }
                            Command = new EditCommand(_studentService, new StudentDTO(name, students[Selected - 5].grade), Selected - 5);
                            await Run();
                        }
                        else if (pos == 1)
                        {
                            Console.WriteLine("Input grade(0 <= grade <= 10):");
                            string inp = Console.ReadLine();
                            if (!int.TryParse(inp, out int grade) || grade < 0 || grade > 10)
                            {
                                Console.WriteLine("Wrong input. Press any button");
                                Console.ReadKey();
                                break;
                            }
                            Command = new EditCommand(_studentService, new StudentDTO(students[Selected - 5].name, grade), Selected -5);
                            await Run();
                        }
                        break;

                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
        private async Task Ending()
        {
            Console.Clear();
            StringBuilder s = new StringBuilder("GoodBye!");
            
            while (s.Length != 0)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
                Console.Write($"{s}");
                await Task.Delay(300);
                s.Remove(s.Length - 1, 1);
                Console.Clear();
            }
            Process.GetCurrentProcess().Kill();
        }
        private void ListViewing(int pos)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            if(pos==0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine("Change name");
            Console.ForegroundColor = ConsoleColor.White;
            if (pos == 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine("Change grade");
            Console.ForegroundColor= ConsoleColor.White;
            Console.WriteLine(space);
            Console.WriteLine($"Student name: {students[Selected - 5].name}\nGrade: {students[Selected - 5].grade}");
            Console.WriteLine(space);
        }
        private async Task Run() => students = await Command.Execute();
    }
}
