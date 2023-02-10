$('#table3').DataTable({
    "searching": true,
    "lengthChange": false,
    "info": false,
});

// ------------ FUNCION AGREGAR ----------------

function AddPuestos() {
    event.preventDefault();
    var x = $("#AddPuestos").valid(); // ATENCIÓN AQUÍ, 
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
                var depa = {  // PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS
                    PuestoDescripcion: $("#PuestoDescripcion").val(), 
                    PuestoAreaId: $("#PuestoAreaId").val(), 
                    PuestoJerarquiaSuperiorPuestoId: $("#PuestoJerarquiaSuperiorPuestoId").val(), 
                }
                $.ajax({
                    type: "POST",
                    url: "/AddPuestos",
                    data: { puestos: depa }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("#btnAddPuestos").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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
function DeletePuestos(Id) {
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
                url: "/DeletePuestos?Id=" + Id, // COMO SIEMPRE, REVISA EL NOMBRE DE TU FUNCIÓN Y DE TU VARIABLE ESTABLECIDOS EN EL CONTROLADOR Y EN EL HTML
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
        url: "/PuestosById/" + Id,
        success: function (response) {
            $('#Edit').modal('show');
            $("#Id").val(response.puestoId);
            $("#PuestoDescripcionE").val(response.puestoDescripcion);
            $("#PuestoAreaIdE").val(response.puestoAreaId);
            $("#PuestoJerarquiaSuperiorPuestoIdE").val(response.puestoJerarquiaSuperiorPuestoId);
             // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
        },
        error: function (err) {
            console.log(err)
        }
    });
}

// ------------------------ FUNCIÓN DE EDICIÓN DE DATOS ------------------------
// NOTA: NO OLVIDES REVISAR BIEN EL NOMBRE DE TUS VARIABLES
function EditPuestos() {
    event.preventDefault();
    var x = $("#EditPuestos").valid(); // Edita el nombre
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
                    PuestoId: $("#Id").val(),
                    PuestoDescripcion: $("#PuestoDescripcionE").val(),
                    PuestoAreaId: $("#PuestoAreaIdE").val(),
                    PuestoJerarquiaSuperiorPuestoId: $("#PuestoJerarquiaSuperiorIdE").val(),
                }
                $.ajax({
                    type: "POST",
                    url: "/EditPuestos", // Edita el nombre
                    data: { depa: depa_ },
                    beforeSend: function () {
                        $("#btnEditPuestos").prop("disabled", true); // Edita el nombre
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

// FUNCIÓN DE SELECT DEPENDIENTE DE OTRO SELECT ------------------------------------------------------------

$('#DepaId').on('change', function () {
    $("#PuestoAreaId").html("Seleccione una opción")
});
function SeleccionarDepartamento() {

    var Id = document.getElementById('DepaId').value;
    $.ajax({
        type: "GET",
        url: "/SeleccionarDepartamento?Id=" + Id ,
        success: function (data) {
            Areas = data.areasDep;
            Areas.forEach(function(Areas)
                {
                    $("#PuestoAreaId").append(
                        "<option value=" + Areas.areaId + ">" + Areas.areaDescripcion + "</option>"
                    );
                })
        }
    });
}

$(document).ready(function () {
    AsignarPermisosPuestos();
});

function AsignarPermisosPuestos() {

    $.ajax({
        type: "GET",
        url: "/PermisosPuesto/",
        success: function (data) {

            if (data.banderaPermisos == true) {
                alert("No tienes permiso para ver esta página. Redireccionando a Home");
                window.location.href = "/home";
            } else {


                // Validación para URPCREAR
                if (data.listaPermisos[0].urpCrear == true) {
                    $("#btnAgregarPuesto").show();

                } else {
                    $("#btnAgregarPuesto").hide();
                }

                // Validación para URPMOSTRAR
                if (data.listaPermisos[0].urpMostrar != true) {

                    window.location.href = "/home";

                }


                // Validación para URPELIMINAR
                if (data.listaPermisos[0].urpEliminar == true) {
                    $(".EliminarPuesto").show();

                } else {
                    $(".EliminarPuesto").hide();
                }

            }
        }
    });
}