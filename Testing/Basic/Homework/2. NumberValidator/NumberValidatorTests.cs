using NUnit.Framework;
using FluentAssertions;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(17, 2, true, "0.0", TestName = "IsValidNumber_ShouldBeTrue_WhenZeroDecimalWithinPrecisionAndScale")]
    [TestCase(17, 2, true, "0", TestName = "IsValidNumber_ShouldBeTrue_WhenZeroWithoutFraction")]
    [TestCase(4, 2, true, "+1.23", TestName = "IsValidNumber_ShouldBeTrue_WhenPositiveNumberWithinPrecisionAndScale")]
    [TestCase(5, 2, false, "-1.23", TestName = "IsValidNumber_ShouldBeTrue_WhenNegativeNumberAllowedAndWithinPrecision")]
    [TestCase(3, 1, false, "9.9", TestName = "IsValidNumber_ShouldBeTrue_WhenPrecisionExactlyMatchesNumberLength")]
    [TestCase(5, 0, false, "-1234", TestName = "IsValidNumber_ShouldBeTrue_WhenIntegerWithinPrecisionWithoutFraction")]
    [TestCase(5, 2, false, "999.99", TestName = "IsValidNumber_ShouldBeTrue_WhenPrecisionAndScaleAtUpperBound")]
    public void IsValidNumber_ShouldBeTrue_WhenParametersValid(int precision, int scale, bool onlyPositive, string number)
    {
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(number).Should().Be(true);
    }
    
    [TestCase(3, 2, true, "00.00", TestName = "IsValidNumber_ShouldBeFalse_WhenPrecisionExceededByLeadingZeros")]
    [TestCase(3, 2, true, "-0.00", TestName = "IsValidNumber_ShouldBeFalse_WhenNegativeAndOnlyPositiveAllowed")]
    [TestCase(3, 2, true, "+0.00", TestName = "IsValidNumber_ShouldBeFalse_WhenPrecisionExceededBySign")]
    [TestCase(3, 2, true, "+1.23", TestName = "IsValidNumber_ShouldBeFalse_WhenNumberExceedsPrecision")]
    [TestCase(17, 2, true, "0.000", TestName = "IsValidNumber_ShouldBeFalse_WhenFractionalPartExceedsScale")]
    [TestCase(3, 2, true, "-1.23", TestName = "IsValidNumber_ShouldBeFalse_WhenNegativeAndOnlyPositiveAllowed")]
    [TestCase(10, 2, false, null, TestName = "IsValidNumber_ShouldBeFalse_WhenValueIsNull")]
    [TestCase(10, 2, false, "", TestName = "IsValidNumber_ShouldBeFalse_WhenValueIsEmptyString")]
    [TestCase(10, 2, false, "abc", TestName = "IsValidNumber_ShouldBeFalse_WhenContainsLetters")]
    [TestCase(10, 2, false, "1..2", TestName = "IsValidNumber_ShouldBeFalse_WhenContainsTwoDots")]
    [TestCase(10, 2, false, "1.", TestName = "IsValidNumber_ShouldBeFalse_WhenEndsWithDotWithoutFraction")]
    [TestCase(10, 2, false, "1.2.3", TestName = "IsValidNumber_ShouldBeFalse_WhenContainsMultipleFractionSeparators")]
    [TestCase(10, 2, false, "a.sd", TestName = "IsValidNumber_ShouldBeFalse_WhenInvalidCharactersUsed")]
    [TestCase(5, 2, false, "123456", TestName = "IsValidNumber_ShouldBeFalse_WhenPrecisionExceededByIntegerPart")]
    [TestCase(3, 0, false, "1234", TestName = "IsValidNumber_ShouldBeFalse_WhenIntegerPartLongerThanPrecision")]
    [TestCase(5, 2, false, "000.000", TestName = "IsValidNumber_ShouldBeFalse_WhenTotalDigitsExceedPrecision")]
    public void IsValidNumber_ShouldBeFalse_WhenParametersInvalid(int precision, int scale, bool onlyPositive, string number)
    {
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(number).Should().Be(false);
    }
    
    [TestCase(-1, 2, true, typeof(ArgumentException), TestName = "Constructor_ShouldThrow_WhenPrecisionNegative")]
    [TestCase(1, -1, false, typeof(ArgumentException), TestName = "Constructor_ShouldThrow_WhenScaleNegative")]
    [TestCase(1, 1, false, typeof(ArgumentException), TestName = "Constructor_ShouldThrow_WhenScaleEqualsPrecision")]
    [TestCase(0, 2, false, typeof(ArgumentException), TestName = "Constructor_ShouldThrow_WhenPrecisionZero")]
    [TestCase(2, 5, false, typeof(ArgumentException), TestName = "Constructor_ShouldThrow_WhenScaleGreaterThanPrecision")]
    [TestCase(0, -1, false, typeof(ArgumentException), TestName = "Constructor_ShouldThrow_WhenPrecisionZeroAndScaleNegative")]
    [TestCase(-5, -5, false, typeof(ArgumentException), TestName = "Constructor_ShouldThrow_WhenBothPrecisionAndScaleNegative")]
    public void Constructor_ShouldThrowException_WhenPrecisionOrScaleInvalid(int precision, int scale, bool onlyPositive,
        Type expectedExceptionType)
    {
        var createNumberValidator = () => new NumberValidator(precision, scale, onlyPositive);
        
        createNumberValidator.Should().Throw<Exception>().Which.Should().BeOfType(expectedExceptionType);
    }
    
    [TestCase(1, 0, true, TestName = "Constructor_ShouldNotThrow_WhenMinimalValidPrecisionAndScale")]
    [TestCase(5, 2, false, TestName = "Constructor_ShouldNotThrow_WhenCommonValidValues")]
    [TestCase(10, 0, true, TestName = "Constructor_ShouldNotThrow_WhenScaleIsZero")]
    [TestCase(10, 9, false, TestName = "Constructor_ShouldNotThrow_WhenScaleJustBelowPrecision")]
    public void Constructor_ShouldNotThrow_WhenParametersValid(int precision, int scale, bool onlyPositive)
    {
        var createNumberValidator = () => new NumberValidator(precision, scale, onlyPositive);
        
        createNumberValidator.Should().NotThrow();
    }
}