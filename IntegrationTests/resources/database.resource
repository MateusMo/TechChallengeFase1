*** Settings ***
Resource          ../resources/base.resource

*** Variables ***
${DB_NAME}        ContactZone
${DB_TABLE}       Contact
${DB_USER}        sa
${DB_PASSWORD}    @Sql2019
${DB_HOST}        localhost
${DB_PORT}        1433
${DB_API}         pymssql

*** Keywords ***
Buscar contatos cadastrados no banco de dados
    Connect To Database    ${DB_API}    ${DB_NAME}    ${DB_USER}    ${DB_PASSWORD}    ${DB_HOST}    ${DB_PORT}
    ${DB_QUERY_RESULT}    Query    SELECT * FROM ${DB_TABLE}
    Log    ${DB_QUERY_RESULT}
    Set Test Variable    ${DB_QUERY_RESULT}
    Disconnect From Database

Deletar contatos cadastrados via banco de dados
    Connect To Database    ${DB_API}    ${DB_NAME}    ${DB_USER}    ${DB_PASSWORD}    ${DB_HOST}    ${DB_PORT}
    ${DB_QUERY_RESULT}    Execute Sql String    DELETE FROM ${DB_TABLE}
    Log    ${DB_QUERY_RESULT}
    Disconnect From Database
