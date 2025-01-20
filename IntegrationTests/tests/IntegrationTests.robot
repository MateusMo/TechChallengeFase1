*** Settings ***
Resource          ../resources/base.resource
Test Setup        Setup

*** Test Cases ***
Cenário 01 - Criar contato com dados inválidos
    [Tags]    Cenario01
    Dado que o sistema esteja funcionando e o banco de dados esteja ativo
    Quando eu realizar um "POST" no endpoint "/api/Contact" com os dados inválidos de contato
    Então o sistema retornará o status code "400" e a mensagem "One or more validation errors occurred."

Cenário 02 - Criar contato com dados válidos e validar no banco de dados
    [Tags]    Cenario02
    Dado que o sistema esteja funcionando e o banco de dados esteja ativo
    Quando eu realizar um "POST" no endpoint "/api/Contact" com os dados válidos de contato
    Então o sistema retornará o status code "201" e o response com os dados inputados
    E a tabela "Contact" foi atualizada com o contato cadastrado

Cenário 03 - Editar um contato criado anteriormente
    [Tags]    Cenario03
    Dado que o sistema esteja funcionando e o banco de dados esteja ativo
    E já exista um contato cadastrado
    Quando eu realizar um "PUT" no endpoint "/api/Contact/${CONTACT_ID}" com os dados válidos de contato
    # Então o sistema retornará o status code "200" e o response com os dados inputados
    # E a tabela "Contact" foi atualizada com o contato editado

Cenário 04 - Deletar um contato criado anteriormente
    [Tags]    Cenario04
    Dado que o sistema esteja funcionando e o banco de dados esteja ativo
    E já exista um contato cadastrado
    Quando eu realizar um "DELETE" no endpoint "/api/Contact/${CONTACT_ID}"
    Então o sistema retornará o status code "204"
    E a tabela "Contact" foi atualizada com o contato deletado

Cenário 05 - Buscar um contato criado anteriormente
    [Tags]    Cenario05
    Dado que o sistema esteja funcionando e o banco de dados esteja ativo
    E já exista um contato cadastrado
    Quando eu realizar um "GET" no endpoint "/api/Contact/GetByID/${CONTACT_ID}"
    Então o sistema retornará o status code "200" e o response com os dados do contato
