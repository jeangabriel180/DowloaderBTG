# DownloaderApp

## Visão geral

Este projeto apresenta a análise e correção de um código que realiza downloads HTTP utilizando múltiplas operações assíncronas em C#.

O objetivo foi identificar problemas de concorrência, gerenciamento de recursos, sincronização e uso incorreto de programação assíncrona, e implementar
uma solução correta, determinística e eficiente.

---

# Problemas identificados no código original

---

## 1️⃣ Tasks não eram aguardadas corretamente

### Onde

As chamadas assíncronas eram iniciadas sem aguardar sua conclusão:

```csharp
DownloadAsync(url);
```

### ⚠ Impacto

- O programa continuava executando antes da conclusão dos downloads
- O cache poderia conter menos elementos que o esperado
- Resultados inconsistentes
- Execução não determinística

### Correção

Armazenamento das Tasks e uso de `Task.WhenAll` para aguardar sua conclusão:

```csharp
await Task.WhenAll(tasks);
```

Benefícios:

- Execução determinística
- Garantia de conclusão de todas as operações
- Comportamento previsível

---

## 2️⃣ Uso de coleção não thread-safe (race condition)

### Onde

```csharp
private static List<string> cache = new List<string>();
```

E múltiplas operações concorrentes executando:

```csharp
cache.Add(data);
```

### ⚠ Impacto

`List<T>` não é thread-safe e pode causar:

- Race conditions
- Corrupção interna da estrutura
- Exceções intermitentes
- Dados inconsistentes

### Correção

Substituição por `ConcurrentBag<string>`:

```csharp
private static readonly ConcurrentBag<string> cache = new();
```

Benefícios:

- Thread-safe por design
- Suporte nativo a concorrência
- Elimina necessidade de sincronização manual
- Melhor performance em cenários concorrentes

---

## 3️⃣ Criação de múltiplas instâncias de HttpClient

### Onde

```csharp
using HttpClient client = new HttpClient();
```

Executado dentro do método de download.

### ⚠ Impacto

- Baixa performance
- Alto overhead de criação de conexões
- Exaustão de conexões TCP (Socket exhaustion)
- Uso ineficiente de recursos do sistema

### Correção

Reutilização de uma única instância de HttpClient:

```csharp
private static readonly HttpClient client = new();
```

Benefícios:

- Reutilização de conexões HTTP
- Melhor performance
- Uso eficiente de recursos
- Boa prática recomendada pela Microsoft

---

# Solução implementada

A solução final utiliza:

- Programação assíncrona com async/await
- Task.WhenAll para sincronização das operações
- ConcurrentBag para armazenamento thread-safe
- Instância única reutilizável de HttpClient
- Execução concorrente segura e eficiente

---

---

# Resultado final

A implementação agora garante:

- Execução concorrente correta
- Thread safety
- Gerenciamento adequado de recursos
- Execução determinística
- Alta performance
- Uso correto de programação assíncrona em C#

---