To Activate Postgresql Locally
```aiignore
brew services start postgresql@17

psql -U postgres

CREATE DATABASE fridge;

CREATE USER myuser WITH PASSWORD 'pw';

GRANT ALL PRIVILEGES ON DATABASE fridge to myuser;

ALTER SCHEMA public OWNER TO myuser;

-- Drop Database
SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname='fridge';
DROP DATABASE fridge;
```
Migration
```aiignore
dotnet ef migrations add FixComments4
dotnet ef database update

```

Scaffold
```aiignore
dotnet aspnet-codegenerator controller -name UsersController -async -api -m User -dc RecipeContext -outDir Controllers
dotnet aspnet-codegenerator controller -name RecipesController -async -api -m Recipe -dc RecipeContext -outDir Controllers
dotnet aspnet-codegenerator controller -name IngredientsController -async -api -m Ingredient -dc RecipeContext -outDir Controllers
dotnet aspnet-codegenerator controller -name CommentsController -async -api -m Comment -dc RecipeContext -outDir Controllers
```

