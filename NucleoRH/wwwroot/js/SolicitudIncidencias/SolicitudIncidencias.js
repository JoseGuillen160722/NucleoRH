$('#table3').DataTable({
    "searching": true,
    "lengthChange": false,
    "info": false,
});

// Al atributo de fecha le inserta la fecha del día actual. Ya que está validado para que sea un readonly y no se pueda modificar
window.onload = function () {
    var fecha = new Date(); //Fecha actual
    var mes = fecha.getMonth() + 1; //obteniendo mes
    var dia = fecha.getDate(); //obteniendo dia
    var ano = fecha.getFullYear(); //obteniendo año
    if (dia < 10)
        dia = '0' + dia; //agrega cero si el menor de 10
    if (mes < 10)
        mes = '0' + mes //agrega cero si el menor de 10
    document.getElementById('SolInciFechaRegistro').value = ano + "-" + mes + "-" + dia;
}

// ------------ FUNCION AGREGAR ----------------

function AddSolicitudIncidencias() {
    event.preventDefault();

    var x = $("#AddSolicitudIncidencias").valid(); // ATENCIÓN AQUÍ, 

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
                var solinci = {
                    SolInciReInciId: $("#SolInciReInciId").val(), // PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS
                    SolInciEmpId: $("#SolInciEmpId").val(),
                    SolInciFechaRegistro: $("#SolInciFechaRegistro").val(),
                    SolInciPuestoSuperior: $("#SolInciPuestoSuperior").val(),
                    SolInciPerfiles: $("#SolInciPerfiles").val(),
                    SolInciFlujoId: $("#SolInciFlujoId").val()
                }
                $.ajax({
                    type: "POST",
                    url: "/AddSolicitudIncidencias",
                    data: { Soli: solinci }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("#btnAddSolicitudIncidencias").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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

// ------------------------ FUNCIÓN MODIFICAR ------------------------
// * Primero se debe programar la función de abrir el modal
// * ATENCIÓN ESPECIAL EN LOS NOMBRES DE LAS VARIABLES


// ------------------------ FUNCIÓN DE ABRIR EL MODAL ------------------------
// NOTA: NO OLVIDES REVISAR BIEN EL NOMBRE DE TUS VARIABLES
function OpenModalDetails(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/SolicitudById/" + Id,
        success: function (response) {
            $('#Details').modal('show');
            $("#Id").val(response.soli.solInciId);
            $("#SolInciReInciIdD").val(response.soli.solInciReInciId); // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
            $("#SolInciEmpIdD").val(response.soli.solInciEmpId);
            $("#SolInciFechaRegistroD").val(response.fechaCortada);
            $("#SolInciPuestoSuperiorD").val(response.soli.solInciPuestoSuperior);
            $("#SolInciPerfilesD").val(response.soli.solInciPerfiles);
            $("#SolInciFlujoIdD").val(response.soli.solInciFlujoId);
        },
        error: function (err) {
            console.log(err)
        }
    });
}

function OpenModalEdit(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/SolicitudById/" + Id,
        success: function (response) {
            $('#Edit').modal('show');
            $("#Id").val(response.soli.solInciId);
            $("#SolInciReInciIdE").val(response.soli.solInciReInciId); // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
            $("#SolInciEmpIdE").val(response.soli.solInciEmpId);
            $("#SolInciFechaRegistroE").val(response.fechaCortada);
            $("#SolInciPuestoSuperiorE").val(response.soli.solInciPuestoSuperior);
            $("#SolInciPerfilesE").val(response.soli.solInciPerfiles);
            $("#SolInciFlujoIdE").val(response.soli.solInciFlujoId);
        },
        error: function (err) {
            console.log(err)
        }
    });
}

// ------------------------ FUNCIÓN DE EDICIÓN DE DATOS ------------------------
// NOTA: NO OLVIDES REVISAR BIEN EL NOMBRE DE TUS VARIABLES
function EditSolicitudIncidencia() {
    event.preventDefault();
    var x = $("#EditSolicitudIncidencia").valid(); // Edita el nombre
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
                var solinci = { // Edita los nombres de las variables
                    SolInciId: $("#Id").val(),
                    SolInciReInciId: $("#SolInciReInciIdE").val(), // PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS
                    SolInciEmpId: $("#SolInciEmpIdE").val(),
                    SolInciFechaRegistro: $("#SolInciFechaRegistroE").val(),
                    SolInciPuestoSuperior: $("#SolInciPuestoSuperiorE").val(),
                    SolInciPerfiles: $("#SolInciPerfilesE").val(),
                    SolInciFlujoId: $("#SolInciFlujoIdE").val()
                }
                $.ajax({
                    type: "POST",
                    url: "/EditSolicitudIncidencia", // Edita el nombre
                    data: { Soli: solinci },
                    beforeSend: function () {
                        $("#btnEditDepartamento").prop("disabled", true); // Edita el nombre
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


$(document).ready(function () {
    AsignarPermisosSolicitudIncidencias();
});

function AsignarPermisosDepartamentos() {

    $.ajax({
        type: "GET",
        url: "/PermisosSolicitudIncidencias/",
        success: function (data) {

            // Validación para URPCREAR
            if (data.listaPermisos[0].urpCrear == true) {
                $("#btnAgregarSolicitud").show();

            } else {
                $("#btnAgregarSolicitud").hide();
            }

            // Validación para URPMOSTRAR
            if (data.listaPermisos[0].urpMostrar != true) {

                window.location.href = "/home";

            }

            // Validación para URPMODIFICAR
            if (data.listaPermisos[0].urpModificar == true) {
                $(".EditarSolicitud").show();

            } else {
                $(".EditarSolicitud").hide();
            }

            
        }
    });
}