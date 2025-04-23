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

1. Upewnij się, że masz zainstalowany **.NET SDK** (minimum .NET 7 lub 8).
2. Zainstaluj globalnie narzędzie Entity Framework CLI (jeśli jeszcze go nie masz):

    ```bash
    dotnet tool install --global dotnet-ef
    ```

3. Ustaw zmienną środowiskową `JWT_TOKEN`, lub dodaj token do pliku (zostawiłem go w pliku) `appsettings.json`.

4. Wejdź do katalogu z backendem:

    ```bash
    cd ./ContactsApp
    ```

5. Zaktualizuj bazę danych:

    ```bash
    dotnet ef database update
    ```

6. Uruchom backend:

    ```bash
    dotnet watch
    ```

---

7. Otwórz **nowy terminal** i przejdź do folderu z frontendem:

    ```bash
    cd ./ContactsView
    ```

8. Zainstaluj zależności:

    ```bash
    npm install
    ```

9. Uruchom stronę internetową:

    ```bash
    npm run dev
    ```

---

### 🌐 Domyślne adresy aplikacji:

- **Backend**: http://localhost:5025  
- **Frontend**: http://localhost:5173

---
