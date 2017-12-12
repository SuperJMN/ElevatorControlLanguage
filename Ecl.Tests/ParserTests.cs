using Superpower;
using Xunit;

namespace Ecl.Tests
{
    public class ParserTests
    {       
        [Fact]
        public void NumberParser()
        {
            var tokenizer = new EclTokenizer();
            var parsed = EclParser.Number.Parse(tokenizer.Tokenize("123"));

            Assert.Equal(123, parsed);
        }    
        
        [Fact]
        public void DownKeyword()
        {
            var tokenizer = new EclTokenizer();
            var parsed = EclParser.Command.Parse(tokenizer.Tokenize("(DOWN 123)"));

            var elevatorCommand = new ElevatorCommand()
            {
                Distance = new Distance() { IsRelative = false, Value = -123},
            };

            Assert.Equal(elevatorCommand, parsed);
        }    

        [Fact]
        public void List()
        {
            var tokenizer = new EclTokenizer();
            var parsed = EclParser.Parse("(UP 100),(DOWN 200),(DOWN 100),(DOWN @1),(UP @3),(WAIT)(UP 300)");                     
        }    
    }
}