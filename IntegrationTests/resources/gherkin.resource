*** Settings ***
Resource          ../resources/base.resource

*** Keywords ***
Dado que o sistema esteja funcionando
    Criar sessão na api

Quando eu realizar um "POST" no endpoint "${endpoint}" com os dados inválidos de contato
    Gerar body de request com arquivo selecionado    dados_invalidos
    Realizar POST no endpoint indicado    ${endpoint}    ${body}

Então o sistema retornará o status code "${status_code}" e a mensagem "${message}"
    Validar status code e mensagem no response    ${status_code}    ${message}
