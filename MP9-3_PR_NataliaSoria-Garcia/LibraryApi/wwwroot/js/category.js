(async function () {
  setupLogout();
  setupRatings();
  redirectToCategory();
  await fetchBooks(); // Esperamos que termine de cargar
})();

async function fetchBooks() {
  const params = new URLSearchParams(window.location.search);
  const userId = params.get("id_user");
  const state = params.get("state");

  const category1Container = document.querySelector('.category-section .book-grid');
  category1Container.innerHTML = '';

  if (!userId || !state) {
    console.error("❌ Faltan parámetros en la URL");
    return;
  }

  const url = `/api/UserBook/state/${state}?id_user=${userId}`; // URL relativa para producción
  console.log("📡 Haciendo fetch a:", url);

  try {
    const response = await fetch(url);
    if (!response.ok) throw new Error(`Fetch fallido con código ${response.status}`);

    const data = await response.json();
    console.log("📦 Datos recibidos:", data);

    const books = data.userLibraryBooks || data; 
    const usersBooks = data.userBookItems || [];

    if (!books || books.length === 0) {
      const noBooksMessage = document.createElement('p');
      noBooksMessage.textContent = 'Todavía no hay libros añadidos.';
      noBooksMessage.classList.add("no-books-message");
      category1Container.appendChild(noBooksMessage);
      return;
    }

    books.forEach((book, index) => {
      const userBook = usersBooks.find(ub => ub.bookId === book.id);

      if (!userBook) {
        console.warn(`No se encontró el userBook para bookId ${book.id}`);
        return; // evitar errores si falta userBook
      }

      const bookCard = document.createElement('div');
      bookCard.classList.add(index === 0 ? 'featured-book' : 'book-row');

      // Hacer que toda la tarjeta sea clicable excepto el botón eliminar:
      bookCard.style.cursor = "pointer";

      // Cuando se hace clic en el bookCard, redirigimos a la página detalle
      bookCard.addEventListener('click', () => {
        window.location.href = `/book?bookId=${book.id}`;
      });

      // HTML con botón eliminar (evitamos que el botón propague el clic a bookCard)
      if (index === 0) {
        bookCard.innerHTML = `
          <div class="featured-cover">
            <img src="${book.urlcover}" alt="${book.title}" />
          </div>
          <div class="featured-info">
            <h2>${book.title}</h2>
            <p>${book.author} • ${book.category}</p>
            <p>${book.description ?? ''}</p>
          </div>
          <div class="delete-button">
            <button class="action-button" aria-label="Eliminar libro">−</button>
          </div>
        `;
      } else {
        bookCard.innerHTML = `
          <div class="image-placeholder small">
            <img src="${book.urlcover}" alt="${book.title}" />
          </div>
          <div class="book-info">
            <h3>${book.title}</h3>
            <p>${book.author} • ${book.category}</p>
            <p>${book.description ?? ''}</p>
          </div>
          <div class="delete-button">
            <button class="action-button" aria-label="Eliminar libro">−</button>
          </div>
        `;
      }

      // Capturamos el botón para que no propague el clic a bookCard
      const deleteBtn = bookCard.querySelector('button.action-button');
      deleteBtn.addEventListener('click', (event) => {
        event.stopPropagation(); // Evita que el clic llegue a bookCard
        deleteBook(userBook.id);
      });

      category1Container.appendChild(bookCard);
    });

  } catch (e) {
    console.error("❌ Error al recuperar los libros:", e);
    const errorMessage = document.createElement('p');
    errorMessage.textContent = 'Ocurrió un error al cargar los libros.';
    errorMessage.style.color = 'red';
    errorMessage.style.textAlign = 'center';
    category1Container.appendChild(errorMessage);
  }
}





//REDIRECCION A LAS DIFERENTES CATEGORIAS DE LIBROS




function  redirectToCategory() {
  const pendingBooks = document.getElementById("pendings");
  const actualBooks = document.getElementById("actuals");
  const readedBooks = document.getElementById("readed");

  const loggedUser = JSON.parse(localStorage.getItem("loggedUser"));
  console.log("pendingBooks:", pendingBooks);
  console.log("actualBooks:", actualBooks);
  console.log("readedBooks:", readedBooks);

  //redireccion a la pagina de libros pendientes
  pendingBooks.addEventListener("click", async () => {
    window.location.href = `/category?state=pendiente&id_user=${loggedUser.id}`;
  });
  //redireccion a la pagina de libros pendientes
  actualBooks.addEventListener("click", async () => {
    window.location.href = `/category?state=actuales&id_user=${loggedUser.id}`;
  });
  //redireccion a la pagina de libros pendientes
  readedBooks.addEventListener("click", async () => {
    window.location.href = `/category?state=leidos&id_user=${loggedUser.id}`;
  });
}




//MODIFICAR ESTRELLAS




function setupRatings() {
  document.querySelectorAll(".rating").forEach((rating) => {
    const stars = rating.querySelectorAll(".star");
    const bookId = rating.dataset.bookId;

    let currentRating = parseInt(localStorage.getItem(`rating-${bookId}`)) || 0;
    highlightStars(stars, currentRating);

    stars.forEach((star) => {
      star.addEventListener("click", () => {
        const value = parseInt(star.dataset.value);

        if (currentRating === value) {
          
          localStorage.removeItem(`rating-${bookId}`);
          currentRating = 0;
        } else {
          
          localStorage.setItem(`rating-${bookId}`, value);
          currentRating = value;
        }

        highlightStars(stars, currentRating);
      });
    });
  });

  function highlightStars(stars, value) {
    stars.forEach((star, index) => {
      star.textContent = index < value ? "★" : "☆";
    });
  }
}
  



// LOGIN Y REGISTER




//deslogar usuario
function setupLogout() {
  document.getElementById("logout-btn").addEventListener("click", (e) =>{
  e.preventDefault();
  localStorage.removeItem("loggedEmail");
  window.location.href = "/login"; 
  });
}



// ELIMINAR LIBRO



async function deleteBook(bookId) {
  if (!confirm("¿Estás seguro de que quieres eliminar este libro?")) return;

  try {
    const response = await fetch(`http://localhost:5129/api/UserBook/id/${bookId}`, {
      method: 'DELETE',
    });

    if (!response.ok) throw new Error(`Error al eliminar libro: ${response.status}`);

    console.log(`✅ Libro ${bookId} eliminado`);
    await fetchBooks(); 

  } catch (error) {
    console.error("❌ Error al eliminar libro:", error);
    alert("Ocurrió un error al eliminar el libro.");
  }
}



