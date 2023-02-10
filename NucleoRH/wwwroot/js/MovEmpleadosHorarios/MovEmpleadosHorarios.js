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
    document.getElementById('EmpHoraFechaRegistro').value = ano + "-" + mes + "-" + dia;

    $("#FHasta").hide();

    
}

$(document).ready(function () {
    selected = true;
    $('#SeleccionarPermanente').val(this.checked);
    $('#SeleccionarPermanente').change(function () {
        if (this.checked) {
            $("#FHasta").show();
            $('#SeleccionarPermanente').val('Deseleccionar');
        } else {
            $("#FHasta").hide();
            $('#SeleccionarPermanente').val('Seleccionar');
        }
        selected = !selected;
    });
});

// ------------ FUNCION AGREGAR MOVIMIENTOS ----------------

function AddMovimientoEmpleadoHorarios() {
    event.preventDefault();

    var x = $("#AddMovimientoEmpleadoHorarios").valid(); // ATENCIÓN AQUÍ,

   

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
                var MovEmpHor = { // PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS
                   
                    EmpHoraFechaRegistro: $("#EmpHoraFechaRegistro").val(),
                    EmpHoraFechaDesde: $("#EmpHoraFechaDesde").val(),
                    EmpHoraFechaHasta: $("#EmpHoraFechaHasta").val()

                }
                var NumeroEmpleado = document.getElementById('EmpHoraEmpId').value;
                var HoraEntradaLV = document.getElementById('HoraEntradaLV').value;
                var HoraSalidaLV = document.getElementById('HoraSalidaLV').value;
                var HoraEntradaS = document.getElementById('HoraEntradaS').value;
                var HoraSalidaS = document.getElementById('HoraSalidaS').value;
                var HoraComidaLV = document.getElementById('HoraComidaLV').value;
                var HoraComidaS = document.getElementById('HoraComidaS').value;
                var HoraComidaSLV = document.getElementById('HoraComidaSLV').value;
                var HoraComidaSS = document.getElementById('HoraComidaSS').value;
                $.ajax({
                    type: "POST",
                    url: "/AddMovimientoEmpleadoHorarios",
                    data: { meh: MovEmpHor, HELV: HoraEntradaLV, HSLV: HoraSalidaLV, HES: HoraEntradaS, HSS: HoraSalidaS, NoNomina: NumeroEmpleado, HCELV: HoraComidaLV, HCES: HoraComidaS, HCSLV: HoraComidaSLV, HCSS: HoraComidaSS }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("#btnAddMovimientoEmpleadoHorarios").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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

// ------------ FUNCION AGREGAR HORARIOS ----------------

function AddHorarios() {
    event.preventDefault();

    var x = $("#AddHorarios").valid(); // ATENCIÓN AQUÍ,
        
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
                var Hor = { // PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS

                    HoraEntrada: $("#HoraEntrada").val(),
                    HoraSalida: $("#HoraSalida").val(),
                    HoraSabadoEntrada: $("#HoraSabadoEntrada").val(),
                    HoraSabadoSalida: $("#HoraSabadoSalida").val()                   

                }
                $.ajax({
                    type: "POST",
                    url: "/AddHorarios",
                    data: { hora: Hor }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("#btnAddHorarios").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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
function DeleteMovimientos(Id) {
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
                url: "/DeleteMovimientos?Id=" + Id, // COMO SIEMPRE, REVISA EL NOMBRE DE TU FUNCIÓN Y DE TU VARIABLE ESTABLECIDOS EN EL CONTROLADOR Y EN EL HTML
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
        url: "/MovimientosById/" + Id,
        success: function (response) {
            $('#Edit').modal('show');
            $("#Id").val(response.empHoraId);
            $("#EmpHoraFechaRegistroE").val(response.fechaRegistroCorta)
            $("#EmpHoraEmpIdE").val(response.nombreCompleto),
                $("#EmpHoraHorIdE").val(response.horarioCadena),
                $("#EmpHoraFechaDesdeE").val(response.fechaInicioCorta),
                $("#EmpHoraFechaHastaE").val(response.fechaCortaHasta)
           // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
        },
        error: function (err) {
            console.log(err)
        }
    });
}

// ------------------------ FUNCIÓN DE EDICIÓN DE DATOS ------------------------
// NOTA: NO OLVIDES REVISAR BIEN EL NOMBRE DE TUS VARIABLES
function EditMovimientos() {
    event.preventDefault();
    var x = $("#EditMovimientos").valid(); // Edita el nombre
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
                var MovEmpHor = { // Edita los nombres de las variables
                    EmpHoraId: $("#Id").val(),
                    EmpHoraFechaRegistro: $("#EmpHoraFechaRegistroE").val(),
                    EmpHoraEmpId: $("#EmpHoraEmpIdE").val(),
                    EmpHoraHorId: $("#EmpHoraHorIdE").val(),
                    EmpHoraFechaDesde: $("#EmpHoraFechaDesdeE").val(),
                    EmpHoraFechaHasta: $("#EmpHoraFechaHastaE").val()
                }
                $.ajax({
                    type: "POST",
                    url: "/EditMovimientos", // Edita el nombre
                    data: { meh: MovEmpHor },
                    beforeSend: function () {
                        $("#btnEditMovimientos").prop("disabled", true); // Edita el nombre
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
    AsignarPermisosMovimientos();
});

function AsignarPermisosMovimientos() {

    $.ajax({
        type: "GET",
        url: "/PermisosMovimientos/",
        success: function (data) {

            if (data.banderaPermisos == true) {
                alert("No tienes permiso para ver esta página. Redireccionando a Home");
                window.location.href = "/home";
            } else {

                // Validación para URPCREAR
                if (data.listaPermisos[0].urpCrear == true) {
                    $("#btnAgregarMovimiento").show();
                    $("#btnAgregarHorario").show();

                } else {
                    $("#btnAgregarMovimiento").hide();
                    $("#btnAgregarHorario").hide();
                }

                // Validación para URPMOSTRAR
                if (data.listaPermisos[0].urpMostrar != true) {

                    window.location.href = "/home";

                }

                // Validación para URPMODIFICAR
                if (data.listaPermisos[0].urpModificar == true) {
                    $(".EditarMovimiento").show();

                } else {
                    $(".EditarMovimiento").hide();
                }

                // Validación para URPELIMINAR
                if (data.listaPermisos[0].urpEliminar == true) {
                    $(".EliminarMovimiento").show();

                } else {
                    $(".EliminarMovimiento").hide();
                }

            }

        }
    });
}

function OpenModalDetails(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/MovimientosDetalleById/" + Id,
        success: function (response) {
            $('#Details').modal('show');
            $("#EmpHoraFechaRegistroD").val(response.fechaRegistroCorta)
            $("#EmpHoraEmpIdD").val(response.nombreCompleto),
                $("#EmpHoraHorIdD").val(response.horarioCadena),
                $("#EmpHoraFechaDesdeD").val(response.fechaInicioCorta),
                $("#EmpHoraFechaHastaD").val(response.fechaCortaHasta)
           // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES

            // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
        },
        error: function (err) {
            console.log(err)
        }
    });
}