using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exam
{
	static class C2_4
	{
		public static void Test()
		{
			var s1 = new Student("Имя1", new Date(1, 1, 2001), "М", 1);
			var s2 = new Student("Имя2", new Date(2, 2, 2002), "М", 1);
			var s3 = new Student("Имя3", new Date(3, 3, 2001), "Ж", 2);
			var s4 = new Student("Имя4", new Date(4, 4, 2002), "Ж", 1);
			var s5 = new Student("Имя5", new Date(5, 5, 2001), "М", 2);
			var s6 = new Student("Имя6", new Date(6, 6, 2002), "Ж", 1);

			Console.WriteLine($"{s1}, возраст: {s1.GetAge()}");
			s1.PrintCourse();
			s1++;
			s1.PrintCourse();
			s1--;
			Console.WriteLine();
			s1.PrintCourse();
			s2.PrintCourse();
			s3.PrintCourse();

			var dean = new Dean();
			dean.NextYear += s1.NextYear;
			dean.NextYear += s2.NextYear;
			dean.NextYear += s3.NextYear;
			Console.WriteLine("\nПосле события NextCourse");
			dean.InvokeNextYear();

			s1.PrintCourse();
			s2.PrintCourse();
			s3.PrintCourse();

			Console.WriteLine("\nГруппа");
			var group = new Group<Student>(s1, s2, s3, s4, s5);
			foreach (var item in group.Students)
			{
				item.PrintCourse();
			}

			string sex = "М";
			Console.WriteLine($"Кол-во человек пола {sex} в группе: {group.CountBySex(sex)}");
			int course = 2;
			Console.WriteLine($"Кол-во человек в группе с курсом больше или равного {course}: {group.CountByCourse(course)}");
		}

		abstract class AbstractStudent
		{
			public string Name { get; set; }
			public Date Birthday { get; set; }
			public string Sex { get; set; }

			public AbstractStudent(string name, Date birthday, string sex)
			{
				Name = name;
				Birthday = birthday;
				Sex = sex;
			}
		}

		class Date
		{
			public int Day { get; set; }
			public int Month { get; set; }
			public int Year { get; set; }

			public Date(int day, int month, int year)
			{
				Day = day;
				Month = month;
				Year = year;
			}

			public override string ToString()
			{
				return $"{Day}.{Month}.{Year}";
			}
		}

		class Student : AbstractStudent
		{
			public int Course { get; private set; }

			static Student()
			{
				// Статический конструктор
			}

			public Student(string name, Date birthday, string sex, int course = 1) : base(name, birthday, sex)
			{
				Course = course;
			}

			public void PrintCourse()
			{
				Console.WriteLine($"Студент {this} находится на {Course} курсе");
			}

			public double GetAge()
			{
				// Мне тут впадлу делать поправку на месяц и день
				return DateTime.Now.Year - Birthday.Year;
			}

			// Для задания 3
			public void NextYear()
			{
				Course++;
			}

			// Задание 2
			public static Student operator ++(Student student)
			{
				return new Student(student.Name, student.Birthday, student.Sex, ++student.Course);
			}

			public static Student operator --(Student student)
			{
				return new Student(student.Name, student.Birthday, student.Sex, --student.Course);
			}

			public override string ToString()
			{
				return Name;
			}
		}

		// Задание 3
		class Dean
		{
			public event Action NextYear;

			public void InvokeNextYear()
			{
				NextYear.Invoke();
			}
		}

		// Задание 4
		class Group<T> where T : Student
		{
			public List<Student> Students;

			public Group(params Student[] students)
			{
				Students = new List<Student>(students);
			}

			public void Add(Student student)
			{
				Students.Add(student);
			}

			public bool Remove(Student student)
			{
				return Students.Remove(student);
			}

			public int CountBySex(string sex)
			{
				return Students.Where(item => item.Sex == sex).Count();
			}

			public int CountByCourse(int course)
			{
				return Students.Where(item => item.Course >= course).Count();
			}
		}
	}
}
