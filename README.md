# booli-price-predictor
![](booli_logo.png?raw=true "Booli")

[![Build Status](https://dev.azure.com/niklasbrannlund/house-price-predictor/_apis/build/status/niklasbrannlund.booli-price-predictor?branchName=master)](https://dev.azure.com/niklasbrannlund/house-price-predictor/_build/latest?definitionId=1&branchName=master)

Predict house prices from current available listings.
Uses the [BooliAPI](https://www.booli.se/p/api/) to gather data of historical sales and current listing and [ML.NET](https://dotnet.microsoft.com/apps/machinelearning-ai/ml-dotnet)-framework to predict prices of the current listings.


## Configuration
Create a config-file named `privateappsettings.config` with the following data
```C#
<appSettings>
  <add key="ApiKey" value="YOUR-BOOLI-API-KEY" />
  <add key="CallerId" value="YOUR-BOOLI-CALLERID" />
</appSettings>
```
