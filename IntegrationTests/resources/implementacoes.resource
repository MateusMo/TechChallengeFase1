*** Settings ***
Resource          ../resources/base.resource

*** Keywords ***
Setup
    Deletar contatos cadastrados via banco de dados

Teardown
    Deletar contatos cadastrados via banco de dados

Criar sessão na api
    ${HEADERS}    Create Dictionary    Accept=*/*    Content-Type=application/json
    Create Session    ContactZone    http://localhost:8080    ${HEADERS}    debug=2

Validar banco de dados ativo

Gerar body de request com arquivo selecionado
    [Arguments]    ${file}
    ${body}    OperatingSystem.Get File    ${CURDIR}/../data/inputs/${file}.json
    Log    ${body}
    Set Test Variable    ${body}

Realizar POST no endpoint indicado
    [Arguments]    ${endpoint}    ${body}
    ${response}    POST On Session    ContactZone    ${endpoint}    ${body}    expected_status=any
    Set Test Variable    ${response}
    Log    ${response.json()}

Realizar PUT no endpoint indicado
    [Arguments]    ${endpoint}    ${body}
    ${response}    PUT On Session    ContactZone    ${endpoint}    ${body}    expected_status=any
    Set Test Variable    ${response}
    # Log    ${response.json()}

Realizar DELETE no endpoint indicado
    [Arguments]    ${endpoint}
    ${response}    DELETE On Session    ContactZone    ${endpoint}    expected_status=any
    Set Test Variable    ${response}
    # Log    ${response.json()}

Realizar GET no endpoint indicado
    [Arguments]    ${endpoint}
    ${response}    GET On Session    ContactZone    ${endpoint}    expected_status=any
    Set Test Variable    ${response}
    Log    ${response.json()}

Validar status code e mensagem no response
    [Arguments]    ${status_code}    ${message}
    Should Be Equal As Strings    ${response.status_code}    ${status_code}
    Should Be Equal As Strings    ${response.json()["title"]}    ${message}

Validar status code e response body conforme dados inputados
    [Arguments]    ${status_code}
    Should Be Equal As Strings    ${response.status_code}    ${status_code}
    ${body}    Convert String to JSON    ${body}
    Dictionary Should Contain Sub Dictionary    ${response.json()}    ${body}

Validar status_code
    [Arguments]    ${status_code}
    Should Be Equal As Strings    ${response.status_code}    ${status_code}

Validar contato cadastrado no banco de dados
    Buscar contatos cadastrados no banco de dados
    Should Not Be Empty    ${DB_QUERY_RESULT}

Validar contato deletado no banco de dados
    Buscar contatos cadastrados no banco de dados
    Should Be Empty    ${DB_QUERY_RESULT}

Criar contato no banco de dados
    Gerar body de request com arquivo selecionado    dados_validos
    ${response}    POST On Session    ContactZone    /api/Contact    ${body}    expected_status=any
    Set Test Variable    ${response}
    Set Test Variable    ${CONTACT_ID}    ${response.json()["id"]}
    Log    ${CONTACT_ID}
