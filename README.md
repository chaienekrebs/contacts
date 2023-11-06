# Aplicação para gerenciamento de contatos
- **API REST** que gerencia as pessoas e seus contatos, onde permite criar, atualizar, obter e excluir tanto as pessoas quanto os contatos. Uma pessoa pode ter vários contatos, como telefone, e-mail ou whatsapp.

## Funções Pessoa
- Cadastrar Pessoa
- Atualizar Pessoa
- Busca Pessoas
- Busca apenas uma Pessoa
- Excluir Pessoa


| Método HTTP  | Endpoint | Descrição |
| --- | --- | --- |
| POST | /Person | Cadastra uma nova Pessoa |
| PUT | /Person/:id | Atualiza as informações de uma pessoa |
| GET | /Person | Retorna uma lista paginada de pessoas |
| GET | /Person/:id | Retorna uma pessoa específica pelo ID |
| DELETE | /Person/:id | Exclui uma pessoa específica pelo ID |

## Funções Tipo Contato
- Cadastrar Tipo
- Atualizar Tipo
- Busca Tipos
- Busca apenas um Tipo
- Excluir Tipo

| Método HTTP  | Endpoint | Descrição |
| --- | --- | --- |
| POST | /ContactType | Cadastra um novo tipo |
| PUT | /ContactType/:id | Atualiza as informações de um tipo |
| GET | /ContactType | Retorna uma lista paginada de tipos |
| GET | /ContactType/:id | Retorna um tipo específico pelo ID  |
| DELETE | /ContactType/:id | Exclui um tipo específico pelo ID |


## Funções Contato
- Cadastrar Contato
- Atualizar Contato
- Busca Contatos
- Busca apenas um Contato
- Excluir Contato

| Método HTTP  | Endpoint | Descrição |
| --- | --- | --- |
| POST | /Contact | Cadastra um novo contato |
| PUT | /Contact/:id | Atualiza as informações de um contato |
| GET | /Contact | Retorna uma lista paginada de contatos |
| GET | /Contact/:id | Retorna um contato específico pelo ID |
| DELETE | /Contact/:id | Exclui um Contato específico pelo ID |

## Funções Log
- Busca Logs
- Busca apenas um Log

| Método HTTP  | Endpoint | Descrição |
| --- | --- | --- |
| GET | /Log | Retorna uma lista paginada de logs |
| GET | /Log/:id | Retorna as informações do Ip de um log específico pelo ID |

- Integração com a <a href="https://apiip.net/">apiip</a>, para localizar e identificar os IPs dos visitantes.
---

## Próximas Features
- Autenticação
- Ao adicionar um Tipo de Contato ter a opção de definir uma máscara padrão, onde ao inserir um contato daquele tipo faça tal validação. Ex: Telefone, e-mail...
- Front-End

# Sobre o Projeto
## Stack
- .Net 6

## Banco de Dados
- PostgreSQL

## Testes Unitários
- xUnit  
- Padrão AAA
- AutoFixture
- Moq
- Shouldly

## Documentação
- Swagger

## Instruções para rodar o projeto no Visual Studio
- .Net 6 instalado
Definir o projeto da API como projeto de inicialização
```
 CTRL + F5 -> Rodar o projeto 
 F5 -> Debuggar o projeto
```
Com o projeto rodando, abrir a aplicação no IIS:
<p align="center">
 <img src="https://github.com/chaienekrebs/lista-contatos/assets/45368276/33183e26-9542-4a6e-a819-ebdc6eec3785"/>
</p>
Com a aplicação aberta no navegador, a documentação Swagger pode ser visualizada adicioando um /swagger/index.html

## Criando/Atualizando o Banco de Dados com Migration
Ter o postgreSQL instalado e configurado, alterar o ```ContactsDbContext.cs``` com as suas configurações.

- Definir somente a Api para inicializar;
- Abrir o console em: Ferramentas -> Gerenciador de Pacotes Nuget -> console;
- Selecionar a camada de conexão (persistence);
- ```add-migration update-(versão do update);```
- ```update-database ```

