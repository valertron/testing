using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Kontur.Courses.Testing.Patterns.AAA
{
	[TestFixture]
	public class Zip_tests
	{
		[Test]
		public void TestZipEqualSizeArrays()
		{
			var arr1 = new[] { 1 };
			var arr2 = new[] { 2 };

			var result = arr1.Zip(arr2, Tuple.Create).ToArray();

			Assert.AreEqual(new[] { Tuple.Create(1, 2) }, result);
		}

		[Test]
		public void TestBothEmpty()
		{
			var arr1 = new int[0];
			var arr2 = new int[0];

			var result = arr1.Zip(arr2, Tuple.Create).ToArray();

			CollectionAssert.IsEmpty(result);
		}

		[Test]
		public void TestFirstIsEmpty()
		{
			var arr1 = new int[0];
			var arr2 = new[] { 1, 2 };

			var result = arr1.Zip(arr2, Tuple.Create).ToArray();

			CollectionAssert.IsEmpty(result);
		}

		[Test]
		public void TestSecondIsEmpty()
		{
			var arr1 = new[] { 1, 2 };
			var arr2 = new int[0];

			var result = arr1.Zip(arr2, Tuple.Create).ToArray();

			CollectionAssert.IsEmpty(result);
		}

		[Test]
		public void TestFirstIsInfinit()
		{
			var arr2 = new[] { 1, 2 };

			var result = Infinite().Zip(arr2, Tuple.Create).ToArray();

			Assert.AreEqual(new[] { Tuple.Create(42, 1), Tuple.Create(42, 2) }, result);
		}

		[Test]
		public void TestSecondIsInfinit()
		{
			var arr1 = new[] { 1, 2 };

			var result = arr1.Zip(Infinite(), Tuple.Create).ToArray();

			Assert.AreEqual(new[] { Tuple.Create(1, 42), Tuple.Create(2, 42) }, result);
		}

		private IEnumerable<int> Infinite()
		{
			while (true)
				yield return 42;
			// ReSharper disable once FunctionNeverReturns
		}

	}
}