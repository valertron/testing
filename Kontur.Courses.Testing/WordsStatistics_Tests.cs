using System;
using System.Collections.Generic;
using System.Threading;
using Kontur.Courses.Testing.Implementations;
using NUnit.Framework;

namespace Kontur.Courses.Testing
{
	public class WordsStatistics_Tests
	{
		public Func<IWordsStatistics> createStat = () => new WordsStatistics_CorrectImplementation();
			// меняется на разные реализации при запуске exe

		public IWordsStatistics stat;

		[SetUp]
		public void SetUp()
		{
			stat = createStat();
		}

		[Test]
		public void no_stats_if_no_words()
		{
			CollectionAssert.IsEmpty(stat.GetStatistics());
		}

		public void same_word_twice()
		{
			stat.AddWord("xxx");
			stat.AddWord("xxx");
			CollectionAssert.AreEqual(new[] {Tuple.Create(2, "xxx")}, stat.GetStatistics());
		}

		[Test]
		public void single_word()
		{
			stat.AddWord("hello");
			CollectionAssert.AreEqual(new[] {Tuple.Create(1, "hello")}, stat.GetStatistics());
		}

		[Test]
		public void two_same_words_one_other()
		{
			stat.AddWord("hello");
			stat.AddWord("world");
			stat.AddWord("world");
			CollectionAssert.AreEqual(new[] {Tuple.Create(2, "world"), Tuple.Create(1, "hello")}, stat.GetStatistics());
		}

		[Test]
		public void length_test()
		{
			stat.AddWord("hello1234511");
			CollectionAssert.AreEqual(new[] { Tuple.Create(1, "hello12345") }, stat.GetStatistics());
		}

		[Test]
		public void upper_case()
		{
			stat.AddWord("HELLO");
			CollectionAssert.AreEqual(new[] {Tuple.Create(1, "hello")}, stat.GetStatistics());
		}

		[Test]
		public void wrong_order()
		{
			stat.AddWord("bbbbb");
			stat.AddWord("bbbbb");
			stat.AddWord("aaaaa");
			stat.AddWord("aaaaa");
			CollectionAssert.AreEqual(new[] { Tuple.Create(2, "aaaaa"), Tuple.Create(2, "bbbbb") }, stat.GetStatistics());
		}
		[Test]
		public void wrong_test()
		{
			stat.AddWord("bbbbb");
			stat.AddWord("BBBBB");
			CollectionAssert.AreEqual(new[] { Tuple.Create(2, "bbbbb") }, stat.GetStatistics());
		}

		[Test]
		public void frequency_order()
		{
			stat.AddWord("ccccc");
			stat.AddWord("bbbbb");
			stat.AddWord("bbbbb");
			CollectionAssert.AreEqual(new[] { Tuple.Create(2, "bbbbb"), Tuple.Create(1, "ccccc") }, stat.GetStatistics());
		}
		[Test]
		public void null_wrong()
		{
			stat.AddWord(null);
		}

		[Test]
		public void empty_string()
		{
			stat.AddWord("");
			stat.AddWord("111");
			CollectionAssert.AreEqual(new[] { Tuple.Create(1, "111") }, stat.GetStatistics());
		}
		[Test]
		public void elleven_char()
		{
			stat.AddWord("asdfghjklo1");
			stat.AddWord("asdfghjklo2");
			CollectionAssert.AreEqual(new[] { Tuple.Create(2, "asdfghjklo") }, stat.GetStatistics());
		}

		[Test]
		public void order_freq_wrong_test()
		{
			stat.AddWord("ccccc");
			stat.AddWord("ccccc");
			stat.AddWord("bbbbb");
			CollectionAssert.AreEqual(new[] { Tuple.Create(2, "ccccc"), Tuple.Create(1, "bbbbb") }, stat.GetStatistics());
		}

		[Test, Timeout(200)]
		public void highload_test()
		{
			for (int i = 0; i < 10000; i++)
			{
				stat.AddWord(i.ToString());
			}
		}
	}
}