using NUnit.Framework;
using FluentAssertions;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(17, 2, true, "0.0", true)]
    [TestCase(17, 2, true, "0", true)]
    [TestCase(3, 2, true, "00.00", false)]
    [TestCase(3, 2, true, "-0.00", false)]
    [TestCase(3, 2, true, "+0.00", false)]
    [TestCase(4, 2, true, "+1.23", true)]
    [TestCase(3, 2, true, "+1.23", false)]
    [TestCase(17, 2, true, "0.000", false)]
    [TestCase(3, 2, true, "-1.23", false)]
    [TestCase(3, 2, false, "-1.23", false)]
    [TestCase(5, 2, false, "-1.23", true)]
    [TestCase(10, 2, false,null, false)]
    [TestCase(10, 2, false, "", false)]
    [TestCase(10, 2, false, "abc", false)]
    [TestCase(10, 2, false, "1..2", false)]
    [TestCase(10, 2, false, "1.", false)]
    [TestCase(10, 2, false, "1.2.3", false)]
    [TestCase(10, 2, false, "a.sd", false)]
    public void IsValidNumber_ShouldBeAsExpected(int precision, int scale, bool onlyPositive, string number, bool expectedResult)
    {
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(number).Should().Be(expectedResult);
    }
    
    [TestCase(-1, 2, true, typeof(ArgumentException))]
    [TestCase(1, -1, false, typeof(ArgumentException))]
    [TestCase(1, 1, false, typeof(ArgumentException))]
    public void Constructor_ShouldThrowException_WhenPrecisionOrScaleInvalid(int precision, int scale, bool onlyPositive,
        Type expectedExceptionType)
    {
        var createNumberValidator = () => new NumberValidator(precision, scale, onlyPositive);
        
        createNumberValidator.Should().Throw<Exception>().Which.Should().BeOfType(expectedExceptionType);
    }
    
    [TestCase(1, 0, true)]
    [TestCase(5, 2, false)]
    [TestCase(10, 0, true)]
    public void Constructor_ShouldNotThrow_WhenParametersValid(int precision, int scale, bool onlyPositive)
    {
        var createNumberValidator = () => new NumberValidator(precision, scale, onlyPositive);
        
        createNumberValidator.Should().NotThrow();
    }
}