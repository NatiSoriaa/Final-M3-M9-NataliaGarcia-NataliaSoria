

// MARCAR Y DESMARCAR ESTRELLAS 
window.onload = function () {
      

    // Función para obtener los libros desde la API
    async function fetchBooks() {
        try {
            const response = await fetch('http://localhost:5129/LibraryItems');
            if (!response.ok) {
                throw new Error('Error al obtener los libros');
            }
            const books = await response.json();

            const category1Container = document.querySelector('.category-section:nth-of-type(1) .book-grid');
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
    fetchBooks(); 

  document.addEventListener("DOMContentLoaded", function () {
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
  });


  // DROPDOWN MENU USER 


  document.addEventListener("DOMContentLoaded", function () {
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
  });




  // LOGIN Y REGISTER



  document.addEventListener("DOMContentLoaded", function () {
    setupFormEvents();
    setupModalEvents();

    function setupFormEvents() {

        document.querySelector('#loginForm').addEventListener('submit', (event) => {
            event.preventDefault();
            const email = document.getElementById('log-user').value;
            const password = document.getElementById('log-passw').value;
            localStorage.setItem('loggedEmail', email); 
            login(email, password); 
        });

        document.querySelector('#registerForm').addEventListener('submit', (event) => {
            event.preventDefault();
            const username = document.getElementById('reg-user').value;
            const email = document.getElementById('reg-email').value;
            const password = document.getElementById('reg-passw').value;
            createUser(username, email, password); 
        });
    }

    function setupModalEvents() {
        const loginBtn = document.getElementById("showLogin");
        const registerBtn = document.getElementById("showRegister");
        const loginForm = document.getElementById("loginForm");
        const registerForm = document.getElementById("registerForm");

        loginForm.classList.add("active");

        loginBtn.addEventListener("click", () => {
            toggleActiveState(loginForm, registerForm, loginBtn, registerBtn);
        });

        registerBtn.addEventListener("click", () => {
            toggleActiveState(registerForm, loginForm, registerBtn, loginBtn);
        });

        function toggleActiveState(activeForm, inactiveForm, activeBtn, inactiveBtn) {
            activeForm.classList.add("active");
            inactiveForm.classList.remove("active");
            activeBtn.classList.add("active");
            inactiveBtn.classList.remove("active");
        }
    }
    
  });
}
  
