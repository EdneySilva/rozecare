# RozeCare

RozeCare é um MVP de Prontuário Pessoal Unificado (PHR) construído com ASP.NET Core 8, Clean Architecture e React PWA. O projeto prioriza consentimento granular do paciente, auditoria e interoperabilidade inicial inspirada no FHIR.

## Visão Geral da Arquitetura

```
/src
  RozeCare.Domain        -> Entidades de domínio, enums e value objects
  RozeCare.Application   -> Casos de uso (CQRS com MediatR), DTOs e validações
  RozeCare.Infrastructure-> EF Core, Identity, persistência, integrações Azure e serviços
  RozeCare.Api           -> API ASP.NET Core com autenticação JWT, Swagger e Health Checks
  RozeCare.Worker        -> Worker console pronto para rotinas assíncronas futuras
/web
  roze-pwa               -> Aplicação React + Vite + Tailwind (PWA)
/ops
  docker-compose.yml, GitHub Actions, scripts de automação
```

## Principais Funcionalidades

- Registro/login via Identity + JWT (15 minutos) com refresh tokens persistidos.
- Middleware de consentimento validando escopos por paciente e registrando auditoria.
- API REST para perfis, observações, medicamentos, alergias, encontros, documentos (upload Azure Blob), providers, consents, auditoria e endpoint FHIR simplificado (`/api/fhir/Observation`).
- Migração inicial EF Core com índices úteis e seed de dados (paciente, clínico, provider, consent, documentos e registros clínicos).
- Observabilidade com Serilog, Health Checks e OpenTelemetry (console exporter).
- PWA com dashboards, visualização de registros clínicos, gestão de consents e upload de documentos.
- Estrutura pronta para CI/CD (GitHub Actions), docker-compose (API, PostgreSQL, Redis, Azurite, frontend) e deploy em Azure App Service/PostgreSQL/Blob Storage.

## Execução Rápida

### Pré-requisitos

- Docker e Docker Compose
- Node 18+ (para desenvolvimento frontend local)
- .NET SDK 8 (opcional se for compilar fora dos contêineres)

### Variáveis de ambiente

Crie um arquivo `.env` na raiz da API com os valores necessários (exemplo abaixo). Para desenvolvimento via docker-compose estes valores já são usados:

```env
ConnectionStrings__Default=Host=roze-db;Port=5432;Database=rozecare;Username=postgres;Password=postgres
Jwt__Authority=https://localhost
Jwt__Audience=roze-api
Jwt__SigningKey=local-development-signing-key-please-change
Azure__Blob__ConnectionString=UseDevelopmentStorage=true;
Azure__Blob__Container=roze-docs
Redis__Connection=roze-redis:6379
```

Para o frontend copie `.env.example` dentro de `web/roze-pwa` e ajuste `VITE_API_BASE_URL` para o endpoint da API.

### Docker Compose

```bash
docker compose -f ops/docker-compose.yml up --build
```

Serviços expostos por padrão:

- API: http://localhost:5000 (Swagger em `/swagger`)
- Frontend PWA: http://localhost:5173
- PostgreSQL: localhost:5432 (usuario `postgres` / senha `postgres`)
- Redis: localhost:6379
- Azurite Blob: localhost:10000-10001

O seed cria os seguintes usuários:

| Papel     | Email                    | Senha         |
|-----------|--------------------------|---------------|
| Paciente  | patient@rozecare.test    | `P@ssword123!`|
| Clínico   | clinician@rozecare.test  | `P@ssword123!`|

### Desenvolvimento Local

#### Backend

```bash
cd src/RozeCare.Api
# dotnet restore   (se possuir .NET SDK instalado)
# dotnet run
```

#### Frontend

```bash
cd web/roze-pwa
npm install
npm run dev
```

## Migrações e Banco de Dados

As migrações EF Core estão em `src/RozeCare.Infrastructure/Migrations`.

Para aplicar manualmente (com .NET instalado):

```bash
cd src/RozeCare.Api
dotnet ef database update
```

## Testes

- Backend: xUnit + FluentAssertions (estrutura preparada em `tests/` para expansão).
- Frontend: Vitest + React Testing Library (`npm run test`).

## Segurança e Compliance

- Tokens JWT curtos + refresh tokens revogáveis.
- RBAC básico via roles Identity.
- Middleware de consentimento auditando todas as tentativas de acesso.
- Logs com mascaramento de PII via Serilog.
- Rate limiting configurável via Redis (placeholder para ampliação).

## Próximos Passos

- Expandir coverage de testes (domínio e handlers) para >= 70%.
- Implementar fila/background jobs para notificações de expiração de consentimentos.
- Completar interoperabilidade FHIR (Patient, Medication, Observation, DocumentReference).
- Automatizar build/deploy com GitHub Actions (`ops/github-actions`).
- Melhorar suporte offline na PWA (cache seletivo de dados críticos).

## Licença

Projeto disponibilizado apenas para fins de demonstração do MVP.
