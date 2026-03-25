namespace EnsureValidation.PtBr.Tests;

public class PtBRMessagesTests
{
    [Fact]
    public void Configure_SetsPortugueseMessages()
    {
        // Arrange
        PtBRMessages.Configure();

        // Act
        var cpfMessage = EnsureValidation.Messages.To("Cpf");
        var cnpjMessage = EnsureValidation.Messages.To("Cnpj");
        var requiredMessage = EnsureValidation.Messages.To("Required");

        // Assert
        Assert.Contains("CPF", cpfMessage, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("CNPJ", cnpjMessage, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("obrigatório", requiredMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Configure_ContainsAllExpectedMessages()
    {
        // Arrange
        PtBRMessages.Configure();

        // Act & Assert - verify some key messages are available
        var messages = new[] { "Required", "Cpf", "Cnpj", "MinLength", "MaxLength", "EmailAddress" };

        foreach (var key in messages)
        {
            var message = EnsureValidation.Messages.To(key);
            Assert.False(message.StartsWith("["), $"Message for '{key}' should be configured");
        }
    }

    [Theory]
    [InlineData("Cpf")]
    [InlineData("Cnpj")]
    [InlineData("Required")]
    [InlineData("MinLength")]
    [InlineData("MaxLength")]
    [InlineData("EmailAddress")]
    [InlineData("ExactLength")]
    [InlineData("Matches")]
    [InlineData("EqualTo")]
    [InlineData("GreaterThan")]
    [InlineData("LessThan")]
    [InlineData("Between")]
    [InlineData("After")]
    [InlineData("Before")]
    [InlineData("InTheFuture")]
    [InlineData("InThePast")]
    [InlineData("IsTrue")]
    [InlineData("IsFalse")]
    [InlineData("InEnum")]
    public void Configure_ContainsMessage(string messageKey)
    {
        // Arrange
        PtBRMessages.Configure();

        // Act
        var message = EnsureValidation.Messages.To(messageKey);

        // Assert
        Assert.False(message.StartsWith("["), $"Message for '{messageKey}' should be configured in Portuguese");
    }
}
