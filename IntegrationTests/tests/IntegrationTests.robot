*** Settings ***
Resource          ../resources/base.resource

*** Test Cases ***
Cenário 01 - Criar contato com dados inválidos
    Dado que o sistema esteja funcionando
    Quando eu realizar um "POST" no endpoint "/api/Contact" com os dados inválidos de contato
    Então o sistema retornará o status code "400" e a mensagem "One or more validation errors occurred."

Cenário 02 - Criar contato com dados válidos
    Dado que o sistema esteja funcionando
    Quando eu realizar um "POST" no endpoint "/api/Contact" com os dados válidos de contato
    Então o sistema retornará o status code "201" e o response com os dados inputados
