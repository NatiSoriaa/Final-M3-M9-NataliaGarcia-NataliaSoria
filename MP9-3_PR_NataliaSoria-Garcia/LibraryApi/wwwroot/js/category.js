document.addEventListener("DOMContentLoaded", function () {
    setupLogout();
    setupRatings();
    setupDropdown();
    redirectToCategory();
});

//RECUPERAR LOS DATOS E IMPRIMIRLOS

async function fetchBooks() {
  const params = new URLSearchParams(window.location.search);
  const userId = params.get("id_user");
  const state = params.get("state");

  const category1Container = document.querySelector('.category-section .book-grid');
  category1Container.innerHTML = ''; 

  try {
    const response = await fetch(`http://localhost:5129/api/UserBook/state/${state}?id_user=${userId}`)
    if (!response.ok) {
      throw new Error('Error al obtener los libros');
    }
    else if (respponse.usersBooks.lenghth === 0) {
      const noBooksMessage = document.createElement('p');
      noBooksMessage.textContent = 'Todavía no hay libros añadidos.';
    }
    else
    {
      const data = await response.json();

      const books = data.books;
      const usersBooks = data.usersBooks;

      books.forEach((book , index)=> { 
        const bookCard = document.createElement('div');
        bookCard.classList.add('book-card');
        bookCard.innerHTML = `
            <img src="${book.urlcover}" alt="${book.title}" class="book-cover">
            <h3>${book.title}</h3>
            <p>${book.author}</p>
            <p>Año: ${book.publishedDate}</p>
            <p>Rating: ${usersBooks[index].rating}</p>
        `;
        category1Container.appendChild(bookCard);
      });
    }
  }
  catch (e)
  {
    console.error("Error al recuperar los libros")
  }
}

//REDIRECCION A LAS DIFERENTES CATEGORIAS DE LIBROS
 function  redirectToCategory() {
  const pendingBooks = document.getElementById("pending");
  const actualBooks = document.getElementById("actuals");
  const readedBooks = document.getElementById("readed");

  const loggedUser = JSON.parse(localStorage.getItem("loggedUser"));

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
  

// DROPDOWN MENU USER 

function setupDropdown() {
  const userIcon = document.querySelector(".user-icon");
  const dropdownMenu = document.querySelector(".dropdown-menu");

  userIcon.addEventListener("click", (event) => {
    event.preventDefault(); 
    dropdownMenu.classList.toggle("show"); 
  });

  document.addEventListener("click", (event) => {
    if (!userIcon.contains(event.target) && !dropdownMenu.contains(event.target)) {
      dropdownMenu.classList.remove("show");
    }
  });
};


// LOGIN Y REGISTER
//deslogar usuario
function setupLogout() {
  document.getElementById("logout-btn").addEventListener("click", (e) =>{
  e.preventDefault();
  localStorage.removeItem("loggedEmail");
  window.location.href = "/login"; 
  });
}
