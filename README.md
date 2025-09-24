# Embrujo Cerveza

Aplicación web en ASP.NET Core para gestionar el inventario de una cervecería artesanal. Permite dar de alta, baja y modificar estilos, registrar lotes asociados a cada receta y administrar los tipos de botella disponibles.

## Características principales

- Registro completo de estilos de cerveza: nombre, descripción, ABV, IBU e imagen representativa.
- Carga y administración de lotes por estilo con cantidad de botellas, fecha de envasado y notas.
- ABM de tipos de botella para controlar materiales y capacidades.
- Búsqueda y filtrado rápido desde la web para estilos, lotes y envases.
- Interfaz en español con soporte para mensajes de confirmación.
- Persistencia mediante SQLite y creación automática de la base de datos al iniciar la aplicación.

## Requisitos

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

## Puesta en marcha

1. Restaurar paquetes y compilar el proyecto:
   ```bash
   dotnet restore
   dotnet build
   ```
2. Ejecutar la aplicación web:
   ```bash
   dotnet run --project src/EmbrujoCerveza.Web
   ```
3. Abrir un navegador y navegar a `https://localhost:7248` (o la URL indicada por la consola).

Las imágenes cargadas se almacenan en `wwwroot/uploads`. Este directorio está incluido en el control de versiones mediante un marcador `.gitkeep`, pero los archivos subidos se omiten por el `.gitignore`.

## Estructura del proyecto

- `EmbrujoCerveza.sln`: solución de Visual Studio.
- `src/EmbrujoCerveza.Web`: proyecto web principal con páginas Razor.
- `wwwroot`: archivos estáticos (CSS, imágenes subidas por los usuarios).
- `Pages/BeerStyles`: páginas Razor dedicadas a la administración del inventario.
- `Pages/BeerLots`: ABM de lotes relacionados con cada estilo.
- `Pages/BottleTypes`: gestión de capacidades y materiales de envases.

## Migraciones de base de datos

La aplicación crea la base de datos automáticamente si no existe. Si prefieres trabajar con migraciones de Entity Framework Core, puedes generarlas con los siguientes comandos:

```bash
dotnet ef migrations add InitialCreate --project src/EmbrujoCerveza.Web --startup-project src/EmbrujoCerveza.Web
dotnet ef database update --project src/EmbrujoCerveza.Web --startup-project src/EmbrujoCerveza.Web
```

> Para ejecutar los comandos anteriores necesitas tener instalado el paquete `dotnet-ef` de Entity Framework Core Tools (`dotnet tool install --global dotnet-ef`).
