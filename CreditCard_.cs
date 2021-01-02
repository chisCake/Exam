using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam
{
	static class CreditCard_
	{
		public static void Test()
		{
			var list = new List<CreditCard>()
			{
				new CreditCard("1111 2222 3333 4400", 1111, 2222, 1200),
				new CreditCard("1100 2200 3300 4400", 1111, 2222, 1600),
				new CreditCard("1111 2222 3333 4444", 1111, 2222, 1400),
				new CreditCard("2222 3333 4444 0000", 1111, 2222, 800),
			};

			Console.WriteLine("Просмотр баланса на карте " + list[0]);
			Console.WriteLine(list[0].GetBalance());
			list[0].Add(1000);
			Console.WriteLine("Баланс после пополнения карты " + list[0]);
			Console.WriteLine(list[0].GetBalance());

			try
			{
				list[3].Get(5000);
			}
			catch (Exception e)
			{
				Console.WriteLine("\n" + e.Message);
			}

			Console.WriteLine("\nСумма балансов карт: " + CreditCard.GetBalanceSum(list));
		}

		interface ICard
		{
			public void Add(double money);
			public void Get(double money);
		}

		class CreditCard : ICard
		{
			public string Number { get; }
			double balance;
			readonly int pin1;
			readonly int pin2;

			public CreditCard(string cardNumber, int pin1, int pin2, double balance = 0)
			{
				Number = cardNumber;
				this.pin1 = pin1;
				this.pin2 = pin2;
				this.balance = balance;
			}

			public double GetBalance()
			{
				int counter = 0;
				bool success = false;

				do
				{
					Console.Write("Введите pin1: ");
					if (int.TryParse(Console.ReadLine(), out int pin) && pin == pin1)
						success = true;
					else
						Console.WriteLine("Неверно введён pin, вы ввели: " + pin);
					counter++;
				} while (counter < 3 && !success);

				if (success)
					return balance;
				else
				{
					do
					{
						Console.Write("Введите pin2: ");
						if (int.TryParse(Console.ReadLine(), out int pin) && pin == pin2)
							success = true;
						else
							Console.WriteLine("Неверно введён pin, вы ввели: " + pin);
					} while (!success);
					return balance;
				}
			}

			public void Add(double money)
			{
				balance += money;
			}

			public void Get(double money)
			{
				if (balance < money)
					throw new Exception("На карте недостаточно средств");

				balance -= money;
			}

			// Не уверен что тут сделано то что надо, ибо чел задание немного непонятно описал
			public static double GetBalanceSum(List<CreditCard> cards)
			{
				return cards
					.Where(card => 
						card.balance > 1000 && 
						card.Number.Contains('0') && 
						card.Number.Contains('2'))
					.Sum(card => card.balance);
			}

			public override string ToString()
			{
				return Number;
			}
		}
	}
}
