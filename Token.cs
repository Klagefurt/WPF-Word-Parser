namespace TableParser;

public class Token
{
	public readonly int Length;

	public readonly int Position;
	public readonly string Value;

	/// <param name="value">Token's value</param>
	/// <param name="position">Token's position in the base line</param>
	/// <param name="length">Token's length in the base line, can be not equal to <paramref name="value" /></param>
	public Token(string value, int position, int length)
	{
		Position = position;
		Length = length;
		Value = value;
	}

	public override bool Equals(object obj)
	{
		if ((Token) obj == null)
			return false;
		return Equals((Token) obj);
	}

	protected bool Equals(Token other)
	{
		return Length == other.Length && Position == other.Position && string.Equals(Value, other.Value);
	}

	public override int GetHashCode()
	{
		unchecked
		{
			var hashCode = Length;
			hashCode = (hashCode * 397) ^ Position;
			hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
			return hashCode;
		}
	}

	public int GetIndexNextToToken()
	{
		return Position + Length;
	}

	public override string ToString()
	{
		var value = $"[{Value}]";
		return $"{value.PadRight(10)} Position={Position:##0} Length={Length}";
	}
}