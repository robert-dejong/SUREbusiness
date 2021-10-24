# SUREbusiness wagenpark beheer API

Het doel van deze API is om een wagenpark te beheren. De API bevat de volgende functionaliteiten:

- Het toevoegen van een voertuig (inclusief kenteken validatie bij RDW open data)
- Het bijwerken van de status van een voertuig
- Het uitlenen van een voertuig aan een persoon
- Opvragen van voertuigen, inclusief paginering + filters op status en persoon aan wie voertuigen uitgeleend zijn

# Benodigdheden

- De API is ontwikkeld met .NET 5.0. De SDK hiervoor is hier te downloaden: https://dotnet.microsoft.com/download/dotnet/5.0
- GIT
- Visual Studio

# Installatie

Om de installatie zo makkelijk mogelijk te maken is er gebruik gemaakt van een In-Memory database, welke bij het opstarten van de API automatisch gevuld wordt met 100 voertuigen.

1. Clone de repository
2. Open het project in Visual Studio
3. Build de solution (let op dat .NET 5.0 geïnstalleerd is)
4. Run de API
5. Indien Swagger niet automatisch geopend wordt is deze hier te bereiken: https://localhost:44309/swagger/index.html

# Documentatie

De API bevat 4 endpoints. Hieronder zal ik per endpoint beschrijven hoe deze aangeroepen wordt en wat de business rules zijn die van toepassing zijn.
Het makkelijkst is om de calls via de Swagger pagina aan te roepen.

## Ophalen van voertuigen
```code
GET /api/vehicles
```

Dit endpoint haalt standaard alle voertuigen op. Daarnaast is er de mogelijk om de volgende parameters mee te geven aan de url:

```page``` De pagina om op te halen. Deze moet in combinatie met de parameter 'pageSize' gebruikt worden.

```pageSize``` Het aantal voertuig per pagina om op te halen. Deze moet in combinatie met de parameter 'page' gebruikt worden.

```status``` Hiermee kun je filteren op een bepaalde status. De mogelijke statussen: 'uitgeleend', 'beschikbaar', 'in reparatie', 'verkocht', 'in bestelling'

```loanedTo``` Hiermee kun je filteren op de persoon aan wie een voertuig uitgeleend is. De database wordt standaard gevuld met 3 mogelijke namen: Robert, Peter, Jan

Al deze parameters kunnen in combinatie gebruikt worden met elkaar.

### Voorbeelden

```GET /api/vehicles?status=beschikbaar```
Haalt alle voertuigen op waarvan de status 'beschikbaar' is.

```GET /api/vehicles?loanedTo=Robert```
Haalt alle voertuigen op die uitgeleend zijn aan Robert.

```GET /api/vehicles?status=uitgeleend&loanedTo=Robert```
Haalt alle voertuigen op waarvan de status 'uitgeleend' is en welke uitgeleend is aan Robert.

```GET /api/vehicles?page=1&pageSize=5```
Haalt de eerste pagina van de voertuigen op, met een pagina grootte van 5 voertuigen per pagina.

```GET /api/vehicles?page=1&pageSize=5&status=beschikbaar```
Haalt de eerste pagina van de voertuigen op waarvan de status 'beschikbaar', met een pagina grootte van 5 voertuigen per pagina.

## Ophalen van een voertuig o.b.v. ID
```code
GET /api/vehicles/{id}
```

Met dit endpoint kun je een enkel voertuig ophalen, dit doe je op basis van het ID van het voertuig.

### Voorbeeld
```GET /api/vehicles/1```
Haalt het voertuigen op waarvan de ID 1 is.

## Voertuig toevoegen
```code
POST /api/vehicles
{
    "licensePlate": "string",
    "color": "string",
    "yearOfManufacture": integer,
    "loanedTo": "string",
    "status": "string",
    "remarks": "string"
}
```

Met dit endpoint kun je een voertuig toevoegen. Hier op zijn een aantal regels van toepassing:
- Het kenteken mag niet leeg zijn
- Het kenteken moet bestaan bij RDW open data
- De status mag alleen een van de volgende zijn: 'uitgeleend', 'beschikbaar', 'in reparatie', 'verkocht', 'in bestelling'
- Het veld 'loanedTo' mag niet leeg zijn als de status 'uitgeleend' is
- Het veld 'loanedTo' moet leeg zijn als de status niet 'uitgeleend' is

### Voorbeeld

```code
POST /api/vehicles
{
    "licensePlate": "34-tdk-2",
    "color": "Rood",
    "yearOfManufacture": 2010,
    "loanedTo": "",
    "status": "beschikbaar",
    "remarks": "Ruitenwisservloeistof is op"
}
```
Deze voldoet aan alle regels en wordt dus zonder problemen toegevoegd.

## Voertuig bijwerken
```code
PATCH /api/vehicles/{id}
[
    {
        "op": "replace",
        "path": "/loanedTo",
        "value": "string"
    },
    {
        "op": "replace",
        "path": "/status",
        "value": "string"
    }
]
```

Met dit endpoint kun je de status en de persoon aan wie een voertuig is uitgeleend bijwerken.
Ook hier zijn een aantal regels van toepassing:

- De status mag alleen een van de volgende zijn: 'uitgeleend', 'beschikbaar', 'in reparatie', 'verkocht', 'in bestelling'
- Het veld 'loanedTo' mag niet leeg zijn als de status 'uitgeleend' is
- Het veld 'loanedTo' moet leeg zijn als de status niet 'uitgeleend' is
- De status mag niet aangepast worden als de huidige status 'verkocht' is
- De velden 'status' en 'loanedTo' mogen alleen aangepast worden als de huidige status 'beschikbaar' is
