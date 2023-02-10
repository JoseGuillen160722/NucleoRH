$('#table3').DataTable({
    "searching": true,
    "lengthChange": false,
    "info": false,
});

// ------------ FUNCION AGREGAR ----------------

function AddUsuario() {
    event.preventDefault();

    var x = $("#AddUsuario").valid(); // ATENCIÓN AQUÍ, 

    if (x != false) {
        Swal.fire({
            title: '¿Desea Guardar?',
            text: "¡No se podrá revertir!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            cancelButtonText: 'Cancelar',
            confirmButtonText: 'Aceptar'
        }).then((result) => {
            if (result.value) {
                var usuario = {
                    UserName: $("#NoNomina").val(), // PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS
                    Email: $("#UserCorreo").val(),
                    PasswordHash: $("#UserPassword").val()
                }
                $.ajax({
                    type: "POST",
                    url: "/AddUsuario",
                    data: { user: usuario }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("#btnAddUsuario").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
                        Swal.fire({
                            title: 'Guardando...',
                            allowEscapeKey: true,
                            allowOutsideClick: true,
                            showConfirmButton: false,
                            onOpen: () => {
                                Swal.showLoading();
                            }
                        });
                    },
                    complete: function (data) {
                        swal({
                            type: 'success',
                            title: '¡Listo!.',
                            text: "Se ha guardado con éxito"
                        }).then((result) => {
                            location.reload();
                        });
                    }
                });
            }
        });
    }
}

// ------------ FUNCIÓN BORRAR -----------------
function DeleteUsuario(Id) {
    event.preventDefault();
    Swal.fire({
        title: '¿Está seguro?',
        text: "¡SE BORRARÁ EL REGISTRO!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'BORRAR',
        cancelButtonText: 'Cancelar',
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "POST",
                url: "/DeleteUsuario?Id=" + Id, // COMO SIEMPRE, REVISA EL NOMBRE DE TU FUNCIÓN Y DE TU VARIABLE ESTABLECIDOS EN EL CONTROLADOR Y EN EL HTML
                success: function (response) {
                    Swal.fire(
                        '¡ELIMINADO!',
                        'Registro Eliminado.',
                        'success'
                    ).then((result) => {
                        location.reload();
                    })
                }
            });
        }
    })
}


$(document).ready(function () {
    AsignarPermisosAltaUsuarios();
});

function AsignarPermisosAltaUsuarios() {

    $.ajax({
        type: "GET",
        url: "/PermisosAltaUsuarios/",
        success: function (data) {

            if (data.banderaPermisos == true) {
                alert("No tienes permiso para ver esta página. Redireccionando a Home");
                window.location.href = "/home";
            } else {
                // Validación para URPCREAR
                if (data.listaPermisos[0].urpCrear == true) {
                    $("#btnAgregarUsuario").show();

                } else {
                    $("#btnAgregarUsuario").hide();
                }

                // Validación para URPMOSTRAR
                if (data.listaPermisos[0].urpMostrar != true) {

                    window.location.href = "/home";

                }

                // Validación para URPELIMINAR
                if (data.listaPermisos[0].urpModificar == true) {
                    $(".ModificarPassword").show();

                } else {
                    $(".ModificarPassword").hide();
                }

                // Validación para URPELIMINAR
                if (data.listaPermisos[0].urpEliminar == true) {
                    $(".EliminarUsuario").show();

                } else {
                    $(".EliminarUsuario").hide();
                }
            }


        }
    });
}

// ------------------------ FUNCIÓN MODIFICAR ------------------------
// * Primero se debe programar la función de abrir el modal
// * ATENCIÓN ESPECIAL EN LOS NOMBRES DE LAS VARIABLES


// ------------------------ FUNCIÓN DE ABRIR EL MODAL ------------------------
// NOTA: NO OLVIDES REVISAR BIEN EL NOMBRE DE TUS VARIABLES
function OpenModalEdit(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/UsuarioById/" + Id,
        success: function (response) {
            $('#Edit').modal('show');
            $("#Id").val(response.id); // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
            $("#NumNominaE").val(response.userName);
        },
        error: function (err) {
            console.log(err)
        }
    });
}

// ------------------------ FUNCIÓN DE EDICIÓN DE DATOS ------------------------
// NOTA: NO OLVIDES REVISAR BIEN EL NOMBRE DE TUS VARIABLES
function EditPassword() {
    event.preventDefault();

    var Password1 = document.getElementById('PasswordE').value;
    var Password2 = document.getElementById('CPasswordE').value;

    if (Password1 != Password2) {
        alert("Las contraseñas no coinciden, revise las mismas.")
    } else {
        var x = $("#EditPassword").valid(); // Edita el nombre
        if (x != false) {
            Swal.fire({
                title: '¿Desea Guardar los cambios?',
                text: "¡No se podrá revertir!",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                cancelButtonText: 'Cancelar',
                confirmButtonText: 'Aceptar'
            }).then((result) => {
                if (result.value) {
                    var NumNominaE = document.getElementById('NumNominaE').value;
                    var PetronilaE = document.getElementById('PasswordE').value;
                    $.ajax({
                        type: "POST",
                        url: "/EditPassword", // Edita el nombre
                        data: { NumNomina: NumNominaE, Password: PetronilaE },
                        beforeSend: function () {
                            $("#btnEditPassword").prop("disabled", true); // Edita el nombre
                            Swal.fire({
                                title: 'Guardando...',
                                allowEscapeKey: false,
                                allowOutsideClick: false,
                                showConfirmButton: false,
                                onOpen: () => {
                                    Swal.showLoading();
                                }
                            });
                        },
                        complete: function (data) {
                            swal({
                                type: 'success',
                                title: '¡Listo!.',
                                text: "Se ha guardado con éxito"
                            }).then((result) => {
                                location.reload();
                            });
                        },

                        error: function (data) {
                            alert('ERROR AL OBTENER DATOS');
                        }
                    });
                }
            });
        }
    }

    
}