using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using NUnit.Framework;

namespace TableParser;

[TestFixture]
public class FieldParserTaskTests
{
    [TestCase("text", new[] { "text" })]
    [TestCase("hello world", new[] { "hello", "world" })]
    [TestCase("hello  world", new[] { "hello", "world" })]
    [TestCase("", new string[0])]
    [TestCase("\"\"", new string[] { "" })]
    [TestCase(" hello", new[] { "hello" })]
    [TestCase("\" \"", new[] { " " })]
    [TestCase("hello \'a world b\'", new[] { "hello", "a world b" })]
    [TestCase("\'hello\' world", new[] { "hello", "world" })]
    [TestCase("\'hello", new[] { "hello" })]
    [TestCase("\"hello \'my world\'\"", new[] { "hello \'my world\'" })]
    [TestCase("\'hello \"my world\"\'", new[] { "hello \"my world\"" })]
    [TestCase("\'hello ", new[] { "hello " })]
    [TestCase("a\"b\"f", new[] { "a", "b", "f" })]
    [TestCase("'\\\'a\\\''", new[] { "\'a\'" })]
    [TestCase("\"\\\"a\\\"\"", new[] { "\"a\"" })]
    [TestCase("'\\\\'", new[] { "\\" })]
    [TestCase("\\slash\\_in_simple_field_is_just_slash\\",new[] {"\\slash\\_in_simple_field_is_just_slash\\"})]
    [TestCase("a\"b c d e\"f", new[] { "a", "b c d e", "f" })]
    [TestCase(" d ", new[] { "d" })]
    public static void Test(string input, string[] expectedResult)
	{
		var actualResult = FieldsParserTask.ParseLine(input);
		Assert.AreEqual(expectedResult.Length, actualResult.Count);
		for (int i = 0; i < expectedResult.Length; ++i)
		{
			Assert.AreEqual(expectedResult[i], actualResult[i].Value);
		}
	}
}

public class FieldsParserTask
{
	public static List<Token> ParseLine(string line)
	{
		if (string.IsNullOrEmpty(line))
			return new List<Token>();

        var tokenList = new List<Token>();
        var counter = 0;

        while(counter < line.Length)
        {
            if (line[counter] == ' ')
                counter++;
            else
            {
                Token token;
                if (line[counter] == '\'' || line[counter] == '\"')
                    token = QuotedFieldTask.ReadQuotedField(line, counter);
                else
                    token = ReadField(line, counter);

                tokenList.Add(token);
                counter = token.GetIndexNextToToken();
            }
        }
        return tokenList;
	}
        
	private static Token ReadField(string line, int startIndex)
	{
        var counter = startIndex;
        var sb = new StringBuilder();

        while (counter < line.Length)
        {
            var cur_char = line[counter];

            if (cur_char == ' ' || cur_char == '\'' || cur_char == '\"')
            {
                break;
            }
            else
            {
                sb.Append(cur_char);
                counter++;
            }         
        }
        return new Token(sb.ToString(), startIndex, counter - startIndex);
    }

	public static Token ReadQuotedField(string line, int startIndex)
	{
		return QuotedFieldTask.ReadQuotedField(line, startIndex);
	}
}