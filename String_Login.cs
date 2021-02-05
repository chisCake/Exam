using System;
using System.Text;

namespace Exam
{
	static class String_Login
	{
		public static void Test()
		{
			var login = new Login("mylogin", "mypassword");
			Console.WriteLine(login);
			login.SetLogin("mysimplelogin");
			Console.WriteLine(login);
			Console.WriteLine("Валидный ли пароль: " + ((login as IValidate).Validate() ? "да" : "нет"));
			login.SetPassword("myverystrongpassword");
			Console.WriteLine(login);
			Console.WriteLine("Валидный ли пароль: " + ((login as IValidate).Validate() ? "да" : "нет"));

			Console.WriteLine();

			var sb = new StringBuilder("123");
			Console.WriteLine(sb);
			
		}

		class MyString
		{
			public StringBuilder Builder { get; private set; }

			public MyString(string str)
			{
				Builder = new StringBuilder(str);
			}

			public void SetString(string newStr)
			{
				Builder = new StringBuilder(newStr);
			}

			public override string ToString()
			{
				return Builder.ToString();
			}
		}

		// Задание 3
		class MyBox : MyString
		{
			string color;
			(int width, int length, int height) size;

			// Конструктор, который вызовет конструктор в базовом классе (public MyString(string str))
			public MyBox(string str) : base(str)
			{
				color = "белый";
				size = (10, 10, 10);
			}

			// Изменения содержимого
			public void ChangeData(string newStr)
			{
				// Вызов метода из предка
				// Вообще его можно вызывать сразу, а не обращаться к ChangeData
				SetString(newStr);
			}
		}

		interface IValidate
		{
			bool Validate();
		}

		class Login : IValidate
		{
			public MyBox login;
			public MyBox password;

			public Login(string login, string password)
			{
				this.login = new MyBox(login);
				this.password = new MyBox(password);
			}

			public void SetLogin(string newLogin)
			{
				login.ChangeData(newLogin);
				// Можно использовать метод MyString, а не MyBox
				// login.SetString(newLogin);
			}

			public void SetPassword(string newPassword)
			{
				password.ChangeData(newPassword);
			}

			// Задание 4
			// Явно реализованный интерфейс
			bool IValidate.Validate()
			{
				// Задание 5
				// Тут надо было сделать зачем-то try catch, сделал как понял
				try
				{
					if (password.ToString().Length > 6 &&
						password.ToString().Length < 12 &&
						password.ToString() != login.ToString())
						return true;
					else
					{
						throw new Exception("Не верный пароль");
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
					return false;
				}
			}

			public override string ToString()
			{
				return $"{login}: {password}";
			}
		}
	}
}
