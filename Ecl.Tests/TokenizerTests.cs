using Xunit;

namespace Ecl.Tests
{
    public class TokenizerTests
    {       
        [Fact]
        public void AllTokenKinds()
        {
            var sut = new EclTokenizer();
            var str = "UP DOWN@\n,()123";
            var tryTokenize = sut.TryTokenize(str);
            Assert.Null(tryTokenize.ErrorMessage);
        }
    } 
}
