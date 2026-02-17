DownloaderApp

Este projeto demonstra uma implementação correta de downloads concorrentes em C# utilizando programação assíncrona com async/await, garantindo segurança em ambiente multi-thread e uso eficiente de recursos.

O objetivo foi corrigir problemas comuns relacionados a concorrência, gerenciamento de recursos e sincronização.


PROBLEMAS DA VERSÃO ORIGINAL

1. Tasks não eram aguardadas corretamente

Código problemático:

DownloadAsync(url);

Isso fazia com que os downloads fossem iniciados, mas o programa continuasse executando sem esperar sua conclusão.

Consequências:

- O programa poderia terminar antes dos downloads finalizarem
- O cache poderia conter menos dados do que o esperado
- Comportamento inconsistente


2. Uso de coleção não thread-safe

Código problemático:

private static List<string> cache = new List<string>();

List<T> não é segura para acesso concorrente.

Consequências possíveis:

- Race conditions
- Corrupção de memória
- Exceções em tempo de execução
- Dados inconsistentes


3. Criação de múltiplas instâncias de HttpClient

Código problemático:

using HttpClient client = new HttpClient();

Criar uma instância de HttpClient por requisição pode causar:

- Alto overhead
- Baixa performance
- Exaustão de conexões TCP


CORREÇÕES APLICADAS

1. Aguardar todas as Tasks com Task.WhenAll

Código corrigido:

await Task.WhenAll(tasks);

Benefícios:

- Garante que todos os downloads terminem antes de continuar
- Evita execução incompleta
- Comportamento previsível


2. Uso de coleção thread-safe (ConcurrentBag)

Código corrigido:

private static readonly ConcurrentBag<string> cache = new();

Benefícios:

- Seguro para acesso concorrente
- Elimina race conditions
- Melhor performance em cenários paralelos


3. Reutilização de HttpClient

Código corrigido:

private static readonly HttpClient client = new();

Benefícios:

- Melhor performance
- Evita socket exhaustion
- Reutiliza conexões HTTP
- Boa prática recomendada pela Microsoft