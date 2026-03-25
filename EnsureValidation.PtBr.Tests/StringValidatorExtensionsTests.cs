namespace EnsureValidation.PtBr.Tests;

public class StringValidatorExtensionsTests
{
    [Theory]
    [InlineData("11144477735")]      // Valid CPF
    [InlineData("52998222060")]      // Another valid CPF
    public void Cpf_WithValidCpf_PassesValidation(string cpf)
    {
        // Arrange
        var entity = new TestEntity { Cpf = cpf };
        var context = new EnsureContext();

        // Act
        entity.Validate(context);

        // Assert
        Assert.Empty(context.Notifications);
    }

    [Theory]
    [InlineData("11111111111")]      // All same digits
    [InlineData("12345678901")]      // Invalid check digits
    [InlineData("123456789")]        // Too short
    [InlineData("123456789012")]     // Too long
    [InlineData("1234567890a")]      // Contains letters
    [InlineData("")]                 // Empty (should pass as per documentation)
    [InlineData(null)]               // Null (should pass as per documentation)
    public void Cpf_WithInvalidOrEmptyCpf_FailsValidation(string? cpf)
    {
        // Arrange
        var entity = new TestEntity { Cpf = cpf };
        var context = new EnsureContext();

        // Act
        entity.Validate(context);

        // Assert
        if (string.IsNullOrEmpty(cpf))
        {
            // Empty or null values should pass validation
            Assert.Empty(context.Notifications);
        }
        else
        {
            // Invalid CPF should fail
            Assert.NotEmpty(context.Notifications);
            Assert.Single(context.Notifications);
            Assert.Contains("CPF", context.Notifications[0].Message, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Theory]
    [InlineData("11222333000181")]   // Valid CNPJ
    [InlineData("34028316000152")]   // Another valid CNPJ
    public void Cnpj_WithValidCnpj_PassesValidation(string cnpj)
    {
        // Arrange
        var entity = new TestEntity { Cnpj = cnpj };
        var context = new EnsureContext();

        // Act
        entity.Validate(context);

        // Assert
        Assert.Empty(context.Notifications);
    }

    [Theory]
    [InlineData("11111111111111")]   // All same digits
    [InlineData("12345678901234")]   // Invalid check digits
    [InlineData("123456789")]        // Too short
    [InlineData("123456789012345")]  // Too long
    [InlineData("1234567890123a")]   // Contains letters
    [InlineData("")]                 // Empty (should pass as per documentation)
    [InlineData(null)]               // Null (should pass as per documentation)
    public void Cnpj_WithInvalidOrEmptyCnpj_FailsValidation(string? cnpj)
    {
        // Arrange
        var entity = new TestEntity { Cnpj = cnpj };
        var context = new EnsureContext();

        // Act
        entity.Validate(context);

        // Assert
        if (string.IsNullOrEmpty(cnpj))
        {
            // Empty or null values should pass validation
            Assert.Empty(context.Notifications);
        }
        else
        {
            // Invalid CNPJ should fail
            Assert.NotEmpty(context.Notifications);
            Assert.Single(context.Notifications);
            Assert.Contains("CNPJ", context.Notifications[0].Message, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void Cpf_WithPortugueseMessages_UsesPortugueseMessage()
    {
        // Arrange
        PtBRMessages.Configure();
        var entity = new TestEntity { Cpf = "invalid" };
        var context = new EnsureContext();

        // Act
        entity.Validate(context);

        // Assert
        Assert.NotEmpty(context.Notifications);
        Assert.Contains("CPF", context.Notifications[0].Message, StringComparison.OrdinalIgnoreCase);
        // Check that message contains Portuguese words
        Assert.True(
            context.Notifications[0].Message.Contains("válido", StringComparison.OrdinalIgnoreCase) ||
            context.Notifications[0].Message.Contains("inválido", StringComparison.OrdinalIgnoreCase),
            "Message should be in Portuguese"
        );
    }

    [Fact]
    public void Cnpj_WithPortugueseMessages_UsesPortugueseMessage()
    {
        // Arrange
        PtBRMessages.Configure();
        var entity = new TestEntity { Cnpj = "invalid" };
        var context = new EnsureContext();

        // Act
        entity.Validate(context);

        // Assert
        Assert.NotEmpty(context.Notifications);
        Assert.Contains("CNPJ", context.Notifications[0].Message, StringComparison.OrdinalIgnoreCase);
        // Check that message contains Portuguese words
        Assert.True(
            context.Notifications[0].Message.Contains("válido", StringComparison.OrdinalIgnoreCase) ||
            context.Notifications[0].Message.Contains("inválido", StringComparison.OrdinalIgnoreCase),
            "Message should be in Portuguese"
        );
    }

    [Fact]
    public void CpfAndCnpj_BothValidated_AllValidationsPass()
    {
        // Arrange
        var entity = new TestEntity
        {
            Cpf = "11144477735",      // Valid CPF
            Cnpj = "11222333000181"   // Valid CNPJ
        };
        var context = new EnsureContext();

        // Act
        entity.Validate(context);

        // Assert
        Assert.Empty(context.Notifications);
    }

    [Fact]
    public void CpfAndCnpj_BothInvalid_BothValidationsFail()
    {
        // Arrange
        var entity = new TestEntity
        {
            Cpf = "invalid",          // Invalid CPF
            Cnpj = "invalid"          // Invalid CNPJ
        };
        var context = new EnsureContext();

        // Act
        entity.Validate(context);

        // Assert
        Assert.Equal(2, context.Notifications.Count);
    }

    // Test helper class
    private class TestEntity : Notifiable
    {
        public string? Cpf { get; set; }
        public string? Cnpj { get; set; }

        public void Validate(EnsureContext context)
        {
            new StringValidator(Cpf, nameof(Cpf), context)
                .Cpf();

            new StringValidator(Cnpj, nameof(Cnpj), context)
                .Cnpj();
        }
    }
}
