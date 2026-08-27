namespace VideoOS.Management.Server.ConfigurationApi;

public static class JScriptEvaluator
{
	private enum Symbol
	{
		TRUE,
		FALSE,
		OR,
		NOT,
		AND,
		END,
		PSTART,
		PEND,
		INVALID
	}

	private static object _lockObject = new object();

	private static string Statement;

	private static int ix;

	private static Symbol symbol = Symbol.END;

	public static bool Evaluate(string statement)
	{
		lock (_lockObject)
		{
			Statement = statement;
			ix = 0;
			JScriptEvaluator.symbol = GetSymbol();
			Symbol symbol = Expression();
			if (JScriptEvaluator.symbol != Symbol.END)
			{
				return false;
			}
			return symbol == Symbol.TRUE;
		}
	}

	private static Symbol Expression()
	{
		Symbol symbol = Symbol.INVALID;
		if (JScriptEvaluator.symbol == Symbol.NOT)
		{
			JScriptEvaluator.symbol = GetSymbol();
			return Expression() switch
			{
				Symbol.TRUE => Symbol.FALSE, 
				Symbol.FALSE => Symbol.TRUE, 
				_ => Symbol.INVALID, 
			};
		}
		if (JScriptEvaluator.symbol == Symbol.PSTART)
		{
			JScriptEvaluator.symbol = GetSymbol();
			symbol = Expression();
			if (JScriptEvaluator.symbol != Symbol.PEND)
			{
				return Symbol.INVALID;
			}
		}
		else
		{
			if (JScriptEvaluator.symbol != Symbol.TRUE && JScriptEvaluator.symbol != Symbol.FALSE)
			{
				return Symbol.INVALID;
			}
			symbol = JScriptEvaluator.symbol;
		}
		JScriptEvaluator.symbol = GetSymbol();
		while (JScriptEvaluator.symbol == Symbol.OR || JScriptEvaluator.symbol == Symbol.AND)
		{
			if (JScriptEvaluator.symbol == Symbol.OR)
			{
				JScriptEvaluator.symbol = GetSymbol();
				Symbol symbol2 = Expression();
				if (symbol2 != Symbol.FALSE && symbol2 != Symbol.TRUE)
				{
					return Symbol.INVALID;
				}
				if (symbol2 == Symbol.TRUE || symbol == Symbol.TRUE)
				{
					symbol = Symbol.TRUE;
				}
			}
			while (JScriptEvaluator.symbol == Symbol.AND)
			{
				JScriptEvaluator.symbol = GetSymbol();
				Symbol symbol3 = Expression();
				if (symbol3 != Symbol.FALSE && symbol3 != Symbol.TRUE)
				{
					return Symbol.INVALID;
				}
				symbol = ((symbol3 != Symbol.TRUE || symbol != Symbol.TRUE) ? Symbol.FALSE : Symbol.TRUE);
			}
		}
		return symbol;
	}

	private static Symbol GetSymbol()
	{
		while (ix < Statement.Length && Statement[ix] == ' ')
		{
			ix++;
		}
		if (ix >= Statement.Length)
		{
			return Symbol.END;
		}
		if (Statement[ix] == '{')
		{
			while (ix < Statement.Length)
			{
				if (Statement[ix] == '}')
				{
					ix++;
					return Symbol.TRUE;
				}
				ix++;
			}
			if (ix >= Statement.Length)
			{
				return Symbol.END;
			}
		}
		if (Statement[ix] == '(')
		{
			ix++;
			return Symbol.PSTART;
		}
		if (Statement[ix] == ')')
		{
			ix++;
			return Symbol.PEND;
		}
		if (Statement[ix] == '!')
		{
			ix++;
			return Symbol.NOT;
		}
		if (Statement.Substring(ix, 2) == "||")
		{
			ix += 2;
			return Symbol.OR;
		}
		if (Statement.Substring(ix, 2) == "&&")
		{
			ix += 2;
			return Symbol.AND;
		}
		if (Statement.Substring(ix, 4) == "true")
		{
			ix += 4;
			return Symbol.TRUE;
		}
		if (Statement.Substring(ix, 5) == "false")
		{
			ix += 5;
			return Symbol.FALSE;
		}
		if (Statement.Substring(ix, 10) == "&amp;&amp;")
		{
			ix += 10;
			return Symbol.AND;
		}
		return Symbol.INVALID;
	}
}
