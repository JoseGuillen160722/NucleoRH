$('#table3').DataTable({
    "searching": true,
    "lengthChange": false,
    "info": false,
});

// ------------ FUNCION AGREGAR ----------------

function AddPlantilla() {
    event.preventDefault();

    var x = $("#AddPlantilla").valid(); // ATENCIÓN AQUÍ, 

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
                    PlantiDepaId: $("#PlantiDepaId").val(),
                    PlantiSucuId: $("#PlantiSucuId").val(),
                    PlantiPuestoId: $("#PlantiPuestoId").val() // Preguntar aquí porque no está guardando el valor -------------------------------------------
                }
                $.ajax({
                    type: "POST",
                    url: "/AddPlantilla",
                    data: { Plantillas: depa }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("#btnAddPlantilla").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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
function DeletePlantilla(Id) {
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
                url: "/DeletePlantilla?Id=" + Id, // COMO SIEMPRE, REVISA EL NOMBRE DE TU FUNCIÓN Y DE TU VARIABLE ESTABLECIDOS EN EL CONTROLADOR Y EN EL HTML
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
        url: "/PlantillaById/" + Id,
        success: function (response) {
            $('#Edit').modal('show');
            $("#Id").val(response.plantiId); // Recuerda también cambiar el valor del ID de la tabla. ES LA PRIMARY KEY
            $("#PlantiDepaIdE").val(response.plantiDepaId);
            $("#PlantiSucuIdE").val(response.plantiSucuId);
            $("#PlantiPuestoIdE").val(response.plantiPuestoId);
            // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES DESPUÉS DEL VAL, VAN EN MINÚSCULAS
        },
        error: function (err) {
            console.log(err)
        }
    });
}

// ------------------------ FUNCIÓN DE EDICIÓN DE DATOS ------------------------
// NOTA: NO OLVIDES REVISAR BIEN EL NOMBRE DE TUS VARIABLES
function EditPlantilla() {
    event.preventDefault();
    var x = $("#EditPlantilla").valid(); // Edita el nombre
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
                    PlantiId: $("#Id").val(),
                    PlantiDepaId: $("#PlantiDepaIdE").val(),
                    PlantiSucuId: $("#PlantiSucuIdE").val(),
                    PlantiPuestoId: $("#PlantiPuestoIdE").val()
                }
                $.ajax({
                    type: "POST",
                    url: "/EditPlantilla", // Edita el nombre
                    data: { planti: depa_ },
                    beforeSend: function () {
                        $("#btnEditPlantilla").prop("disabled", true); // Edita el nombre
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



$(document).ready(function () {
    AsignarPermisosPlantilla();
});

function AsignarPermisosPlantilla() {

    $.ajax({
        type: "GET",
        url: "/PermisosPlantilla/",
        success: function (data) {

            // Validación para URPCREAR
            if (data.listaPermisos[0].urpCrear == true) {
                $("#btnAgregarPlantilla").show();

            } else {
                $("#btnAgregarPlantilla").hide();
            }

            // Validación para URPMOSTRAR
            if (data.listaPermisos[0].urpMostrar != true) {

                window.location.href = "/home";

            }

            // Validación para URPMODIFICAR
            if (data.listaPermisos[0].urpModificar == true) {
                $(".EditarPlantilla").show();

            } else {
                $(".EditarPlantilla").hide();
            }

            // Validación para URPELIMINAR
            if (data.listaPermisos[0].urpEliminar == true) {
                $(".EliminarPlantilla").show();

            } else {
                $(".EliminarPlantilla").hide();
            }




        }
    });
}