using System.Collections.Generic;
using System.Linq;
using System.Text;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;

namespace Ecl
{
    public class EclTokenizer : Tokenizer<EclToken>
    {
        protected override IEnumerable<Result<EclToken>> Tokenize(TextSpan span)
        {
            var next = SkipWhiteSpace(span);
            do
            {
                var nextValue = next.Value;
                if (char.IsWhiteSpace(nextValue))
                {
                    yield return Result.Value(EclToken.Whitespace, next.Location, next.Remainder);
                    next = next.Remainder.ConsumeChar();
                }

                if (char.IsLetter(next.Value))
                {
                    var keywordBuilder = new StringBuilder();

                    var keywordStart = next.Location;
                    keywordBuilder.Append(next.Value);
                    var keywords = new[] { "UP", "DOWN", "WAIT" };
                    do
                    {
                        next = next.Remainder.ConsumeChar();
                        if (char.IsLetter(next.Value))
                        {
                            keywordBuilder.Append(next.Value);
                        }
                    } while (!keywords.Contains(keywordBuilder.ToString()) && next.HasValue && char.IsLetter(next.Value));

                    next = next.Remainder.ConsumeChar();

                    var keyword = keywordBuilder.ToString();
                    var token = EclToken.None;
                    switch (keyword)
                    {
                        case "UP":
                            token = EclToken.UpKeyword;
                            break;
                        case "DOWN":
                            token = EclToken.DownKeyword;
                            break;
                        case "WAIT":
                            token = EclToken.WaitKeyword;
                            break;
                        default:
                            yield return Result.Empty<EclToken>(keywordStart, $"Unexpected keyword {keyword}");
                            break;
                    }

                    yield return Result.Value(token, keywordStart, next.Location);
                }

                else if (!char.IsDigit(next.Value))
                {
                    yield return EmitOneCharToken(next);
                    next = next.Remainder.ConsumeChar();
                }
                else if (char.IsDigit(next.Value))
                {
                    var integer = Numerics.Integer(next.Location);
                    next = integer.Remainder.ConsumeChar();
                    yield return Result.Value(EclToken.Number, integer.Location, integer.Remainder);
                }
            } while (next.HasValue);
        }

        private static Result<EclToken> EmitOneCharToken(Result<char> next)
        {
            EclToken simpleToken = EclToken.None;
            switch (next.Value)
            {
                case '(':
                    simpleToken = EclToken.LParen;
                    break;
                case ')':
                    simpleToken = EclToken.RParen;
                    break;
                case '@':
                    simpleToken = EclToken.AtSymbol;
                    break;
                case ',':
                    simpleToken = EclToken.Comma;
                    break;
            }

            return Result.Value(simpleToken, next.Location, next.Remainder);
        }
    }
}