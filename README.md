# 📚 Proyecto Biblioteca React + .NET API Natalia Soria y Natalia García

Con este proyecto hemos querido programar una aplicación web para gestionar una biblioteca personal, donde pudieras ir almacenando tus lecturas actuales, futuras o ya realizadas. También puedes puntuar los libros ya leídos y poner comentarios para que el resto de usuarios puedan saber que te parecen.

###URL WEB HOSTING: http://natgsarabia-001-site1.qtempurl.com/login 

usuarios: nagasa contraseña: nagasa123
         admin   contraseña: admin123


Actualizaciones y cambios que se han tenido que hacer para que se pudiera realizar el hosting:
1. Convertir la aplicacion a NET 6.0
2. Migrar la base de datos de sqLite a sqlServer
3. Actualizar todas las url de de las llamadas a la API con la url de la BBDD que hemos creado, teniendo en cuenta que todas fueran http y no https


---

## 🎯 Justificación y motivación del proyecto

Este proyecto surge de la necesidad de contar con una app/web digital y fácil de comprender que permita a los lectores organizar sus lecturas. Leer es un hobby frecuente y que cada vez se está poniendo más de moda gracias influencers que empiezan a patrocinarlo en redes. Pero, cuando eres un lector muy voraz, a veces es dificil llevar un control sobre tus lecturas y las opiniones sobre ellas. 

Con esta aplicación buscamos ofrecer una experiencia útil, accesible y personalizada para llevar un control de las lecturas y ayudar al resto de la comunidad lectora  compartiendo puntuaciones y comentarios.

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

## 🧱 Esquema de arquitectura

La arquitectura del proyecto sigue el modelo cliente-servidor y se compone de tres partes principales:

1. **Frontend (React):**
   - Se encarga de la interfaz visual del usuario.
   - Realiza peticiones al backend CRUD y  muestra los resultados por FE.
   - Contiene los componentes React como `Login`, `Books`, `Categories`, `User`, etc. para realizar las llamadas al BE y que las paginas se rendericen de manera dinámica dependieno de la información que reciban.

2. **Backend (API .NET Core):**
   - Expuesta como API RESTful.
   - Gestiona la lógica de negocio, acceso a base de datos y validaciones. Desde program.cs además de levantar la aplicación llamamos a la API books externa para revisar si se han añadido libros nuevos y actualizar nuestra BBDD en caso afirmativo.
   - Contiene los controladores (`Controllers`), servicios (`Services`) y modelos (`Models`).

3. **Base de datos (SQLite):**
   - Almacena la información correspondiente a los usuarios, libros y los comentarios y puntuaciones de estos.

---
  
## 🧠 Aplicación del modelo MVC en el BE

El backend se ha implentado en ASP.NET Core y sigue la arquitectura MVC:

- **Modelo (`/Models`):**
  - En esta carpeta estarían todos los archivos correspondiente a los objetos que tenemos en nuestra BBDD.

- **Vista (`/Views`):**
  - Encontramos todos los archivos .html de nuestro proyecto.
  - En program.cs creamos los enrutamientos a cada uno de los archivos creados.
 
- - **Vista (`/wwwroot`):**
  - Encontramos las carpetas /cs, /js y /img con todos los archivos que se encargan del renderizadoo y estilado de nuestra web.

- **Controlador (`/Controllers`):**
  - Contienen los tres archivos que se encargan de todas las funciones CRUD que hacemos a nuestras BBDD. Definiendo las rutas de las API y la lógica del negocio.

**Servicios (`/Services`)**:
- En esta carpeta hemos programado los archivos que se encargan de las llamadas a API's externas y de la gestionión incial de comparartiva de libros BBDD-API que realizamos cada vez que accedemos a la /home.

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

### 🔧 Backend (UserBookController.cs)

#### ✩ `GetBookComents(int id_book)`
Devuelve todos los comentarios de un libro, incluyendo `userId`, `comment`, `rating` e `id` si existe contenido.

#### ✩ `GetUserBookItem(int id_book)`
Devuelve un libro concreto filtrando por su `id`

#### ✩ `GetBookByCategory(string state ,  [FromQuery] int id_user)`
Devuelve todos los libros que existan en BBDD filtrados por un estado.

#### ✩ `PutUserBookItem(int id, [FromQuery] string status, [FromQuery] string comment, [FromQuery] int rating)`
Actualizamos el comentario o la puntuacion de un libro.

#### ✩ `PostUserBookItem(UserBookItem userBookItem)`
Publicamos un nuevo comentario sobre un libro.

#### ✩ `DeleteComment(int id)`
Eliminar un comentario.

#### ✩ `DeleteUserBookItem(int id)`
Eliminar un libro de nuestras categorias Pendiente, Leido y Actual.

#### ✩ `ActualizarPuntuacionLibro(int bookId)`
Esta funcion actualiza la puntuación del genérica del libro cada vez que un usuario añade un nuevo comentario.


---

### 🔧 Backend (UserController.cs)

#### ✩ `GetUserByNickname([FromQuery] string nickname, [FromQuery] string password)`
Funcion para el loggeo del usuario. Chequea que el nickname y la contraseña existan en BBDD y coincidan

#### ✩ `UserExists([FromQuery] string nickname, [FromQuery] string email)`
Revisa si un usuario existe en nuestra BBDD a partir de su email y nickname

#### ✩ `GetUserID([FromQuery] int id)`
Recuperamos la información de un usuario a partir de su ID

#### ✩ `PostUserItem(UserItem userItem)`
Creación de nuevo usuario.

#### ✩ `PutUserItem([FromBody] UserItem userItem)`
Actualizar la información de un usuario existente.

#### ✩ `DeleteUserBookItem(int id)`
Elimina un libro de la biblioteca personal del usuario.


---

### 🔧 Backend (LibraryItemsController.cs)

#### ✩ `GetLibraryItems`
Recuperamos todos los libros de nuestra BBDD

#### ✩ `GetLastBooks()`
Recuperamos los últimoms 10 libros actualizados en nuestra BBDD para ver las novedades

#### ✩ `GetOneBooks(int book_id)`
Recuperamos la información de un libro a partir de su ID

#### ✩ `PostUserItem(UserItem userItem)`
Creación de nuevo usuario.

#### ✩ `PutUserItem([FromBody] UserItem userItem)`
Actualizar la información de un usuario existente.

#### ✩ `DeleteUserBookItem(int id)`
Elimina un libro de la biblioteca personal del usuario.


---

### ⚛️ Frontend (login.js, componentes React)

#### ✩ `checkUserExists(nickname, email)`
Funcion que usamos en el registro, para confirmar las credenciales del usuario.


---

### ⚛️ Frontend (index.js, componentes React)

#### ✩ `fetchBooks(loggedUser)`
Hacemos llamada a BBDD library para recuperar todos los libros y renderizarlos en el DOM

#### ✩ `fetchLastBooksCarousel(loggedUser) `
Hacemos llamada a BBDD library para recuperar los últimos 10 libros y renderizarlos en el DOM


---

### ⚛️ Frontend (category.js, componentes React)

#### ✩ `fetchBooks()`
En este caso la funcion llama a la url para recibir los libros del usuario pero con un estado en concreto y los renderiza en el DOM de forma dinámica.

#### ✩ `deleteBook(bookId)`
Elimina el libro de nuestra categoria.


---

### ⚛️ Frontend (book.js, componentes React)

#### ✩ `searchBookInformation(bookId)`
Busca la información correspondiente al libro en BBDD

#### ✩ `searchBookComents(bookId)`
Busca todos los comentarios en BBDD que existan para ese libro.

#### ✩ `BookDescription(title)`
Llama a una nueva api, donde recuperamos la descripción del libro a partir de su titulo

#### ✩ `Book({book,descripcion,coments,userID})`
Componente que se encarga de renderizar la información correctpondiente al libro (autor, titulo, descripción y puntuación.

#### ✩ `Coments({coments, book, userID})`
Componente que se encarga de renderizar la información correspondiente a los comentarios que tenga el libro. También de gestionar los estados del comentario del usuario, permitiendo añadir un nuevo comentario o editarlo.


---

### ⚛️ Frontend (FQ.js, componentes React)

#### ✩ `sendFormFQ()`
Se encarga de enviar un sweet alert cuando se envie el formulario. También gestiona que no se pueda enviar el formulario si no están todos los campos cumplimentados.


---

### ⚛️ Frontend (user.js, componentes React)

#### ✩ `Person({user})`
Componente que se encarga de renderizar la información recibida del usuario. Tamibién de controlar los estados para que se pueda modificar la información del usuario y se haga la llamada correspondiente a BBDD para realizar el PUT.

#### ✩ `findUser()`
Función para recuperar la información del usuario a partir del id almacenado en localStorage.

#### ✩ `saveNewUserInformation(user)`
Función para llamar a BBDD y guardar la información actualizada del usuario.


---

### 🧩 Extras

#### ✩ `setupLogout()`

Elimina el usuario del `localStorage` y redirige al login.

#### ✩ `redirectToCategory()`

Redirige según el botón pulsado (pendientes, actuales, finalizados) pasando el estado por URL.

#### ✩ `ResetPAssword([FromBody] EmailRequest request)`

Crea una contraseña aleatoria cuando el usuario solicite una por olvidarse de la propia.
 
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

## 🔗 FUENTES CONSULTADAS

* [https://dev.to/isaacojeda/explorando-la-autenticacion-bearer-en-aspnet-core-8-5e95](https://dev.to/isaacojeda/explorando-la-autenticacion-bearer-en-aspnet-core-8-5e95)

## 🔗 APIS USADAS

* [Open Library](https://openlibrary.org/)
* [Google Apis](https://www.googleapis.com)

