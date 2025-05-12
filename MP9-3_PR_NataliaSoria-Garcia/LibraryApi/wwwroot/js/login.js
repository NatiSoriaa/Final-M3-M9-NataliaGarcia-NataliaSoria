
window.onload = function() {
	let loginButton = document.getElementById("login-button");
	let registerButton = document.getElementById("register-button");
	let passwordButton = document.getElementById("password-button");

	loginButton.addEventListener('click', async (e) => {
        e.preventDefault();
    	console.log('Register link clicked');

		const email = document.getElementById('log-user').value;
		const password = document.getElementById('log-passw').value;
		const user = await GetUserByNickname(email, password);
		if (user) {
			alert("Contraseña incorrecta");
			window.location.href = "login-register.html";
			
		}
		else {
			localStorage.setItem('loggedUser', JSON.stringify(user));
			window.location.href = "index.html";
		}
		
      });


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
async function GetUserByNickname(nickname, password) {
    try {
        const response = await fetch(`/api/User/GetUserByNickname?nickname=${nickname}&password=${password}`);
        if (!response.ok) {
			alert("Usuario no encontrado o contraseña incorrecta");
            throw new Error('Usuario no encontrado o contraseña incorrecta');
        }
        const user = await response.json();
        return user;
    } catch (error) {
        console.error(error);
        return null;
    }
}