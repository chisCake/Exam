using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Exam
{
	static class C3_2
	{
		public static void Test()
		{
			var uber = new Park<Taxi>(
				new Taxi("1920AB-7", new Location(75.3432, 12.2353, 0)),
				new Taxi("1432AB-7", new Location(72.1234, 16.1233, 0)),
				new Taxi("4331AB-7", new Location(71.3243, 15.6543, 0)),
				new Taxi("3214AB-7", new Location(77.3123, 14.2123, 0)));

			Print(uber);

			var taxi = new Taxi("1234AB-7", new Location(10.00, 20.00));
			uber.Add(taxi);
			Console.WriteLine("Машина добавлена");
			Print(uber);
			Console.WriteLine("Машина удалена");
			uber.Remove(taxi);
			Print(uber);
			Console.WriteLine();

			Console.WriteLine("Найденная машина с номером 1432AB-7:");
			Console.WriteLine(uber.Find(car => car.Number == "1432AB-7") + "\n");

			double lat = 75.0, _long = 12.0;
			while (true)
			{
				Console.Write("Введите ширину: ");
				if (!double.TryParse(Console.ReadLine(), out lat))
				{
					Console.WriteLine("Неверное введено значение ширины");
					continue;
				}
				Console.Write("Введите долготу: ");
				if (!double.TryParse(Console.ReadLine(), out _long))
				{
					Console.WriteLine("Неверное введено значение долготы");
					continue;
				}
				else
					break;
			}

			uber.Sort(lat, _long);

			Console.WriteLine("После сортировки:");
			Print(uber);

			uber.Cars[0].WriteToFile("car.json");
			Console.WriteLine("\nИнформация о ближайшей машине записана в файл");

			static void Print(Park<Taxi> park)
			{
				foreach (var item in park.Cars)
					Console.WriteLine(item);
			}
		}

		class Location
		{
			public double Lat { get; private set; }
			public double Long { get; private set; }
			public double Speed { get; private set; }

			public Location(double lat, double _long, double speed = 0)
			{
				Lat = lat;
				Long = _long;
				Speed = speed;
			}
		}

		// Для задания 4
		interface ICar
		{
			public Location Location { get; }
		}

		// Задание 2
		class Taxi : ICar
		{
			public string Number { get; private set; }
			public Location Location { get; private set; }
			public Status Status { get; private set; }

			public Taxi(string number, Location location = null, Status status = Status.free)
			{
				Number = number;
				Location = location;
				Status = status;
			}

			// Задание 5
			public void WriteToFile(string file)
			{
				using var sw = new StreamWriter(file);
				sw.Write(JsonSerializer.Serialize(this));
			}

			public override string ToString()
			{
				return $"{Number}: {Location.Lat} {Location.Long}";
			}
		}

		enum Status
		{
			busy,
			free
		}

		class Park<T> where T : ICar
		{
			public List<T> Cars { get; private set; }

			public Park()
			{
				Cars = new List<T>();
			}

			// Если уже есть какой-нибудь список
			public Park(IList<T> cars)
			{
				Cars = new List<T>(cars);
			}

			// Если списка нет
			public Park(params T[] cars)
			{
				Cars = new List<T>(cars);
			}

			public void Add(T car)
			{
				Cars.Add(car);
			}

			public void Remove(T car)
			{
				Cars.Remove(car);
			}

			public void Clear()
			{
				Cars.Clear();
			}

			public T Find(Predicate<T> predicate)
			{
				return Cars.Find(predicate);
			}

			// Задание 4
			public void Sort(double lat, double _long)
			{
				// Сравнение, предикат должен вернуть -1, 0, 1, что и возвращает CompareTo
				// Первый Distance - расстояние первой машины в сравении до заданной координаты
				// Второй Distance, в скобках - расстояние второй машины в сравнении до заданной координаты
				Cars.Sort((car1, car2) =>
				Distance(car1.Location.Lat, lat, car1.Location.Long, _long)
				.CompareTo(Distance(car2.Location.Lat, lat, car2.Location.Long, _long)));

				// Формула из справки
				static double Distance(double carX, double userX, double carY, double userY) =>
					Math.Sqrt(Math.Pow(userX - carX, 2) + Math.Pow(userY - carY, 2));
			}
		}
	}
}
