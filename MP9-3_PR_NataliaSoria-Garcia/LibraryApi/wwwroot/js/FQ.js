document.addEventListener("DOMContentLoaded", function () {
    setupLogout();
    redirectToCategory();
    sendFormFQ();
});



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

//ENVIAR FORMULARIO

//enviamos mensaje cuando se rellana formulario y resteamos info
function sendFormFQ()
{
    const contactButton = document.getElementById("contactButton");

    const inputNombre = document.getElementById("nombre");
    const inputMail = document.getElementById("email");
    const inputMensaje = document.getElementById("mensaje");

    contactButton.addEventListener('click',(e)=>{
        console.log("Pulsando boton enviar formulario");
        e.preventDefault();

        inputNombre.value="";
        inputMail.value="";
        inputMensaje.value="";

        Swal.fire({
            title: "¡Mensaje enviado!",
            text: "Gracias por contactarnos. Te responderemos pronto.",
            icon: "success",
            confirmButtonText: "OK"
        });
    });
}