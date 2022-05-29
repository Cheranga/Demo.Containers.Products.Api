[![Demo.Containers](https://github.com/Cheranga/Demo.Containers.Products.Api/actions/workflows/build-and-deploy.yml/badge.svg)](https://github.com/Cheranga/Demo.Containers.Products.Api/actions/workflows/build-and-deploy.yml)
# Demo.Containers.Products.Api

## Introduction

A product management microservice done in ASP.NET Core 6

## Goals

- [ ] Use GitHub Actions to deploy to Azure App Service.
- [ ] Containerize the API.
- [ ] Use GitHub Action to deploy to Azure Container Instances.

## References

:bulb: [Start/Stop slot in GitHub actions](https://stackoverflow.com/questions/48383093/how-to-stop-a-functionapp-in-a-slot-using-azure-cli)

Couldn't really find an action to do this, but can do this using an Azure CLI command.


```shell
# start the slot
az resource invoke-action --action start --ids  /subscriptions/${{ secrets.AZURE_SUBSCRIPTION_ID }}/resourceGroups/${{ env.RG_NAME }}/providers/Microsoft.Web/sites/api-${{ env.APP_NAME }}-dev/slots/staging

# stop the slot
az resource invoke-action --action stop --ids  /subscriptions/${{ secrets.AZURE_SUBSCRIPTION_ID }}/resourceGroups/${{ env.RG_NAME }}/providers/Microsoft.Web/sites/api-${{ env.APP_NAME }}-dev/slots/staging
```