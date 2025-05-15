# 📚 Proyecto Biblioteca React + .NET API Natalia Soria y Natalia García

Con este proyecto hemos querido programar una aplicación web para gestionar una biblioteca personal, donde pudieras ya almacenando tus lecturas actuales, futuras o ya realizadas. También puedes puntuar los libros ya leídos y poner comentarios para que el resto de usuarios puedan saber que te parecen.


---

## 🧩 Tecnologías utilizadas

- ⚛️ React (v18) + Babel
- 🎨 CSS puro (sin frameworks)
- 🧠 .NET Core API (C#)
- 🗃️ Entity Framework + SQL Server
- 🍭 SweetAlert2 para mensajes emergentes

---

## 🚀 Funcionalidades

- Registro e inicio de sesión de usuarios.
- Visualización de libros (portada, autor, descripción).
- Clasificación de libros: pendientes, actuales, leídos.
- Reseñas con comentarios y estrellas.
- Edición y actualización de reseñas.
- Actualización automática del promedio de puntuación del libro.
- Protección de rutas (requiere login).

---
## 🗂️ Estructura del web-Servidores
- Api OpenLibrary para recuperar información básica de los libros.
- Api googleBooks para recuperar la descripción de cada libro
- BBDD User donde almacenaremos su nickname, email y contraseña
- BBDD Library donde guardaremos la información basíca de los libros y la puntuación total
- BBDD LibraryApi donde guardaremos la informacion sobre libros que tienen guardados cada usuario, su estado, comentario y puntuación indiviual.

## 🗂️ Estructura del proyecto

Carpeta ROOT LibrayApi

📁 /Controllers (archivos donde encontramos todas las funciones a BBDD)
└── LibraryItemsController.css
└── UserBookController.css
└── UserController.css

📁 /Migrations
└── Migraciones creadas a la BBDD SQLite
 
📁 /Models (Modelos creados para manejar SQLite y los objetos que actualizaremos)
└── BookApi.js (para manejar la llamada a la API de libros y guardar la información en nuestra BBDD)
└── LibraryItem.js
└── UserItem.js
└── UserBookItem.js
└──  (...)

📁 /Service (Configuramos los servicios que usaremos en nuestra aplicacion, la llamada a la API de libros y a nuestra BBDD)
└── BookApiCall.cs
└── LibraryServices.cs

📁 /Views (Todos nuestros archivo html )
└── login-register.html
└── index.html
└── user.html
└──  (...)

📁 /wwwwroot (todos nuestros archivos public que sirven para estilar nuestro proyecto)
└── 📁/img
└── 📁/css
└── 📁/js

Y por ultimo tendremos los archivos .db de las tres BBDD y el archivo Program.cs desde controlaremos el arranca de la aplicación y el enrutamiento de las diferentes pantallas. Así como la llamada a la APIbook cada vez que accedamos para revisar y actualizar nuestros libros si fuera necesario.


USUARIO PRUEBA:
  nickname: nagasa
  password: nagasa123

WEBGRAFIA
https://dev.to/isaacojeda/explorando-la-autenticacion-bearer-en-aspnet-core-8-5e95
