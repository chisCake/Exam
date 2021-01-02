using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Exam
{
	static class C8_1
	{
		public static void Test()
		{
			var shop1 = new Shop(new List<Item>()
			{
				new Item("Ваза", "100A", 5.00),
				new Item("Телевизор", "250CF", 250.00),
				new Item("Холодильник", "260A", 200.00),
				new Item("Стол", "55F", 20.00),
				new Item("Стол", "54F", 25.00)
			});

			var shop2 = new Shop(new List<Item>()
			{
				new Item("Ваза", "100A", 5.00),
				new Item("Телевизор", "250CF", 250.00)
			});

			Console.WriteLine("shop1 = shop1: " + (shop1.Equals(shop1) ? "да" : "нет"));
			Console.WriteLine("shop1 = shop2: " + (shop1.Equals(shop2) ? "да" : "нет"));

			Console.WriteLine("\nВещи в shop1:");
			Print(shop1);

			shop1 += new Item("Телефон", "1020", 150.50);
			Console.WriteLine("\nshop1 после добавления нового элемента:");
			Print(shop1);

			shop1 -= new Item("Телевизор", "250CF", 255.00);
			Console.WriteLine("\nshop1 после удаления элемента:");
			Print(shop1);

			var manager = new Manager();
			int counter = 0;
			Console.WriteLine();
			foreach (var item in shop1)
				if (counter++ % 2 == 0)
				{
					manager.Sale += item.ReducePrice;
					Console.WriteLine($"Предмет {item} подписан на распродажи");
				}
			manager.StartSales();
			Console.WriteLine("\nshop1 после начала распродажи");
			Print(shop1);

			static void Print(Shop shop)
			{
				foreach (var item in shop)
					Console.WriteLine(item);
			}

			Console.WriteLine($"\nСумма цен всех столов в shop1: {shop1.PriceSum("Стол"):f2}");
		}

		class Item
		{
			public string Name { get; private set; }
			public string Id { get; private set; }
			public double Price { get; private set; }

			public Item(string name, string id, double price)
			{
				Name = name;
				Id = id;
				Price = price;
			}

			public void ReducePrice()
			{
				Price *= 0.5;
			}

			public override string ToString()
			{
				return $"{Name} {Id} {Price:f2}";
			}
		}

		class Shop : IEnumerable<Item>
		{
			public Queue<Item> Items { get; private set; }

			public Shop()
			{
				Items = new Queue<Item>();
			}

			public Shop(IEnumerable<Item> items)
			{
				Items = new Queue<Item>(items);
			}

			public void Add(Item item)
			{
				Items.Enqueue(item);
			}

			public Item Remove()
			{
				return Items.Dequeue();
			}

			public void Clear()
			{
				Items.Clear();
			}

			// Задание 2
			public override int GetHashCode()
			{
				return Items.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				if (!(obj is Shop))
					return false;
				return Items.Equals((obj as Shop).Items);
			}

			public IEnumerator<Item> GetEnumerator()
			{
				return Items.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return Items.GetEnumerator();
			}

			// Задание 3 - операторы
			public static Shop operator +(Shop shop, Item item)
			{
				shop.Add(item);
				return new Shop(shop.Items);
			}

			public static Shop operator -(Shop shop, Item item)
			{
				var list = new List<Item>();
				foreach (var elem in shop)
					if (elem.Id != item.Id)
						list.Add(elem);
				return new Shop(list);
			}

			// Задание 5
			public double PriceSum(string itemName)
			{
				return Items.Where(item => item.Name == itemName).Sum(item => item.Price);
			}
		}

		// Задание 4
		class Manager
		{
			public event Action Sale;

			public void StartSales()
			{
				Sale.Invoke();
			}
		}
	}
}
