using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam
{
	static class ExamCard_Student
	{
		public static void Test()
		{
			var list = new ExamCard<List<Student>, Student>();

			var igosha = new Student("Игоша", 7, "ООП");
			// Можно так
			((IAction<Student>)list).Add(igosha);
			// А можно так
			(list as IAction<Student>).Add(new Student("Влад", 8, "ООП"));
			(list as IAction<Student>).Add(new Student("Ангелина", 8, "ООП"));
			(list as IAction<Student>).Add(new Student("Алибек", 3, "ООП"));

			// Возможный вопрос: почему так не удобно? Почему не просто list.Add()
			// Потому что интерфейс в классе реализован ЯВНО

			(list as IAction<Student>).Print();

			(list as IAction<Student>).Remove(igosha);
			// Можно ещё так удалять
			//	(list as IAction<Student>).Remove(list.Data[index]);
			Console.WriteLine("\nПосле удаления");
			(list as IAction<Student>).Print();

			Console.WriteLine("\n" + list.Data[0]);
			Console.WriteLine($"Отметка повышена на {list.Beg(list.Data[0])}\nНовая отметка: {list.Data[0]}");

			(list as IAction<Student>).Clear();
			Console.WriteLine("\nПосле очистки");
			(list as IAction<Student>).Print();

		}

		// Задание 1 - интерфейс и класс
		// Здесь V - тип элемента в коллекции
		public interface IAction<V>
		{
			public void Add(V item);
			public void Remove(V item);
			public void Clear();
			public void Print();
		}

		// T - любая обобщённая коллекция (ICollection<>)
		// V - тип элемента в коллекции
		// new() - ограничение на конструктор
		public class ExamCard<T, V> : IAction<V> where T : ICollection<V>, new()
		{
			public T Data { get; private set; }

			public ExamCard()
			{
				// Нельзя так сделать без ограничения new()
				Data = new T();
			}

			// ЯВНО реализованный интерфейс
			void IAction<V>.Add(V item)
			{
				Data.Add(item);
			}

			void IAction<V>.Remove(V item)
			{
				if (Data.Count == 0)
					throw new NullSizeCollctionException("Нельзя ничего удалить из пустой коллекции");

				Data.Remove(item);
			}

			void IAction<V>.Clear()
			{
				if (Data.Count == 0)
					throw new NullSizeCollctionException("Коллекция уже пуста");

				Data.Clear();
			}

			void IAction<V>.Print()
			{
				if (Data.Count() == 0)
					Console.WriteLine("Пусто");
				else
					foreach (var item in Data)
						Console.WriteLine(item);
			}

			// Вот такая вот реализация уже неявная
			//	void Add(T item)
			//	{
			//		Data.Add(item);
			//	}

			// Задание 2 - исключения
			[Serializable]
			public class NullSizeCollctionException : Exception
			{
				public NullSizeCollctionException() { }
				public NullSizeCollctionException(string message) : base(message) { }
				public NullSizeCollctionException(string message, Exception inner) : base(message, inner) { }
				protected NullSizeCollctionException(
				  System.Runtime.Serialization.SerializationInfo info,
				  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
			}

			// Задание 4 - LINQ
			public static int GetNumberOfPassedStudents(ExamCard<List<Student>, Student> card)
			{
				return card.Data.Where(student => student.Mark >= 4).Count();
			}

			public static double GetAvgMark(ExamCard<List<Student>, Student> card)
			{
				return card.Data.Average(student => student.Mark);
			}
		}
	}

	// Задание 5 - метод расширения (создаётся в новом статическом классе)
	static class ExamCardExtension
	{
		public static int Beg(this ExamCard_Student.ExamCard<List<Student>, Student> card, Student student)
		{
			// Поиск индекста элемента в списке
			// Предикат в FindIndex: поиск сравнением заданного элемента student с каждым элементом списка item
			var index = card.Data.FindIndex(item => item == student);
			if (index == -1)
				throw new Exception("Студент не найден");

			// Хоть тут написано от 1 до 4, но возвращать оно будет 1, 2, 3, т.е. [1, 4)
			int rndm = new Random().Next(1, 4);
			if (card.Data[index].Mark + rndm > 10)
			{
				int t = 10 - card.Data[0].Mark;
				card.Data[index].Mark = 10;
				return t;
			}
			else
			{
				card.Data[index].Mark += rndm;
				return rndm;
			}
		}
	}

	// Задание 3
	class Student
	{
		public string Name { get; }
		public int Mark { get; set; }
		public string Subject { get; }

		public Student(string name, int mark, string subject)
		{
			Name = name;
			Mark = mark;
			Subject = subject;
		}

		public override string ToString()
		{
			return $"{Name} - {Mark} по {Subject}";
		}
	}
}