#!/bin/sh

CERT_PATH="./aspnetapp.pfx"

# Verifica se o certificado já existe
if [ ! -f "$CERT_PATH" ]; then
    echo "Gerando certificado SSL para o ASP.NET Core..."
    dotnet dev-certs https -ep "$CERT_PATH" --password "p@ssW0rd"
    dotnet dev-certs https --trust
else
    echo "Certificado SSL já existe."
fi