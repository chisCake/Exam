using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
#pragma warning disable CS1066

namespace Exam
{
	static class C3_3
	{
		public static void Test()
		{
			// Задание 5 - вывод с консоли в файл
			using var sw = new StreamWriter("Console.txt");
			Console.SetOut(sw);

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
				// То что тут стоит справа от "-=" не играет вообще никакой роли
				// Просто так надо из-за синтаксиса, а по заданию только убрать 1 символ в начале
				// Смотри реализацию в SomeString.operator -()
				Console.WriteLine(list[0] -= 1);
				Console.WriteLine(list[0] -= 2);
				Console.WriteLine(list[0] -= "123asd");
				Console.WriteLine(list[0] -= null);
				Console.WriteLine(list[0] -= int.MaxValue);
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

		public class SomeString : IComparer<string>
		{
			public string String { get; set; }

			public SomeString(string str)
			{
				String = str;
			}

			// Задание 1
			// От интерфейса IComparer
			public int Compare([AllowNull] string x, [AllowNull] string y)
			{
				return x.CompareTo(y);
			}

			// От класса Object
			public override bool Equals(object obj)
			{
				// Сначала проверить, является ли вообще переданный объект экземпляром класса SomeString
				// Иначе дальше можно и не сравнивать
				if (!(obj is SomeString))
					return false;

				var str = (obj as SomeString).String;
				// [^1] - то же самое что и [length - 1]
				return str.Length == String.Length && str[0] == String[0] && str[^1] == String[^1];
			}

			public override int GetHashCode()
			{
				return String.GetHashCode();
			}

			public override string ToString()
			{
				return String;
			}
			
			// Задание 2 - операторы
			public static SomeString operator +(SomeString sm, char symbol)
			{
				return new SomeString(sm.String + symbol);
			}

			// Вот хз что тут со вторым аргументом, типо по заданию он не надо, но по факту он надо
			// Ибо по другому вызывать такой метод нельзя
			// Ну логично что для операции "-" нужно что-то слева и что-то справо, но по заданию нужен только сам экземпляр
			// Поэтому object obj = null тут нужен только для того, чтобы был
			// Смотри использование в Test()
			public static SomeString operator -(SomeString ss, object obj = null)
			{
				if (ss.String.Length == 0)
					throw new Exception("В строке нет символов для удаления");

				return new SomeString(ss.String.Remove(0, 1));
			}

			// Задание 4 - LINQ
			public static int CountSpaces(IList<SomeString> strs)
			{
				// Тащемта тут всё просто
				return strs
					// 1) Посчитать сумму, взяв каждую строку экземпляра в списке
					.Sum(str => str.String
					// 2) Привести строку к массиву символов
					.ToCharArray()
					// 3) Взять все символы, которые являются пробелами
					.Where(symbol => symbol == ' ')
					// 4) Посчитать их кол-во, вернуть значение в пункт 1), т.е. прибавить к сумме
					.Count());
			}
		}

	}

	// Задание 3 - расширения
	// Класс расширения должен находиться в пространстве имён (namespace)!
	static class SomeStringExtension
	{
		// "C3_3" тут находится только из-за того, что у меня такая структура проекта, чтобы разделять билеты было удобно
		// Типо на экзе вложенных классов не будет (если в задании не потребуется), т.е. не надо будет так заморачиваться
		// И писать можно будет просто как обычно типо
		//
		// static void Main() {}
		// class SomeString {}
		// static class SomeStringExtension {}
		//
		public static int CountSpaces(this C3_3.SomeString ss)
		{
			// Приведение строки к массиву символов и взаимодействие с помощью LINQ как с обычным массивом
			return ss.String.ToCharArray().Where(symbol => symbol == ' ').Count();
		}

		public static void RemovePunctuationMarks(this C3_3.SomeString ss)
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
#pragma warning restore CS1066
