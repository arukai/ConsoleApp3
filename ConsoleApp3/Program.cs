using System;
using System.Collections.Generic;

namespace TrpoLab
{
    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Person(string lastName, string firstName, string middleName)
        {
            FirstName = firstName; LastName = lastName; MiddleName = middleName;
        }
        public string GetGIO() { return LastName + " " + FirstName + " " + MiddleName; }
    }

    class Tutor : Person
    {
        public string Position { get; set; }
        public Tutor(string lastName = "", string firstName = "", string middleName = "", string position = "")
            : base(lastName, firstName, middleName) { Position = position; }
        public string About() { return GetGIO() + ", должность: " + Position; }
    }

    class Student : Person
    {
        public ushort Year { get; set; }
        public ulong GradebookNumber { get; set; }
        public string Group { get; set; }
        public Student(string lastName = "", string firstName = "", string middleName = "", ushort year = 0, ulong gradebookNumber = 0, string group = "")
            : base(lastName, firstName, middleName)
        {
            Year = year; GradebookNumber = gradebookNumber; Group = group;
        }
        public string Info() { return Year + " курс, " + Group + ", № " + GradebookNumber; }
    }

    class Grade
    {
        public byte Mark { get; set; }
        public string DateOfIssue { get; set; }
        public Student Student { get; set; }
        public Grade(byte mark = 0, string dateOfIssue = "", Student student = null)
        {
            Mark = mark; DateOfIssue = dateOfIssue; Student = student;
        }
        public string Print() { return Student.GetGIO() + " (" + Student.Info() + ") | " + Mark + " | " + DateOfIssue; }
    }

    class Statement
    {
        protected string Subject;
        protected Tutor Tutor;
        protected List<Grade> Grades;

        public Statement(string subject = "", Tutor tutor = null)
        {
            Subject = subject; Tutor = tutor; Grades = new List<Grade>();
        }
        public void Add(Grade grade) { Grades.Add(grade); }
        public void Erase(int item) { Grades.RemoveAt(item - 1); }
        public void Clear() { Grades.Clear(); }
        public int GetCount() { return Grades.Count; }

        public void Print()
        {
            if (Grades.Count > 0)
            {
                int index = 1;
                Console.WriteLine("--------------------------------------------------------------------------");
                Console.WriteLine("ВЕДОМОСТЬ");
                Console.WriteLine("Дисциплина: " + Subject);
                Console.WriteLine("Преподаватель: " + Tutor.About());
                Console.WriteLine("--------------------------------------------------------------------------");
                Console.WriteLine("№ п/п | ФИО студента (курс, группа, номер зач. книжки) | Оценка | Дата");
                foreach (Grade grade in Grades)
                {
                    Console.WriteLine(index + " | " + grade.Print());
                    index++;
                }
                Console.WriteLine("--------------------------------------------------------------------------");
            }
            else { Console.WriteLine("Ведомость пуста!"); }
        }
    }

    // === ГЛАВНАЯ ПРОГРАММА С МЕНЮ ===
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Создание новой ведомости...");
            Console.Write("Введите название дисциплины: ");
            string subject = Console.ReadLine();

            Console.WriteLine("\nДанные преподавателя:");
            Console.Write("Фамилия: "); string tLastName = Console.ReadLine();
            Console.Write("Имя: "); string tFirstName = Console.ReadLine();
            Console.Write("Отчество: "); string tMiddleName = Console.ReadLine();
            Console.Write("Должность: "); string tPosition = Console.ReadLine();

            Tutor tutor = new Tutor(tLastName, tFirstName, tMiddleName, tPosition);
            Statement statement = new Statement(subject, tutor);

            byte selectItem = 0;
            while (selectItem != 5)
            {
                Console.Clear();
                Console.WriteLine("=== ГЛАВНОЕ МЕНЮ ===");
                Console.WriteLine("1 - Очистить ведомость");
                Console.WriteLine("2 - Внести оценку в ведомость");
                Console.WriteLine("3 - Удалить оценку из ведомости");
                Console.WriteLine("4 - Отобразить ведомость");
                Console.WriteLine("5 - Выход");
                Console.Write("> ");

                string input = Console.ReadLine();
                byte.TryParse(input, out selectItem);

                switch (selectItem)
                {
                    case 1:
                        statement.Clear();
                        Console.WriteLine("Ведомость успешно очищена!");
                        Console.ReadLine();
                        break;

                    case 2:
                        Console.WriteLine("\n-- Внесение новой оценки --");
                        Student student = new Student();
                        Console.Write("Фамилия студента: "); student.LastName = Console.ReadLine();
                        Console.Write("Имя студента: "); student.FirstName = Console.ReadLine();
                        Console.Write("Отчество студента: "); student.MiddleName = Console.ReadLine();
                        Console.Write("Группа: "); student.Group = Console.ReadLine();

                        ushort tmpYear = 0;
                        bool correctInput = false;
                        do
                        {
                            Console.Write("Введите курс студента (число): ");
                            correctInput = ushort.TryParse(Console.ReadLine(), out tmpYear);
                            if (!correctInput) Console.WriteLine("Ошибка! Введите целое число.");
                        } while (!correctInput);
                        student.Year = tmpYear;

                        ulong tmpGradebook = 0; correctInput = false;
                        do
                        {
                            Console.Write("Номер зачетной книжки (число): ");
                            correctInput = ulong.TryParse(Console.ReadLine(), out tmpGradebook);
                            if (!correctInput) Console.WriteLine("Ошибка! Введите целое число.");
                        } while (!correctInput);
                        student.GradebookNumber = tmpGradebook;

                        Grade grade = new Grade { Student = student };

                        byte tmpMark = 0; correctInput = false;
                        do
                        {
                            Console.Write("Оценка (число): ");
                            correctInput = byte.TryParse(Console.ReadLine(), out tmpMark);
                            if (!correctInput) Console.WriteLine("Ошибка! Введите число.");
                        } while (!correctInput);
                        grade.Mark = tmpMark;

                        Console.Write("Дата выставления (напр. 10.10.2023): ");
                        grade.DateOfIssue = Console.ReadLine();

                        statement.Add(grade);
                        Console.WriteLine("Оценка успешно добавлена!");
                        Console.ReadLine();
                        break;

                    case 3:
                        if (statement.GetCount() == 0)
                        {
                            Console.WriteLine("Ведомость пуста, удалять нечего!");
                        }
                        else
                        {
                            statement.Print();
                            Console.Write("\nВведите номер (№ п/п) удаляемой записи: ");
                            if (int.TryParse(Console.ReadLine(), out int delIndex) && delIndex > 0 && delIndex <= statement.GetCount())
                            {
                                statement.Erase(delIndex);
                                Console.WriteLine("Запись успешно удалена.");
                            }
                            else { Console.WriteLine("Ошибка: неверный номер записи."); }
                        }
                        Console.ReadLine();
                        break;

                    case 4:
                        Console.Clear();
                        statement.Print();
                        Console.WriteLine("\nНажмите Enter для возврата в меню...");
                        Console.ReadLine();
                        break;
                    case 5:
                        Console.WriteLine("Выход из программы...");
                        break;
                    default:
                        Console.WriteLine("Неверный пункт меню, попробуйте снова.");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}