using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    private const string InvalidPrecisionError = "precision must be a positive number";
    private const string InvalidScaleError = "scale must be a non-negative number less than precision";

    [TestCase(2, -1, InvalidScaleError, TestName = "WhenNegativeScale")]
    [TestCase(-1, 2, InvalidPrecisionError, TestName = "WhenNegativePrecision")]
    [TestCase(2, 3, InvalidScaleError, TestName = "WhenScaleGreaterThanPrecision")]
    [TestCase(2, 2, InvalidScaleError, TestName = "WhenScaleEqualPrecision")]
    public void NumberValidatorConstructor_Throws(int precision, int scale, string message)
    {
        Action action = () => new NumberValidator(precision, scale);
        action.Should().Throw<ArgumentException>().WithMessage(message);
    }

    [TestCase(2, 0, TestName = "WhenPrecisionIsPositive")]
    [TestCase(2, 1, TestName = "WhenScaleLessThanPrecision")]
    public void NumberValidatorConstructor_DontThrows(int precision, int scale)
    {
        Action action = () => new NumberValidator(precision, scale);
        action.Should().NotThrow<Exception>();
    }

    [TestCase(2, 1, true, "1.12", TestName = "FracPartGreaterThanScaleWhenPositive")]
    [TestCase(3, 1, false, "-3.21", TestName = "FracPartGreaterThanScaleWhenNegative")]
    [TestCase(2, 1, false, "11.1", TestName = "ValueGreaterThanPrecision")]
    [TestCase(3, 1, true, "-1.1", TestName = "WithSignWhenOnlyPositive")]
    [TestCase(5, 4, false, "a", TestName = "Letter")]
    [TestCase(5, 4, false, "seven", TestName = "Word")]
    [TestCase(5, 4, false, "one.five", TestName = "WordsWithCorrectSeparator")]
    [TestCase(5, 4, false, "   ", TestName = "Spaces")]
    [TestCase(5, 4, false, "", TestName = "EmptyString")]
    [TestCase(5, 4, false, null, TestName = "NumberIsNull")]
    [TestCase(5, 4, false, "IV", TestName = "NotArabicNumber")]
    [TestCase(5, 4, false, "1 . 1", TestName = "SpacesAroundSeparator")]
    [TestCase(5, 4, false, "1. 1", TestName = "SpaceAfterSeparator")]
    [TestCase(5, 4, false, "1 .1", TestName = "SpaceBeforeSeparator")]
    [TestCase(5, 4, false, "10_000", TestName = "NumberFormatWithUnderscore")]
    [TestCase(5, 4, false, "10 000", TestName = "NumberFormatWithSpaces")]
    [TestCase(5, 4, false, "10.", TestName = "WithSeparatorWithoutFracPart")]
    [TestCase(5, 4, false, ".1", TestName = "WithSeparatorWithoutIntPart")]
    [TestCase(5, 4, false, "+.1", TestName = "WithSeparatorWithoutIntPartWithPlusSign")]
    [TestCase(5, 4, false, "-.1", TestName = "WithSeparatorWithoutIntPartWithMinusSign")]
    [TestCase(5, 4, false, "5*3", TestName = "MathExpression")]
    [TestCase(5, 4, false, "+-1", TestName = "GreaterThanOneSign")]
    public void IsNumberValid_ReturnsFalse_WithInvalidNumber(int precision, int scale, bool onlyPositive, string number)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(number).Should().BeFalse();
    }

    [TestCase(4, 0, true, "123", TestName = "CorrectInt")]
    [TestCase(4, 3, true, "1.23", TestName = "CorrectFloat")]
    [TestCase(4, 3, true, "0.000", TestName = "OnlyZeros")]
    [TestCase(4, 3, true, "00.00", TestName = "LeadingZeros")]
    [TestCase(1, 0, true, "1", TestName = "PositiveIntWhenPrecisionEqualsNumberLength")]
    [TestCase(5, 0, true, "514", TestName = "PositiveIntWhenPrecisionGreaterThanNumberLength")]
    [TestCase(3, 0, false, "-99", TestName = "NegativeIntWhenPrecisionEqualsNumberLength")]
    [TestCase(10, 0, false, "-314", TestName = "NegativeIntWhenPrecisionGreaterThanNumberLength")]
    [TestCase(5, 4, false, "1.1", TestName = "DotSeparator")]
    [TestCase(5, 4, false, "1,1", TestName = "CommaSeparator")]
    [TestCase(5, 3, false, "+11", TestName = "IntWithPlusSign")]
    [TestCase(5, 3, false, "-11", TestName = "IntWithMinusSign")]
    [TestCase(5, 3, false, "+1.111", TestName = "FloatWithPlusSign")]
    [TestCase(5, 3, false, "-1.111", TestName = "FloatWithMinusSign")]
    public void IsNumberValid_ReturnsTrue_WithValidNumber(int precision, int scale, bool onlyPositive, string number)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        validator.IsValidNumber(number).Should().BeTrue();
    }
}