Mac地端啟動DB 測試用
```bash
# Mac啟用postgresql
brew services start postgresql@17

# 於Terminal登入postgres
psql -U postgres

# 建立DB
CREATE DATABASE fridge;

# 建立DB用戶
CREATE USER myuser WITH PASSWORD 'pw';

# 調整用戶權限
GRANT ALL PRIVILEGES ON DATABASE fridge to myuser;
ALTER SCHEMA public OWNER TO myuser;

# 移除DB
SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname='fridge';
DROP DATABASE fridge;
```

DB Migration
```aiignore
dotnet ef migrations add {更新msg}
dotnet ef database update
```
建立Controllers
```aiignore
dotnet aspnet-codegenerator controller -name UsersController -async -api -m User -dc RecipeContext -outDir Controllers
dotnet aspnet-codegenerator controller -name RecipesController -async -api -m Recipe -dc RecipeContext -outDir Controllers
dotnet aspnet-codegenerator controller -name IngredientsController -async -api -m Ingredient -dc RecipeContext -outDir Controllers
dotnet aspnet-codegenerator controller -name CommentsController -async -api -m Comment -dc RecipeContext -outDir Controllers
```

