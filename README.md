# booli-price-predictor
[![Build Status](https://dev.azure.com/niklasbrannlund/house-price-predictor/_apis/build/status/niklasbrannlund.booli-price-predictor?branchName=master)](https://dev.azure.com/niklasbrannlund/house-price-predictor/_build/latest?definitionId=1&branchName=master)

Predict house prices from current available listings.
Uses the BooliAPI to gather data from historical sales


## Configuration
Create a config-file named `privateappsettings.config` with the following data
```C#
<configuration>
  <add key="ApiKey" value="YOUR-BOOLI-API-KEY" />
  <add key="CallerId" value="YOUR-BOOLI-CALLERID" />
</configuration>
```
