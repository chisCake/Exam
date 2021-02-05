using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Exam
{
	static class C1_3
	{
		public static void Test()
		{
			// Задание 5 - вывод с консоли в файл
			using var sw = new StreamWriter("Console.txt");
			//Console.SetOut(sw);

			var list = new List<SomeString>()
			{
				new SomeString("123"),
				new SomeString("123"),
				new SomeString("124"),
				new SomeString("103"),
				new SomeString("1003")
			};

			Console.WriteLine("CompareTo");
			Compare(list[0], list[1]);
			Compare(list[0], list[2]);
			Compare(list[0], list[3]);
			Compare(list[0], list[4]);
			Console.WriteLine("Equals");
			Equals(list[0], list[1]);
			Equals(list[0], list[2]);
			Equals(list[0], list[3]);
			Equals(list[0], list[4]);

			Console.WriteLine("\n" + list[0]);
			list[0] += '0';
			Console.WriteLine("После добавления 0: " + list[0]);
			try
			{
				Console.WriteLine(list[0]--);
				Console.WriteLine(list[0]--);
				Console.WriteLine(list[0]--);
				Console.WriteLine(list[0]--);
				Console.WriteLine(list[0]--);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Exception: {e.Message}\n");
			}

			var sstr = new SomeString("sa d.123,asd 213:12 ; sa");
			Console.WriteLine($"Кол-во пробелов в строке \"{sstr}\": {sstr.CountSpaces()}");
			sstr.RemovePunctuationMarks();
			Console.WriteLine($"После удаление знаков препинания: \"{sstr}\"\n");

			var listOfSpaces = new List<SomeString>()
			{
				new SomeString("1 23 4"),
				new SomeString("1  asz,x 4"),
				new SomeString("1 dm d eiu0"),
				new SomeString("1 kd ks 9"),
				new SomeString(" 123  ss"),
				new SomeString(" jsd8  s ")
			};

			// Локальные функции просто для удобства
			static void Compare(SomeString s1, SomeString s2)
			{
				string res;
				if (s1.String.CompareTo(s2.String) == 0)
					res = "равна";
				else
					res = "неравна";
				Console.WriteLine($"Строка \"{s1}\" {res} строке \"{s2}\"");
			}

			static void Equals(SomeString s1, SomeString s2)
			{
				string res;
				if (s1.Equals(s2))
					res = "равна";
				else
					res = "неравна";
				Console.WriteLine($"Строка \"{s1}\" {res} строке \"{s2}\"");
			}
		}

		// Хз что за интерфейс "ICompare", скорее всего "IComparer"
		public class SomeString : IComparer
		{
			public string String { get; set; }

			public SomeString()
			{
				String = "";
			}

			public SomeString(string value)
			{
				String = value;
			}

			// Задание 2
			public static SomeString operator +(SomeString str, char symbol)
			{
				var newStr = new SomeString(str.String);
				newStr.String += symbol;
				return newStr;
			}

			public static SomeString operator --(SomeString str)
			{
				if (str.String.Length == 0)
					throw new Exception("В строке нет символов");

				return new SomeString(str.String[1..]);
			}

			// Задание 1
			public override bool Equals(object obj)
			{
				if (!(obj is SomeString) && !(obj is string))
					return false;

				string str = obj is SomeString ? (obj as SomeString).String : obj as string;
				return 
					str.Length == String.Length && 
					str[0] == String[0] && 
					str[^1] == String[^1];
			}

			public override int GetHashCode()
			{
				return String.GetHashCode();
			}

			public int Compare(object x, object y)
			{
				if (!(x is SomeString) || !(y is SomeString))
					throw new Exception();

				var xs = (x as SomeString).String;
				var ys = (y as SomeString).String;

				if (xs == ys)
					return 0;

				if (xs.Length < ys.Length)
					return -1;
				else
					return 1;
			}

			public override string ToString()
			{
				return String;
			}
		}
	}

	// См. C3_3.cs
	static class SomeStringExtension1_3
	{
		public static int CountSpaces(this C1_3.SomeString ss)
		{
			// Приведение строки к массиву символов и взаимодействие с помощью LINQ как с обычным массивом
			return ss.String.ToCharArray().Where(symbol => symbol == ' ').Count();
		}

		public static void RemovePunctuationMarks(this C1_3.SomeString ss)
		{
			ss.String = ss.String.Replace(".", "");
			ss.String = ss.String.Replace(",", "");
			ss.String = ss.String.Replace("!", "");
			ss.String = ss.String.Replace(";", "");
			ss.String = ss.String.Replace(":", "");
			ss.String = ss.String.Replace("...", "");
			ss.String = ss.String.Replace("-", "");
		}
	}
}
