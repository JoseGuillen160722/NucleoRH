$('#table3').DataTable({
    "searching": true,
    "lengthChange": false,
    "info": false,
});

function AddEmpTrazabilidad() {
    event.preventDefault();
    var x = $("#AddEmpTrazabilidad").valid(); // ATENCIÓN AQUÍ, 
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
                var traza = {
                    EmpTrazaPassword: $("#EmpTrazaPassword").val() // PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS
                }
                var Nomina = document.getElementById("EmpTrazaNomina").value;
                $.ajax({
                    type: "POST",
                    url: "/AddEmpTrazabilidad",
                    data: { emptraza: traza, Nomina: Nomina }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("#btnAddTrazabilidad").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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
function OpenModalEdit(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/EmpTrazaById/" + Id,
        success: function (response) {
            $('#Edit').modal('show');
            $("#Id").val(response.empTrazaId);
            $("#EmpTrazaNominaE").val(response.empNumero); 
            $("#EmpTrazaPasswordE").val(response.empTrazaPassword); // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
            $("#divpassword").hide();
        },
        error: function (err) {
            console.log(err)
        }
    });
}

function ShowPassword() {
    $("#divpassword").show();
}

// ------------------------ FUNCIÓN DE EDICIÓN DE DATOS ------------------------
// NOTA: NO OLVIDES REVISAR BIEN EL NOMBRE DE TUS VARIABLES
function EditEmpTrazabilidad() {
    event.preventDefault();
    var x = $("#EditEmpTrazabilidad").valid(); // Edita el nombre
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
                var EmpTraza = { // Edita los nombres de las variables
                    EmpTrazaId: $("#Id").val(),
                    EmpTrazaPassword: $("#EmpTrazaPasswordE").val()
                }
                $.ajax({
                    type: "POST",
                    url: "/EditEmpTrazabilidad", // Edita el nombre
                    data: { Traza: EmpTraza },
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

// ------------ FUNCIÓN BORRAR -----------------
function DeleteEmpTrazabilidad(Id) {
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
                url: "/DeleteEmpTrazabilidad?Id=" + Id, // COMO SIEMPRE, REVISA EL NOMBRE DE TU FUNCIÓN Y DE TU VARIABLE ESTABLECIDOS EN EL CONTROLADOR Y EN EL HTML
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
    AsignarPermisosEmpTrazabilidad();
});

function AsignarPermisosEmpTrazabilidad() {

    $.ajax({
        type: "GET",
        url: "/PermisosEmpTrazabilidad/",
        success: function (data) {

            if (data.banderaPermisos == true) {
                alert("No tienes permiso para ver esta página. Redireccionando a Home");
                window.location.href = "/home";
            } else {
                // Validación para URPCREAR
                if (data.listaPermisos[0].urpCrear == true) {
                    $("#btnAgregarDepartamentos").show();

                } else {
                    $("#btnAgregarDepartamentos").hide();
                }

                // Validación para URPMOSTRAR
                if (data.listaPermisos[0].urpMostrar != true) {

                    window.location.href = "/home";

                }

                // Validación para URPMODIFICAR
                if (data.listaPermisos[0].urpModificar == true) {
                    $(".EditarEmpTrazabilidad").show();

                } else {
                    $(".EditarEmpTrazabilidad").hide();
                }

                // Validación para URPELIMINAR
                if (data.listaPermisos[0].urpEliminar == true) {
                    $(".EliminarEmpTrazabilidad").show();

                } else {
                    $(".EliminarEmpTrazabilidad").hide();
                }
            }


        }
    });
}