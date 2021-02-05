using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Exam
{
	static class C3_1
	{
		public static void Test()
		{
			var date = new MyDate(22, 4);
			Console.WriteLine(date);
			date.Day = 32;
			date.Month = 11;
			Console.WriteLine(date);

			Reflector.PrintAll(typeof(MyDate));

			var d1 = new MyDate(11, 6);
			var d2 = new MyDate(12, 6);
			var d3 = new MyDate(6, 4);

			Console.WriteLine("\nОператор ==");
			Compare1(d1, d2);
			Compare1(d1, d3);
			Compare1(d2, d3);
			Compare1(d1, d1);

			Console.WriteLine("\nОператор <=");
			Compare2(d1, d2);
			Compare2(d1, d3);
			Compare2(d2, d3);
			Compare2(d1, d1);

			Console.WriteLine("\nEquals");
			Compare3(d1, d2);
			Compare3(d1, d3);
			Compare3(d2, d3);
			Compare3(d1, d1);

			// Задание 4
			var list = new List<MyDate>()
			{
				new MyDate(1,1),
				new MyDate(24,2),
				new MyDate(12,3),
				new MyDate(8,2),
				new MyDate(30,11)
			};

			Console.WriteLine("\nСписок, отсортированный по месяцу");
			foreach (var item in list.OrderBy(item => item.Month))
				Console.WriteLine(item);

			Console.WriteLine("\nСписок, отсортированный по дате");
			foreach (var item in list.OrderBy(item => item.Day))
				Console.WriteLine(item);

			int month = 2;
			Console.WriteLine($"\nВсе дни месяца {month}");
			var searchRes = list.Where(item => item.Month == month);

			if (searchRes.Count() == 0)
				Console.WriteLine("Ничего не найдено");
			else
				foreach (var item in searchRes)
					Console.WriteLine(item);

			// System.Text.Json.JsonSerializer есть в .Net Core последних версий
			// Если такого не будет, то надо скачивать Newtonsoft.Json
			var json = JsonSerializer.Serialize(list);
			Console.WriteLine("\nСериализованный список");
			Console.WriteLine(json);

			void Compare1(MyDate date1, MyDate date2)
			{
				string res = date1 == date2 ? "равно" : "неравно";
				Console.WriteLine($"{date1} {res} {date2}");
			}

			void Compare2(MyDate date1, MyDate date2)
			{
				string res = date1 <= date2 ? "меньше либо равно" : "больше";
				Console.WriteLine($"{date1} {res} {date2}");
			}

			void Compare3(MyDate date1, MyDate date2)
			{
				string res = date1.Equals(date2) ? "равно" : "неравно";
				Console.WriteLine($"{date1} {res} {date2}");
			}
		}

		class MyDate
		{
			// Задание 1
			private int _day;
			public int Day
			{
				get => _day;
				set
				{
					if (value > 31 || value < 1)
					{
						_day = 31;
						using var sw = new StreamWriter("Error.log");
						sw.WriteLine("Попытка установки неверного дня в объекте ...(хз чё тут написать)");
					}
					else
						_day = value;
				}
			}

			private int _month;
			public int Month
			{
				get => _month;
				set
				{
					if (value > 12 || value < 1)
					{
						_month = 12;
						using var sw = new StreamWriter("Error.log");
						sw.WriteLine("Попытка установки неверного месяца в объекте ...(хз чё тут написать)");
					}
					_month = value;
				}
			}

			public MyDate(int day, int month)
			{
				Day = day;
				Month = month;
			}

			// Задание 3
			public static bool operator ==(MyDate date1, MyDate date2)
			{
				return date1.Day == date2.Day && date1.Month == date2.Month;
			}

			// Идёт в связке с ==
			public static bool operator !=(MyDate date1, MyDate date2)
			{
				return date1.Day != date2.Day || date1.Month != date2.Month;
			}

			public static bool operator <=(MyDate date1, MyDate date2)
			{
				if (date1.Month < date2.Month)
					return true;
				else
				{
					if (date1.Month == date2.Month)
						return date1.Day <= date2.Day;
					else
						return false;
				}
			}

			// Идёт в связке с <=
			public static bool operator >=(MyDate date1, MyDate date2)
			{
				if (date1.Month > date2.Month)
					return true;
				else
				{
					if (date1.Month == date2.Month)
						return date1.Day >= date2.Day;
					else
						return false;
				}
			}

			public override bool Equals(object obj)
			{
				if (!(obj is MyDate))
					return false;

				var date = obj as MyDate;
				return Day == date.Day && Month == date.Month;
			}

			// Для удобства вывода
			public override string ToString()
			{
				return $"{Day}.{Month.ToString().PadLeft(2).Replace(' ', '0')}";
			}

			public override int GetHashCode()
			{
				return (Day * Month - (Day / Month) * 7).GetHashCode();
			}
		}

		// Задание 2
		static class Reflector
		{
			public static void PrintAll(Type type)
			{
				var constructors = type.GetConstructors();
				var properties = type.GetProperties();
				var methods = type.GetMethods();
				var fields = type.GetFields();

				Console.WriteLine("\nТип " + type.Name);

				Console.WriteLine("\nКонструкторы");
				foreach (var item in constructors)
					Console.WriteLine(item);

				Console.WriteLine("\nСвойства");
				foreach (var item in properties)
					Console.WriteLine(item);

				Console.WriteLine("\nМетоды");
				foreach (var item in methods)
					Console.WriteLine(item);

				// Закрытые поля не выводятся
				Console.WriteLine("\nПоля");
				foreach (var item in fields)
					Console.WriteLine(item);
			}
		}
	}
}
