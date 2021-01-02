using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Exam
{
	static class C2_6
	{
		public static void Test()
		{
			var list = new List<User>()
			{
				new User("somemail@gmail.com", "abc123"),
				new User("somemail@mail.com", "abc123", Status.signin),
				new User("mymail@gmail.com", "123abc", Status.signin),
				new User("mail@gmail.com", "abc123"),
				new User("mail@mail.com", "abc123")
			};

			Compare(list[0], list[0]);
			Compare(list[0], list[1]);
			Compare(list[0], list[2]);
			Compare(list[0], list[4]);
			Compare(list[2], list[3]);
			Compare(list[3], list[3]);
			Compare(list[3], list[4]);

			var github = new WebNet(list);

			Console.WriteLine("Пользователей онлайн: " + github.UsersOnline());

			Console.WriteLine();
			Console.WriteLine(list[0].Serialize());
			Console.WriteLine();
			Console.WriteLine(User.StaticSerialize(github.Users));

			static void Compare(User user1, User user2)
			{
				Console.WriteLine($"{user1} ? {user2}");
				if (user1.CompareTo(user2) == 0)
					Console.WriteLine("Это один пользователь\n");
				else
					Console.WriteLine("Это разные пользователи\n");
			}
		}

		// Задание 1
		[Serializable]
		class User : IComparable
		{
			string email;
			string password;
			Status status;

			// Для вида сериализации
			public string Email { get => email; }
			public string Password { get => password; }
			public string Status { get => status.ToString(); }

			public User(string email, string password, Status status = C2_6.Status.signout)
			{
				this.email = email;
				this.password = password;
				this.status = status;
			}

			// Пригодится в 4 задании
			public bool IsOnline() => status == C2_6.Status.signin;

			// Задание 2
			public override bool Equals(object obj)
			{
				return base.Equals(obj);
			}

			public override int GetHashCode()
			{
				return (email + password).GetHashCode();
			}

			public override string ToString()
			{
				return $"{email} ({status})";
			}

			// От интерфейса IComparable
			public int CompareTo(object obj)
			{
				// Проверить является ли вообще объект пользователем, чтобы достать оттуда почту
				if (!(obj is User))
					return -1;
				return email.CompareTo((obj as User).email);
			}

			// Задание 5 - сериализация
			// Сериализовать 1 шт, вызовом метода экземпляра
			public string Serialize()
			{
				return JsonSerializer.Serialize(this);
			}

			// Сериализовать несколько штук, закинув какой-нибудь список в стат метод класса
			public static string StaticSerialize(IEnumerable<User> users)
			{
				return JsonSerializer.Serialize(users);
			}
		}

		// К заданию 1
		enum Status
		{
			signin,
			signout
		}

		// Задание 3
		class WebNet
		{
			public LinkedList<User> Users { get; private set; }

			public WebNet()
			{
				Users = new LinkedList<User>();
			}

			public WebNet(IList<User> users)
			{
				Users = new LinkedList<User>(users);
			}

			public bool Add(User user)
			{
				if (!Users.Contains(user))
				{
					Users.AddLast(user);
					return true;
				}
				return false;
			}

			public bool Remove(User user)
			{
				return Users.Remove(user);
			}

			// Задание 4 - LINQ
			public int UsersOnline()
			{
				return Users.Where(user => user.IsOnline()).Count();
			}
		}
	}
}
