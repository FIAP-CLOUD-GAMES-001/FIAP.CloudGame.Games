# FIAP CloudGames - Games API

API REST para gerenciamento de games, pedidos e pagamentos desenvolvida em .NET 8.

## 📋 Sobre

Sistema de e-commerce de games que permite cadastrar jogos, criar pedidos, processar pagamentos e gerenciar promoções.  
A API integra com o microserviço de pagamentos via **HTTP** e consome **notificações assíncronas de status de pagamento via RabbitMQ**, utilizando autenticação JWT.

## 🏗️ Arquitetura

Aplicação seguindo Clean Architecture com separação em camadas:

- **API**: Controllers, Middlewares, Filters e Configurações
- **Domain**: Entidades, Interfaces, Enums, Exceptions e Models
- **Service**: Lógica de negócio e Validações (FluentValidation)
- **Infrastructure**: Repositórios, DataContext, Migrations e Consumo de Mensageria

## 🛠️ Tecnologias

- **.NET 8.0**
- **Entity Framework Core** (SQL Server)
- **JWT Bearer Authentication**
- **FluentValidation**
- **Serilog** (MongoDB para logs)
- **Swagger/OpenAPI**
- **Health Checks**
- **RabbitMQ**
- **Docker / Kubernetes (AKS)**

## ⚙️ Configuração

### Pré-requisitos

- .NET 8 SDK
- SQL Server
- MongoDB (para logs)
- RabbitMQ

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=CloudGamesGames;...",
    "MongoDB": "mongodb://.../logsdb"
  },
  "Jwt": {
    "Key": "...",
    "Issuer": "FIAP.CloudGames",
    "Audience": "FIAP.CloudGames"
  },
  "PaymentService": {
    "BaseAddress": "http://localhost:5286"
  }
}
````

### Executando

```bash
# Restaurar dependências
dotnet restore

# Aplicar migrations
dotnet ef database update --project FIAP.CloudGames.Games.Infrastructure --startup-project FIAP.CloudGames.Games.Api

# Executar
cd FIAP.CloudGames.Games.Api
dotnet run
```

A API estará disponível em: `https://localhost:5001` ou `http://localhost:5000`

## 📚 Endpoints Principais

### 🎮 Games (`/api/Game`)

* `GET /api/Game` - Lista todos os games
* `POST /api/Game` - Cria um novo game (requer autenticação)
* `GET /api/Game/{id}/recommendations` - Recomendações de games
* `GET /api/Game/metrics` - Métricas de games

### 🛒 Orders (`/api/Order`)

* `GET /api/Order` - Lista todos os pedidos
* `GET /api/Order/{id}` - Obtém pedido por ID
* `GET /api/Order/user/{userId}` - Lista pedidos do usuário
* `GET /api/Order/available-games` - Lista games disponíveis
* `POST /api/Order` - Cria novo pedido (requer autenticação)
* `PUT /api/Order` - Atualiza status do pedido (requer autenticação)
* `POST /api/Order/payment-notification` - Recebe notificação de pagamento (público)

### 💳 Payments (`/api/Payment`)

* `GET /api/Payment` - Lista todos os pagamentos
* `GET /api/Payment/{id}` - Obtém pagamento por ID
* `GET /api/Payment/order/{orderId}` - Lista pagamentos por pedido

### 🎁 Promotions (`/api/Promotion`)

* `GET /api/Promotion` - Lista todas as promoções
* `POST /api/Promotion` - Cria nova promoção (requer autenticação)

## 🔐 Autenticação

A API utiliza JWT Bearer Token. Para acessar endpoints protegidos:

```
Authorization: Bearer {seu_token_jwt}
```

## 📦 Fluxo de Pedido e Pagamento

1. **Criar Pedido**: `POST /api/Order`

   * Recebe: `games[]`, `userId`, `paymentMethod`
   * Cria o pedido com status `Progress`
   * Envia requisição **HTTP** para o microserviço de pagamentos
   * Salva o pagamento com status `Pending`

2. **Notificação de Pagamento (Assíncrona)**:

   * O microserviço de pagamentos publica o status no RabbitMQ
   * A Games API consome a notificação
   * Atualiza status do pagamento (`Pending`, `Processing`, `Approved`, `Rejected`)
   * Atualiza status do pedido:

     * `Approved` → `Authored`
     * `Rejected` → `Unauthorized`
     * `Processing` → `Progress`

## 📊 Status

### Status de Pedido (`EOrderStatus`)

* `Created` - Pedido criado
* `Progress` - Em processamento
* `Authored` - Pagamento autorizado
* `Unauthorized` - Pagamento rejeitado

### Status de Pagamento (`EPaymentStatus`)

* `Pending` (0) - Aguardando processamento
* `Processing` (1) - Processando
* `Approved` (2) - Aprovado
* `Rejected` (3) - Rejeitado

### Métodos de Pagamento (`EPaymentMethod`)

* `CreditCard`, `DebitCard`, `Pix`, `Boleto`, `GiftCard`

## 🧪 Swagger

Documentação interativa disponível em:

* Swagger UI: `/swagger`
* JSON: `/swagger/v1/swagger.json`

## 🏥 Health Check

Endpoint de saúde da aplicação:

* `GET /health` - Verifica conexão com SQL Server

## 📝 Logs

Logs são armazenados no MongoDB usando Serilog com enriquecimento de informações do usuário autenticado.

## 🐳 Docker

A aplicação inclui `Dockerfile` e `docker-compose.yaml` para containerização e execução em ambiente Kubernetes.

## 📄 Licença

Projeto desenvolvido para FIAP.