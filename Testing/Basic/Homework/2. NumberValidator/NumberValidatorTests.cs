using NUnit.Framework;
using FluentAssertions;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(17, 2, true, "0.0")]
    [TestCase(17, 2, true, "0")]
    [TestCase(4, 2, true, "+1.23")]
    [TestCase(5, 2, false, "-1.23")]
    public void IsValidNumber_ShouldBeTrue_WhenParametersValid(int precision, int scale, bool onlyPositive, string number)
    {
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(number).Should().Be(true);
    }
    
    [TestCase(3, 2, true, "00.00")]
    [TestCase(3, 2, true, "-0.00")]
    [TestCase(3, 2, true, "+0.00")]
    [TestCase(3, 2, true, "+1.23")]
    [TestCase(17, 2, true, "0.000")]
    [TestCase(3, 2, true, "-1.23")]
    [TestCase(3, 2, false, "-1.23")]
    [TestCase(10, 2, false,null)]
    [TestCase(10, 2, false, "")]
    [TestCase(10, 2, false, "abc")]
    [TestCase(10, 2, false, "1..2")]
    [TestCase(10, 2, false, "1.")]
    [TestCase(10, 2, false, "1.2.3")]
    [TestCase(10, 2, false, "a.sd")]
    public void IsValidNumber_ShouldBeFalse_WhenParametersInvalid(int precision, int scale, bool onlyPositive, string number)
    {
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(number).Should().Be(false);
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