# Digitalt Dödsbo

Hjälper anhöriga avsluta en avliden persons digitala tjänster (telekom, streaming,
bank, försäkring m.m.) genom att generera färdiga brev per leverantör och hålla koll
på status på varje ärende.

## Arkitektur

```
backend/
  DigitaltDodsbo.Core/            Domänmodeller + interfaces (ingen EF-koppling)
  DigitaltDodsbo.Infrastructure/  EF Core, SQLite, repository-implementationer
  DigitaltDodsbo.Api/             ASP.NET Core Web API, controllers, DI, CORS
frontend/
  digitalt-dodsbo-web/            React + TypeScript + Vite
```

Detta är "Clean Architecture-light": Core känner inte till Infrastructure eller Api,
vilket gör det enkelt att t.ex. byta SQLite mot PostgreSQL senare utan att röra
affärslogiken.

## Komma igång

### Förkrav
- .NET 8 SDK
- Node.js 18+ och npm

### 1. Backend

```bash
cd backend

# Installera EF Core CLI-verktyget (en gång, globalt)
dotnet tool install --global dotnet-ef

# Skapa första migrationen (skapar databastabeller utifrån modellerna)
cd DigitaltDodsbo.Api
dotnet ef migrations add InitialCreate --project ../DigitaltDodsbo.Infrastructure --startup-project .

# Kör API:et (migrerar databasen automatiskt vid start, se Program.cs)
dotnet run
```

API:et startar på `http://localhost:5000` (eller den port som visas i terminalen —
justera `BASE_URL` i `frontend/.../src/api/client.ts` om den skiljer sig).
Swagger UI finns på `http://localhost:5000/swagger` i utvecklingsläge.

### 2. Frontend

```bash
cd frontend/digitalt-dodsbo-web
npm install
npm run dev
```

Frontend startar på `http://localhost:5173`. CORS är redan konfigurerat i backend
för att tillåta denna origin.

## Flöde i appen

1. Fyll i uppgifter om den avlidne och dig själv som anhörig
2. Välj vilka tjänster (Telia, Spotify, bank, etc.) som ska avslutas
3. Systemet genererar ett färdigt brevutkast per tjänst
4. Du kopierar texten och skickar själv (ingen automatisk utskick i denna version —
   medvetet val för att undvika juridiska/integritetsmässiga risker i en första version)
5. Du uppdaterar status per tjänst (Väntar → Brev klart → Skickat → Avslutat)

## Nästa steg (naturlig utbyggnad)

- **AI-genererade brev**: byt ut `TemplateLetterGenerator` mot en implementation av
  `ILetterGenerator` som anropar Claude API för mer naturligt skrivna, kontextanpassade
  brev. Interfacet är redan förberett för detta — bara att lägga till en ny klass i
  `Infrastructure/Services/` och registrera den i `Program.cs`.
- **Autentisering**: lägg till ASP.NET Core Identity eller liknande om fler användare
  ska kunna ha egna ärenden privat.
- **Mejl-scanning**: automatisk identifiering av vilka tjänster den avlidne hade
  (kräver OAuth mot Gmail/Outlook och noggrann juridisk hantering av samtycke/fullmakt).
- **Riktig e-postutskick**: integrera Resend/Postmark så breven kan skickas direkt
  från appen istället för att kopieras manuellt.
- **Fler leverantörer**: `AppDbContext.OnModelCreating` har seed-data för 8 vanliga
  svenska tjänster — bygg ut listan efter behov.

## Juridisk notis

Detta är ett lärprojekt/portfolio-projekt. Innan något liknande driftsätts med
riktiga personuppgifter (särskilt automatisk mejl-läsning) bör GDPR-grund och krav
kring fullmakt/dödsboanmälan utredas med jurist.
