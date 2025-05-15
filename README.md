# 📚 Proyecto Biblioteca React + .NET API Natalia Soria y Natalia García

Con este proyecto hemos querido programar una aplicación web para gestionar una biblioteca personal, donde pudieras ir almacenando tus lecturas actuales, futuras o ya realizadas. También puedes puntuar los libros ya leídos y poner comentarios para que el resto de usuarios puedan saber que te parecen.

---

## 🧩 Tecnologías utilizadas

* ⚛️ React (v18) + Babel
* 🎨 CSS puro (sin frameworks)
* 🧠 .NET Core API (C#)
* 💃 Entity Framework + SQL Server
* 🍭 SweetAlert2 para mensajes emergentes

---

## 🚀 Funcionalidades

* Registro e inicio de sesión de usuarios.
* Visualización de libros (portada, autor, descripción).
* Clasificación de libros: pendientes, actuales, leídos.
* Reseñas con comentarios y estrellas.
* Edición y actualización de reseñas.
* Actualización automática del promedio de puntuación del libro.
* Protección de rutas (requiere login).

---

## 🗂️ Estructura del web-Servidores

* API OpenLibrary para recuperar información básica de los libros.
* API GoogleBooks para recuperar la descripción de cada libro.
* BBDD **User** donde almacenamos nickname, email y contraseña.
* BBDD **Library** donde guardamos la información básica de los libros y su puntuación total.
* BBDD **LibraryApi** donde almacenamos qué libros tiene cada usuario, su estado, comentarios y puntuación individual.

## 🗂️ Estructura del proyecto

**Carpeta ROOT: LibraryApi**

**📁 /Controllers** (acciones sobre BBDD)

* `LibraryItemsController.cs`
* `UserBookController.cs`
* `UserController.cs`

**📁 /Migrations**

* Migraciones generadas para la BBDD SQLite

**📁 /Models** (entidades)

* `LibraryItem.cs`
* `UserItem.cs`
* `UserBookItem.cs`
* `BookApi.cs` (modelo para guardar libros recuperados desde API externa)

**📁 /Service**

* `BookApiCall.cs`
* `LibraryServices.cs`

**📁 /Views** (HTML)

* `login-register.html`
* `index.html`
* `user.html`, etc.

**📁 /wwwroot** (archivos estáticos)

* `/css`, `/img`, `/js`

Y finalmente, el archivo `Program.cs` donde configuramos el servidor, rutas y servicios.

---

## 🧠 EXPLICACIÓN DE FUNCIONES CLAVE

### 🔧 Backend (UserBookController.cs, LibraryItemsController.cs, etc.)

#### ✩ `GetBookComents(int id_book)`

Devuelve todos los comentarios de un libro, incluyendo `userId`, `comment`, `rating` e `id` si existe contenido.

#### ✩ `PostUserBookItem(UserBookItem userBookItem)`

Guarda una nueva relación usuario-libro en la tabla `UserBookItems`. Llama también a `ActualizarPuntuacionLibro` para recalcular la puntuación global.

#### ✩ `PutUserBookItem(int id, string status, string comment, int rating)`

Actualiza la relación usuario-libro (estado, comentario y/o puntuación) y recalcula el promedio del libro.

#### ✩ `ActualizarPuntuacionLibro(int bookId)`

Suma todas las puntuaciones registradas del libro y actualiza el promedio en `LibraryItems`.

#### ✩ `GetBookByCategory(string state, int id_user)`

Devuelve los libros de un usuario según el estado seleccionado: pendiente, actual o leído.

#### ✩ `DeleteUserBookItem(int id)`

Elimina un libro de la biblioteca personal del usuario.

---

### ⚛️ Frontend (book.js, componentes React)

#### ✩ `searchBookInformation(bookId)`

Recupera los datos básicos del libro desde `/LibraryItems/GetOneBooks/{id}`.

#### ✩ `searchBookComents(bookId)`

Obtiene comentarios del libro, luego usa `userId` para buscar los `nickname` con otro `fetch`.

#### ✩ `BookDescription(bookTitle)`

Hace una llamada a Google Books API para obtener la descripción literaria del libro.

#### ✩ `Coments({ coments, book, userID })`

Componente que muestra:

* Reseñas de otros usuarios.
* Reseña del usuario actual (si existe), con opción de editar.
* Formulario para crear nueva reseña si el usuario aún no ha comentado.

#### ✩ `handleEnviarcomentario()`

Controla el envío de reseñas. Detecta si el usuario ya tiene reseña:

* Si sí: realiza un `PUT`.
* Si no: realiza un `POST`.
  Luego recarga la vista para mostrar la puntuación actualizada.

#### ✩ `prinStarts(container, puntuation)`

Pinta visualmente las estrellas del libro (de 1 a 5) según la puntuación total.

---

### 🧩 Extras

#### ✩ `setupLogout()`

Elimina el usuario del `localStorage` y redirige al login.

#### ✩ `redirectToCategory()`

Redirige según el botón pulsado (pendientes, actuales, finalizados) pasando el estado por URL.

---

## ⚙️ Ejecución del programa y pruebas

1. Abre la solución `LibraryApi` en Visual Studio o VS Code.
2. Restaura los paquetes:

```bash
dotnet restore
```

3. Aplica las migraciones:

```bash
dotnet ef database update
```

4. Ejecuta el servidor:

```bash
dotnet run --launch-profile https
```

### 🧪 Usuario de Pruebas:

* **nickname:** `nagasa`
* **password:** `nagasa123`

---

## 🛠️ Mejoras futuras

* [ ] Validación de formularios en frontend.
* [ ] Filtros/búsquedas avanzadas.
* [ ] Paginación en los comentarios.
* [ ] Enriquecer información de los libros: categorías, número de páginas, etc.
* [ ] Soporte para imágenes de perfil de usuario.

---

## 🔗 Webgrafía

* [https://dev.to/isaacojeda/explorando-la-autenticacion-bearer-en-aspnet-core-8-5e95](https://dev.to/isaacojeda/explorando-la-autenticacion-bearer-en-aspnet-core-8-5e95)

