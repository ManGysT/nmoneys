﻿using System.Linq;

namespace NMoneys.Change
{
	public static class Operations
	{
		public static ChangeSolution MinChange(this Money money, Denomination[] denominations)
		{
			Positive.Amounts.AssertArgument(nameof(money), money.Amount);

			Denomination[] orderedDenominations = denominations.OrderByDescending(d => d.Value).ToArray();
			decimal remainder = money.Amount;
			bool canContinue = true;

			ChangeSolution solution = new ChangeSolution();
			while (remainder > 0 && canContinue)
			{
				for (int i = 0; i < orderedDenominations.Length; i++)
				{
					while (orderedDenominations[i].CanBeSubstractedFrom(remainder))
					{
						orderedDenominations[i].SubstractFrom(ref remainder);
						solution.AddOrUpdate(orderedDenominations[i], d => d.Quantity++);
					}
				}
				canContinue = false;
			}
			solution.Remainder = new Money(remainder, money.CurrencyCode);

			return solution;
		}
	}
}