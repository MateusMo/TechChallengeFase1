*** Settings ***
Resource          ../resources/base.resource
Library           Collections
Library           JSONLibrary

*** Keywords ***
Criar sessão na api
    ${HEADERS}    Create Dictionary    Accept=*/*    Content-Type=application/json
    Create Session    ContactZone    http://localhost:8080    ${HEADERS}    debug=2

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

Validar status code e mensagem no response
    [Arguments]    ${status_code}    ${message}
    Should Be Equal As Strings    ${response.status_code}    ${status_code}
    Should Be Equal As Strings    ${response.json()["title"]}    ${message}

Validar status code e response body conforme dados inputados
    [Arguments]    ${status_code}
    Should Be Equal As Strings    ${response.status_code}    ${status_code}
    ${body}    Convert String to JSON    ${body}
    Dictionary Should Contain Sub Dictionary    ${response.json()}    ${body}
