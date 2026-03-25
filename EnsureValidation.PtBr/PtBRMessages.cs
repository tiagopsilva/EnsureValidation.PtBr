namespace EnsureValidation.PtBR;

/// <summary>
/// Provides Brazilian Portuguese (pt-BR) translations for all EnsureValidation default messages,
/// including CPF and CNPJ specific messages.
/// Call <see cref="Configure"/> at application startup to switch to pt-BR messages.
/// </summary>
public static class PtBRMessages
{
    private static readonly Dictionary<string, string> Messages = new()
    {
        // General
        { "Required",               "O campo '{0}' é obrigatório." },
        { "NotEmpty",               "O campo '{0}' não pode ser vazio." },
        { "Must",                   "O campo '{0}' é inválido." },

        // String
        { "ExactLength",            "O campo '{0}' deve ter exatamente {1} caracteres." },
        { "ExactTrimmedLength",     "O campo '{0}' deve ter exatamente {1} caracteres (sem espaços nas extremidades)." },
        { "MinLength",              "O campo '{0}' deve ter no mínimo {1} caracteres." },
        { "MaxLength",              "O campo '{0}' deve ter no máximo {1} caracteres." },
        { "Matches",                "O campo '{0}' está em um formato inválido." },
        { "EmailAddress",           "O campo '{0}' não é um endereço de e-mail válido." },

        // CPF / CNPJ (PtBR package extensions)
        { "Cpf",                    "O campo '{0}' não contém um CPF válido." },
        { "Cnpj",                   "O campo '{0}' não contém um CNPJ válido." },

        // Numeric (Int, Long, Decimal, Double)
        { "EqualTo",                "O campo '{0}' deve ser igual a {1}." },
        { "NotEqualTo",             "O campo '{0}' não deve ser igual a {1}." },
        { "GreaterThan",            "O campo '{0}' deve ser maior que {1}." },
        { "GreaterThanOrEqualTo",   "O campo '{0}' deve ser maior ou igual a {1}." },
        { "LessThan",               "O campo '{0}' deve ser menor que {1}." },
        { "LessThanOrEqualTo",      "O campo '{0}' deve ser menor ou igual a {1}." },
        { "Between",                "O campo '{0}' deve estar entre {1} e {2}." },

        // DateTime
        { "After",                  "O campo '{0}' deve ser posterior a {1}." },
        { "Before",                 "O campo '{0}' deve ser anterior a {1}." },
        { "InTheFuture",            "O campo '{0}' deve ser uma data futura." },
        { "InThePast",              "O campo '{0}' deve ser uma data passada." },

        // Boolean
        { "IsTrue",                 "O campo '{0}' deve ser verdadeiro." },
        { "IsFalse",                "O campo '{0}' deve ser falso." },

        // Enum
        { "InEnum",                 "O campo '{0}' deve conter um valor válido." },
    };

    /// <summary>
    /// Configures EnsureValidation to use Brazilian Portuguese messages.
    /// Call this once at application startup, before any validation is performed.
    /// </summary>
    public static void Configure()
    {
        EnsureValidation.Messages.To = key =>
            Messages.GetValueOrDefault(key, $"[Nenhuma mensagem registrada para '{key}']");
    }
}
