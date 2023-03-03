$('#table3').DataTable({
    "searching": true,
    "lengthChange": false,
    "info": false,
});

$('#table4').DataTable({
    "searching": true,
    "lengthChange": false,
    "info": false,
});

$('#table5').DataTable({
    "searching": true,
    "lengthChange": false,
    "info": false,
});

$('#table6').DataTable({
    "searching": true,
    "lengthChange": false,
    "info": false,
});

$('#table7').DataTable({
    "searching": true,
    "lengthChange": false,
    "info": false,
});

$('#table8').DataTable({
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
    var diaminimo = dia - 5;
    if (diaminimo < 10) {
        diaminimo = "0" + diaminimo;
    }
    document.getElementById('Fecha').value = ano + "-" + mes + "-" + dia;
    document.getElementById('DetFecha').min = ano + "-" + mes + "-" + diaminimo;
    document.getElementById('FechaInicio').min = ano + "-" + mes + "-" + diaminimo;
    document.getElementById('FechaFinal').min = ano + "-" + mes + "-" + diaminimo;
    document.getElementById('FechaPresentacion').min = ano + "-" + mes + "-" + diaminimo;
    ValidarURL();
}

// FUNCIÓN DE SHOW AND HIDE CON LOS FORMULARIOS ------------------------------------------------------------
$(document).ready(function () {
    $("#PermisoPersonal").hide();
    $("#PermisoComision").hide();
    $("#PermisoVacaciones").hide();
});

function SeleccionarIncidencia() {
    var Id = document.getElementById('ReInciInciId').value;
    $.ajax({
        type: "GET",
        url: "/MostrarFormularios/" + Id,
        success: function (data) {
            if ( Id == '8') {
                $("#PermisoPersonal").show();
                $("#HoraLabel").show();
                $("#PermisoComision").hide();
                $("#PermisoVacaciones").hide();
               HorariosDisplay = data.horarios;
                HorariosDisplay.forEach(function (horario) {
                    if (horario.hora == "") {
                        $("#DetInciHorarioID").append(
                            "<option value=" + horario.horaID + ">" + "L-V No se presenta a trabajar. " + " Y los sábados de: " + horario.horaIngresoSabado + " a " + horario.horaSalidaSabado + "</option>"
                        );
                    } else if (horario.horaIngresoSabado == "") {
                        $("#DetInciHorarioID").append(
                            "<option value=" + horario.horaID + ">" + "L-V de: " + horario.hora + " a " + horario.horaSali + " Y los sábados no se presenta a trabajar" + "</option>"
                            );
                    } else {
                        $("#DetInciHorarioID").append(
                            "<option value=" + horario.horaID + ">" + "L-V de: " + horario.hora + " a " + horario.horaSali + " Y los sábados de: " + horario.horaIngresoSabado + " a " + horario.horaSalidaSabado + "</option>"
                           );
                           }
                })
            } else if (Id == '6' || Id == '7') {
               $("#PermisoPersonal").show();
                $("#HoraLabel").hide();
                $("#PermisoComision").hide();
                $("#PermisoVacaciones").hide();
            }
            else if (Id == '11') {
                $("#PermisoPersonal").hide();
                $("#PermisoComision").show();
                $("#PermisoVacaciones").hide();
            } else if (Id == '4') {
                $("#PermisoPersonal").hide();
                $("#PermisoComision").hide();
                $("#PermisoVacaciones").show();
            }
            else if (Id == '5') {
                $("#PermisoPersonal").show();
                $("#HoraLabel").hide();
                $("#PermisoComision").hide();
                $("#PermisoVacaciones").hide();
            } else {
                $("#PermisoPersonal").hide();
                $("#PermisoComision").hide();
                $("#PermisoVacaciones").hide();
            }
        }
    });
}

function ValidacionJefeVacacionesJefeVacacionesIncidenciaById(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/JefeVacacionesIncidenciaById/" + Id,
        success: function (response) {
            if (response.bandera == true) {
                alert("Tu jefe tiene una una solicitud de vacaciones vigente");
            }
        },
        error: function (err) {
            console.log(err)
        }
    });
}

function AddRegistroIncidencias() {
    event.preventDefault();
    var HorarioEmpleado = document.getElementById("HorarioPermanente").value;
    var Estatus = document.getElementById("Estatus").value;
    var fechaI = new Date(document.getElementById("Fecha").value);
    var fechaF = new Date(document.getElementById("DetFecha").value);
    var fechaIncio = new Date(document.getElementById("FechaInicio").value);
    var fechaFinal = new Date(document.getElementById("FechaFinal").value);
    var fechaFinal2 = new Date(document.getElementById("FechaPresentacion").value);
    var tiempo = fechaF.getTime() - fechaI.getTime();
    var dias = Math.floor(tiempo / (1000 * 60 * 60 * 24));
    var tiempo2 = fechaIncio.getTime() - fechaI.getTime();
    var tiempo3 = fechaFinal.getTime() - fechaI.getTime();
    var tiempo4 = fechaFinal2.getTime() - fechaI.getTime();
    var dias2 = Math.floor(tiempo2 / (1000 * 60 * 60 * 24));
    var dias3 = Math.floor(tiempo3 / (1000 * 60 * 60 * 24));
    var dias4 = Math.floor(tiempo4 / (1000 * 60 * 60 * 24));
    if (dias < -5) {
        alert("No se puede solicitar un permiso con más de cinco días de margen con respecto a la fecha actual.");
    } else if (dias2 < -5) {
        alert("No se puede solicitar un permiso con más de cinco días de margen con respecto a la fecha actual.");
    } else if (dias3 < -5) {
        alert("No se puede solicitar un permiso con más de cinco días de margen con respecto a la fecha actual.");
    } else if (dias4 < -5) {
        alert("No se puede solicitar un permiso con más de cinco días de margen con respecto a la fecha actual.");
    }
    else {
        if (HorarioEmpleado == "No tiene") {
            alert("No se puede registrar un permiso si el empleado no tiene asignado un horario permanente");
        } else if (Estatus == "BAJA") {
            alert("No se puede registrar un permiso a un empleado que está dado de baja");
        } else {
            var x = $("#AddRegistroIncidencias").valid(); // ATENCIÓN AQUÍ, 
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
                        var inci = {  // PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS
                            /*ReInciEmpId: $("#ReInciEmpId").val(),*/
                            Fecha: $("#Fecha").val(),
                            ReInciInciId: $("#ReInciInciId").val(),
                            ReInciEstatusId: $("#ReInciEstatusId").val()
                        }
                        var IdEmpleado = document.getElementById('ReInciEmpId').value;
                        var detinci = { // Corresponde a la tabla de detalle de incidencia. LA TABLA DEPENDIENTE DE LA TABLA REGISTRO DE INCIDENCIAS
                            DetInciHorarioID: $("#DetInciHorarioID").val(),
                            MedidaAccion: $("#MedidaAccion").val(),
                            Permiso: $("#Permiso").val(),
                            Asunto: $("#Asunto").val(),
                            Destino: $("#Destino").val(),
                            TelDestino: $("#TelDestino").val(),
                            Contacto1: $("#Contacto1").val(),
                            NombreDestino: $("#NombreDestino").val(),
                            Contacto2: $("#Contacto2").val(),
                            Observaciones: $("#Observaciones").val(),
                            HoraSalida: $("#HoraSalida").val(),
                            HoraRegreso: $("#HoraRegreso").val(),
                            FechaInicio: $("#FechaInicio").val(),
                            FechaFinal: $("#FechaFinal").val(),
                            FechaPresentacion: $("#FechaPresentacion").val(),
                            DiasAusencia: $("#DiasAusencia").val(),
                            PersonaCubrira: $("#PersonaCubrira").val(),
                            Motivo: $("#Motivo").val(),
                            DetFecha: $("#DetFecha").val(),
                            DetInciFechaFinPermisoPersonal: $("#DetFechaFinal").val(),
                            DetInciHoraIngreso: $("#DetInciHoraIngreso").val(),
                            DetInciHoraSalida: $("#DetInciHoraSalida").val(),
                            DetInciHoraSalidaComida: $("#DetInciHoraSalidaComer").val(),
                            DetInciHoraregresoComida: $("#DetInciHoraRegresoComer").val()
                        }
                        $.ajax({
                            type: "POST",
                            url: "/AddRegistroIncidencias",
                            data: { incidencias: inci, detincidencias: detinci, NominaEmpleado: IdEmpleado }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                            beforeSend: function () {
                                $("#btnAddRegistroIncidencias").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
                                Swal.fire({
                                    title: 'Guardando...',
                                    allowEscapeKey: true,
                                    allowOutsideClick: true,
                                    showConfirmButton: false,
                                    onOpen: () => {
                                        Swal.showLoading();
                                    }
                                });

                            },// Aquí se puede meter otro ajax
                            complete: function (data) {
                                swal({
                                    type: 'success',
                                    title: '¡Listo!.',
                                    text: "Se ha guardado con éxito"
                                }).then((result) => {
                                    ValidacionJefeVacacionesJefeVacacionesIncidenciaById(IdEmpleado);
                                    location.reload();
                                });
                            }
                        });
                    }
                });
        }
        }
    }
}

function OpenModalDetails(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/DetallesDeLaIncidencia/" + Id,
        success: function (response) {
            $('#Details').modal('show');
            $("#Id").val(response.reInciId);
            $('#ReInciEmpIdD').val(response.nombreCompleto);
            $('#FechaD').val(response.feGlobal);
            $('#ReInciInciIdD').val(response.incidenciasConversion);
            $('#ReInciEstatusIdD').val(response.estatusConversion);
            $('#DetInciHorarioIDD').val(response.horarioFinal); // Para mostrar los horarios
            $('#DetFechaD').val(response.fechaDetails);
            $('#MotivoD').val(response.motivo);
            $('#MedidaAccionD').val(response.medidaAccion);
            $('#AsuntoD').val(response.asunto);
            $('#DestinoD').val(response.destino);
            $('#TelDestinoD').val(response.telDestino);
            $('#Contacto1D').val(response.contacto1);
            $('#NombreDestinoD').val(response.nombreDestino);
            $('#Contacto2D').val(response.contacto2);
            $('#ObservacionesD').val(response.observaciones);
            $('#HoraSalidaD').val(response.hos);
            $('#HoraRegresoD').val(response.hor);
            $('#FechaInicioD').val(response.feIni);
            $('#FechaFinalD').val(response.feFin);
            $('#FechaPresentacionD').val(response.fePres);
            $('#DiasAusenciaD').val(response.diasAusencia);
            $('#PersonaCubriraD').val(response.empNombreCompleto);

            $('#DetFechaFinalD').val(response.fefinper);
            $('#DetInciHoraIngresoD').val(response.horaIngreso);
            $('#DetInciHoraSalidaD').val(response.horaSalida);
            $('#DetInciHoraSalidaComerD').val(response.horaSComida);
            $('#DetInciHoraRegresoComerD').val(response.horaRComida);
            if (response.incidenciasConversion == "VACACIONES") {
                $("#PermisoVacacionesDetalles").show();
                $("#PermisoComisionDetalles").hide();
                $("#PermisoPersonalDetalles").hide();
            }
            else if (response.incidenciasConversion == "ENTRADAS/SALIDAS" || response.incidenciasConversion == "PERMISOS CON GOCE" || response.incidenciasConversion == "PERMISOS SIN GOCE" || response.incidenciasConversion == "CAMBIOS DE HORARIO") {
                $("#PermisoVacacionesDetalles").hide();
                $("#PermisoComisionDetalles").hide();
                $("#HoraLabelD").hide();
                $("#PermisoPersonalDetalles").show();
                $("#HoraEntradaSD").hide();
                $("#HorasComidaD").hide();
                if (response.incidenciasConversion == "CAMBIOS DE HORARIO") {
                    $("#HoraLabelD").show();

                    $("#HoraEntradaSD").hide();
                    $("#HorasComidaD").hide();
                    
                }
                if (response.incidenciasConversion == "ENTRADAS/SALIDAS") {
                    $("#HoraLabelD").hide();
                    $("#HoraEntradaSD").show();
                    $("#HorasComidaD").show();
                    
                }
            }
            else if (response.incidenciasConversion == "PERMISO EN COMISIÓN") {
                $("#PermisoVacacionesDetalles").hide();
                $("#PermisoComisionDetalles").show();
                $("#PermisoPersonalDetalles").hide();
            } else {
            }
            if (response.reInciEstatusId == 4) {
                $(".Cancelacion2").show();
            }
            else {
                $(".Cancelacion2").hide();
            }
            // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
        },
        error: function (err) {
            console.log(err)
        }
    });
}

 async function OpenModalEdit(Id) {
    return new Promise(await function (resolve, reject) {
        event.preventDefault();
        $.ajax({
            type: "GET",
            url: "/RegistroIncidenciaById/" + Id,
            success: function (response) {
                $('#Edit').modal('show');
                $("#Id").val(response.reInciId);
                $('#ReInciEmpIdE').val(response.idEmpleado);
                $('#FechaE').val(response.fechaCortada);
                $('#ReInciInciIdE').val(response.reInciInciId);
                $('#ReInciEstatusIdE').val(response.reInciEstatusId);
                $('#DetInciHorarioIDOME').val(response.horarioFinalOME); // Para mostrar los horarios
                $('#DetFechaOME').val(response.fechaDetailsOME);
                $('#MotivoOME').val(response.motivoOME);
                $('#MedidaAccionOME').val(response.maOME);
                $('#AsuntoOME').val(response.asuntoOME);
                $('#DestinoOME').val(response.destinoOME);
                $('#TelDestinoOME').val(response.teldestinoOME);
                $('#Contacto1OME').val(response.con1OME);
                $('#NombreDestinoOME').val(response.nombredestinoOME);
                $('#Contacto2OME').val(response.con2OME);
                $('#ObservacionesOME').val(response.observacionesOME);
                $('#HoraSalidaOME').val(response.hosOME);
                $('#HoraRegresoOME').val(response.horOME);
                $('#FechaInicioOME').val(response.feIniOME);
                $('#FechaFinalOME').val(response.feFinOME);
                $('#FechaPresentacionOME').val(response.fePresOME);
                $('#DiasAusenciaOME').val(response.diasausenciaOME);
                $('#PersonaCubriraOME').val(response.personaCubriraOME);

                $('#DetFechaFinalOME').val(response.feIniOME);
                $('#DetInciHoraIngresoOME').val(response.feFinOME);
                $('#DetInciHoraSalidaOME').val(response.fePresOME);
                $('#DetInciHoraSalidaComerOME').val(response.diasausenciaOME);
                $().val(response.personaCubriraOME);
                // Aquí se muestran o se ocultan los DIV de las incidencias dependiendo de cuál se tenga que mostrar
                if (response.permisoBotonEditar == true) {
                    $("#btnEditRegistroIncidencias").show();
                } else {
                    $("#btnEditRegistroIncidencias").hide();
                }
                if (response.banderaEstadoIncidencia == false) {
                    $('#ReInciEstatusIdE').attr("readonly", "readonly");
                    $('#ObservacionesRegistros').attr("readonly", "readonly");
                    $("#btnEditRegistroIncidencias").hide();
                } else {
                    $('#ReInciEstatusIdE').removeAttr("readonly");
                    $('#ObservacionesRegistros').removeAttr("readonly");
                    $("#btnEditRegistroIncidencias").show();
                }
                if (response.incidenciasConversion == "VACACIONES") {

                    $("#PermisoVacacionesDetallesOME").show();
                    $("#PermisoComisionDetallesOME").hide();
                    $("#PermisoPersonalDetallesOME").hide();
                }
                else if (response.incidenciasConversion == "ENTRADAS/SALIDAS" || response.incidenciasConversion == "PERMISOS CON GOCE" || response.incidenciasConversion == "PERMISOS SIN GOCE" || response.incidenciasConversion == "CAMBIOS DE HORARIO") {
                    $("#PermisoVacacionesDetallesOME").hide();
                    $("#PermisoComisionDetallesOME").hide();
                    $("#PermisoPersonalDetallesOME").show();
                    $("#HoraLabelOME").hide();
                    if (response.incidenciasConversion == "CAMBIOS DE HORARIO") {
                        $("#HoraLabelOME").show();
                    }
                    if (response.incidenciasConversion == "ENTRADAS/SALIDAS") {
                        $("#HoraLabelOME").hide();

                        $("#HorasLaboralesCancelacionOME").show();
                        $("#HorasSalidasComidaOME").show();
                    } else {
                        $("#HorasLaboralesCancelacionOME").hide();
                        $("#HorasSalidasComidaOME").hide();
                    }
                }
                else if (response.incidenciasConversion == "PERMISO EN COMISIÓN") {
                    $("#PermisoVacacionesDetallesOME").hide();
                    $("#PermisoComisionDetallesOME").show();
                    $("#PermisoPersonalDetallesOME").hide();
                } else {

                }
                        resolve(true);
                if (response.reInciEstatusId == 4) {
                    $(".Cancelacion2").show();
                }
                else {
                    $(".Cancelacion2").hide();
                }
            },
            error: function (err) {
                console.log(err)
            }
        });
    });

}

// ------------------------ FUNCIÓN DE EDICIÓN DE DATOS ------------------------
// NOTA: NO OLVIDES REVISAR BIEN EL NOMBRE DE TUS VARIABLES
function EditRegistroIncidencias() {
    event.preventDefault();
    var x = $("#EditRegistroIncidencias").valid(); // Edita el nombre
    var EstatusCambiado = document.getElementById("ReInciEstatusIdE").value;
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
                var observaciones = {
                    BitInciReInciId : $("#Id").val(),
                    BitInciObservaciones: $("#ObservacionesRegistros").val()
                }
                $.ajax({
                    type: "POST",
                    url: "/AddBitacoraIncidencias", // Edita el nombre
                    data: { BitInci: observaciones , EstatusNew: EstatusCambiado},
                    beforeSend: function () {
                        $("#btnEditRegistroIncidencias").prop("disabled", true); // Edita el nombre
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
                });
            }
        });
    }
}

function OpenModalEditDetails(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/DetallesIncidenciaById/" + Id,
        success: function (response) {
            $('#EditDetails').modal('show');
            $("#IdED").val(response.reInciId);
            $('#ReInciEmpIdED').val(response.nombreCompletoReadOnly);
            $('#FechaED').val(response.fechaCortita);
            $('#ReInciInciIdED').val(response.incidenciasReadOnly);
            $('#ReInciEstatusIdED').val(response.estatusReadOnly);
            HorariosEdicion = response.horariosedicion;
            HorariosEdicion.forEach(function (horarioedicion) {
                $("#DetInciHorarioIDED").append(
                    "<option value=" + horarioedicion.horaID + ">" + "L-V de: " + horarioedicion.hora + " a " + horarioedicion.horaSali + " Y los sábados de: " + horarioedicion.horaIngresoSabado + " a " + horarioedicion.horaSalidaSabado + "</option>"
                );
            })
            var IdDelHorario = response.detInciHorarioId;
            for (var i = 0; i < response.horariosedicion.length; i++) {
                if (IdDelHorario == response.horariosedicion[i].horaID) {
                    $('#DetInciHorarioIDED').val(response.horariosedicion[i].horaID);
                }
            }
            $('#DetFechaED').val(response.fechaDetalleCorta);
            $('#MotivoED').val(response.motivo);
            $('#MedidaAccionED').val(response.medidaAccion);
            $('#AsuntoED').val(response.asunto);
            $('#DestinoED').val(response.destino);
            $('#TelDestinoED').val(response.telDestino);
            $('#Contacto1ED').val(response.contacto1);
            $('#NombreDestinoED').val(response.nombreDestino);
            $('#Contacto2ED').val(response.contacto2);
            $('#ObservacionesED').val(response.observaciones);
            var HoraHora = response.horaSalida.hours;
            var HoraMinutos = response.horaSalida.minutes;
            var CeroIzquierda = "0";
            if (HoraHora < 10) {
                HoraHora = CeroIzquierda + HoraHora;
            } else {
                HoraHora = HoraHora;
            }
            if (HoraMinutos < 10) {
                HoraMinutos = CeroIzquierda + HoraMinutos;
            } else {
                HoraMinutos = HoraMinutos;
            }
            var HoraSegundos = response.horaSalida.seconds;
            if (HoraSegundos < 10) {
                HoraSegundos = CeroIzquierda + HoraSegundos;
            } else {
                HoraSegundos = HoraSegundos;
            }
            var HoraFinalSalida = HoraHora + ":" + HoraMinutos + ":" + HoraSegundos;
            $('#HoraSalidaED').val(HoraFinalSalida);
            var HoraHoraRegreso = response.horaRegreso.hours;
            var HoraMinutosRegreso = response.horaRegreso.minutes;
            if (HoraMinutosRegreso < 10) {
                HoraMinutosRegreso = CeroIzquierda + HoraMinutosRegreso;
            } else {
                HoraMinutosRegreso = HoraMinutosRegreso;
            }
            var HoraSegundosRegreso = response.horaRegreso.seconds;
            if (HoraSegundosRegreso < 10) {
                HoraSegundosRegreso = CeroIzquierda + HoraSegundosRegreso;
            } else {
                HoraSegundosRegreso = HoraSegundosRegreso;
            }
            var HoraFinalRegreso = HoraHoraRegreso + ":" + HoraMinutosRegreso + ":" + HoraSegundosRegreso;
            $('#HoraRegresoED').val(HoraFinalRegreso);
            $('#FechaInicioED').val(response.fechaInicioVacaCorta);
            $('#FechaFinalED').val(response.fechaFinVacaCorta);
            $('#FechaPresentacionED').val(response.fechaPVacaCorta);
            $('#DiasAusenciaED').val(response.diasAusencia);
            $('#PersonaCubriraED').val(response.empId);
            $('#DetFechaFinalED').val(response.fefinper);
            $('#DetInciHoraIngresoED').val(response.horaIngreso);
            $('#DetInciHoraSalidaED').val(response.horaSalidaPermisoPersonal);
            $('#DetInciHoraSalidaComerED').val(response.horaSComida);
            $('#DetInciHoraRegresoComerED').val(response.horaRComida); 

            // Aquí se muestran o se ocultan los DIV de las incidencias dependiendo de cuál se tenga que mostrar
            if (response.validacionPersonal == true) {
                $("#DIVPERSONAL").show();
            } else {
                $("#DIVPERSONAL").hide();
            }
            if (response.validacionComision == true) {
                $("#DIVCOMISION").show();
            } else {
                $("#DIVCOMISION").hide();
            }
            if (response.validacionVacaciones == true) {
                $("#DIVVacaciones").show();
            } else {
                $("#DIVVacaciones").hide();
            }
            if (response.validacionMuestraHorarios == true) {
                $("#HorarioEditable").show();
                if (response.incidenciasReadOnly == "ENTRADAS/SALIDAS") {
                    $("#HorarioEditable").hide();
                }
            } else {
                $("#HorarioEditable").hide();
            }
            if (response.banderaEstadoIncidencia == false) {
                $("#DetInciHorarioIDED").attr("readonly", "readonly");
                $('#DetFechaED').attr("readonly", "readonly");
                $('#MotivoED').attr("readonly", "readonly");
                $('#MedidaAccionED').attr("readonly", "readonly");
                $('#AsuntoED').attr("readonly", "readonly");
                $('#DestinoED').attr("readonly", "readonly");
                $('#TelDestinoED').attr("readonly", "readonly");
                $('#Contacto1ED').attr("readonly", "readonly");
                $('#NombreDestinoED').attr("readonly", "readonly");
                $('#Contacto2ED').attr("readonly", "readonly");
                $('#ObservacionesED').attr("readonly", "readonly");
                $('#HoraSalidaED').attr("readonly", "readonly");
                $('#HoraRegresoED').attr("readonly", "readonly");
                $('#FechaInicioED').attr("readonly", "readonly");
                $('#FechaFinalED').attr("readonly", "readonly");
                $('#FechaPresentacionED').attr("readonly", "readonly");
                $('#DiasAusenciaED').attr("readonly", "readonly");
                $('#PersonaCubriraED').attr("readonly", "readonly");
                $('#DetFechaFinalED').attr("readonly", "readonly");
                $('#DetInciHoraIngresoED').attr("readonly", "readonly");
                $('#DetInciHoraSalidaED').attr("readonly", "readonly");
                $('#DetInciHoraSalidaComerED').attr("readonly", "readonly");
                $('#DetInciHoraRegresoComerED').attr("readonly", "readonly");
                $("#btnEditDetailsRegistroIncidencias").hide();
            } else {
                $("#DetInciHorarioIDED").removeAttr("readonly");
                $('#DetFechaED').removeAttr("readonly");
                $('#MotivoED').removeAttr("readonly");
                $('#MedidaAccionED').removeAttr("readonly");
                $('#AsuntoED').removeAttr("readonly");
                $('#DestinoED').removeAttr("readonly");
                $('#TelDestinoED').removeAttr("readonly");
                $('#Contacto1ED').removeAttr("readonly");
                $('#NombreDestinoED').removeAttr("readonly");
                $('#Contacto2ED').removeAttr("readonly");
                $('#ObservacionesED').removeAttr("readonly");
                $('#HoraSalidaED').removeAttr("readonly");
                $('#HoraRegresoED').removeAttr("readonly");
                $('#FechaInicioED').removeAttr("readonly");
                $('#FechaFinalED').removeAttr("readonly");
                $('#FechaPresentacionED').removeAttr("readonly");
                $('#DiasAusenciaED').removeAttr("readonly");
                $('#PersonaCubriraED').removeAttr("readonly");
                $('#DetFechaFinalED').removeAttr("readonly");
                $('#DetInciHoraIngresoED').removeAttr("readonly");
                $('#DetInciHoraSalidaED').removeAttr("readonly");
                $('#DetInciHoraSalidaComerED').removeAttr("readonly");
                $('#DetInciHoraRegresoComerED').removeAttr("readonly");
                $("#btnEditDetailsRegistroIncidencias").show();
            }
            if (response.reInciEstatusId == 4) {
                $(".Cancelacion2").show();
            }
            else {
                $(".Cancelacion2").hide();
            }
        },
        error: function (err) {
            console.log(err)
        }
    });
}

// ------------------------ FUNCIÓN DE EDICIÓN DE DATOS ------------------------
// NOTA: NO OLVIDES REVISAR BIEN EL NOMBRE DE TUS VARIABLES
function EditDetailsRegistroIncidencias() {
    event.preventDefault();
    var x = $("#EditDetailsRegistroIncidencias").valid(); // Edita el nombre
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
                var BitInci = { // Edita los nombres de las variables
                    BitInciReInciId: $("#IdED").val(),
                }
                var DetInci = {
                    DetInciHorarioID: $("#DetInciHorarioIDED").val(),
                    MedidaAccion: $("#MedidaAccionED").val(),
                    Asunto: $("#AsuntoED").val(),
                    Destino: $("#DestinoED").val(),
                    TelDestino: $("#TelDestinoED").val(),
                    Contacto1: $("#Contacto1ED").val(),
                    NombreDestino: $("#NombreDestinoED").val(),
                    Contacto2: $("#Contacto2ED").val(),
                    Observaciones: $("#ObservacionesED").val(),
                    HoraSalida: $("#HoraSalidaED").val(),
                    HoraRegreso: $("#HoraRegresoED").val(),
                    FechaInicio: $("#FechaInicioED").val(),
                    FechaFinal: $("#FechaFinalED").val(),
                    FechaPresentacion: $("#FechaPresentacionED").val(),
                    DiasAusencia: $("#DiasAusenciaED").val(),
                    PersonaCubrira: $("#PersonaCubriraED").val(),
                    Motivo: $("#MotivoED").val(),
                    DetFecha: $("#DetFechaED").val()
                }
                $.ajax({
                    type: "POST",
                    url: "/EditDetailsRegistroIncidencias", // Edita el nombre
                    data: { Bitacora: BitInci, DetailsInci: DetInci },
                    beforeSend: function () {
                        $("#btnEditDetailsRegistroIncidencias").prop("disabled", true); // Edita el nombre
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

function OpenModalEditComision(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/ComisionById/" + Id,
        success: function (response) {
            $('#EditComision').modal('show');
            $("#IdEC").val(response.reInciId);
            $('#ReInciEmpIdEC').val(response.nombreCompletoReadOnly);
            $('#FechaEC').val(response.fechaCortita);
            $('#ReInciInciIdEC').val(response.incidenciasReadOnly);
            $('#ReInciEstatusIdEC').val(response.estatusReadOnly);
            $('#AsuntoEC').val(response.asunto);
            $('#DestinoEC').val(response.destino);
            $('#TelDestinoEC').val(response.telDestino);
            $('#Contacto1EC').val(response.contacto1);
            $('#NombreDestinoEC').val(response.nombreDestino);
            $('#Contacto2EC').val(response.contacto2);
            $('#ObservacionesEC').val(response.observaciones);
            var CeroIzquierda = "0";
            var HoraHora = response.horaSalida.hours;
            if (HoraHora < 10) {
                HoraHora = CeroIzquierda + HoraHora;
            } else {
                HoraHora = HoraHora;
            }
            var HoraMinutos = response.horaSalida.minutes;
            
            if (HoraMinutos < 10) {
                HoraMinutos = CeroIzquierda + HoraMinutos;
            } else {
                HoraMinutos = HoraMinutos;
            }
            var HoraSegundos = response.horaSalida.seconds;
            if (HoraSegundos < 10) {
                HoraSegundos = CeroIzquierda + HoraSegundos;
            } else {
                HoraSegundos = HoraSegundos;
            }
            var HoraFinalSalida = HoraHora + ":" + HoraMinutos + ":" + HoraSegundos;
            $('#HoraSalidaEC').val(HoraFinalSalida);
            var HoraHoraRegreso = response.horaRegreso.hours;
            var HoraMinutosRegreso = response.horaRegreso.minutes;

            if (HoraMinutosRegreso < 10) {
                HoraMinutosRegreso = CeroIzquierda + HoraMinutosRegreso;
            } else {
                HoraMinutosRegreso = HoraMinutosRegreso;
            }
            var HoraSegundosRegreso = response.horaRegreso.seconds;
            if (HoraSegundosRegreso < 10) {
                HoraSegundosRegreso = CeroIzquierda + HoraSegundosRegreso;
            } else {
                HoraSegundosRegreso = HoraSegundosRegreso;
            }
            var HoraFinalRegreso = HoraHoraRegreso + ":" + HoraMinutosRegreso + ":" + HoraSegundosRegreso;
            $('#HoraRegresoEC').val(HoraFinalRegreso);
            // Aquí se muestran o se ocultan los DIV de las incidencias dependiendo de cuál se tenga que mostrar
            if (response.permisoBotonEditar == true) {
                $("#btnEditComision").show();
            } else {
                $("#btnEditComision").hide();
            }
            if (response.reInciEstatusId == 4) {
                $(".Cancelacion2").show();
            }
            else {
                $(".Cancelacion2").hide();
            }
        },
        error: function (err) {
            console.log(err)
        }
    });
}

// ------------------------ FUNCIÓN DE EDICIÓN DE DATOS ------------------------
// NOTA: NO OLVIDES REVISAR BIEN EL NOMBRE DE TUS VARIABLES
function EditComision() {
    event.preventDefault();
    var x = $("#EditComisionVigilancia").valid(); // Edita el nombre
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
                var BitInci = { // Edita los nombres de las variables
                    BitInciReInciId: $("#IdEC").val(),
                }
                var DetInci = {
                    Asunto: $("#AsuntoEC").val(),
                    Destino: $("#DestinoEC").val(),
                    TelDestino: $("#TelDestinoEC").val(),
                    Contacto1: $("#Contacto1EC").val(),
                    NombreDestino: $("#NombreDestinoEC").val(),
                    Contacto2: $("#Contacto2EC").val(),
                    Observaciones: $("#ObservacionesEC").val(),
                    HoraSalida: $("#HoraSalidaEC").val(),
                    HoraRegreso: $("#HoraRegresoEC").val()
                }
                $.ajax({
                    type: "POST",
                    url: "/EditComision", // Edita el nombre
                    data: { Bitacora: BitInci, DetailsInci: DetInci },
                    beforeSend: function () {
                        $("#btnEditComision").prop("disabled", true); // Edita el nombre
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
                });
            }
        });
    }
}

// -------------------------------------------- PESTAÑAS ----------------------------------------------
function easyTabs() {
    var groups = document.querySelectorAll('.t-container');
    if (groups.length > 0) {
        for (i = 0; i < groups.length; i++) {
            var tabs = groups[i].querySelectorAll('.t-tab');
            for (t = 0; t < tabs.length; t++) {
                tabs[t].setAttribute("index", t + 1);
                if (t == 0) tabs[t].className = "t-tab selected";
            }
            var contents = groups[i].querySelectorAll('.t-content');
            for (c = 0; c < contents.length; c++) {
                contents[c].setAttribute("index", c + 1);
                if (c == 0) contents[c].className = "t-content selected";
            }
        }
        var clicks = document.querySelectorAll('.t-tab');
        for (i = 0; i < clicks.length; i++) {
            clicks[i].onclick = function () {
                var tSiblings = this.parentElement.children;
                for (i = 0; i < tSiblings.length; i++) {
                    tSiblings[i].className = "t-tab";
                }
                this.className = "t-tab selected";
                var idx = this.getAttribute("index");
                var cSiblings = this.parentElement.parentElement.querySelectorAll('.t-content');
                for (i = 0; i < cSiblings.length; i++) {
                    cSiblings[i].className = "t-content";
                    if (cSiblings[i].getAttribute("index") == idx) {
                        cSiblings[i].className = "t-content selected";
                    }
                }
            };
        }
    }
}

(function () {
    easyTabs();
})();


$(document).ready(function () {
    AsignarPermisosRegistroIncidencias();
    ValidacionPorPerfiles();
});

function AsignarPermisosRegistroIncidencias() {
    $.ajax({
        type: "GET",
        url: "/PermisosRegistroIncidencias/",
        success: function (data) {
            if (data.banderaPermisos == true) {
                alert("No tienes permiso para ver esta página. Redireccionando a Home");
                window.location.href = "/home";
            } else {
                // Validación para URPCREAR
                if (data.listaPermisos[0].urpCrear == true) {
                    $("#btnAgregarRegistro").show();
                } else {
                    $("#btnAgregarRegistro").hide();
                }
                // Validación para URPMOSTRAR
                if (data.listaPermisos[0].urpMostrar != true) {
                    window.location.href = "/home";
                }
            }
        }
    });
}

function ValidacionPorPerfiles() {
    $.ajax({
        type: "GET",
        url: "/ValidacionPorPerfiles/",
        success: function (data) {
            // Validación para Las incidencias propias
            if (data.propiosRegistros == true) {
                $(".TablaIncidenciasPropias").show();
                $("#IncidenciasPropias").show();
            } else {
                $(".TablaIncidenciasPropias").hide();
                $("#IncidenciasPropias").hide();
            }
            // Validación para Las incidencias abiertas
            if (data.registrosAbiertos == true) {
                $(".TablaIncidenciasAbiertas").show();
                $("#IncidenciasAbiertas").show();
            } else {
                $(".TablaIncidenciasAbiertas").hide();
                $("#IncidenciasAbiertas").hide();
            }
            // Validación para Las incidencias abiertas
            if (data.todosLosRegistros == true) {
                $(".TablaIncidenciasCerradas").show();
                $("#IncidenciasCerradas").show();
            } else {
                $(".TablaIncidenciasCerradas").hide();
                $("#IncidenciasCerradas").hide();
            }
            if (data.registrosVigilancia == true) {
                $(".TablaIncidenciasComision").show();
                $("#IncidenciasComision").show();
            } else {
                $(".TablaIncidenciasComision").hide();
                $("#IncidenciasComision").hide();
            }
            if (data.registrosCapitalHumano == true) {
                $(".TablaIncidenciasCapitalHumano").show();
                $("#IncidenciasCapitalHumano").show();
            } else {
                $(".TablaIncidenciasCapitalHumano").hide();
                $("#IncidenciasCapitalHumano").hide();
            }
            if (data.registrosHechosCapitalHumano == true) {
                $(".TablaIncidenciasHechasCH").show();
                $("#IncidenciasHechasCapitalHumano").show();
            } else {
                $(".TablaIncidenciasHechasCH").hide();
                $("#IncidenciasHechasCapitalHumano").hide();
            }
        }
    });
}



function  ValidarURL(a0s9d8f7g6, Fkch23, gs54gf) {
    const valores = window.location.search;
    if (valores != "") {
        const separador1 = "&";
        const separador2 = "=";
        var ListaValores = valores.split(separador1);
        var IdUsuarioJunto = ListaValores[1];
        var FolioJunto = ListaValores[2];
        var BanderaJunta = ListaValores[3];
        var SeparacionUsuario = IdUsuarioJunto.split(separador2);
        var SeparacionFolio = FolioJunto.split(separador2);
        var SeparacionBandera = BanderaJunta.split(separador2);
        IdUsuario = SeparacionUsuario[1];
        var Folio = SeparacionFolio[1];
        Bandera = SeparacionBandera[1];
        Id = Folio;
        let OME;
        var BanderaGlobal = false;
        $.ajax({
            type: "GET",
            url: "/ValidacionIncidenciaById/" + Id + "/" + IdUsuario + "/" + Bandera,
            success: async function (response) {
                if (response.redflag == false) {
                    alert("Datos no correspondientes a la incidencia \n");
                    window.location.href = "/home";
                } else {
                    if (Bandera == "3424hjlk234") { // Aceptada
                          let OME =  await OpenModalEdit(Folio).then();
                        if(OME) {
                            await EditRegistroIncidenciasCorreo();
                        }
                    } else if (Bandera == "jfnROs34") { // Rechazada
                        OpenModalEdit(Folio);
                    } else if (Bandera == "4RT55cgd6FOR") { // Detalles
                        OpenModalDetails(Folio);
                    } else {
                    }
                }
            },
            error: function (err) {
                console.log(err)
            }
        });
    } 
}

// ------------------------ FUNCIÓN DE EDICIÓN DE DATOS ------------------------
// NOTA: NO OLVIDES REVISAR BIEN EL NOMBRE DE TUS VARIABLES
function EditRegistroIncidenciasCorreo() {
    event.preventDefault();
    var x = $("#EditRegistroIncidencias").valid(); // Edita el nombre
    var EstatusCambiado = document.getElementById("ReInciEstatusIdE").value;
    if (x != false) {
                var observaciones = {
                    BitInciReInciId: $("#Id").val(),
                    BitInciObservaciones: $("#ObservacionesRegistros").val()
                }
                $.ajax({
                    type: "POST",
                    url: "/AddBitacoraIncidencias", // Edita el nombre
                    data: { BitInci: observaciones, EstatusNew: EstatusCambiado },
                    beforeSend: function () {
                        $("#btnEditRegistroIncidencias").prop("disabled", true); // Edita el nombre
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
                            window.location.href = "/CatRegistroIncidencias";
                        });
                    },
                });
    }
}

function OpenModalCancelacion(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/DetallesIncidenciaCancelacion/" + Id,
        success: function (response) {
            $('#DetailsCancelacion').modal('show');
            $("#IdC").val(response.reInciId);
            $('#ReInciEmpIdC').val(response.nombreCompleto);
            $('#FechaC').val(response.feGlobal);
            $('#ReInciInciIdC').val(response.incidenciasConversion);
            $('#ReInciEstatusIdC').val(response.estatusConversion);
            $('#DetInciHorarioIDC').val(response.horarioFinal); // Para mostrar los horarios
            $('#DetFechaC').val(response.fechaDetails);
            $('#MotivoC').val(response.motivo);
            $('#MedidaAccionC').val(response.medidaAccion);
            $('#AsuntoC').val(response.asunto);
            $('#DestinoC').val(response.destino);
            $('#TelDestinoC').val(response.telDestino);
            $('#Contacto1C').val(response.contacto1);
            $('#NombreDestinoC').val(response.nombreDestino);
            $('#Contacto2C').val(response.contacto2);
            $('#ObservacionesC').val(response.observaciones);
            $('#HoraSalidaC').val(response.hos);
            $('#HoraRegresoC').val(response.hor);
            $('#FechaInicioC').val(response.feIni);
            $('#FechaFinalC').val(response.feFin);
            $('#FechaPresentacionC').val(response.fePres);
            $('#DiasAusenciaC').val(response.diasAusencia);
            $('#PersonaCubriraC').val(response.personaCubrira);

            $('#DetFechaFinalC').val(response.fefinper);
            $('#DetInciHoraIngresoC').val(response.horaIngreso);
            $('#DetInciHoraSalidaC').val(response.horaSalidaPermisoPersonal);
            $('#DetInciHoraSalidaComerC').val(response.horaSComida);
            $('#DetInciHoraRegresoComerC').val(response.horaRComida);

            if (response.incidenciasConversion == "VACACIONES") {
                $("#PermisoVacacionesDetallesC").show();
                $("#PermisoComisionDetallesC").hide();
                $("#PermisoPersonalDetallesC").hide();
            }
            else if (response.incidenciasConversion == "ENTRADAS/SALIDAS" || response.incidenciasConversion == "PERMISOS CON GOCE" || response.incidenciasConversion == "PERMISOS SIN GOCE" || response.incidenciasConversion == "CAMBIOS DE HORARIO") {
                $("#PermisoVacacionesDetallesC").hide();
                $("#PermisoComisionDetallesC").hide();
                $("#HoraLabelC").hide();
                $("#PermisoPersonalDetallesC").show();
                if (response.incidenciasConversion == "CAMBIOS DE HORARIO") {
                    $("#HoraLabelC").show();
                }
                if (response.incidenciasConversion == "ENTRADAS/SALIDAS") {
                    $('#HorasLaboralesCancelacion').show();
                    $('#HorasSalidasComida').show();
                   
                } else {
                    $('#HorasLaboralesCancelacion').hide();
                    $('#HorasSalidasComida').hide();
                    
                }
            }
            else if (response.incidenciasConversion == "PERMISO EN COMISIÓN") {
                $("#PermisoVacacionesDetallesC").hide();
                $("#PermisoComisionDetallesC").show();
                $("#PermisoPersonalDetallesC").hide();
            } else {
            }
            if (response.reInciEstatusId == 4) {
                $(".Cancelacion2").show();
            }
            else {
                $(".Cancelacion2").hide();
            }
            // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
        },
        error: function (err) {
            console.log(err)
        }
    });
}

// ------------------------ FUNCIÓN DE EDICIÓN DE DATOS ------------------------
// NOTA: NO OLVIDES REVISAR BIEN EL NOMBRE DE TUS VARIABLES
function CancelarPermiso() {
    event.preventDefault();
    var x = $("#CancelarRegistroIncidencias").valid(); // Edita el nombre
    var Id = document.getElementById("IdC").value;
    var Motivo = document.getElementById("MotivoCancelacion").value;
    if (x != false) {
        $.ajax({
            type: "POST",
            url: "/CancelarPermiso", // Edita el nombre
            data: { Id: Id, MotivoCancelacion: Motivo },
            beforeSend: function () {
                $("#btnEditRegistroIncidencias").prop("disabled", true); // Edita el nombre
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
                    window.location.href = "/CatRegistroIncidencias";
                });
            },
        });
    }
}

function ValidarEmpleado(){
    var IdEmpleado = document.getElementById("ReInciEmpId").value;

    $.ajax({
        type: "GET",
        url: "/ValidacionEmpleado/" + IdEmpleado,
        success: function (response) {
            $("#ReInciEmpNombreCompleto").val(response.nombreEmpleado);
            $('#HorarioPermanente').val(response.horarioPermanente);
            $('#Estatus').val(response.estDescripcion);
        },
        error: function (err) {
            console.log(err)
        }
    });

}