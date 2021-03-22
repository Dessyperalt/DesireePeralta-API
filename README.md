# DesireePeralta-API
.NET core API. Retrieve the exchange rate from the current date for a given currency (USD and BRL) and makes a purchase in Argentina Pesos.


## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. 

### Prerequisites

What things you need to install the software and how to install them

```
- SQL Server
- Visual Studio
```

### Installing

```
- Open folder DesireePeraltaAPI and run the script scriptDB.sql in your database. 
- In the DesireePeraltaAPI\DesireePeraltaAPI\Appsettings.json file, change the connection string if needed.
```

## Running the Project

1. To see the current rate of a selected currency, use the following url with the currency code: 

https://localhost:[Your local port]/api/ExchangeRates/Currency/[URL]

For example:

```
https://localhost:44314/api/ExchangeRates/Currency/USD # Returns the Dollar exchange rate
https://localhost:44314/api/ExchangeRates/Currency/BRL # Returns the Brazilian Real exchange rate

```
2. To make a currency purchase, the API uses the following url: 

https://localhost:[your local port]/api/CurrencyPurchases

For example:

```
{
    "UserID":1,
    "amount":"500",
    "currencyCode":"USD"
}
```
- You can't buy more than 200 USD or 300 BRL per user per month. 
- You only can buy USD and BRL.


## Built With

* [Visual Studio .NET Core 2019](https://visualstudio.microsoft.com/es/vs/)
* [Microsoft SQL Server 2014](https://www.microsoft.com/es-es/download/details.aspx?id=42299)


## Author

* **Desiree Peralta** - [DessyPeralt](https://github.com/DessyPeralt)
