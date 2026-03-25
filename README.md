# EnsureValidation.PtBR

[![NuGet](https://img.shields.io/nuget/v/EnsureValidation.PtBR.svg)](https://www.nuget.org/packages/EnsureValidation.PtBR)
[![CI](https://github.com/tiagosilva/EnsureValidation/actions/workflows/ci.yml/badge.svg)](https://github.com/tiagosilva/EnsureValidation/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

Extensão do `EnsureValidation` com suporte a **mensagens em Português (pt-BR)** e **validadores de documentos brasileiros** (CPF e CNPJ).

## O que é EnsureValidation.PtBR?

`EnsureValidation.PtBR` é um pacote complementar ao [EnsureValidation](https://www.nuget.org/packages/EnsureValidation) que fornece:

- ✅ **Todas as mensagens de validação traduzidas para Português (pt-BR)**
- ✅ **Validadores específicos para documentos brasileiros** — CPF e CNPJ
- ✅ **Sem reflexão, sem alocações desnecessárias**
- ✅ **Zero overhead** — extensões de método simples

## Instalação

```bash
dotnet add package EnsureValidation.PtBR
```

> **Requisito**: você já deve ter `EnsureValidation` instalado.

```bash
dotnet add package EnsureValidation
```

## Quick Start

### 1. Configure mensagens em Português

```csharp
using EnsureValidation.PtBR;

// Na inicialização da sua aplicação (Program.cs, Startup.cs, etc)
PtBRMessages.Configure();
```

E pronto! Todas as mensagens de validação passarão a ser exibidas em Português.

### 2. Use validadores de CPF e CNPJ

```csharp
using EnsureValidation;
using EnsureValidation.PtBR;

public class Company : Notifiable<Company>
{
    public string? Name { get; set; }
    public string? Cnpj { get; set; }

    protected override void OnValidate()
    {
        PtBRMessages.Configure();

        Ensure.That(Name, nameof(Name))
            .Required()
            .MinLength(3)
            .MaxLength(150);

        Ensure.That(Cnpj, nameof(Cnpj))
            .Required()
            .Cnpj();  // ← Validador específico do PtBR
    }
}
```

### 3. Validar

```csharp
var company = new Company { Name = "AB", Cnpj = "12345678901234" };
company.Validate();

if (company.IsInvalid)
{
    foreach (var notification in company.Notifications)
        Console.WriteLine($"{notification.Field}: {notification.Message}");

    // Saída esperada:
    // Name: O campo 'Name' deve ter no mínimo 3 caracteres.
    // Cnpj: O campo 'Cnpj' não contém um CNPJ válido.
}
```

## Mensagens Disponíveis

Quando `PtBRMessages.Configure()` é chamado, as seguintes mensagens são ativadas em Português:

| Validador | Mensagem (pt-BR) |
|-----------|------------------|
| `Required()` | O campo '{0}' é obrigatório. |
| `MinLength(n)` | O campo '{0}' deve ter no mínimo {1} caracteres. |
| `MaxLength(n)` | O campo '{0}' deve ter no máximo {1} caracteres. |
| `ExactLength(n)` | O campo '{0}' deve ter exatamente {1} caracteres. |
| `EmailAddress()` | O campo '{0}' não é um endereço de e-mail válido. |
| `Cpf()` | O campo '{0}' não contém um CPF válido. |
| `Cnpj()` | O campo '{0}' não contém um CNPJ válido. |
| `GreaterThan(n)` | O campo '{0}' deve ser maior que {1}. |
| `Between(min, max)` | O campo '{0}' deve estar entre {1} e {2}. |
| `IsTrue()` | O campo '{0}' deve ser verdadeiro. |
| `IsFalse()` | O campo '{0}' deve ser falso. |

E muitas outras! Veja a lista completa em [PtBRMessages.cs](EnsureValidation.PtBr/PtBRMessages.cs).

## Validadores Brasileiros

### CPF

Valida se a string é um CPF válido (11 dígitos + check digits):

```csharp
Ensure.That(cpf, nameof(cpf))
    .Required()
    .Cpf();
```

**Exemplos válidos**: `11144477735`, `52998222060`
**Exemplos inválidos**: `11111111111` (todos os dígitos iguais), `12345678901` (dígitos verificadores inválidos)

### CNPJ

Valida se a string é um CNPJ válido (14 dígitos + check digits):

```csharp
Ensure.That(cnpj, nameof(cnpj))
    .Required()
    .Cnpj();
```

**Exemplos válidos**: `11222333000181`, `34028316000152`
**Exemplos inválidos**: `11111111111111` (todos os dígitos iguais), `12345678901234` (dígitos verificadores inválidos)

> **Nota**: Tanto `Cpf()` quanto `Cnpj()` pulam a validação se o valor for `null` ou vazio. Se você quer rejeitar valores vazios, use `.Required()` antes.

## Estrutura do Projeto

```
EnsureValidation.PtBr/
├── EnsureValidation.PtBr/
│   ├── PtBRMessages.cs              # Mensagens em português
│   ├── StringValidatorExtensions.cs # Validadores de CPF/CNPJ
│   └── EnsureValidation.PtBR.csproj
├── EnsureValidation.PtBr.Tests/
│   ├── PtBRMessagesTests.cs
│   ├── StringValidatorExtensionsTests.cs
│   └── EnsureValidation.PtBr.Tests.csproj
├── EnsureValidation.PtBr.slnx
└── README.md
```

## Executando os Testes

```bash
# Build
dotnet build

# Testes
dotnet test

# Testes com cobertura de código
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

## Compatibilidade

- ✅ .NET 8.0
- ✅ .NET 9.0

## Contribuindo

1. Faça um fork do repositório
2. Crie uma branch de feature (`git checkout -b feature/nova-feature`)
3. Escreva testes para suas mudanças
4. Garanta que todos os testes passam (`dotnet test`)
5. Abra um Pull Request

## Suporte

Para reportar bugs, sugerir melhorias ou fazer perguntas, abra uma [issue no GitHub](https://github.com/tiagosilva/EnsureValidation/issues).

## Licença

Este projeto está licenciado sob a MIT License — veja o arquivo [LICENSE](../LICENSE) para detalhes.

## Relacionados

- [EnsureValidation](https://www.nuget.org/packages/EnsureValidation) - Biblioteca principal
- [FluentValidation](https://fluentvalidation.net/) - Alternativa com suporte a reflexão
