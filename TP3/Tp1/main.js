const form = document.getElementById('formulario')
form.addEventListener('submit', function(event) {
    event.preventDefault(); 
    console.log('entre al submit')
    const fNombre = document.getElementById('fNombre');
    const fEmail = document.getElementById('fEmail');
    const fTelefono = document.getElementById('fTelefono');
    const fPassword = document.getElementById('fPassword');

    if (fNombre.value === '' || fEmail.value === '' ||fTelefono.value === '' ||fPassword.value === ''){
        alert("Por favor complete sus datos antes de cargar el formulario");

    }else {
        form.submit();

    }
});
