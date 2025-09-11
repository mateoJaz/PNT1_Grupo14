const form = document.getElementById('formulario')
form.addEventListener('submit', function(event) {
    event.preventDefault(); 
    const fNombre = document.getElementById('fNombre');
    const fEmail = document.getElementById('fEmail');
    const fTelefono = document.getElementById('fTelefono');
    const fPassword = document.getElementById('fPassword');
    let alerta = '';

    if (nombreInvalido(fNombre.value)) {alerta+= 'Su nombre no esta completo\n';}
    if (emailInvalido(fEmail.value)) {alerta+= 'Su email no es valido\n';}
    if (telefonoInvalido(fTelefono.value)) {alerta+= 'Su telefono no es valido\n';}
    if (passwordInvalido(fPassword.value)) {alerta+= 'Su password no es valida\n';}

    if(alerta === '') {
        form.submit()
    }else{
        alert(alerta)
    }
    
});

function nombreInvalido(nombre) {
  return nombre === '';
}
function emailInvalido(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; //email base, redundante?
    return !emailRegex.test(email);
}
function telefonoInvalido(telefono) {
    const telRegex = /^\d{8}$/ //8 digitos
    return !telRegex.test(telefono);
}
function passwordInvalido(pass) {
    const passRegex = /^(?=.*[a-z])(?=.*[A-Z]).{8,}$/; //1 mayuscula, 1 miniscula, 8 caracteres
    return !passRegex.test(pass);
}