namespace CalculadoraCusto.Application.DTOs;

public record IngredienteDto(
    Guid Id,
    string Nome,
    string UnidadePadrao,
    bool Ativo,
    DateTime CriadoEm
);

public record CriarIngredienteDto(
    string Nome,
    string UnidadePadrao
);

public record AtualizarIngredienteDto(
    string Nome
);
