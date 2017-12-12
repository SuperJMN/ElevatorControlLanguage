using System.Collections.Generic;
using Superpower;
using Superpower.Parsers;

namespace Ecl
{
    public static class EclParser
    {
        public static TokenListParser<EclToken, int> Number = 
            Token.EqualTo(EclToken.Number).Apply(Numerics.IntegerInt32);

        public static TokenListParser<EclToken, ElevatorCommand> Up =
            from _ in Token.EqualTo(EclToken.UpKeyword)
            from __ in Token.EqualTo(EclToken.Whitespace)
            from distance in Distance
            select new ElevatorCommand
            {
                Distance = distance,
            };

        public static TokenListParser<EclToken, Distance> AbsoluteDistance =
            from number in Number
            select new Distance()
            {
                IsRelative = false,
                Value = number,
            };

        public static TokenListParser<EclToken, Distance> RelativeDistance =
            from _ in Token.EqualTo(EclToken.AtSymbol)
            from number in Number
            select new Distance()
            {
                IsRelative = true,
                Value = number,
            };

        public static TokenListParser<EclToken, Distance> Distance =
            RelativeDistance.Or(AbsoluteDistance);

        public static TokenListParser<EclToken, ElevatorCommand> Down =
            from _ in Token.EqualTo(EclToken.DownKeyword)
            from __ in Token.EqualTo(EclToken.Whitespace)
            from distance in Distance
            select new ElevatorCommand
            {
                Distance = distance.GetOpposite(),
            };

        public static TokenListParser<EclToken, ElevatorCommand> Wait =
            from _ in Token.EqualTo(EclToken.WaitKeyword)
            select new ElevatorCommand
            {
                IsWait = true,
            };

        public static TokenListParser<EclToken, ElevatorCommand> Command =
            from _ in Token.EqualTo(EclToken.LParen)
            from command in Up.Or(Down).Or(Wait)
            from __ in Token.EqualTo(EclToken.RParen)
            select command;

        public static TokenListParser<EclToken, ElevatorCommand[]> Commands =
            Command.ManyDelimitedBy(Token.EqualTo(EclToken.Comma));

        public static IEnumerable<ElevatorCommand> Parse(string text)
        {
            return Commands.Parse(new EclTokenizer().Tokenize(text));
        }
    }

    public struct Distance
    {
        public bool IsRelative { get; set; }
        public int Value { get; set; }
    }

    public static class DistanceMixin
    {
        public static Distance GetOpposite(this Distance d)
        {
            return new Distance()
            {
                IsRelative = d.IsRelative,
                Value = -d.Value,
            };
        }
    }
}