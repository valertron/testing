using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Kontur.Courses.Testing.Patterns.Antipatterns
{
	public class Stack_tests2
	{
		[Test]
		public void Test()
		{
			//var lines = File.ReadAllLines(@"d:\users\vasya\Antipatterns\data.txt")
			var lines = File.ReadAllLines(@"data.txt")
				.Select(line => line.Split(' '))
				.Select(line => new { push = line[0] == "push", value = line[1] });

			var stack = new Stack<string>();
			foreach (var line in lines)
			{
				if (line.push)
					stack.Push(line.value);
				else
					Assert.AreEqual(line.value, stack.Pop());
			}
		}
		#region Почему это плохо?
		/*
		## Проблема

		Тест не будет работать на машине другого человека или на Build-сервере. 
		Да и у того же самого человека после Clean Solution / переустановки ОС / повторного Clone репозитория / ...

		## Мораль

		Тест не должен зависеть от особенностей локальной среды.
		*/
		#endregion
	}
}
