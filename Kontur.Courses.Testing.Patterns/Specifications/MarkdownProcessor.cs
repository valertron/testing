using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Kontur.Courses.Testing.Patterns.Specifications
{
	public class MarkdownProcessor
	{
		public string Render(string input)
		{
			if (input==null)
				throw new ArgumentException();

			var emReplacer = new Regex(@"([^\w\\]|^)_(.*?[^\\])_(\W|$)");
			var strongReplacer = new Regex(@"([^\w\\]|^)__(.*?[^\\])__(\W|$)");
			input = strongReplacer.Replace(input,
					match => match.Groups[1].Value +
							"<strong>" + match.Groups[2].Value + "</strong>" +
							match.Groups[3].Value);
			input = emReplacer.Replace(input,
					match => match.Groups[1].Value +
							"<em>" + match.Groups[2].Value + "</em>" +
							match.Groups[3].Value);
			input = input.Replace(@"\_", "_");
			return input;
		}
	}

	[TestFixture]
	public class MarkdownProcessor_should
	{
		private readonly MarkdownProcessor md = new MarkdownProcessor();

		[Test]
		public void pass_with_no_mark()
		{
			var s = "text with no marks";
			Assert.AreEqual(s, md.Render(s));
		}

		[Test]
		public void pass_with_empty()
		{
			Assert.AreEqual("", md.Render(""));
		}

		[Test]
		public void pass_with_null()
		{
			Assert.Throws<ArgumentException>(() => md.Render(null));
		}

		[TestCase("_a a_", Result = "<em>a a</em>", TestName = "Em_without_enviroment")]
		[TestCase("cc _a a_", Result = "cc <em>a a</em>", TestName = "Em_with_Left_enviroment")]
		[TestCase("_a a_ cc", Result = "<em>a a</em> cc", TestName = "Em_with_Right_enviroment")]
		[TestCase("cc _a a_ cc", Result = "cc <em>a a</em> cc", TestName = "Em_with_enviroment")]
		[TestCase("__bb", Result = "__bb", TestName = "Not_Matched_Strong")]
		[TestCase("_bb", Result = "_bb", TestName = "Not_Matched_Em")]
		public string surroundWithEm_textInsideUnderScores(string value)
		{
			return md.Render(value);
		}

		[TestCase(@"\_bb\_", Result = "_bb_", TestName = "screening")]
		public string support(string value)
		{
			return md.Render(value);
		}

		[TestCase("cc_a_cc__bb__bb", Result = "cc_a_cc__bb__bb", TestName = "Dont_Em&Strong")]
		public string notMark_Inside_Internal(string value)
		{
			return md.Render(value);
		}

		[TestCase("__a a__", Result = "<strong>a a</strong>", TestName = "Strong_without_enviroment")]
		[TestCase("cc __a a__", Result = "cc <strong>a a</strong>", TestName = "Strong_with_Left_enviroment")]
		[TestCase("__a a__ cc", Result = "<strong>a a</strong> cc", TestName = "Strong_with_Right_enviroment")]
		[TestCase("cc __a a__ cc", Result = "cc <strong>a a</strong> cc", TestName = "Strong_with_enviroment")]
		public string surroundWithStrong_textInsideUnderScores(string value)
		{
			return md.Render(value);
		}

		[TestCase("_cc __a a__ cc_", Result = "<em>cc <strong>a a</strong> cc</em>", TestName = "Strong_into_Em")]
		[TestCase("__cc _a a_ cc__", Result = "<strong>cc <em>a a</em> cc</strong>", TestName = "Em_into_Strong")]
		public string mark_textWithInvestment(string value)
		{
			return md.Render(value);
		}
		
		
	}
}
