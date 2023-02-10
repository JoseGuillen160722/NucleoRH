$('#table3').DataTable({
    "searching": true,
    "lengthChange": false,
    "info": false,
});

window.onload = function () {
    var fecha = new Date(); //Fecha actual
    var mes = fecha.getMonth() + 1; //obteniendo mes
    var dia = fecha.getDate(); //obteniendo dia
    var ano = fecha.getFullYear(); //obteniendo año
    if (dia < 10)
        dia = '0' + dia; //agrega cero si el menor de 10
    if (mes < 10)
        mes = '0' + mes //agrega cero si el menor de 10
    document.getElementById('SVFechaRegistro').value = ano + "-" + mes + "-" + dia;
}

//  ------------ FUNCION AGREGAR MOVIMIENTOS ----------------

function AddSaldoDeVacaciones() {
    event.preventDefault();
    var x = $("#AddSaldoDeVacaciones").valid(); // ATENCIÓN AQUÍ,
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
                var IdEmpleado = document.getElementById('SVEmpId').value;

                var vacasaldos = { // PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS

                    
                    SVEjercicio: $("#SVEjercicio").val(),
                    SVPeriodoId: $("#SVPeriodoId").val(),
                    SVFechaRegistro: $("#SVFechaRegistro").val(),
                    SVAniosAntiguedad: $("#SVAniosAntiguedad").val(),
                    SVAntiId: $("#SVAntiId").val(),
                    SVDiasDisfrutados: $("#SVDiasDisfrutados").val(),
                    SVDiasRestantes: $("#SVDiasRestantes").val()

                }
                $.ajax({
                    type: "POST",
                    url: "/AddSaldoDeVacaciones",
                    data: { saldovaca: vacasaldos, NoNomina: IdEmpleado}, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("#btnAddSaldoDeVacaciones").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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

$(document).ready(function () {
    AsignarPermisosSaldos();
});

function AsignarPermisosSaldos() {

    $.ajax({
        type: "GET",
        url: "/PermisosSaldos/",
        success: function (data) {

            if (data.banderaPermisos == true) {
                alert("No tienes permiso para ver esta página. Redireccionando a Home");
                window.location.href = "/home";
            } else {
                // Validación para URPCREAR
                if (data.listaPermisos[0].urpCrear == true) {
                    $("#btnAgregarSaldo").show();

                } else {
                    $("#btnAgregarSaldo").hide();
                }

                // Validación para URPMOSTRAR
                if (data.listaPermisos[0].urpMostrar != true) {

                    window.location.href = "/home";

                }

            }

        }
    });
}