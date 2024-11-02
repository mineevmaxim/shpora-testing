using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void NumberValidator_Throws_WhenNegativePrecision()
    {
        Action action = () => new NumberValidator(-1, 2, true);
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void NumberValidator_Throws_WhenNegativeScale()
    {
        Action action = () => new NumberValidator(2, -1, true);
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void NumberValidator_Throws_WhenScaleGreaterThanPrecision()
    {
        Action action = () => new NumberValidator(2, 3, true);
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void NumberValidator_Throws_WhenScaleEqualPrecision()
    {
        Action action = () => new NumberValidator(2, 2, true);
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void NumberValidator_DontThrows_WhenPrecisionIsPositive()
    {
        Action action = () => new NumberValidator(2, 1, true);
        action.Should().NotThrow<Exception>();
    }

    [Test]
    public void NumberValidator_DontThrows_WhenScaleLessThanPrecision()
    {
        Action action = () => new NumberValidator(2, 1, true);
        action.Should().NotThrow<Exception>();
    }

    [TestCase(1, true, "1")]
    [TestCase(2, true, "21")]
    [TestCase(3, true, "13")]
    [TestCase(5, true, "514")]
    [TestCase(10, false, "-314")]
    [TestCase(2, false, "-3")]
    [TestCase(3, false, "-99")]
    public void NumberValidator_ReturnsTrue_WithValidIntegerNumbers_WithScaleZero(int precision, bool onlyPositive,
        string number) => TestWithValidParameters(precision, 0, onlyPositive, number);

    [TestCase(1, true, "11")]
    [TestCase(2, true, "001")]
    [TestCase(3, true, "4134124")]
    [TestCase(10, false, "-031442424324243")]
    [TestCase(2, false, "-13")]
    [TestCase(3, false, "-993")]
    public void NumberValidator_ReturnsFalse_WithInvalidIntegerNumbers_WithScaleZero(int precision,
        bool onlyPositive, string number) => TestWithInvalidParameters(precision, 0, onlyPositive, number);

    [TestCase(2, 1, true, "1.1")]
    [TestCase(4, 3, true, "2.21")]
    [TestCase(4, 3, true, "0.000")]
    [TestCase(4, 3, true, "00.00")]
    [TestCase(5, 1, true, "51.4")]
    [TestCase(10, 5, false, "-31.414")]
    [TestCase(3, 1, false, "-3.2")]
    [TestCase(2, 1, false, "-1")]
    public void NumberValidator_ReturnsTrue_WithValidFloatNumbers(int precision, int scale, bool onlyPositive,
        string number) => TestWithValidParameters(precision, scale, onlyPositive, number);

    [TestCase(2, 1, true, "1.12")]
    [TestCase(4, 3, true, "2.2112")]
    [TestCase(5, 1, true, "51.43")]
    [TestCase(10, 5, false, "-31.41431231")]
    [TestCase(3, 1, false, "-3.21")]
    [TestCase(2, 1, false, "-1.1")]
    public void NumberValidator_ReturnsFalse_WithInvalidFloatNumbers(int precision, int scale, bool onlyPositive,
        string number) => TestWithInvalidParameters(precision, scale, onlyPositive, number);

    [TestCase("a")]
    [TestCase("seven")]
    [TestCase("one.five")]
    [TestCase("   ")]
    [TestCase("")]
    [TestCase(null)]
    public void NumberValidator_ReturnsFalse_WithNonNumber(string number)
        => TestWithInvalidParameters(5, 4, false, number);

    [TestCase("IV")]
    [TestCase("1 . 1")]
    [TestCase("1. 1")]
    [TestCase("1 .1")]
    [TestCase("10_000")]
    [TestCase("10 000")]
    [TestCase("10.")]
    [TestCase(".1")]
    [TestCase("+.1")]
    [TestCase("-.1")]
    [TestCase("5*3")]
    public void NumberValidator_ReturnsFalse_WithWrongFormat(string number)
        => TestWithInvalidParameters(5, 4, false, number);

    [TestCase("1,1")]
    [TestCase("1.1")]
    [TestCase("11")]
    public void NumberValidator_CorrectWork_WithCorrectSeparatorFormat(string number)
        => TestWithValidParameters(5, 4, false, number);

    [TestCase("+11")]
    [TestCase("+1111")]
    [TestCase("+1.111")]
    [TestCase("-1111")]
    [TestCase("-1.111")]
    [TestCase("-1.1")]
    [TestCase("-11")]
    [TestCase("+1.1")]
    [TestCase("1.1")]
    [TestCase("11")]
    public void NumberValidator_CorrectWork_WithAndWithoutSign(string number)
        => TestWithValidParameters(5, 3, false, number);

    private static void TestWithValidParameters(int precision, int scale, bool onlyPositive,
        string number)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(number).Should().BeTrue();
    }

    private static void TestWithInvalidParameters(int precision, int scale, bool onlyPositive,
        string number)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(number).Should().BeFalse();
    }
}