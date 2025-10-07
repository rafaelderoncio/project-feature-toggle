namespace Project.FeatureToggle.Domain.Responses;

/// <summary>
/// Representa o formato padrão de resposta de erro da API.
/// </summary>
/// <remarks>
/// Este record é utilizado para padronizar mensagens de erro retornadas pelos endpoints.  
/// Pode ser usado para erros de validação, não encontrados, ou falhas internas do sistema.
/// 
/// Exemplo de JSON retornado:
/// ```json
/// {
///   "type": "ValidationError",
///   "title": "Erro de Validação",
///   "messages": [
///     "O campo 'Name' é obrigatório.",
///     "A quantidade mínima deve ser maior que zero."
///   ]
/// }
/// ```
/// </remarks>
public record ExceptionResponse
{
    /// <summary>
    /// Identifica o tipo do erro ocorrido.
    /// </summary>
    /// <remarks>
    /// Exemplos de valores:
    /// - "ValidationError" – erro de validação de dados
    /// - "NotFound" – recurso não encontrado
    /// - "ServerError" – erro interno do servidor
    /// </remarks>
    public string Type { get; set; }

    /// <summary>
    /// Mensagem resumida sobre o erro.
    /// </summary>
    /// <remarks>
    /// Deve fornecer um resumo claro e conciso do problema.
    /// Exemplo: "Erro de Validação", "Recurso não encontrado".
    /// </remarks>
    public string Title { get; set; }

    /// <summary>
    /// Lista de mensagens detalhadas que descrevem o erro.
    /// </summary>
    /// <remarks>
    /// Pode conter múltiplos detalhes ou instruções para o usuário ou consumidor da API.
    /// Exemplo:
    /// ["O campo 'Name' é obrigatório.", "A quantidade mínima deve ser maior que zero."]
    /// </remarks>
    public string[] Messages { get; set; }
}
