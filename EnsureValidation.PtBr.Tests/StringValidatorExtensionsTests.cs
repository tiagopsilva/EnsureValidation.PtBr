namespace EnsureValidation.PtBr.Tests;

public class StringValidatorExtensionsTests
{
    [Theory]
    [InlineData("11144477735")]      // Valid CPF
    [InlineData("52998222060")]      // Another valid CPF
    public void Cpf_WithValidCpf_PassesValidation(string cpf)
    {
        var entity = new TestEntity { Cpf = cpf };
        entity.Validate();

        Assert.Empty(entity.Notifications);
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
        var entity = new TestEntity { Cpf = cpf };
        entity.Validate();

        if (string.IsNullOrEmpty(cpf))
        {
            Assert.Empty(entity.Notifications);
        }
        else
        {
            Assert.NotEmpty(entity.Notifications);
            Assert.Single(entity.Notifications);
            Assert.Contains("CPF", entity.Notifications[0].Message, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Theory]
    [InlineData("11222333000181")]   // Valid CNPJ
    [InlineData("34028316000152")]   // Another valid CNPJ
    public void Cnpj_WithValidCnpj_PassesValidation(string cnpj)
    {
        var entity = new TestEntity { Cnpj = cnpj };
        entity.Validate();

        Assert.Empty(entity.Notifications);
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
        var entity = new TestEntity { Cnpj = cnpj };
        entity.Validate();

        if (string.IsNullOrEmpty(cnpj))
        {
            Assert.Empty(entity.Notifications);
        }
        else
        {
            Assert.NotEmpty(entity.Notifications);
            Assert.Single(entity.Notifications);
            Assert.Contains("CNPJ", entity.Notifications[0].Message, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void Cpf_WithPortugueseMessages_UsesPortugueseMessage()
    {
        PtBRMessages.Configure();
        var entity = new TestEntity { Cpf = "invalid" };
        entity.Validate();

        Assert.NotEmpty(entity.Notifications);
        Assert.Contains("CPF", entity.Notifications[0].Message, StringComparison.OrdinalIgnoreCase);
        Assert.True(
            entity.Notifications[0].Message.Contains("válido", StringComparison.OrdinalIgnoreCase) ||
            entity.Notifications[0].Message.Contains("inválido", StringComparison.OrdinalIgnoreCase),
            "Message should be in Portuguese"
        );
    }

    [Fact]
    public void Cnpj_WithPortugueseMessages_UsesPortugueseMessage()
    {
        PtBRMessages.Configure();
        var entity = new TestEntity { Cnpj = "invalid" };
        entity.Validate();

        Assert.NotEmpty(entity.Notifications);
        Assert.Contains("CNPJ", entity.Notifications[0].Message, StringComparison.OrdinalIgnoreCase);
        Assert.True(
            entity.Notifications[0].Message.Contains("válido", StringComparison.OrdinalIgnoreCase) ||
            entity.Notifications[0].Message.Contains("inválido", StringComparison.OrdinalIgnoreCase),
            "Message should be in Portuguese"
        );
    }

    [Fact]
    public void CpfAndCnpj_BothValidated_AllValidationsPass()
    {
        var entity = new TestEntity
        {
            Cpf = "11144477735",      // Valid CPF
            Cnpj = "11222333000181"   // Valid CNPJ
        };
        entity.Validate();

        Assert.Empty(entity.Notifications);
    }

    [Fact]
    public void CpfAndCnpj_BothInvalid_BothValidationsFail()
    {
        var entity = new TestEntity
        {
            Cpf = "invalid",          // Invalid CPF
            Cnpj = "invalid"          // Invalid CNPJ
        };
        entity.Validate();

        Assert.Equal(2, entity.Notifications.Count);
    }

    // Test helper class
    private class TestEntity : Notifiable<TestEntity>
    {
        public string? Cpf { get; set; }
        public string? Cnpj { get; set; }

        protected override void OnValidate()
        {
            Ensure.That(Cpf, nameof(Cpf)).Cpf();
            Ensure.That(Cnpj, nameof(Cnpj)).Cnpj();
        }
    }
}
