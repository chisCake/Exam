using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Exam
{
	static class Transport_Air
	{
		public static void Test()
		{
			// Задание 3 - вывод в файл (чёт я не нашёл как сделать "ещё и в файл", без костылей никак)
			using var sw = new StreamWriter("Console.txt");
			//Console.SetOut(sw);

			var plane = new Air(100);
			Console.WriteLine(plane);
			(plane as IAirable).Check();
			Console.WriteLine("После проверки:\n" + plane);
			plane.Speed = 800;
			(plane as IAirable).Check();
			Console.WriteLine("После изменения скорости и проверки:\n" + plane);
			plane.Fly();
			plane.Speed = 0;
			plane.CountOfPassengers = 0;
			(plane as IAirable).Check();
			Console.WriteLine("После изменения скорости, кол-ва пассажиров и проверки:\n" + plane);
			try
			{
				plane.Fly();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			Console.WriteLine();
			(plane as IAir).Check();
			plane.CountOfPassengers = 80;
			Console.WriteLine("Число пассажиров изменено\n" + plane);
			(plane as IAir).Check();

			var list = new List<Air>()
			{
				new Air(60, 700, Status.fly),
				new Air(150, 1000, Status.fly),
				new Air(60, 0, Status.ready),
				new Air(120, 900, Status.fly),
				new Air(0, 0, Status.stop),
			};

			Console.WriteLine("\nСписок самолётов");
			foreach (var item in list)
				Console.WriteLine(item);
			Console.WriteLine("Кол-во самолётов в воздухе: " + Air.InAir(list));
			Console.WriteLine($"Средняя скорость самолётов в воздухе: {Air.AvgSpeed(list):f2}");
		}

		abstract class Transport
		{
			protected string Type { get; }

			public Transport(string type)
			{
				Type = type;
			}
		}

		interface IAirable
		{
			public void Check();
			public void Fly();
		}

		// Задание 4
		interface IAir
		{
			void Check();
		}

		class Air : Transport, IAirable, IAir
		{
			public int CountOfPassengers { get; set; }
			public int Speed { get; set; }
			public Status Status { get; private set; }

			public Air(int countOfPassengers, int speed = 0, Status status = Status.ready, string type = "airliner") : base(type)
			{
				CountOfPassengers = countOfPassengers;
				Speed = speed;
				Status = status;
			}

			// Интерфейс IAirable, явно
			void IAirable.Check()
			{
				if (CountOfPassengers == 0 && Speed == 0)
					Status = Status.stop;
				else if (CountOfPassengers > 0 && Speed == 0)
					Status = Status.ready;
				else
					Status = Status.fly;
			}

			// Неявно
			public void Fly()
			{
				if (Status == Status.fly)
					Console.WriteLine("Flying");
				else
					throw new Exception("Not flying");
			}

			// Задание 4 - интерфейс IAir
			void IAir.Check()
			{
				if (CountOfPassengers > 20 && CountOfPassengers < 100)
					Console.WriteLine("Ready");
				else
					Console.WriteLine("Not ready");
			}

			// Задание 5
			public static int InAir(IList<Air> planes)
			{
				return planes.Where(plane => plane.Status == Status.fly).Count();
			}

			public static double AvgSpeed(IList<Air> planes)
			{
				return planes.Where(plane => plane.Status == Status.fly).Average(plane => plane.Speed);
			}

			public override string ToString()
			{
				return $"{Type}, {CountOfPassengers} пассажиров, {Speed}м/с, {Status}";
			}
		}

		enum Status
		{
			fly,
			ready,
			stop
		}
	}
}