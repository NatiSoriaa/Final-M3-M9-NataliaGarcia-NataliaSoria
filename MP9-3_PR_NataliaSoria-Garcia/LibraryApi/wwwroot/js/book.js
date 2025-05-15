(async function (){
    //Revisamos que el usuario este loggeado 
    const loggedUserRaw = localStorage.getItem('loggedUser');
     const loggedUser = JSON.parse(loggedUserRaw);
    if (!loggedUser ) {
        Swal.fire({
            title: "Debe estear logueado para acceder a esta página",
            imageUrl: "https://c.tenor.com/1FV-IcFk-uMAAAAC/tenor.gif",
            imageWidth: 400,
            imageHeight: 200,
            imageAlt: "Custom image",
            confirmButtonText: "Ir al login",
            //esto es para que el fondo quede en blur
            backdrop: `
                rgba(0,0,0,0.4)
                left top
                no-repeat
                fixed
            `,
            showClass: {
                popup: `
                animate__animated
                animate__fadeInUp
                animate__faster
                `
            },
            hideClass: {
                popup: `
                animate__animated
                animate__fadeOutDown
                animate__faster
                `
            },
            //esto es para activar y desactivar el blur del fondo:
            didOpen: () => {
                document.getElementById('page-content').style.filter = 'blur(5px)';
            },
            willClose: () => {
                document.getElementById('page-content').style.filter = 'none';
            }
            }).then(() => {
            window.location.href = "/login"; 
            
        });
    }
   
    console.log("El chequedo del usuario loggeado funciona",loggedUser);

    setupLogout();
    redirectToCategory();

    //REVISAMOS QUE LA INFORMACIÓN LLEGA CORRECTA Y RECUPERAMOS LOS DATOS DE LIBRO; DESCRIPCION Y COMENTARIOS.
    const params = new URLSearchParams(window.location.search);
    const bookId = params.get("bookId");
    console.log("ID libro: ",bookId);
    
    const infoLibro= await searchBookInformation(bookId);
    const comentarios = await searchBookComents(bookId,loggedUser.id);
    const descripcion= await BookDescription (infoLibro.title);

    ReactDOM.render(<Book book={infoLibro} descripcion={descripcion} coments={comentarios} userID={loggedUser.id} />, document.getElementById('app'));

    const div = document.querySelector(".fixedRating");
    prinStarts(div,infoLibro.puntuation)


})();

//COMPONENTE PAGINA
function Book({book,descripcion,coments,userID}){
    return (
        <div className="main-header-libro">
            <h1 className="category-title">{book.title}</h1>
        
            <section className="book-detail">
                <div className="image-placeholder detail">
                    <div className="author-Rating">
                        <h4>Autor: {book.author}</h4>
                        <div className="fixedRating" data-book-id="123">
                            <span className="star" data-value="1">☆</span>
                            <span className="star" data-value="2">☆</span>
                            <span className="star" data-value="3">☆</span>
                            <span className="star" data-value="4">☆</span>
                            <span className="star" data-value="5">☆</span>
                        </div>
                    </div>
                    <img className="bookCover" src={book.urlcover}></img>
                </div>
                <div className="book-data">
                    <p className="description">
                        {descripcion.items[0].volumeInfo.description}
                    </p>
                </div>
            </section>
            <Coments coments={coments} book={book} userID={userID}/>
        </div>

    )
}

function Coments({coments, book, userID})
{
    const [editing, setEditing] = React.useState(false);
    const [newComment, setNewComment] = React.useState("");
    const [rating, setRating] = React.useState(0);

    //filtamos los comentarios
    const comentarioUser = coments.find(c => c.userId === userID);
    const comentarios = coments.filter(c => c.userId !== userID);

    const handlePuntuar = (value) =>{
        setRating(value==rating?0:value);
    };

    const handleEnviarcomentario = async(e) => {
        console.log("Comentario: ", newComment, "Rating: ",rating, "userid", userID);

        e.preventDefault();
          const body = {
            userId: userID,
            bookId: book.id,
            comment: newComment,
            rating: rating,
            status: "leidos" //cuando ponemos comentario se actualiza a leido directamente
        };

        try {
            if(comentarioUser)
            {
                const PutResponse = await fetch(`/api/UserBook/${comentarioUser.id}?status=${body.status}&comment=${body.comment}&rating=${body.rating}`,{
                    method: "PUT"
                });

                if(!PutResponse.ok)
                {
                    alert ("Lo sentimos, se ha producido un error al añadir el comentario. Pruebe de nuevo más tarde");
                    throw new Error("Error enviar comentario");
                }
            }
            else
            {
                const  postResponde = await fetch(`/api/UserBook`,{
                    method: "POST",
                    headers : {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(body)
                });

                if(!postResponde.ok)
                {
                    alert ("Lo sentimos, se ha producido un error al añadir el comentario. Pruebe de nuevo más tarde");
                    throw new Error("Error enviar comentario");
                }    
            
            }
        }catch(e)
        {
            alert ("Lo sentimos, se ha producido un error al añadir el comentario. Pruebe de nuevo más tarde");
            console.error(e);
        }
        
    }

    return (
        <section className="reviews">
            <h3>Reseñas</h3>

            {(!comentarios || comentarios.length === 0) && (
                <div className="review">
                <p>Todavía no existen reseñas para este libro</p>
                </div>
            )}

            {comentarios && comentarios.length > 0 && comentarios.map((coment, index) => (
                <div className="review" key={index}>
                    <p><strong>Nickname:</strong> {coment.nickname}</p>
                    <div className="rating" data-book-id="123">
                        <span className="star" data-value="1">☆</span>
                        <span className="star" data-value="2">☆</span>
                        <span className="star" data-value="3">☆</span>
                        <span className="star" data-value="4">☆</span>
                        <span className="star" data-value="5">☆</span>
                    </div>
                    <p>Comentario:</p>
                    <p>{coment.Comment}</p>
                </div>
                ))
            }
            
            <hr />
            {comentarioUser && !editing && (
                <div className="review">
                <p><strong>Tu comentario:</strong></p>
                <div className="rating">
                    {[1, 2, 3, 4, 5].map(val => (
                    <span key={val} className="star">{val <= comentarioUser.rating ? "★" : "☆"}</span>
                    ))}
                </div>
                <p>{comentarioUser.comment}</p>
                <button onClick={() => {
                    setNewComment(comentarioUser.comment);
                    setRating(comentarioUser.rating);
                    setEditing(true);
                }}>Editar</button>
                </div>
            )}
            {(editing || !comentarioUser) && (
                <form onSubmit={handleEnviarcomentario} className="review-form">
                    <label>Tu puntuación:</label>
                    <div className="rating">
                        {[1, 2, 3, 4, 5].map(val => (
                        <span
                            key={val}
                            className="star"
                            style={{ cursor: "pointer", color: val <= rating ? "#ffc107" : "#ccc" }}
                            onClick={() => handlePuntuar(val)}
                        >
                            ★
                        </span>
                        ))}
                    </div>

                    <label>Comentario:</label>
                    <textarea
                        value={newComment}
                        onChange={e => setNewComment(e.target.value)}
                        rows="4"
                        placeholder="Escribe tu reseña..."
                        required
                    />

                    <button type="submit">{comentarioUser ? "Actualizar" : "Publicar"}</button>
                </form>
            )}
        </section>
    )
}

async function searchBookInformation(bookId)
{
    console.log("...Comenzando recuperación informacion libro...");
    const libroData = await fetch (`/LibraryItems/GetOneBooks/${bookId}`);
    if(!libroData.ok)
    {
        throw new Error ('Error al obtener la información del libro');
    }
    const infoLibro= await libroData.json();
    console.log("Informacion libro: ",infoLibro);
    return infoLibro;
}

async function searchBookComents(bookId,userId)
{
    //Recuperar comentarios
    console.log("...comenzando fecth comentarios libro...");
    const comentsData = await fetch(`/api/UserBook?id_book=${bookId}`);
    if (!comentsData.ok) {
      throw new Error('Error al obtener los comentarios del libro');
    }
    
    const listaComentarios = await comentsData.json();
    console.log("Comentarios actuales: ",listaComentarios);
    
    //buscar a los usuarios de esos comentarios
    const listaComentariosConNicknames= await Promise.all(
        listaComentarios.map(async (coment) =>{
            if(coment.userId!=userId){
                const searchUser = await fetch((`/api/User/GetUserID?id=${coment.userId}`));
                
                const userData = await searchUser.json();
                const nickname = userData.userExists.nickname
                return {
                    nickname,
                    comment: coment.coment,
                    rating: coment.rating
                };}
    }));
    return listaComentariosConNicknames;
}

async function BookDescription (bookTitle)
{
    console.log("El titulo que recibimos es: ",bookTitle)
    console.log("...comenzando a extraer la descripcion del libro...");
    const urlfecth=`https://www.googleapis.com/books/v1/volumes?q=${encodeURIComponent(bookTitle)}`;
    console.log("url del fetch: ",urlfecth)
    const descriptionResponse = await fetch(urlfecth);
    if(!descriptionResponse.ok)
    {
        throw new Error("Error al recuperar la descripcion del libro");
        return bookDescription ="Sin datos.";
    } 
    const bookDescription = await descriptionResponse.json();
    console.log("Descripcion del libro: ",bookDescription.items[0].volumeInfo.description);
    return bookDescription;
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

//PINTAT ESTRELLAS CON PUNTUACIÓN:
function prinStarts(container,puntuation)
{
    const stars = container.querySelectorAll(".star");
    stars.forEach((star, index) => {
        star.textContent = index < puntuation ? "★" : "☆"; 
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

