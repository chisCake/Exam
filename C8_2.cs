using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Exam
{
	static class C8_2
	{
		public static void Test()
		{
			var fit = new Fit(
				new Abiturient("Студент1", new Date(1, 1, 2000), 50, 70, 50, 80),
				new Abiturient("Студент2", new Date(2, 2, 2000), 50, 20, 40, 50),
				new Abiturient("Студент3", new Date(3, 3, 2001), 50, 20, 50, 20),
				new Abiturient("Студент4", new Date(4, 4, 2001), 20, 20, 20, 20),
				new Abiturient("Студент5", new Date(5, 5, 2000), 20, 20, 20, 20),
				new Abiturient("Студент6", new Date(6, 6, 2002), 30, 20, 60, 50)
				);

			Print(fit);

			// Встаёт без проблем
			fit.Add(new Abiturient("Студент7", new Date(7, 7, 2002), 100, 100, 100, 100));
			// Вытесняет худшего
			fit.Add(new Abiturient("Студент8", new Date(8, 8, 2002), 100, 100, 100, 100));
			// Вызывает CollisionException
			try
			{
				fit.Add(new Abiturient("Студент8", new Date(9, 9, 2002), 20, 20, 20, 20));
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			Console.WriteLine("\nПосле добавления и удаления");
			Print(fit);

			Console.WriteLine();
			Compare(fit[4], fit[5]);
			Compare(fit[5], fit[6]);

			Console.WriteLine($"\nСредний балл всех студентов: {fit.GetAvg():f2}");

			static void Print(Fit fit)
			{
				foreach (var item in fit.Abiturients)
					Console.WriteLine(item);
			}

			static void Compare(Abiturient abiturient1, Abiturient abiturient2)
			{
				var res = abiturient1.Equals(abiturient2) ? "равна" : "не равна";
				Console.WriteLine($"Сумма оценок {abiturient1.Name} {res} сумме оценок {abiturient2.Name}");
			}
		}

		public class Abiturient
		{
			const int MAX_MARKS_AMT = 4;
			const int LOW_MARK = 20;
			const int MAX_MARK = 100;

			public string Name { get; }
			public Date Birthday { get; }
			int[] _marks;
			public int[] Marks
			{
				get => _marks;
				set
				{
					// Вот это всё для проверки на границы от 20 до 100 и макс кол-во оценок (4)
					if (value.Length > MAX_MARKS_AMT)
						throw new ArgumentOutOfRangeException($"Возможное кол-во оценок: {MAX_MARKS_AMT}, переданное кол-во оценок: {value.Length}");
					foreach (var item in value)
						if (item < LOW_MARK || item > MAX_MARK)
							throw new ArgumentOutOfRangeException($"Допустимый предел оценки: [{LOW_MARK};{MAX_MARK}], передданое значение: {item}");

					_marks = value;
				}
			}

			public Abiturient(string name, Date birthday, params int[] marks)
			{
				Name = name;
				Birthday = birthday;
				Marks = marks;
			}

			public override string ToString()
			{
				return $"{Name}, {Birthday}, {Marks.Average()}";
			}

			// Задание 2
			public override bool Equals(object obj)
			{
				if (!(obj is Abiturient))
					return false;

				return Marks.Sum() == (obj as Abiturient).Marks.Sum();
			}

			public override int GetHashCode()
			{
				return (Name + Birthday).GetHashCode();
			}
		}

		public class Date
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

		// IEnumerable не обязательно
		public class Fit : IEnumerable
		{
			const int countStudent = 7;

			public List<Abiturient> Abiturients { get; private set; }

			// Необязательно
			public Abiturient this[int index]
			{
				get => Abiturients[index];
				private set => Abiturients[index] = value;
			}

			public Fit()
			{
				Abiturients = new List<Abiturient>();
			}

			public Fit(params Abiturient[] abiturients)
			{
				if (abiturients.Length > countStudent)
					throw new ArgumentOutOfRangeException($"Допустимое кол-во студентов: {countStudent}, переданное кол-во студентов: {abiturients.Length}");
				Abiturients = new List<Abiturient>(abiturients);
			}

			public bool Add(Abiturient abiturient)
			{
				// Задание 3-4
				// Сначала проверка на доступные места, если есть, то просто добавить
				if (Abiturients.Count < countStudent)
				{
					Abiturients.Add(abiturient);
					return true;
				}
				else
				{
					// Иначе взять худшего из имеющихся
					var worst = Abiturients.OrderBy(abiturient => abiturient.Marks.Sum()).First();
					// Если сумма оценок худшего меньше суммы оценок нового
					if (worst.Marks.Sum() < abiturient.Marks.Sum())
					{
						// То удалить худшего и добавить нового
						Abiturients.Remove(worst);
						Abiturients.Add(abiturient);
						return true;
					}
					else
					{
						// Иначе проверить, если их сумма оценок равна, то выбросить пользовательское исключение
						if (worst.Marks.Sum() == abiturient.Marks.Sum())
							throw new CollisionException("Сумма оценок нового студента равна сумме оценок худшего студента");
						else
							// Если сумма оценок не равна, то ничего не делать и просто вернуть false
							return false;
					}
				}
			}

			public bool Remove(Abiturient abiturient)
			{
				return Abiturients.Remove(abiturient);
			}

			// Необязательно
			public IEnumerator GetEnumerator()
			{
				return Abiturients.GetEnumerator();
			}

			// Задание 2 - оператор
			public static Fit operator --(Fit fit)
			{
				fit.Remove(
					fit.Abiturients
					// Отсортировать абитуриентов по сумме балллов
					.OrderBy(abiturient => abiturient.Marks.Sum())
					// Взять первого, он же самый худший, который пойдёт в Remove()
					.First());
				// Возвращается коллекция без худшего
				return fit;
			}

			// К заданию 3-4
			[Serializable]
			public class CollisionException : Exception
			{
				public CollisionException() { }
				public CollisionException(string message) : base(message) { }
				public CollisionException(string message, Exception inner) : base(message, inner) { }
				protected CollisionException(
				  System.Runtime.Serialization.SerializationInfo info,
				  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
			}
		}
	}

	// Задание 5
	static class FitExtension
	{
		public static double GetAvg(this C8_2.Fit fit)
		{
			return fit.Abiturients.Average(abiturient => abiturient.Marks.Average());
		}
	}
}
