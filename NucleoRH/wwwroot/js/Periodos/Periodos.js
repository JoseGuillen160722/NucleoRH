$('#table3').DataTable({
    "searching": true,
    "lengthChange": false,
    "info": false,
});


function AddPeriodo() {
    event.preventDefault();

    var x = $("#AddPeriodo").valid(); // ATENCIÓN AQUÍ, 

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
                var per = {
                    PerNum: $("#PerNum").val(),
                    PerFechaDesde: $("#PerFechaDesde").val(),
                    PerFechaHasta: $("#PerFechaHasta").val()// PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS
                }
                $.ajax({
                    type: "POST",
                    url: "/AddPeriodo",
                    data: { periodos: per }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("#btnAddPeriodo").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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
function DeletePeriodo(Id) {
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
                url: "/DeletePeriodo?Id=" + Id, // COMO SIEMPRE, REVISA EL NOMBRE DE TU FUNCIÓN Y DE TU VARIABLE ESTABLECIDOS EN EL CONTROLADOR Y EN EL HTML
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
        url: "/PeriodoById/" + Id,
        success: function (response) {
            $('#Edit').modal('show');
            $("#Id").val(response.perId);
            $("#PerNumE").val(response.perNum);
            var FechaDesde = response.yearDesde + "-" + response.mesDesde + "-" + response.diaD;
            $("#PerFechaDesdeE").val(FechaDesde);
            var FechaHasta = response.yearDesde + "-" + response.mesH + "-" + response.diaH;
            $("#PerFechaHastaE").val(FechaHasta);// LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
        },
        error: function (err) {
            console.log(err)
        }
    });
}

// ------------------------ FUNCIÓN DE EDICIÓN DE DATOS ------------------------
// NOTA: NO OLVIDES REVISAR BIEN EL NOMBRE DE TUS VARIABLES
function EditPeriodo() {
    event.preventDefault();
    var x = $("#EditPeriodo").valid(); // Edita el nombre
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
                var per = { // Edita los nombres de las variables
                    PerId: $("#Id").val(),
                    PerNum: $("#PerNumE").val(),
                    PerFechaDesde: $("#PerFechaDesdeE").val(),
                    PerFechaHasta: $("#PerFechaHastaE").val()
                }
                $.ajax({
                    type: "POST",
                    url: "/EditPeriodo", // Edita el nombre
                    data: { periodos: per },
                    beforeSend: function () {
                        $("#btnEditPeriodo").prop("disabled", true); // Edita el nombre
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
    AsignarPermisosPeriodos();
});

function AsignarPermisosPeriodos() {

    $.ajax({
        type: "GET",
        url: "/PermisosPeriodos/",
        success: function (data) {

            if (data.banderaPermisos == true) {
                alert("No tienes permiso para ver esta página. Redireccionando a Home");
                window.location.href = "/home";
            } else {
                // Validación para URPCREAR
                if (data.listaPermisos[0].urpCrear == true) {
                    $("#btnAgregarPeriodo").show();

                } else {
                    $("#btnAgregarPeriodo").hide();
                }

                // Validación para URPMOSTRAR
                if (data.listaPermisos[0].urpMostrar != true) {

                    window.location.href = "/home";

                }

                // Validación para URPMODIFICAR
                if (data.listaPermisos[0].urpModificar == true) {
                    $(".EditarPeriodos").show();

                } else {
                    $(".EditarPeriodos").hide();
                }

                // Validación para URPELIMINAR
                if (data.listaPermisos[0].urpEliminar == true) {
                    $(".EliminarPeriodos").show();

                } else {
                    $(".EliminarPeriodos").hide();
                }
            }


        }
    });
}