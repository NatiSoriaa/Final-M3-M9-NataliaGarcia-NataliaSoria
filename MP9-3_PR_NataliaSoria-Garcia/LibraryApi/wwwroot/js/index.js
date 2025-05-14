document.addEventListener("DOMContentLoaded", function () {
    fetchBooks(); 
    // setupFormEvents();
    // setupModalEvents();
    setupLogout();
    setupRatings();
    setupDropdown();
    fetchLastBooksCarousel();
    redirectToCategory();
});

  
// Función para obtener los libros desde la API
async function fetchBooks() {
    try {
        //recuperamos TODOS los libros
        const response = await fetch('http://localhost:5129/LibraryItems');
        if (!response.ok) {
            throw new Error('Error al obtener los libros');
        }
        const books = await response.json();

        const category1Container = document.querySelector('.category-section .book-grid');
        category1Container.innerHTML = ''; 

        books.forEach(book => { 
            const bookCard = document.createElement('div');
            bookCard.classList.add('book-card');
            bookCard.innerHTML = `
                <img src="${book.urlcover}" alt="${book.title}" class="book-cover">
                <h3>${book.title}</h3>
                <p>${book.author}</p>
                <p>Año: ${book.publishedDate}</p>
            `;
            category1Container.appendChild(bookCard);
        });

    } catch (error) {
        console.error('Error:', error);
    }
}

//funcion carrusel imagenes de CODEPIN
async function fetchLastBooksCarousel() {
    try {
      //recuperamos los 10 ultimos libros para añadirlos en el carrusel "ultimos añadidos"
      const lastBooks = await fetch('http://localhost:5129/LibraryItems/lastBooks');
      if (!lastBooks.ok) {
          throw new Error('Error al obtener los últimos libros');
      }
      const lastBooksData = await lastBooks.json();
 
      const $carousel = $('.carrusel-inspiracion-edp');
      console.log('$carousel:', $carousel);
      $carousel.html(''); 

      lastBooksData.forEach(book => {
        const item = `
          <div class="item-inspiracion">
              <a href="#">
                  <img src="${book.urlcover}" class="img-responsive" alt="${book.title}">
              </a>
              <p class="text-description">
                  ${book.author}
                  <a href="#" class="button-info">más info</a>
              </p>
          </div>
          `;
        $carousel.append(item);
      });

      // Inicializar slick
      if ($carousel.hasClass('slick-initialized')) {
            $carousel.slick('unslick');
        }
      $carousel.slick({
        dots: true,
        arrows: true,
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 1,
        responsive: [
          {
            breakpoint: 768,
            settings: {
                slidesToShow: 1
            }
          }
        ]
      });

    } catch (error) {
        console.error(error);
    }
}

//REDIRECCION A LAS DIFERENTES CATEGORIAS DE LIBROS
 function  redirectToCategory() {
  const pendingBooks = document.getElementById("pendings");
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
  localStorage.removeItem("loggedUser");
  window.location.href = "/login"; 
  });
}





//NO HACEN FALTA YA ESTARÍAN EN EL LOGIN

// function setupFormEvents() {

//     document.querySelector('#loginForm').addEventListener('submit', (event) => {
//         event.preventDefault();
//         const email = document.getElementById('log-user').value;
//         const password = document.getElementById('log-passw').value;
//         localStorage.setItem('loggedEmail', email); 
//         login(email, password); 
//     });

//     document.querySelector('#registerForm').addEventListener('submit', (event) => {
//         event.preventDefault();
//         const username = document.getElementById('reg-user').value;
//         const email = document.getElementById('reg-email').value;
//         const password = document.getElementById('reg-passw').value;
//         createUser(username, email, password); 
//     });
// }

// function setupModalEvents() {
//     const loginBtn = document.getElementById("showLogin");
//     const registerBtn = document.getElementById("showRegister");
//     const loginForm = document.getElementById("loginForm");
//     const registerForm = document.getElementById("registerForm");

//     loginForm.classList.add("active");

//     loginBtn.addEventListener("click", () => {
//         toggleActiveState(loginForm, registerForm, loginBtn, registerBtn);
//     });

//     registerBtn.addEventListener("click", () => {
//         toggleActiveState(registerForm, loginForm, registerBtn, loginBtn);
//     });

//     function toggleActiveState(activeForm, inactiveForm, activeBtn, inactiveBtn) {
//         activeForm.classList.add("active");
//         inactiveForm.classList.remove("active");
//         activeBtn.classList.add("active");
//         inactiveBtn.classList.remove("active");
//     }
// }