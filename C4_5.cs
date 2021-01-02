using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Exam
{
	static class C4_5
	{
		public static void Test()
		{
			// Создание
			var wallet = new Wallet<Bill>(Bill.GetRndmCash(5));
			Print(wallet);

			// Добавление 1ым способом
			wallet.Add(new Bill(10), new Bill(100));
			Print(wallet);

			// Добавление 2ым способом
			var cash = new List<Bill>() { new Bill(5), new Bill(50) };
			wallet.Add(cash);
			Print(wallet);

			// Вывод кол-ва разных типов купюр
			wallet.DisplayBills();

			// Сериализация
			Wallet<Bill>.Serialize(wallet, "wallet.json");

			// Просто для вывода содержимого кошелька
			static void Print(Wallet<Bill> wallet)
			{
				foreach (var item in wallet.Cash)
					Console.WriteLine(item.Number);
				Console.WriteLine();
			}
		}
	}

	// Задание 1
	interface IBill
	{
		public int Number { get; }
	}

	// Задание 2
	class Bill : IBill
	{
		// В этой задаче только это свойство реализует интерфейс IBill
		public int Number { get; }

		public Bill(int number)
		{
			// Ограничения
			if (number == 5 || number == 10 || number == 50 || number == 100)
				Number = number;
			else
				throw new ArgumentException("Valid values for Number: 5, 10, 50, 100");
		}

		// Не по заданию (для теста)
		public static Bill[] GetRndmCash(int amt)
		{
			var cash = new List<Bill>();
			var rndm = new Random();
			var allowedBills = new int[] { 5, 10, 50, 100 };
			for (int i = 0; i < amt; i++)
				cash.Add(new Bill(allowedBills[rndm.Next(0, 4)]));
			return cash.ToArray();
		}
	}

	// Задание 3
	class Wallet<T> where T : Bill
	{
		public List<T> Cash { get; private set; }

		// Можно создать пустой кошелёк
		public Wallet()
		{
			Cash = new List<T>();
		}

		// А можно сразу чем-то заполнить, либо так
		public Wallet(params T[] cash)
		{
			// Выбросить своё исключение, если кошелёк будет переполнен
			if (cash.Length > 200)
				throw new WalletIsFullException("Wallet will be over crowded");

			Cash = new List<T>(cash);
		}

		// либо так, ниже написано в чём разница
		public Wallet(IList<T> cash)
		{
			if (Cash.Count > 200)
				throw new WalletIsFullException("Wallet will be over crowded");

			Cash = new List<T>(cash);
		}

		// Задание 3 - добавление
		// Добавление по штучно или массивом, типо Add(new Bill(5), new Bill(10), ...)
		public void Add(params T[] cash)
		{
			// Выбросить своё исключение, если кошелёк будет переполнен
			if (Cash.Count + cash.Length > 200)
				throw new WalletIsFullException("Wallet will be over crowded");

			Cash.AddRange(cash);
		}

		// Добавление добавление коллекцией-списком, типо Add(new List<Bill>() {new Bill(5), new Bill(10)}}
		public void Add(IList<T> cash)
		{
			if (Cash.Count + cash.Count > 200)
				throw new WalletIsFullException("Wallet will be over crowded");

			Cash.AddRange(cash);
		}

		// Задание 3 - удаление (минимального)
		public T Remove()
		{
			// Выбросить своё исключение, если кошелёк пуст
			if (Cash.Count == 0)
				throw new WalletIsEmptyException();

			// Сортировать по возростанию, взять первый (он же и минимальный)
			var min = Cash.OrderBy(item => item).First();
			Cash.Remove(min);
			return min;
		}

		// Задание 4 - вывод кол-ва по опред значениям Number
		public void DisplayBills()
		{
			var bills = Cash.GroupBy(item => item.Number).OrderBy(item => item.Key);
			foreach (var bill in bills)
				Console.WriteLine($"Bill {bill.Key}: {bill.Count()}");
		}

		// Задание 5 - сериализовать wallet в json
		public static void Serialize(Wallet<T> wallet, string file)
		{
			var json = JsonSerializer.Serialize(wallet);
			using var sw = new StreamWriter(file);
			sw.Write(json);
		}

		// Задание 3 - свои классы ошибок
		// На самом деле, хоть тут много чего написано, но это можно легко сгенерировать введя
		// "Exception" и нажав Tab 1-2 раза, останется только имя исключения поменять
		// (либо сразу, либо выделив MyException и нажав ctrl+r ctrl+r, либо вручную)

		[Serializable]
		public class MyException : Exception
		{
			public MyException() { }
			public MyException(string message) : base(message) { }
			public MyException(string message, Exception inner) : base(message, inner) { }
			protected MyException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
		}
		[Serializable]
		public class WalletIsFullException : Exception
		{
			public WalletIsFullException() { }
			public WalletIsFullException(string message) : base(message) { }
			public WalletIsFullException(string message, Exception inner) : base(message, inner) { }
			protected WalletIsFullException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
		}

		[Serializable]
		public class WalletIsEmptyException : Exception
		{
			public WalletIsEmptyException() { }
			public WalletIsEmptyException(string message) : base(message) { }
			public WalletIsEmptyException(string message, Exception inner) : base(message, inner) { }
			protected WalletIsEmptyException(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
		}
	}
}
