
window.onload = function() {
	let loginButton = document.getElementById("login-button");
	let registerButton = document.getElementById("register-button");
	let passwordButton = document.getElementById("password-button");


	//HACER LOGIN
	loginButton.addEventListener('click', async (e) => {
        e.preventDefault();
    	console.log('Register link clicked');

		const nickname = document.getElementById('log-user').value;
		const password = document.getElementById('log-passw').value;
		const user = await GetUserByNickname(nickname, password);
		if (!user) {
			alert("Contraseña incorrecta");
			window.location.href = "/login";
			
		}
		else {
			localStorage.setItem('loggedUser', JSON.stringify(user));
			window.location.href = "/home";
		}
		
      });

	//HACER REGISTRO USUARIO 
	// Mostrar el modal de registro
	const registerLink = document.getElementById('register-link');
	const registerModal = document.getElementById('register-modal');
	const closeRegisterModal = document.getElementById('close-register-modal');


	registerLink.addEventListener('click', (e) => {
		e.preventDefault();
		registerModal.style.display = 'block';
	});

	closeRegisterModal.addEventListener('click', () => {
		registerModal.style.display = 'none';
	});

	//Crear nuevo registro
	registerButton.addEventListener('click', async (e) => {
		e.preventDefault();

		const nickname = document.getElementById('reg-user').value;
		const email = document.getElementById('reg-email').value;
		const password = document.getElementById('reg-pass').value;
		const user = await checkUserExists(nickname, email );
		console.log(user);
		if (user) {
			alert("Ya existe un usuario en nuestra BBDD con ese nickmane o email.");
			return;
		}
		else {
			
			const newUser={
				nickname: nickname,
				email: email,
				password: password
			}

			try {
				const createUserResponse = await fetch('/api/User', {
					method: 'POST',
					headers: {
						'Content-Type': 'application/json'
					},
					body: JSON.stringify(newUser)
				});

				
				document.getElementById('register-modal').style.display = 'none';
				alert("Usuario creado correctamente");
				return;
				
			}
			catch (error) {
				alert("Error al crear el usuario");
				return;
			}
			
		}	
	});

	//RECUPERAR/RESTABLECER CONTRASEÑA
	// Mostrar el modal de recuperar contraseña
	const forgotPasswordLink = document.getElementById('forgot-password-link');
	const forgotPasswordModal = document.getElementById('forgot-password-modal');
	const closeForgotPasswordModal = document.getElementById('close-forgot-password-modal');

	forgotPasswordLink.addEventListener('click', (e) => {
		e.preventDefault();
		forgotPasswordModal.style.display = 'block';
	});

	closeForgotPasswordModal.addEventListener('click', () => {
		forgotPasswordModal.style.display = 'none';
	});

	//hacer reset de contraseña
	passwordButton.addEventListener('click', async (e) => {
		e.preventDefault();

		const email = document.getElementById('forgot-email').value;

		try {
			const resetPAsswordResponse = await fetch('/api/User/passwordForgot', {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json'
				},
				body: JSON.stringify({ email: email })
			});

			if(resetPAsswordResponse.ok) {
				const result = await resetPAsswordResponse.json();
				if (result.nuevaPassword) {
					const  newPassword = result.nuevaPassword;
					alert("Contaseña restablecida. Su contraseña actual es: "+newPassword);
					document.getElementById('forgot-password-modal').style.display = 'none';
				} else {
					alert("Respuesta inesperada del servidor.");
				}
			}
			else {
				alert("No existe un usuario en nuestra BBDD con ese email.");

			}
			window.location.href = "/login";
		}
		catch (error) {
			alert("Error al restablecer la contraseña: "+error.message);
			console.error(error.message);
		}
	});



	// Cerrar los modales al hacer clic fuera de ellos
	window.addEventListener('click', (e) => {
		if (e.target === registerModal) {
			registerModal.style.display = 'none';
		}
		if (e.target === forgotPasswordModal) {
			forgotPasswordModal.style.display = 'none';
		}
	});

}

// FUNCIONES
//funcion para login
async function GetUserByNickname(nickname, password) {
    try {
        const response = await fetch(`/api/User/GetUserByNickname?nickname=${nickname}&password=${password}`);
        // if (!response.ok) {
		// 	alert("Usuario no encontrado o contraseña incorrecta");
        //     throw new Error('Usuario no encontrado o contraseña incorrecta');
        // }
        const user = await response.json();
        return user;
    } catch (error) {
        console.error(error);
        return null;
    }
}

//funcion para register
async function checkUserExists(nickname, email) {
 
	const response = await fetch(`/api/User/UserExists?nickname=${nickname}&email=${email}`);
	if(!response.ok)
	{
		throw new Error("Error al comprobar si el usuario existe");
	}
	const userExists=await response.json();
	console.log("Respuesta existencia usuario:",userExists);

	return userExists.userExists;

}