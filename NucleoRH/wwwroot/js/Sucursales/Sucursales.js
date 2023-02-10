$('#table3').DataTable({
    "searching": true,
    "lengthChange": false,
    "info": false,
});

// ------------ FUNCION AGREGAR ----------------

function AddSucursales() {
    event.preventDefault();

    var x = $("#AddSucursales").valid(); // ATENCIÓN AQUÍ, 

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
                var depa = { // PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS
                    SucuNcorto: $("#SucuNcorto").val(),
                    SucuNombre: $("#SucuNombre").val(),
                    SucuPatId: $("#SucuPatId").val(), // Preguntar aquí porque no está guardando el valor -------------------------------------------
                    SucuEmail: $("#SucuEmail").val()
                }
                $.ajax({
                    type: "POST",
                    url: "/AddSucursales",
                    data: { sucu: depa }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("#btnAddSucursales").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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
function DeleteSucursales(Id) {
    event.preventDefault();
    Swal.fire({
        title: '¿Está seguro?',
        text: "¡SE BORRARÁ EL REGISTRO!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Borrar',
        cancelButtonText: 'Cancelar',
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "POST",
                url: "/DeleteSucursales?Id=" + Id, // COMO SIEMPRE, REVISA EL NOMBRE DE TU FUNCIÓN Y DE TU VARIABLE ESTABLECIDOS EN EL CONTROLADOR Y EN EL HTML
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

// ------------------------ FUNCIÓN MODIFICAR ------------------------
// * Primero se debe programar la función de abrir el modal
// * ATENCIÓN ESPECIAL EN LOS NOMBRES DE LAS VARIABLES


// ------------------------ FUNCIÓN DE ABRIR EL MODAL ------------------------
// NOTA: NO OLVIDES REVISAR BIEN EL NOMBRE DE TUS VARIABLES
function OpenModalEdit(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/SucursalesById/" + Id,
        success: function (response) {
            $('#Edit').modal('show');
            $("#Id").val(response.sucuId);
            $("#SucuNcortoE").val(response.sucuNcorto);
            $("#SucuNombreE").val(response.sucuNombre);
            $("#SucuPatIdE").val(response.sucuPatId);
            $("#SucuEmailE").val(response.sucuEmail);// LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
        },
        error: function (err) {
            console.log(err)
        }
    });
}

// ------------------------ FUNCIÓN DE EDICIÓN DE DATOS ------------------------
// NOTA: NO OLVIDES REVISAR BIEN EL NOMBRE DE TUS VARIABLES
function EditSucursales() {
    event.preventDefault();
    var x = $("#EditSucursales").valid(); // Edita el nombre
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
                var depa_ = { // Edita los nombres de las variables
                    SucuId: $("#Id").val(),
                    SucuNcorto: $("#SucuNcortoE").val(),
                    SucuNombre: $("#SucuNombreE").val(),
                    SucuPatId: $("#SucuPatIdE").val(),
                    SucuEmail: $("#SucuEmailE").val()
                }
                $.ajax({
                    type: "POST",
                    url: "/EditSucursales", // Edita el nombre
                    data: { sucu: depa_ },
                    beforeSend: function () {
                        $("#btnEditSucursales").prop("disabled", true); // Edita el nombre
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
                    }
                });
            }
        });
    }
}


$(document).ready(function () {
    AsignarPermisosSucursales();
});

function AsignarPermisosSucursales() {

    $.ajax({
        type: "GET",
        url: "/PermisosSucursales/",
        success: function (data) {

            if (data.banderaPermisos == true) {
                alert("No tienes permiso para ver esta página. Redireccionando a Home");
                window.location.href = "/home";
            } else {

                // Validación para URPCREAR
                if (data.listaPermisos[0].urpCrear == true) {
                    $("#btnAgregarSucursal").show();

                } else {
                    $("#btnAgregarSucursal").hide();
                }

                // Validación para URPMOSTRAR
                if (data.listaPermisos[0].urpMostrar != true) {

                    window.location.href = "/home";

                }

                // Validación para URPMODIFICAR
                if (data.listaPermisos[0].urpModificar == true) {
                    $(".EditarSucursal").show();

                } else {
                    $(".EditarSucursal").hide();
                }

                // Validación para URPELIMINAR
                if (data.listaPermisos[0].urpEliminar == true) {
                    $(".EliminarSucursal").show();

                } else {
                    $(".EliminarSucursal").hide();
                }

            }

        }
    });
}