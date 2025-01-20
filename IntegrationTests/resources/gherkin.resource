*** Settings ***
Resource          ../resources/base.resource

*** Keywords ***
Dado que o sistema esteja funcionando e o banco de dados esteja ativo
    Criar sessão na api

E já exista um contato cadastrado
    Criar contato no banco de dados

Quando eu realizar um "POST" no endpoint "${endpoint}" com os dados inválidos de contato
    Gerar body de request com arquivo selecionado    dados_invalidos
    Realizar POST no endpoint indicado    ${endpoint}    ${body}

Quando eu realizar um "POST" no endpoint "${endpoint}" com os dados válidos de contato
    Gerar body de request com arquivo selecionado    dados_validos
    Realizar POST no endpoint indicado    ${endpoint}    ${body}

Quando eu realizar um "PUT" no endpoint "${endpoint}" com os dados válidos de contato
    Gerar body de request com arquivo selecionado    dados_validos
    Realizar PUT no endpoint indicado    ${endpoint}    ${body}

Quando eu realizar um "DELETE" no endpoint "${endpoint}"
    Realizar DELETE no endpoint indicado    ${endpoint}

Quando eu realizar um "GET" no endpoint "${endpoint}"
    Realizar GET no endpoint indicado    ${endpoint}

Então o sistema retornará o status code "${status_code}" e a mensagem "${message}"
    Validar status code e mensagem no response    ${status_code}    ${message}

Então o sistema retornará o status code "${status_code}" e o response com os dados inputados
    Validar status code e response body conforme dados inputados    ${status_code}

Então o sistema retornará o status code "${status_code}" e o response com os dados do contato
    Validar status code e response body conforme dados inputados    ${status_code}

Então o sistema retornará o status code "204"
    Validar status_code    204

E a tabela "Contact" foi atualizada com o contato cadastrado
    Validar contato cadastrado no banco de dados

E a tabela "Contact" foi atualizada com o contato deletado
    Validar contato deletado no banco de dados
