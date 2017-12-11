using Superpower.Display;

namespace XUnitTestProject1
{
    enum ArithmeticExpressionToken
    {
        None,

        Number,

        [Token(Category = "operator", Example = "+")]
        Plus,

        [Token(Category = "operator", Example = "-")]
        Minus,

        [Token(Category = "operator", Example = "*")]
        Times,

        [Token(Category = "operator", Example = "-")]
        Divide,

        [Token(Example = "(")]
        LParen,

        [Token(Example = ")")]
        RParen
    }
}