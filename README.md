# 🚀 Projeto Teste Técnico - Cadastro de Empresas

Olá! Este repositório é a minha entrega para o teste técnico da vaga de Desenvolvedor Fullstack .NET Júnior. Nele, você encontrará uma API para gerenciar o cadastro de empresas de forma eficiente.

## ✨ O que você vai encontrar?

* **Autenticação Robusta:** Usuários podem se cadastrar e fazer login usando tokens JWT e senhas seguras.
* **Cadastro Inteligente de Empresas:** Basta informar o CNPJ e a API se conecta à ReceitaWS para puxar todos os detalhes da empresa.
* **Listagem Paginada:** Visualize suas empresas cadastradas de forma organizada, com paginação para melhor performance.

## 🛠️ Tecnologias Principais

* **Backend:** ASP.NET Core (C#)
* **Banco de Dados:** SQL Server (com Entity Framework Core)
* **Segurança:** JWT e BCrypt para senhas.

## 🚀 Projeto Online!

Para facilitar sua avaliação, a API está hospedada no Azure:

* **Link da API no Azure:** `https://localizeteste-hjadadftcwauaeh4.brazilsouth-01.azurewebsites.net/api`

## 💻 Como Rodar Localmente (e Testar)

### Pré-requisitos

* [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Visual Studio 2022](https://visualstudio.microsoft.com/vs/community/) (Recomendado)
* SQL Server LocalDB (ou outra instância SQL Server)
* [Postman](https://www.postman.com/downloads/) ou [Insomnia](https://insomnia.rest/download/)

### Passos

1.  **Clone o projeto:**
    ```bash
    git clone https://github.com/Alberto-Oliveira00/LocalizeTeste.git
    cd LocalizeTeste

    ```

2.  **Configuração do Banco de Dados e JWT:**
    * No arquivo `appsettings.Development.json`, localize as seções `"ConnectionStrings"` e `"Jwt"`.
    * Para `"ConnectionStrings"`, configure a `DefaultConnection` para apontar para seu **SQL Server LocalDB** local. Exemplo:

        ```json
        "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EmpresasDb;Trusted_Connection=True;MultipleActiveResultSets=true"
        ```
    * Para a seção `"Jwt"`, ajuste `Issuer` e `Audience` para o URL **HTTPS** do seu ambiente de desenvolvimento local (verifique seu `Properties/launchSettings.json`). Exemplo:
        ```json
        "Jwt": {
          "Key": "SuaChaveSecretaMuitoLongaEComplexaParaAssinarTokensJWT",
          "Issuer": "https://localhost:7000",
          "Audience": "https://localhost:7000"
        }
        ```

3.  **Crie e Aplique o Banco de Dados:**
    * Abra o **Package Manager Console** no Visual Studio.
    * Certifique-se de que seu projeto de API está selecionado como "Projeto Padrão".
    * Execute o comando:
        ```powershell
        Update-Database
        ```

4.  **Execute a Aplicação:**
    * No Visual Studio, pressione `F5` para iniciar a API. Ela estará disponível no URL configurado (ex: `https://localhost:7000`).

### Como Testar a API

Use Postman ou Insomnia para interagir com a API.

* **Endpoint Base Local:** `https://localhost:7000/api` (ajuste a porta)
* **Endpoint Base Azure:** `https://localizeteste-hjadadftcwauaeh4.brazilsouth-01.azurewebsites.net/api`

**Fluxo de Teste:**

1.  **Registrar Usuário:**
    * `POST [ENDPOINT_BASE]/Auth/register`
    * **Body:**
        ```json
        {
          "name": "Usuario Teste",
          "email": "usuario@exemplo.com",
          "password": "Senha@123"
        }
        ```

2.  **Fazer Login (Obter Token):**
    * `POST [ENDPOINT_BASE]/Auth/login`
    * **Body:**
        ```json
        {
          "email": "usuario@exemplo.com",
          "password": "Senha@123"
        }
        ```
    * **Ação:** Copie o `token` da resposta.

3.  **Cadastrar Empresa:**
    * `POST [ENDPOINT_BASE]/Companies`
    * **Headers:** `Authorization: Bearer [SEU_TOKEN_JWT_AQUI]`
    * **Body:**
        ```json
        {
          "cnpj": "00.000.000/0001-91" # Use um CNPJ real para teste
        }
        ```

4.  **Listar Empresas (com Paginação):**
    * `GET [ENDPOINT_BASE]/Companies?pageNumber=1&pageSize=5`
    * **Headers:** `Authorization: Bearer [SEU_TOKEN_JWT_AQUI]`

---

## 👋 Contato

Fico à disposição para qualquer dúvida ou para um bate-papo!

* **Alberto Francisco de Oliveira**
* **https://www.linkedin.com/in/alberto-oliveira-1876a1301/**

---
