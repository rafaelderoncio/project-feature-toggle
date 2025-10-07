# Project.FeatureToggle

Projeto de Feature Toggle (Feature Flags) para gerenciamento de funcionalidades da aplicação de forma dinâmica, permitindo ativar ou desativar recursos sem necessidade de deploy.

---

## 📂 Estrutura do Projeto

```

.
├── Project.FeatureToggle.Api
├── Project.FeatureToggle.Core
├── Project.FeatureToggle.Domain
├── Project.FeatureToggle.Tests
└── Project.FeatureToggle.sln

````

---

### **Project.FeatureToggle.Api**

- Projeto principal da API REST.
- Contém controllers, configuração de middleware, e endpoints de gerenciamento de features.
- Arquivos importantes:
  - `Program.cs` → ponto de entrada da aplicação.
  - `appsettings.json` / `appsettings.Development.json` → configurações da aplicação.
  - `Controllers/` → endpoints expostos:
    - `FeatureDashboardController` → dashboard com métricas de features.
    - `FeatureManagerController` → CRUD de features.
    - `FeatureToggleController` → ativação/desativação de toggles.
  - `Project.FeatureToggle.Api.http` → coleção de exemplos de requisições HTTP (pode ser usada no VSCode REST Client).

---

### **Project.FeatureToggle.Core**

- Contém a **lógica de negócio** e serviços da aplicação.
- Pastas importantes:
  - `Services/` → interfaces e implementações de serviços (`IFeatureManagerService`, `IFeatureDashboardService`, etc.).
  - `Repositories/` → interfaces e implementações de repositórios para persistência (`IFeatureRepository`).
  - `Middlewares/` → middlewares personalizados, por exemplo para tratamento de erros.
  - `Exceptions/` → exceções customizadas do domínio.
  - `Arguments/` → classes para filtros e argumentos de consulta.
  - `Configurations/` → classes de configuração.
  - `Extensions/` → extensões para objetos, coleções e serviços.

---

### **Project.FeatureToggle.Domain**

- Contém **modelos de dados e DTOs** da aplicação.
- Pastas importantes:
  - `Requests/` → DTOs de requisição (`FeatureRequest`, `FeatureQueryRequest`).
  - `Responses/` → DTOs de resposta (`FeatureResponse`, `PaginationResponse`, `ExceptionResponse`, `FeatureDashboardResponse`).
  - `Constants/` → constantes do domínio, por exemplo filtros de features (`FeatureFilter`).

---

### **Project.FeatureToggle.Tests**

- Projeto de testes unitários.
- Contém testes para serviços, repositórios e controllers.
- Arquivos importantes:
  - `UnitTest1.cs` → exemplo de teste unitário.
- Ferramentas sugeridas:
  - xUnit
  - Moq ou NSubstitute (para mocks)
  - FluentAssertions (para asserções mais legíveis)

---

## 🔧 Tecnologias e Pacotes

- .NET 7/8 (ou versão compatível com seu projeto)
- ASP.NET Core Web API
- Entity Framework Core / qualquer ORM utilizado
- Swagger / OpenAPI para documentação da API
- Redis / MemoryCache (opcional para `ICacheService`)
- xUnit para testes

---

## 🚀 Funcionalidades

- Gerenciamento de feature toggles (CRUD)
- Dashboard de métricas de features (total ativas, inativas, etc.)
- Ativação e desativação de features dinamicamente
- Suporte a paginação e filtros em consultas
- Respostas de erro padronizadas (`ExceptionResponse`)
- Serviços de cache genéricos (`ICacheService`)

---

## ⚡ Exemplos de Endpoints

- `GET /api/feature/dashboard` → retorna métricas do dashboard.
- `GET /api/feature/manager` → lista de features paginada.
- `GET /api/feature/manager/{id}` → detalhes de uma feature.
- `POST /api/feature/manager` → cria uma nova feature.
- `PUT /api/feature/manager/{id}` → atualiza uma feature.
- `DELETE /api/feature/manager/{id}` → remove uma feature.
- `GET /api/feature/toggle/{name}` → consulta o estado de um toggle.
- `PUT /api/feature/toggle/{name}` → alterna o estado de um toggle.

---

## 📦 Como Rodar o Projeto

1. Clone o repositório:
```bash
git clone <URL_DO_REPO>
cd Project.FeatureToggle
````

2. Restaure os pacotes:

```bash
dotnet restore
```

3. Build da solução:

```bash
dotnet build
```

4. Rodar a API:

```bash
cd Project.FeatureToggle.Api
dotnet run
```

5. Testes:

```bash
cd Project.FeatureToggle.Tests
dotnet test
```

6. Acesse a documentação Swagger (se configurada):

```
https://localhost:<porta>/swagger
```

---

## 📖 Observações

* Recomenda-se usar **Dependency Injection** para os serviços e repositórios.
* As respostas de API seguem padrões de **JSON** com DTOs de Request e Response.
* Para testes locais, use **InMemoryDatabase** ou mocks para os repositórios.
* Suporte a **filtros e paginação** para listagem de features.

---

## 📝 Licença

Este projeto está licenciado sob a **MIT License**. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.
