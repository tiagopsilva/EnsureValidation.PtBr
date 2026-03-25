using System.Linq;
using EnsureValidation.Validators;

namespace EnsureValidation.PtBR;

/// <summary>
/// Brazilian-specific extension methods for <see cref="StringValidator"/>.
/// Provides CPF and CNPJ format validation.
/// </summary>
public static class StringValidatorExtensions
{
    // Built-in English fallbacks used when PtBRMessages.Configure() has not been called.
    private const string DefaultCpfTemplate  = "The field '{0}' is not a valid CPF.";
    private const string DefaultCnpjTemplate = "The field '{0}' is not a valid CNPJ.";

    /// <summary>
    /// Validates that the string is a valid Brazilian CPF (11 digits, check digits verified).
    /// Skipped if the value is null or empty.
    /// </summary>
    public static StringValidator Cpf(this StringValidator validator, string? message = null)
    {
        var msg = message ?? string.Format(ResolveTemplate("Cpf", DefaultCpfTemplate), validator.FieldName);
        return validator.Must(v => v is null || v.Length == 0 || IsValidCpf(v), msg);
    }

    /// <summary>
    /// Validates that the string is a valid Brazilian CNPJ (14 digits, check digits verified).
    /// Skipped if the value is null or empty.
    /// </summary>
    public static StringValidator Cnpj(this StringValidator validator, string? message = null)
    {
        var msg = message ?? string.Format(ResolveTemplate("Cnpj", DefaultCnpjTemplate), validator.FieldName);
        return validator.Must(v => v is null || v.Length == 0 || IsValidCnpj(v), msg);
    }

    private static string ResolveTemplate(string key, string fallback)
    {
        var template = EnsureValidation.Messages.To(key);
        return template.StartsWith("[") ? fallback : template;
    }

    private static bool IsValidCpf(string cpf)
    {
        if (cpf.Length != 11 || !cpf.All(char.IsDigit))
            return false;

        if (cpf.Distinct().Count() == 1)
            return false;

        var sum = 0;
        for (var i = 0; i < 9; i++)
            sum += (cpf[i] - '0') * (10 - i);

        var remainder = sum % 11;
        var firstDigit = remainder < 2 ? 0 : 11 - remainder;

        if (cpf[9] - '0' != firstDigit)
            return false;

        sum = 0;
        for (var i = 0; i < 10; i++)
            sum += (cpf[i] - '0') * (11 - i);

        remainder = sum % 11;
        var secondDigit = remainder < 2 ? 0 : 11 - remainder;

        return cpf[10] - '0' == secondDigit;
    }

    private static bool IsValidCnpj(string cnpj)
    {
        if (cnpj.Length != 14 || !cnpj.All(char.IsDigit))
            return false;

        if (cnpj.Distinct().Count() == 1)
            return false;

        int[] firstWeights = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        var sum = 0;
        for (var i = 0; i < 12; i++)
            sum += (cnpj[i] - '0') * firstWeights[i];

        var remainder = sum % 11;
        var firstDigit = remainder < 2 ? 0 : 11 - remainder;

        if (cnpj[12] - '0' != firstDigit)
            return false;

        int[] secondWeights = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        sum = 0;
        for (var i = 0; i < 13; i++)
            sum += (cnpj[i] - '0') * secondWeights[i];

        remainder = sum % 11;
        var secondDigit = remainder < 2 ? 0 : 11 - remainder;

        return cnpj[13] - '0' == secondDigit;
    }
}

