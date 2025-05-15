document.addEventListener("DOMContentLoaded", function () {
    setupLogout();
    redirectToCategory();
    getBookInformation();

});

//FUNCION RECUPERAR INFORMACIÓN DEL LIBRO
async function getBookInformation()
{
    const params = new URLSearchParams(window.location.search);
    const bookId = params.get("bookId");
    console.log("ID libro: ",bookId, "ID usuario: ",userId);

    const libroData = await fetch (`/api/Library/GetOneBooks?book_id=${bookId}`);
    if(!libroData.ok)
    {
        throw new Error ('Error al obtener la información del libro');
    }
    const infoLibro= await libroData.json();

    //Recuperar comentarios
    const comentsData = await fetch(`/api/Library/GetUserBookItem/id_user=${userId}`);
    if (!comentsData.ok) {
      throw new Error('Error al obtener los comentarios del libro');
    }
    else if (respponse.usersBooks.lenghth === 0) {
      const noBooksMessage = document.createElement('p');
      noBooksMessage.textContent = 'Todavía no hay comentarios.';
    }
    else
    {
      const listaComentarios = await comentsData.json();
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


// LOGIN Y REGISTER
//deslogar usuario
function setupLogout() {
  document.getElementById("logout-btn").addEventListener("click", (e) =>{
  e.preventDefault();
  localStorage.removeItem("loggedUser");
  window.location.href = "/login"; 
  });
}

