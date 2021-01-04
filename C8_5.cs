using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace Exam
{
	static class C8_5
	{
		public static void Test()
		{
			var formatter1 = new TextFormatter("Моя, новая, строка.");

			var info = formatter1.Count();
			Console.WriteLine($"Кол-во точек в строке \"{formatter1}\": {info.Item1}, кол-во запятых: {info.Item2}");

			var splitted = formatter1.SplitText();
			// Сортировка
			splitted = splitted.OrderBy(str => str.Length).ToList();
			// Запись
			using (var sw = new StreamWriter("string.txt"))
			{
				foreach (var str in splitted)
					sw.WriteLine(str);
			}
			Console.WriteLine("\nСортировання разбитая строка записана в файл\n");

			var formatter2 = new TextFormatter("Моя");
			var formatter3 = new TextFormatter("Строка");
			var formatter4 = new TextFormatter("123");

			// Сравнения
			Console.WriteLine($"Equals: {formatter1} == {formatter2}: {formatter1.Equals(formatter2)}");
			// (formatter1 as IComparable) потому что IComparable реализован явно
			Console.WriteLine($"CompareTo: {formatter1} == {formatter2}: {(formatter1 as IComparable).CompareTo(formatter2)}");
			Console.WriteLine($"Equals: {formatter2} == {formatter4}: {formatter2.Equals(formatter4)}");
			// Если возникнет вопрос: почему тут -1003?
			// Потому что CompareTo сравнивает символы по их коду
			Console.WriteLine($"CompareTo: {formatter2} == {formatter4}: {(formatter2 as IComparable).CompareTo(formatter4)}");

			var book = new Book<TextFormatter>(
				new TextFormatter("Строка1"),
				new TextFormatter("Строка2"),
				new TextFormatter("Строка3"),
				new TextFormatter("Строка4")
				);

			Console.WriteLine("\nИзначальные данные в book");
			Print(book);

			book.Add(new TextFormatter("Строка5"));
			book.Remove(2);
			Console.WriteLine("\nПосле добавления и удаления");
			Print(book);

			Console.WriteLine("Кол-во элементов в book: " + !book);

			static void Print(Book<TextFormatter> book)
			{
				foreach (var item in book.Data)
					Console.WriteLine(item);
			}
		}

		class TextFormatter : IComparable, IEquatable<TextFormatter>
		{
			public string String { get; set; }

			// Задание 1
			public TextFormatter()
			{
				String = "";
			}

			public TextFormatter(string str)
			{
				String = str;
			}

			// Возвращение кортежа
			public (int,int) Count()
			{
				return (String.Count(symbol => symbol == '.'), String.Count(symbol => symbol == ','));
			}

			// Задание 2
			public List<string> SplitText()
			{
				return String.Split(',').ToList();
			}

			public override string ToString()
			{
				return String;
			}

			// Задание 3
			// Явно
			int IComparable.CompareTo(object obj)
			{
				if (obj == null || !(obj is TextFormatter))
					return -1;

				var tf = obj as TextFormatter;
				if (tf.String.Length == 0)
					return -1;

				if (String.Length == 0)
					return 1;

				return tf.String[0].CompareTo(String[0]);
			}

			// Неявно
			public bool Equals([AllowNull] TextFormatter other)
			{
				if (other == null || other.String.Length < 1 || String.Length < 1)
					return false;

				return String[1] == other.String[1];
			}
		}

		class Book<T>
		{
			public List<T> Data { get; }

			public Book()
			{
				Data = new List<T>();
			}

			public Book(params T[] data)
			{
				Data = new List<T>(data);
			}

			public void Add(T item)
			{
				Data.Add(item);
			}

			// Удаление элемента по экземпляру (аргумент типа T, а не index) тут как-бы возможно
			// Но в виду того, что по заданию надо работать с TextFormatter, в котором переопределён Equals
			// Data.Remove(T item) как надо не работает, но сделать так тоже можно
			// Поэтому тут удаление по индексу
			public bool Remove(int index)
			{
				if (index >= Data.Count())
					return false;

				Data.RemoveAt(index);
				return true;
			}

			// Для таких операций (в которых 1 аргумент) лучше использовать унарные операторы
			// Лучше: !() ~()
			// Но можно и: +(), -(), ++(), --(), true(), false(), только тут уже перегружать надо попарно
			public static int operator !(Book<T> book)
			{
				return book.Data.Count();
			} 
		}
	}
}
