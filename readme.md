========================================================
DownloaderApp
========================================================

Descrição
--------------------------------------------------------
Este projeto demonstra uma implementação correta de downloads concorrentes em C# utilizando programação assíncrona com async/await, garantindo segurança em ambiente multi-thread e uso eficiente de recursos.

O objetivo foi corrigir problemas comuns relacionados a:

• Concorrência
• Gerenciamento de recursos
• Sincronização


========================================================
PROBLEMAS DA VERSÃO ORIGINAL
========================================================

--------------------------------------------------------
1. Tasks não eram aguardadas corretamente
--------------------------------------------------------

Código problemático:

    DownloadAsync(url);

Problema:

Os downloads eram iniciados, mas o programa não aguardava sua conclusão.

Consequências:

• O programa poderia terminar antes dos downloads finalizarem
• O cache poderia conter menos dados do que o esperado
• Comportamento inconsistente
• Race conditions possíveis


--------------------------------------------------------
2. Uso de coleção não thread-safe
--------------------------------------------------------

Código problemático:

    private static List<string> cache = new List<string>();

Problema:

List<T> não é thread-safe e não pode ser usada com múltiplas threads simultaneamente.

Consequências possíveis:

• Race conditions
• Corrupção de memória
• Exceções em tempo de execução
• Dados inconsistentes
• Comportamento imprevisível


--------------------------------------------------------
3. Criação de múltiplas instâncias de HttpClient
--------------------------------------------------------

Código problemático:

    using HttpClient client = new HttpClient();

Problema:

Criar uma instância de HttpClient por requisição é uma má prática.

Consequências:

• Baixa performance
• Exaustão de conexões TCP 
• Uso ineficiente de recursos do sistema


========================================================
CORREÇÕES APLICADAS
========================================================

--------------------------------------------------------
1. Aguardar todas as Tasks com Task.WhenAll
--------------------------------------------------------

Código corrigido:

    await Task.WhenAll(tasks);

Benefícios:

• Garante que todos os downloads terminem antes da continuação do programa
• Elimina execução incompleta
• Garante consistência de dados
• Permite execução concorrente segura


--------------------------------------------------------
2. Uso de coleção thread-safe (ConcurrentBag)
--------------------------------------------------------

Código corrigido:

    private static readonly ConcurrentBag<string> cache = new();

Benefícios:

• Estrutura thread-safe
• Elimina race conditions
• Suporte nativo a concorrência
• Melhor performance em cenários paralelos
• Segurança em ambiente multi-thread


--------------------------------------------------------
3. Reutilização de HttpClient
--------------------------------------------------------

Código corrigido:

    private static readonly HttpClient client = new();

Benefícios:

• Melhor performance
• Reutilização de conexões HTTP
• Boa prática recomendada pela Microsoft
• Uso eficiente de recursos do sistema


========================================================
RESULTADO FINAL
========================================================

A implementação agora garante:

• Execução concorrente correta
• Thread safety
• Gerenciamento adequado de recursos
• Alta performance
• Código seguro e previsível
