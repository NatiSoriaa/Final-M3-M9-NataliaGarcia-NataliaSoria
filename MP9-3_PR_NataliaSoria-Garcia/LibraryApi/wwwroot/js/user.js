
console.log("Babel está funcionando");
(async function () {
    const loggedUser = localStorage.getItem('loggedUser');
    if (!loggedUser || loggedUser === 'null' || loggedUser === '') {
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
    const user= await findUser();
    if(user)
    {
        ReactDOM.render(<Person user={user.userExists} />, document.getElementById('app'));
    }
})();


function Person({user}) {

    const [NewUser, SetNewUser] = React.useState({
        id:user.id,
        nickname:user.nickname,
        email: user.email,
        password:user.password
    });

    const [edit,setEdit] = React.useState(false);
    console.log("Usuario del estado :",NewUser);

    const handleSave = async () => {

        const newUser ={...NewUser};
        if(newUser.password==user.password) delete newUser.password;

        await saveNewUserInformation(newUser);
        setEdit(false);
    };

    return (
    <>
    <div className="container">
        <div className="userInformation">
            <label>Nickname:
            {!edit ?
                <b  id="change">{NewUser.nickname}</b>
            :
                <input type="text" id="newNickname" value={NewUser.nickname} placeholder={user.nickname} onChange={e => SetNewUser({...NewUser, nickname: e.target.value})}/>
            }  
            </label>  
            <label>email:
            {!edit ?
                <b id="change">{NewUser.email}</b>
            :
                <input  type="email" id="newEmail" value={NewUser.email} placeholder={user.email} onChange={e => SetNewUser({...NewUser, email: e.target.value})}/>
            }
            </label>  
            
            <label style={{visibility:(!edit)?'hidden':'visible'}} >Nueva Contraseña:
                <input  type="password" id="newPassword" value={NewUser.password} placeholder="Introduzca nueva contraseña" onChange={e => SetNewUser({...NewUser, password: e.target.value})}/>
            </label> 
            
            
            <button type="submit" className="submitForm" id={(!edit) ? "editButton" :"saveButton" } onClick={edit ? handleSave : ()=>setEdit(!edit)} >{edit ? 'Save Profile' :'Edit Profile'}</button>
    
            <button type="submit" className="submitForm" id="cancelButton" style={{visibility:(!edit)?'hidden':'visible'}} onClick={()=>setEdit(!edit)}>Cancelar</button>
        </div>
    </div>
        
    </>
    );
}

//encontrar usuario
async function findUser(){  
    //recuperamos el id del usuario loggeado
    console.log("Comenzando funcion para encontrar usuario")
    const loggedUser = JSON.parse(localStorage.getItem('loggedUser'));
    if(!loggedUser || !loggedUser.id)
    {
        alert("Error en el login, no se ha recuperado correctamente el id del usuario");
        return;
    }
    const userID =  loggedUser.id;
    console.log("ID del usuario:", userID);

    //buscamos la información de ese usuario
    try 
    {
        const user = await fetch (`/api/User/GetUserID?id=${userID}`)
        if (!user.ok) {
            throw new Error('Error al obtener el usuario');
        }
        const userData = await user.json();
        console.log("usuario encontrado: ",userData)
        return userData;
    }
    catch (e)
    {
        console.log("Error al recuperar la información del usuario");
    } 

};
//FUNCION GUARDAR INFORMACION USUARIO
async function saveNewUserInformation(user)
{

    console.log("🟡 Datos que estás enviando a la API:", JSON.stringify(user, null, 2));

    const updateUser = await fetch (`/api/User/UpdateUser`, {
        method: 'PUT',
        headers : {
            'Content-Type' : 'application/json',
        },
        body: JSON.stringify(user),
    });
    const responseText = await updateUser.text();
    console.log("usuario actualizado correctamente: ",responseText);

    if(!responseText.ok) 
    {
        throw new Error("Error al intentar actualizar el usuario.");
    }
    const userParsed = JSON.parse(responseText);
    console.log("Nueva informacion del usuario: ",userParsed);
    alert('Su información ha sido actualizada correctamente');
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