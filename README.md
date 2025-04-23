# 📇 ContactsApp

## 📄 Specyfikacja techniczna

### 🧩 Opis aplikacji

Aplikacja `ContactsApp` to serwis API napisany w technologii ASP.NET Core. Umożliwia zarządzanie kontaktami z podziałem na kategorie i podkategorie oraz posiada mechanizm autoryzacji oparty o JWT.

---

### 🧱 Klasy i Serwisy

- `AppDbContext` – kontekst bazy danych skonfigurowany do pracy z SQL Server.
- `AuthService` (`IAuthService`) – obsługuje logikę autoryzacji użytkowników.
- `ContactService` (`IContactService`) – zawiera logikę operacji CRUD dla kontaktów.
- `CategoryService` (`ICategoryService`) – zarządza kategoriami.
- `SubcategoryService` (`ISubcategoryService`) – zarządza podkategoriami.

---

### 📦 Wykorzystane biblioteki

- `Microsoft.AspNetCore.Authentication.JwtBearer` – obsługa autoryzacji JWT.
- `Microsoft.EntityFrameworkCore` – ORM do komunikacji z bazą danych.
- `Microsoft.IdentityModel.Tokens` – do weryfikacji tokenów JWT.
- `Swashbuckle.AspNetCore` / `OpenAPI` – generowanie dokumentacji API (Swagger).
- `Microsoft.Extensions.Logging` – logowanie.

---

### ⚙️ Sposób kompilacji i uruchomienia

1. Upewnij się, że masz zainstalowany .NET SDK (min. .NET 7 lub 8).
2. Ustaw zmienną środowiskową `JWT_TOKEN` lub zostaw token w `appsettings.json`.
3. Wejdź w podfolder ContactsApp poleceniem:

```bash
cd ./ContactsApp
```

3. Uruchom aplikację poleceniem:

```bash
dotnet watch
```

4. Nastepnie odtworz nastepny terminal w podfolderze ContactsView
5. Zainstaluj potrzebne zależności poleceniem:

```bash
npm i
```

6. Uruchom strone internetową poleceniem:

```bash
npm run dev
```

Domyślnie aplikacja nasłuchuje na porcie http://localhost:5025 a strona internetowa na http://localhost:5173.
