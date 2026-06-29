# MenuService

Microservicio encargado de la gestión del catálogo de un sistema de gestión para restaurantes.

## Responsabilidades

MenuService administra la información relacionada con:

* Categorías
* Platos
* Bebidas

Además, expone endpoints de integración para permitir la comunicación con otros microservicios del sistema.

---

## Arquitectura

El proyecto fue desarrollado siguiendo principios de Clean Architecture.

### Capas

* MenuService.Api
* MenuService.Application
* MenuService.Domain
* MenuService.Infrastructure

### Patrones utilizados

* Repository Pattern
* Unit of Work
* Dependency Injection
* DTOs
* Middleware Global de Excepciones

---

## Tecnologías

* ASP.NET Core
* Entity Framework Core
* SQL Server
* Swagger

---

## Requisitos Previos

Antes de ejecutar el proyecto es necesario contar con:

* .NET 8 SDK
* SQL Server Express o SQL Server
* Visual Studio 2022 o superior

---

## Instalación

Clonar el repositorio:

```bash
git clone <URL_DEL_REPOSITORIO>
```

Ingresar al directorio:

```bash
cd MenuService
```

---

## Restaurar Dependencias

```bash
dotnet restore
```

---

## Paquetes Utilizados

### MenuService.Infrastructure

```bash
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
```

### MenuService.Api

```bash
Install-Package Microsoft.EntityFrameworkCore.Design
Install-Package Swashbuckle.AspNetCore
```

---

## Configuración de Base de Datos

Modificar la cadena de conexión en:

```txt
MenuService.Api/appsettings.json
```

Ejemplo:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=MenuServiceDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

## Crear la Base de Datos

Abrir la Consola del Administrador de Paquetes y ejecutar:

```powershell
Update-Database
```

Al iniciar la aplicación:

* Se aplicarán las migraciones pendientes.
* Se ejecutará automáticamente el Seed inicial.

---

## Ejecutar el Proyecto

Seleccionar:

```txt
MenuService.Api
```

como proyecto de inicio.

Luego ejecutar:

```bash
dotnet run
```

o iniciar desde Visual Studio.

---

## Swagger

Una vez iniciada la aplicación:

```txt
https://localhost:<puerto>/swagger
```

Desde Swagger se pueden probar todos los endpoints disponibles.

---

## Entidades

### Category

Representa una categoría del menú.

Ejemplos:

* Bebidas
* Platos Principales
* Postres

### Dish

Representa un plato del menú.

Atributos principales:

* Nombre
* Descripción
* Precio
* Tiempo estimado de preparación
* Disponibilidad
* Imagen

### Drink

Representa una bebida del menú.

Atributos principales:

* Nombre
* Descripción
* Precio
* Disponibilidad
* Imagen

---

## Funcionalidades

### Categorías

* Crear categoría
* Obtener categorías
* Obtener categoría por Id
* Actualizar categoría
* Eliminar categoría

### Platos

* Crear plato
* Obtener platos
* Obtener plato por Id
* Obtener platos por categoría
* Actualizar plato
* Eliminar plato

### Bebidas

* Crear bebida
* Obtener bebidas
* Obtener bebida por Id
* Obtener bebidas por categoría
* Actualizar bebida
* Eliminar bebida

---

## Integración entre Microservicios

### OrderService

* Obtener información resumida de platos
* Obtener información resumida de bebidas

### InventoryService

* Verificar existencia de platos

### KitchenService

* Obtener tiempo estimado de preparación de un plato

Endpoints:

```txt
GET /api/v1/menu-integration/dishes/{id}/for-order
GET /api/v1/menu-integration/drinks/{id}/for-order
GET /api/v1/menu-integration/dishes/{id}/exists
GET /api/v1/menu-integration/dishes/{id}/preparation-time
```

---

## Manejo de Errores

El sistema implementa un Middleware Global de Excepciones.

Códigos soportados:

* 200 OK
* 201 Created
* 400 Bad Request
* 404 Not Found
* 409 Conflict
* 500 Internal Server Error

---

## Base de Datos

Base de datos SQL Server:

```txt
MenuServiceDb
```

Tablas principales:

* Categories
* Dishes
* Drinks

---

## Datos Iniciales

El sistema incluye un Seed automático que carga:

* Categorías
* Platos
* Bebidas

durante la primera ejecución de la aplicación.

