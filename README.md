# ERP
O objetivo desde projeto é realizar uma prova de conceito de arquitetura de software, com utilização de padrões de projetos e os melhores conceitos de arquitetura

Neste projeto, foram adotadas tecnologias para atender a diferentes propósitos, com a finalidade de desenvolver uma arquitetura técnica que atendesse os requisitos do cliente.
# Escopo Solicitado
![escopo](https://github.com/leanasousa/ERP/blob/master/docs/escopo.png)

## Descrição da Solução
Para o desenvolvimento dos requisitos funcionais solicitados pelo cliente, a visão de implementação esta descrita a seguir:

O cliente irá realizar diariamente os lançamentos do fluxo de caixa no sistema, que podem ser de dois tipos: Débito e Crédito. Cada lançamento tem a data, a descrição do lançamento e o valor que deve ser um valor decimal maior que 0.  Vale destacar que o usuário que está inserindo o lançamento no sistema tem a possibilidade de realizar as cinco operações básicas: Consulta, Listagem, Inclusão, Edição e Exclusão. Para as operações de Inclusão, Edição e Exclusão, uma vez realizado o lançamento com sucesso, o sistema irá gravar no Banco de dados e enviará uma mensagem (mensageria) com os dados do lançamento. Há um serviço em segundo plano (JOB) responsável por receber a mensagem e processar e criar um repositório resumido dos lançamentos, com identificador, data e valor. Esses dados são consumidos posteriormente por outro serviço em segundo plano responsável por consolidar os dados para que seja possível a emissão do relatório diário de lançamento. O processamento da mensagem no serviço de mensageria prevê que a mensagem somente seja removida da fila caso haja sucesso no processamento. No caso de falha, a mensagem é colocada em uma fila de "mensagens não processadas" (Dead letter exchanges - DLXs). Essa fila controla o reenvio da mensagem para a fila principal com intervalo de 30 segundos. 
A emissão do relatório com os dados consolidados diariamente é feita através de serviço específico de emissão de relatório e consultando um banco de dados não relacional. Essa abordagem permite escalar o sistema de modo a obter performanace, isolando operações de gravação de operações de leitura. 

### Fora de Escopo
- Não foi considerado o desenvolvimento de frontend para essa prova de conceito;
- Não foi considerado o desenvolvimento do cadastro de usuário para autorização e autenticação; 

### Sugestão de Melhoria
- Inclusão de microserviço para dupla checagem da consolidação diária de dados, visando a integridade e atomicidade das informações;

## Desenho da Solução
Nesta seção, são apresentados informações que compõem a arquitetura de aplicação, bem como o diagrama do modelo de dados de forma simplificada

### Arquitetura
- Responsibility separation concerns, SOLID, Clean Code
- Messaging
- DDD - Domain Driven Design (Layers and Domain Model Pattern)
- CQRS (Imediate Consistency)
- Repository
- IoC

### Desenho dos serviços - Comunicação
![diagrama2](https://github.com/leanasousa/ERP/blob/master/docs/arquitetura.png)

### Microserviços
| Nome | Objetivo | 
| ------ | ------ | 
| erp.crud.api | Responsável pela gestão dos lançamentos de fluxo de caixa diários no sistema |
| erp.consolidator.api | Responsável pelo consolidação dos dados para o relatório, através da filha de mensagem |
| erp.report.api | Responsável pela emissão do relatório consolidado de lançamentos diário | 
| erp.identity | Responsável pela autenticação e autorização utilizando JWT | 
| erp.gateway | Gateway responsável pela orquestração e centralização dos serviços | 


### Tecnologias implementadas
Nesta seção, são listadas todas as ferramentas e frameworks utilizados no desenvolvimento da solução.

| Descrição | Objetivo | Versão |
| ------ | ------ | ------ |
| RabbitMQ | Serviço de Mensageria | 3.0 |
| MySQL | Sistema de Banco de Dados relacional | 8.0 |
| MongoDB | Sistema de Banco de Dados não relacional | 6.0 |
| .Net Core | Desenvolvimento | 7.0 |
| Entity Framework | ORM | 7.0 |
| Swashbuckle | Documentação | 7.5 |
| AutoMapper | Mapeamento de objetos | 12.0 |
| XUnit | Testes Unitários | 2.4.2 |
| Mock | Testes Unitários | 4.18.4 |
| Ocelot | API Gateway | 19.0 |
| Docker Windows | Implantação de Container | 20.10 |
| Docker Compose | Implantação de Container | 2.10 |

### Autenticação e Autorização
Os dados de usuário para autorização e autenticação devem ser considerados conforme a tabela a seguir:

| E-mail | Senha | Role | Serviços |
| ------ | ------ | ------ | ------ |
| admin@erp.com.br | admin | ADMIN | erp.crud.api - erp.consolidator.api - erp.report.api
| report@erp.com.br | report | REPORT | erp.report.api

### Testes Unitários

A implementação foi realizada através do XUnit e Mock e encontram-se na pasta /test/. 

# Implantação e Execução
## Container
Para implantação e execução da aplicação, é necessário ter o ambiente Docker, bem como o Docker Compose instalado e configurado.
Depois disso, os seguintes comandos devem ser executados no diretório /src/. 
```sh
docker-compose build
docker-compose up
```

## Local
Para executar a aplicação local, sem a necessidade de serviço docker, basta executar o arquivo bat /src/run-localhost.bat

Este arquivo irá executar os serviços em segundo plano.

## URLs
Uma vez executados os comandos com sucesso, será possivel acessar a aplicação através das seguintes URLs:

| Descrição | URL Direta | URL Gateway
| ------ | ------ | ------ |
| erp.crud.api | http://localhost:5020/swagger | http://localhost:5000/gateway/debit / http://localhost:5000/gateway/credit|
| erp.consolidator.api | http://localhost:5040/swagger | http://localhost:5000/gateway/consolidate |
| erp.report.api | http://localhost:5030/swagger | http://localhost:5000/gateway/balance |
| erp.identity | http://localhost:5010/swagger | http://localhost:5000/gateway/auth |
| erp.apigateway | http://localhost:5000 | http://localhost:5000 |

# Enviroment

RabbitMQ - Porta 5672

    Usuário/senha: guest/guest
    
    
Mysql - Porta 3306

    Usuário/senha: root/root
    
    
Mongo - Porta 27017

    Usuário/senha: admin/admin