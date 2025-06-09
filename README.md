# 📊 Cloth Reports – API para Relatórios de Vendas de Roupas

Este projeto é uma **API em ASP.NET Core (Minimal API)** voltada para o controle de **produtos, vendas e lojas** de um sistema de gerenciamento de roupas. O objetivo principal é **gerar relatórios precisos de vendas**, possibilitando análises por loja, produto, marca, tamanho, entre outros filtros. Baseado, no caso real da minha sogra que necessitava de um produto que contemple esses processos. Tendo em vista que ela vende roupas para diferentes lojas e precisava de um relatório preciso. Aprendi muito, principalmente a levantar requisitos, testar diretamente com o cliente e adequar o produto as reais necessidades do mesmo.

---

## ⚙️ Tecnologias Utilizadas

- ASP.NET Core 8 (Minimal API)
- PostgreSQL
- Dapper
- Swagger

## ✅ Funcionalidades

### 📦 Produtos
- Cadastro, edição e exclusão
- Associação com marca, tipo e tamanho
- Relacionamento com lojas

### 🏬 Lojas
- Cadastro e gerenciamento
- Produtos por loja

### 💰 Vendas
- Registro de vendas com múltiplos produtos
- Associação com loja e data
- Quantidade vendida por item

### 📈 Relatórios
- Vendas por loja
- Produtos mais vendidos
- Agrupamento por filtros diversos

---

## ▶️ Como Executar

### Pré-requisitos

- .NET 8 SDK
- PostgreSQL

### Passos

```bash
git clone https://github.com/IgorGhiberti/cloth-reports.git
cd cloth-reports
dotnet run
