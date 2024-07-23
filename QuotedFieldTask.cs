using NUnit.Framework;
using System.Text;

namespace TableParser;

[TestFixture]
public class QuotedFieldTaskTests
{
    [TestCase("''", 0, "", 2)]
    [TestCase("'a'", 0, "a", 3)]
    [TestCase(@"'a\' b'", 0, "a' b", 7)]
    [TestCase(@"'a\' b\'", 0, "a' b'", 8)]
    [TestCase(@"'a\' b'xx", 0, "a' b", 7)]
    [TestCase(@"some_text ""QF \"""" other_text", 10, "QF \"", 7)]

    public void Test(string line, int startIndex, string expectedValue, int expectedLength)
	{
		var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
		Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
	}
}

class QuotedFieldTask
{
    public static Token ReadQuotedField(string line, int startIndex)
    {
        var sb = new StringBuilder();
        var quote = line[startIndex];
        var count = startIndex + 1;

        while (count < line.Length)
        {
            if (line[count] == '\\')
            {
                count++;
                sb.Append(line[count]);
                count++;
            }
            else if (line[count] == quote)
            {
                count++;
                break;
            }
            else
            {
                sb.Append(line[count]);
                count++;
            }
        }
        return new Token(sb.ToString(), startIndex, count - startIndex);
    }
}