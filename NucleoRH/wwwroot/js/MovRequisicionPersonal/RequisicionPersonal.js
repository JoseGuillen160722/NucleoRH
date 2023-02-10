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

// Al atributo de fecha le inserta la fecha del día actual. Ya que está validado para que sea un readonly y no se pueda modificar
window.onload = function () {
    ValidarURL();
    var fecha = new Date(); //Fecha actual
    var mes = fecha.getMonth() + 1; //obteniendo mes
    var dia = fecha.getDate(); //obteniendo dia
    var ano = fecha.getFullYear(); //obteniendo año
    if (dia < 10)
        dia = '0' + dia; //agrega cero si el menor de 10
    if (mes < 10)
        mes = '0' + mes //agrega cero si el menor de 10
    
    document.getElementById('MRPFechaElaboracion').value = ano + "-" + mes + "-" + dia;
    document.getElementById('MRPFechaRecepcion').value = ano + "-" + mes + "-" + dia;
}

$(document).ready(function () {
    AsignarPermisosRequisiciones();
});

function AsignarPermisosRequisiciones() {

    $.ajax({
        type: "GET",
        url: "/PermisosRequisiciones/",
        success: function (data) {

            if (data.banderaPermisos == true) {
                alert("No tienes permiso para ver esta página. Redireccionando a Home");
                window.location.href = "/home";
            } else {
                // Validación para URPCREAR
                if (data.listaPermisos[0].urpCrear == true) {
                    $("#btnAgregarRequisicion").show();
                    $("#btnAgregarPuesto").show();
                    $(".GenerarPDF").show();
                } else {
                    $("#btnAgregarRequisicion").hide();
                    $("#btnAgregarPuesto").hide();
                    $(".GenerarPDF").hide();
                }

                // Validación para URPMOSTRAR
                if (data.listaPermisos[0].urpMostrar != true) {
                    window.location.href = "/home";
                }

                // Validación para URPMODIFICAR
                if (data.listaPermisos[0].urpModificar == true) {
                    $(".EditarDetallesRequisicion").show();
                    $(".AceptarRequisicion").show();
                } else {
                    $(".EditarDetallesRequisicion").hide();
                    $(".AceptarRequisicion").hide();
                }
                // Validación para URPELIMINAR
                if (data.listaPermisos[0].urpEliminar == true) {
                    $(".CancelarRequisicion").show();
                } else {
                    $(".CancelarRequisicion").hide();
                }
            }
        }
    });
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

// ********************************************************************************************************************************************************

function AgregarPuestos() {
    event.preventDefault();
    var x = $("#AgregarPuestos").valid(); // ATENCIÓN AQUÍ, 
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
                var puesto = {  // PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS
                    PuestoDescripcion: $("#PuestoDescripcionRP").val(),
                    PuestoAreaId: $("#PuestoAreaIdRP").val(),
                    PuestoJerarquiaSuperiorPuestoId: $("#PuestoJerarquiaSuperiorPuestoIdRP").val(),
                }
                $.ajax({
                    type: "POST",
                    url: "/AddPuesto",
                    data: { puestos: puesto }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("#btnAgregarPuestos").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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
                            window.location.href = "/RequisicionPersonal";
                        });
                    }
                });
            }
        });
    }
}

// FUNCIÓN DE SELECT DEPENDIENTE DE OTRO SELECT ------------------------------------------------------------

$('#DepaIdRP').on('change', function () {
    $("#PuestoAreaIdRP").html("Seleccione una opción")
});
function SeleccionarDepartamento() {
    var Id = document.getElementById('DepaIdRP').value;
    $.ajax({
        type: "GET",
        url: "/SeleccionarDepartamentoRP?Id=" + Id,
        success: function (data) {
            Areas = data.areasDep;
            Areas.forEach(function (Areas) {
                $("#PuestoAreaIdRP").append(
                    "<option value=" + Areas.areaId + ">" + Areas.areaDescripcion + "</option>"
                );
            })
        }
    });
}

$('#MRPPuestoId').on('change', function () {
    $("#MRPEmplId").html("Seleccione una opción")
});
function EmpleadoByAreaAndPuesto() {
    var IdMotivo = document.getElementById("MRPMotivoVacante").value;
    var Id = document.getElementById("MRPPuestoId").value;
    if (IdMotivo == 1) {
        $("#MotivoDescripcion").show();
        $("#EmpleadoMRP").hide();
    } else if (Id == "") {
        alert("Por favor introduzca el puesto para realizar la búsqueda de los empleados correspondientes")
    } else {
        $("#MotivoDescripcion").hide();
        $("#EmpleadoMRP").show();
        $.ajax({
            type: "GET",
            url: "/SeleccionarEmpleadoRP?Id=" + Id,
            success: function (data) {
                Emp = data.empleadoJoin;
                Emp.forEach(function (Emp) {
                    $("#MRPEmplId").append(
                        "<option value=" + Emp.epbEmpId + ">" + Emp.epbEmpNombreCompleto + "</option>"
                    );
                })
            }
        });
    }
}

function AgregarRequisicion() {
    event.preventDefault();
    var x = $("#AgregarRequisicion").valid(); // ATENCIÓN AQUÍ, 
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
                var requisicion = {  // PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS
                    MRPNumeroVacantes: $("#MRPNumeroVacantes").val(),
                    MRPSucursalId: $("#MRPSucursalId").val(),                    
                    MRPTipoVacante: $("#MRPTipoVacante").val(),
                    MRPRolarTurno: $("#MRPRolarTurno").val(),
                    MRPTiempoAlimentos: $("#MRPTiempoAlimentos").val(),
                    MRPMotivoVacante: $("#MRPMotivoVacante").val(),
                    MRPMotivoDescripcion: $("#MRPMotivoDescripcion").val(),
                    MRPSexoId: $("#MRPSexoId").val(),
                    MRPEdadMinima: $("#MRPEdadMinima").val(),
                    MRPEdadMaxima: $("#MRPEdadMaxima").val(),
                    MRPEscolaridadId: $("#MRPEscolaridadId").val(),
                    MRPTituloIndispensable: $("#MRPTituloIndispensable").val(),
                    MRPCedulaIndispensable: $("#MRPCedulaIndispensable").val(),
                    MRPExperienciaIndispensable: $("#MRPExperienciaIndispensable").val(),
                    MRPFuncionesPuesto: $("#MRPFuncionesPuesto").val(),
                    MRPConocimientosPuesto: $("#MRPConocimientosPuesto").val(),
                    MRPFechaElaboracion: $("#MRPFechaElaboracion").val(), 
                    MRPTurnoId: $("#MRPTurnoId").val(),
                    MRPPuestoId: $("#MRPPuestoId").val(),
                    MRPEmpId: $("#MRPEmplId").val()
                }
                $.ajax({
                    type: "POST",
                    url: "/AgregarPuestos",
                    data: { MRP: requisicion }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("btnAddPuestos").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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
                            window.location.href = "/RequisicionPersonal";
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

var global;
// ------------------------ FUNCIÓN DE ABRIR EL MODAL ------------------------
// NOTA: NO OLVIDES REVISAR BIEN EL NOMBRE DE TUS VARIABLES
function OpenModalRequisicion(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/RequisicionById/" + Id,
        success: function (response) {
            // Apartado de la creación de la requisición
            $('#Edit').modal('show');
            $("#Id").val(response.mrpId);
            $("#MRPPuestoIdE").val(response.puestoDescripcion);
            $("#MRPNumeroVacantesE").val(response.mrpNumeroVacantes);
            $("#MRPFechaElaboracionE").val(response.fe);
            $("#MRPTipoVacanteE").val(response.tipoVacante);
            $("#MRPTurnoIdE").val(response.turDescripcion);
            $("#MRPRolarTurnoE").val(response.turnoRolado);
            $("#MRPSucursalIdE").val(response.sucuNombre);
            $("#MRPTiempoAlimentosE").val(response.mrpTiempoAlimentos);
            $("#MRPMotivoVacanteE").val(response.motivoVacante);
            if (response.motivoVacante == "Nueva creación") {
                $("#MRPMotivoDescripcionE").show();
                $("#MRPEmpId").hide();
            } else {
                $("#MRPEmpId").show();
                $("#MRPMotivoDescripcionE").hide();
            }
            $("#MRPMotivoDescripcionE").val(response.mrpMotivoDescripcion);
            $("#MRPEmpId").val(response.nombreEmpleado);
            $("#MRPSexoIdE").val(response.sexDescripcion);
            $("#MRPEscolaridadIdE").val(response.escoDescripcion);
            $("#MRPEdadMinimaE").val(response.mrpEdadMinima);
            $("#MRPEdadMaximaE").val(response.mrpEdadMaxima);
            $("#MRPTituloIndispensableE").val(response.titulo);
            $("#MRPCedulaIndispensableE").val(response.cedula);
            $("#MRPExperienciaIndispensableE").val(response.experiencia);
            $("#MRPFuncionesPuestoE").val(response.mrpFuncionesPuesto);
            $("#MRPConocimientosPuestoE").val(response.mrpConocimientosPuesto);
            $("#MRPEstatusId").val(response.mrpEstatusId);
            // Apartado de sueldos
            $("#MRPFolio").val(response.mrpFolio);
            $("#MRPSueldoMensualInicial").val(response.mrpSueldoMensualInicial);
            $("#MRPSueldoMensualPlanta").val(response.mrpSueldoMensualPlanta);
            $("#MRPSueldoMensualMasCosto").val(response.mrpSueldoMensualMasCosto);
            $("#MRPBonoVariable").val(response.mrpBonoVariable);
            $("#MRPEsquemaContratacion").val(response.mrpEsquemaContratacion);
            $("#MRPCandidato").val(response.mrpCandidato);
            $("#MRPFechaRecepcion").val(response.fecharecepcioncorta);
            $("#MRPFechaIngreso").val(response.ingreso);

            var Button = document.querySelector("#btnAceptarRequisicion");
            Button.setAttribute("disabled", "disabled");
            Button.setAttribute("style", "display:none");
            global = response.banderaPaso;
            var respuesta = response.banderaPaso;
            
            $("#btnMostrarSueldos").show();
            $("#btnRechazarRequisicion").show();
            $("#Sueldos").hide();

            if (respuesta == "Dirección") {
                $("#Sueldos").show();
                $("#btnMostrarSueldos").hide();
                $("#btnRechazarRequisicion").show();
                var Button = document.querySelector("#btnAceptarRequisicion");
                Button.removeAttribute("disabled", "disabled");
                Button.removeAttribute("style", "display:none");
                //$('#MRPSueldoMensualInicial').attr("readonly", "readonly");
                //$('#MRPSueldoMensualPlanta').attr("readonly", "readonly");
                //$('#MRPSueldoMensualMasCosto').attr("readonly", "readonly");
                //$('#MRPBonoVariable').attr("readonly", "readonly");
                $('#MRPEsquemaContratacion').attr("readonly", "readonly");
                $('#MRPCandidato').attr("readonly", "readonly");
                $('#MRPFechaIngreso').attr("readonly", "readonly");
                $('#MRPFolio').attr("readonly", "readonly");
                $('#MRPFechaRecepcion').attr("readonly", "readonly");
                $('#MRPFechaRecepcion').show();
            } else if (respuesta == "CapitalHumano") {
                $("#Sueldos").show();
            } else if (respuesta == "CH") {
                $("#btnMostrarSueldos").show();
                $("#btnRechazarRequisicion").show();
                $("#Sueldos").hide();
                $('#MRPSueldoMensualInicial').removeAttr("readonly");
                $('#MRPSueldoMensualPlanta').removeAttr("readonly");
                $('#MRPSueldoMensualMasCosto').removeAttr("readonly");
                $('#MRPBonoVariable').removeAttr("readonly");
                $('#MRPMotivoVacante').removeAttr("readonly");
                $('#MRPCandidato').removeAttr("readonly");
                $('#MRPFechaIngreso').removeAttr("readonly");
                $('#MRPFolio').removeAttr("readonly");
                $('#MRPFechaRecepcion').removeAttr("readonly");
                $('#MRPFechaRecepcion').hide();
                $('#LabelFechaRecepcion').hide();
            } else {
                alert("Excepción encontrada, redirigiendo a home");
                window.location.href = "/home";
            }
            if (response.mrpEstatusId == 4) {
                $(".Cancelacion").show();
                $("#btnMostrarSueldos").hide();
                $("#btnRechazarRequisicion").hide();
                $("#btnAceptarRequisicion").hide();
                var BotonMostrarSueldos = document.querySelector("#btnAceptarRequisicion");
                BotonMostrarSueldos.setAttribute("disabled", "disabled");
                BotonMostrarSueldos.setAttribute("style", "display:none");
                var BotonRechazarRequisicion = document.querySelector("#btnRechazarRequisicion");
                BotonRechazarRequisicion.setAttribute("disabled", "disabled");
                BotonRechazarRequisicion.setAttribute("style", "display:none");
                var BotonAceptarRequisicion = document.querySelector("#btnMostrarSueldos");
                BotonAceptarRequisicion.setAttribute("disabled", "disabled");
                BotonAceptarRequisicion.setAttribute("style", "display:none");
            }
            else {
                $(".Cancelacion").hide();
                $("#btnMostrarSueldos").show();
                $("#btnRechazarRequisicion").show();
                $("#btnAceptarRequisicion").hide();
                //var BotonMostrarSueldos = document.querySelector("#btnAceptarRequisicion");
                //BotonMostrarSueldos.removeAttribute("disabled");
                //BotonMostrarSueldos.removeAttribute("style", "display:none");
                var BotonRechazarRequisicion = document.querySelector("#btnRechazarRequisicion");
                BotonRechazarRequisicion.removeAttribute("disabled");
                BotonRechazarRequisicion.removeAttribute("style", "display:none");
                
            }
        },
        error: function (err) {
            console.log(err)
        }
    });
}

function OpenModalDetails(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/RequisicionDetallesById/" + Id,
        success: function (response) {
            $('#Details').modal('show');
            $("#IdD").val(response.mrpId);
            $("#MRPPuestoIdD").val(response.puestoDescripcion);
            $("#MRPNumeroVacantesD").val(response.mrpNumeroVacantes);
            $("#MRPFechaElaboracionD").val(response.fe);
            $("#MRPTipoVacanteD").val(response.tipoVacante);
            $("#MRPTurnoIdD").val(response.turDescripcion);
            $("#MRPRolarTurnoD").val(response.turnoRolado);
            $("#MRPSucursalIdD").val(response.sucuNombre);
            $("#MRPTiempoAlimentosD").val(response.mrpTiempoAlimentos);
            $("#MRPMotivoVacanteD").val(response.motivoVacante);
            $("#MRPMotivoDescripcionD").val(response.mrpMotivoDescripcion);
            $("#MRPSexoIdD").val(response.sexDescripcion);
            $("#MRPEscolaridadIdD").val(response.escoDescripcion);
            $("#MRPEdadMinimaD").val(response.mrpEdadMinima);
            $("#MRPEdadMaximaD").val(response.mrpEdadMaxima);
            $("#MRPTituloIndispensableD").val(response.titulo);
            $("#MRPCedulaIndispensableD").val(response.cedula);
            $("#MRPExperienciaIndispensableD").val(response.experiencia);
            $("#MRPFuncionesPuestoD").val(response.mrpFuncionesPuesto);
            $("#MRPConocimientosPuestoD").val(response.mrpConocimientosPuesto);
            $("#MRPFolioD").val(response.mrpFolio);
            $("#MRPSueldoMensualInicialD").val(response.mrpSueldoMensualInicial);
            $("#MRPSueldoMensualPlantaD").val(response.mrpSueldoMensualPlanta);
            $("#MRPSueldoMensualMasCostoD").val(response.mrpSueldoMensualMasCosto);
            $("#MRPBonoVariableD").val(response.mrpBonoVariable);
            $("#MRPEsquemaContratacionD").val(response.mrpEsquemaContratacion);
            $("#MRPCandidatoD").val(response.mrpCandidato);
            $("#MRPFechaRecepcionD").val(response.fecharecepcioncorta);
            $("#MRPFechaIngresoD").val(response.ingreso);
            if (response.mrpEstatusId == 4) {
                $(".Cancelacion").show();
            }
            else {
                $(".Cancelacion").hide();
            }
        },
        error: function (err) {
            console.log(err)
        }
    });
}



function AceptarRequisicion() {
    event.preventDefault();
    var x = $("#EditRequisicion").valid(); // ATENCIÓN AQUÍ, 
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
                var Id = document.getElementById("Id").value;
                var Observaciones = {
                    BitRPObservaciones: $("#BitRPObservaciones").val()
                }
                var sueldo = {
                    MRPSueldoMensualInicial: $("#MRPSueldoMensualInicial").val(),
                    MRPSueldoMensualPlanta: $("#MRPSueldoMensualPlanta").val(),
                    MRPSueldoMensualMasCosto: $("#MRPSueldoMensualMasCosto").val(),
                    MRPBonoVariable: $("#MRPBonoVariable").val(),
                    MRPEsquemaContratacion: $("#MRPEsquemaContratacion").val(),
                    MRPCandidato: $("#MRPCandidato").val(),
                    MRPFechaIngreso: $("#MRPFechaIngreso").val(),
                    MRPFolio: $("#MRPFolio").val(),
                }
                $.ajax({
                    type: "POST",
                    url: "/AddBitacoraRequisicion",
                    data: { BRP: Observaciones, Id: Id, MRPSueldos: sueldo }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("#btnAceptarRequisicion").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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
                            window.location.href = "/RequisicionPersonal";
                        });
                    }
                });
            }
        });
    }
}

function HabilitarCamposRequisicion() {
    var Button = document.querySelector("#btnAceptarRequisicion");
    Button.removeAttribute("disabled", "disabled");
    Button.removeAttribute("style", "display:none");
    $("#MRPEstatusId").hide();
    $("#LabelEstatus").hide();
    $("#btnMostrarSueldos").hide();
    $("#btnRechazarRequisicion").hide();
    $("#btnRegresarRequisicion").hide();
    if (global == "CH") {
        $("#Sueldos").show();
    } else if (global == "Dirección") {
        $("#Sueldos").show();
    } else if (global == "CapitalHumano") {
        $("#Sueldos").show();
    } else {
        alert("Excepción encontrada, redirigiendo a home");
        window.location.href = "/home";
    }
}

function RechazarRequisicion() {
    var ObservacionesRP = document.getElementById("BitRPObservaciones").value;
    if (ObservacionesRP == "") {
        alert("Por favor, indique los motivos por los cuales se está rechazando la requisición de personal.");
    } else {
        event.preventDefault();
        var x = $("#EditRequisicion").valid(); // ATENCIÓN AQUÍ, 
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
                    var Id = document.getElementById("Id").value;

                    var Observaciones = {
                        BitRPObservaciones: $("#BitRPObservaciones").val()
                    }
                    var Estatus = document.getElementById("MRPEstatusId").value;
                    $.ajax({
                        type: "POST",
                        url: "/RechazarRequisicion",
                        data: { BRP: Observaciones, Id: Id }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                        beforeSend: function () {
                            $("#btnRechazarRequisicion").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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
                                window.location.href = "/RequisicionPersonal";
                            });
                        }
                    });
                }
            });
        }
    }
}

function RechazarRequisicion() {
    var ObservacionesRP = document.getElementById("BitRPObservaciones").value;
    if (ObservacionesRP == "") {
        alert("Por favor, indique los motivos por los cuales se está rechazando la requisición de personal.");
    } else {
        event.preventDefault();
        var x = $("#EditRequisicion").valid(); // ATENCIÓN AQUÍ, 
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
                    var Id = document.getElementById("Id").value;

                    var Observaciones = {
                        BitRPObservaciones: $("#BitRPObservaciones").val()
                    }
                    var Estatus = document.getElementById("MRPEstatusId").value;
                    $.ajax({
                        type: "POST",
                        url: "/RechazarRequisicion",
                        data: { BRP: Observaciones, Id: Id }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                        beforeSend: function () {
                            $("#btnRechazarRequisicion").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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
                                window.location.href = "/RequisicionPersonal";
                            });
                        }
                    });
                }
            });
        }
    }
}

function ValidarURL() {
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
        $.ajax({
            type: "GET",
            url: "/ValidacionRequisicionById/" + Id + "/" + IdUsuario + "/" + Bandera,
            success: async function (response) {
                if (response.redFlag == false) {
                    alert("Datos no correspondientes a la incidencia \n");
                    window.location.href = "/home";
                } else {
                    if (Bandera == response.flag) { // Detalles
                        OpenModalDetails(Folio);
                    } else if (Bandera == response.flag) { // AceptarRechazar
                        OpenModalRequisicion(Folio);
                    } else if (Bandera == response.flag) { //Edicion sin sueldos
                        OpenModalEditDetails(Id)
                    } 
                    else {
                        alert("Datos no correspondientes a la incidencia \n");
                        window.location.href = "/home";
                    }
                }
            },
            error: function (err) {
                console.log(err)
            }
        });
    }
}

function OpenModalEditDetails(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/RequisicionEditarDetallesById/" + Id,
        success: function (response) {
            $('#EditDetails').modal('show');
            $("#IdED").val(response.mrpId);
            $("#MRPPuestoIdED").val(response.mrpPuestoId);
            $("#MRPNumeroVacantesED").val(response.mrpNumeroVacantes);
            var Fecha = new Date(response.mrpFechaElaboracion);
            var Mes = Fecha.getMonth() + 1;
            var Dia = Fecha.getDate();
            if (Fecha.getMonth() < 10) {
                Mes = "0" + Fecha.getMonth();
            }
            if (Fecha.getDate() < 10) {
                Dia = "0" + Fecha.getDate();
            }
            Fecha = Fecha.getFullYear() + "-" + Mes + "-" + Dia;
            $("#MRPFechaElaboracionED").val(Fecha); 
            $("#MRPTipoVacanteED").val(response.mrpTipoVacante);
            $("#MRPTurnoIdED").val(response.mrpTurnoId);
            var RolarTurno = document.querySelector("#MRPRolarTurnoED");
            if (response.mrpRolarTurno) {
                RolarTurno.checked = true;
            } else {
                RolarTurno.checked = false;
            }
            $("#MRPSucursalIdED").val(response.mrpSucursalId);
            $("#MRPTiempoAlimentosED").val(response.mrpTiempoAlimentos);
            $("#MRPMotivoVacanteED").val(response.mrpMotivoVacante);
            $("#MRPMotivoDescripcionED").val(response.mrpMotivoDescripcion);
            $("#MRPSexoIdED").val(response.mrpSexoId);
            $("#MRPEscolaridadIdED").val(response.mrpEscolaridadId);
            $("#MRPEdadMinimaED").val(response.mrpEdadMinima);
            $("#MRPEdadMaximaED").val(response.mrpEdadMaxima);
            var Titulo = document.querySelector("#MRPTituloIndispensableED");
            if (response.mrpTituloIndispensable) {
                Titulo.checked = true;
            } else {
                Titulo.checked = false;
            }
            var Cedula = document.querySelector("#MRPCedulaIndispensableED");
            if (response.mrpCedulaIndispensable) {
                Cedula.checked = true;
            } else {
                Cedula.checked = false;
            }
            var Exp = document.querySelector("#MRPExperienciaIndispensableED");
            if (response.mrpExperienciaIndispensable) {
                Exp.checked = true;
            } else {
                Exp.checked = false;
            }
            $("#MRPFuncionesPuestoED").val(response.mrpFuncionesPuesto);
            $("#MRPConocimientosPuestoED").val(response.mrpConocimientosPuesto);
            $("#MRPFolioED").val(response.mrpFolio);
            $("#MRPSueldoMensualInicialED").val(response.mrpSueldoMensualInicial);
            $("#MRPSueldoMensualPlantaED").val(response.mrpSueldoMensualPlanta);
            $("#MRPSueldoMensualMasCostoED").val(response.mrpSueldoMensualMasCosto);
            $("#MRPBonoVariableED").val(response.mrpBonoVariable);
            $("#MRPEsquemaContratacionED").val(response.mrpEsquemaContratacion);
            $("#MRPCandidatoED").val(response.mrpCandidato);
            Fecha = new Date(response.mrpFechaRecepcion);
            Mes = Fecha.getMonth() + 1;
            Dia = Fecha.getDate();
            if (Fecha.getMonth() < 10) {
                Mes = "0" + Fecha.getMonth();
            }
            if (Fecha.getDate() < 10) {
                Dia = "0" + Fecha.getDate();
            }
            Fecha = Fecha.getFullYear() + "-" + Mes + "-" + Dia;
            $("#MRPFechaRecepcionED").val(Fecha);
            Fecha = new Date(response.mrpFechaIngreso);
            Mes = Fecha.getMonth() + 1;
            Dia = Fecha.getDate();
            if (Fecha.getMonth() < 10) {
                Mes = "0" + Fecha.getMonth();
            }
            if (Fecha.getDate() < 10) {
                Dia = "0" + Fecha.getDate();
            }
            Fecha = Fecha.getFullYear() + "-" + Mes + "-" + Dia;
            $("#MRPFechaIngresoED").val(Fecha);
            if (response.bandera) {
                $("#SueldosED").show();
                $("#MRPPuestoIdED").attr("readonly", "readonly");
                $("#MRPNumeroVacantesED").attr("readonly", "readonly");
                $("#MRPFechaElaboracionED").attr("readonly", "readonly");
                $("#MRPTipoVacanteED").attr("readonly", "readonly");
                $("#MRPTurnoIdED").attr("readonly", "readonly");
                $("#MRPRolarTurnoED").attr("disabled", "disabled");
                $("#MRPSucursalIdED").attr("readonly", "readonly");
                $("#MRPTiempoAlimentosED").attr("readonly", "readonly");
                $("#MRPMotivoVacanteED").attr("readonly", "readonly");
                $("#MRPMotivoDescripcionED").attr("readonly", "readonly");
                $("#MRPSexoIdED").attr("readonly", "readonly");
                $("#MRPEscolaridadIdED").attr("readonly", "readonly");
                $("#MRPEdadMinimaED").attr("readonly", "readonly");
                $("#MRPEdadMaximaED").attr("readonly", "readonly");
                $("#MRPTituloIndispensableED").attr("disabled", "disabled");
                $("#MRPCedulaIndispensableED").attr("disabled", "disabled");
                $("#MRPExperienciaIndispensableED").attr("disabled", "disabled");
                $("#MRPFuncionesPuestoED").attr("readonly", "readonly");
                $("#MRPConocimientosPuestoED").attr("readonly", "readonly");
                $('#MRPSueldoMensualInicialED').removeAttr("readonly");
                $('#MRPSueldoMensualPlantaED').removeAttr("readonly");
                $('#MRPSueldoMensualMasCostoED').removeAttr("readonly");
                $('#MRPBonoVariableED').removeAttr("readonly");
                $('#MRPCandidatoED').removeAttr("readonly");
                $('#MRPFechaIngresoED').removeAttr("readonly");
                $('#MRPFolioED').removeAttr("readonly");
                $('#MRPFechaRecepcionED').removeAttr("readonly");
                $("#MRPFolioED").attr("required", "required");
                $("#MRPSueldoMensualInicialED").attr("required", "required");
                $("#MRPSueldoMensualPlantaED").attr("required", "required");
                $("#MRPSueldoMensualMasCostoED").attr("required", "required");
                $("#MRPBonoVariableED").attr("required", "required");
                $("#MRPEsquemaContratacionED").attr("required", "required");
                $("#MRPCandidatoED").attr("required", "required");
                $("#MRPFechaRecepcionED").attr("required", "required");
                $("#MRPFechaIngresoED").attr("required", "required");
            } else {
                $("#SueldosED").hide();
                $('#MRPSueldoMensualInicialED').removeAttr("required");
                $('#MRPSueldoMensualPlantaED').removeAttr("required");
                $('#MRPSueldoMensualMasCostoED').removeAttr("required");
                $('#MRPBonoVariableED').removeAttr("required");
                $('#MRPCandidatoED').removeAttr("required");
                $('#MRPFechaIngresoED').removeAttr("required");
                $('#MRPFolioED').removeAttr("required");
                $('#MRPFechaRecepcionED').removeAttr("required");
                $('#MRPEsquemaContratacionED').removeAttr("required");
                $("#MRPFolioED").attr("readonly", "readonly");
                $("#MRPSueldoMensualInicialED").attr("readonly", "readonly");
                $("#MRPSueldoMensualPlantaED").attr("readonly", "readonly");
                $("#MRPSueldoMensualMasCostoED").attr("readonly", "readonly");
                $("#MRPBonoVariableED").attr("readonly", "readonly");
                $("#MRPEsquemaContratacionED").attr("readonly", "readonly");
                $("#MRPCandidatoED").attr("readonly", "readonly");
                $("#MRPFechaRecepcionED").attr("readonly", "readonly");
                $("#MRPFechaIngresoED").attr("readonly", "readonly");
                $("#MRPPuestoIdED").removeAttr("readonly");
                $("#MRPNumeroVacantesED").removeAttr("readonly");
                $("#MRPFechaElaboracionED").removeAttr("readonly");
                $("#MRPTipoVacanteED").removeAttr("readonly");
                $("#MRPTurnoIdED").removeAttr("readonly");
                $("#MRPRolarTurnoED").removeAttr("disabled");
                $("#MRPSucursalIdED").removeAttr("readonly");
                $("#MRPTiempoAlimentosED").removeAttr("readonly");
                $("#MRPMotivoVacanteED").removeAttr("readonly");
                $("#MRPMotivoDescripcionED").removeAttr("readonly");
                $("#MRPSexoIdED").removeAttr("readonly");
                $("#MRPEscolaridadIdED").removeAttr("readonly");
                $("#MRPEdadMinimaED").removeAttr("readonly");
                $("#MRPEdadMaximaED").removeAttr("readonly");
                $("#MRPTituloIndispensableED").removeAttr("disabled");
                $("#MRPCedulaIndispensableED").removeAttr("disabled");
                $("#MRPExperienciaIndispensableED").removeAttr("disabled");
                $("#MRPFuncionesPuestoED").removeAttr("readonly");
                $("#MRPConocimientosPuestoED").removeAttr("readonly");
            }
            if (response.detFlujoOrden == 3) {
                $("#MRPPuestoIdED").attr("readonly", "readonly");
                $("#MRPNumeroVacantesED").attr("readonly", "readonly");
                $("#MRPFechaElaboracionED").attr("readonly", "readonly");
                $("#MRPTipoVacanteED").attr("readonly", "readonly");
                $("#MRPTurnoIdED").attr("readonly", "readonly");
                $("#MRPRolarTurnoED").attr("disabled", "disabled");
                $("#MRPSucursalIdED").attr("readonly", "readonly");
                $("#MRPTiempoAlimentosED").attr("readonly", "readonly");
                $("#MRPMotivoVacanteED").attr("readonly", "readonly");
                $("#MRPMotivoDescripcionED").attr("readonly", "readonly");
                $("#MRPSexoIdED").attr("readonly", "readonly");
                $("#MRPEscolaridadIdED").attr("readonly", "readonly");
                $("#MRPEdadMinimaED").attr("readonly", "readonly");
                $("#MRPEdadMaximaED").attr("readonly", "readonly");
                $("#MRPTituloIndispensableED").attr("disabled", "disabled");
                $("#MRPCedulaIndispensableED").attr("disabled", "disabled");
                $("#MRPExperienciaIndispensableED").attr("disabled", "disabled");
                $("#MRPFuncionesPuestoED").attr("readonly", "readonly");
                $("#MRPConocimientosPuestoED").attr("readonly", "readonly");
                $("#MRPFolioED").attr("readonly", "readonly");
                $("#MRPSueldoMensualInicialED").attr("readonly", "readonly");
                $("#MRPSueldoMensualPlantaED").attr("readonly", "readonly");
                $("#MRPSueldoMensualMasCostoED").attr("readonly", "readonly");
                $("#MRPBonoVariableED").attr("readonly", "readonly");
                $("#MRPEsquemaContratacionED").attr("readonly", "readonly");
                $("#MRPCandidatoED").attr("readonly", "readonly");
                $("#MRPFechaRecepcionED").attr("readonly", "readonly");
                $("#MRPFechaIngresoED").attr("readonly", "readonly");
                $("#btnEditarDetallesRequisicion").attr("disabled", "disabled");
                $("#btnEditarDetallesRequisicion").hide();
            }
            if (response.banderaPaso2) {
                $('#MRPSueldoMensualInicialED').removeAttr("readonly");
                $('#MRPSueldoMensualPlantaED').removeAttr("readonly");
                $('#MRPSueldoMensualMasCostoED').removeAttr("readonly");
                $('#MRPBonoVariableED').removeAttr("readonly");
                $('#MRPCandidatoED').removeAttr("readonly");
                $('#MRPFechaIngresoED').removeAttr("readonly");
                $('#MRPFolioED').removeAttr("readonly");
                $('#MRPFechaRecepcionED').removeAttr("readonly");
            } else {
                $("#MRPFolioED").attr("readonly", "readonly");
                $("#MRPSueldoMensualInicialED").attr("readonly", "readonly");
                $("#MRPSueldoMensualPlantaED").attr("readonly", "readonly");
                $("#MRPSueldoMensualMasCostoED").attr("readonly", "readonly");
                $("#MRPBonoVariableED").attr("readonly", "readonly");
                $("#MRPEsquemaContratacionED").attr("readonly", "readonly");
                $("#MRPCandidatoED").attr("readonly", "readonly");
                $("#MRPFechaRecepcionED").attr("readonly", "readonly");
                $("#MRPFechaIngresoED").attr("readonly", "readonly");
                $("#btnEditarDetallesRequisicion").attr("disabled", "disabled");
                $("#btnEditarDetallesRequisicion").hide();
            }
            if (response.mrpEstatusId == 4) {
                $(".Cancelacion").show();
                $("#MRPPuestoIdED").attr("readonly", "readonly");
                $("#MRPNumeroVacantesED").attr("readonly", "readonly");
                $("#MRPFechaElaboracionED").attr("readonly", "readonly");
                $("#MRPTipoVacanteED").attr("readonly", "readonly");
                $("#MRPTurnoIdED").attr("readonly", "readonly");
                $("#MRPRolarTurnoED").attr("disabled", "disabled");
                $("#MRPSucursalIdED").attr("readonly", "readonly");
                $("#MRPTiempoAlimentosED").attr("readonly", "readonly");
                $("#MRPMotivoVacanteED").attr("readonly", "readonly");
                $("#MRPMotivoDescripcionED").attr("readonly", "readonly");
                $("#MRPSexoIdED").attr("readonly", "readonly");
                $("#MRPEscolaridadIdED").attr("readonly", "readonly");
                $("#MRPEdadMinimaED").attr("readonly", "readonly");
                $("#MRPEdadMaximaED").attr("readonly", "readonly");
                $("#MRPTituloIndispensableED").attr("disabled", "disabled");
                $("#MRPCedulaIndispensableED").attr("disabled", "disabled");
                $("#MRPExperienciaIndispensableED").attr("disabled", "disabled");
                $("#MRPFuncionesPuestoED").attr("readonly", "readonly");
                $("#MRPConocimientosPuestoED").attr("readonly", "readonly");
                $("#MRPFolioED").attr("readonly", "readonly");
                $("#MRPSueldoMensualInicialED").attr("readonly", "readonly");
                $("#MRPSueldoMensualPlantaED").attr("readonly", "readonly");
                $("#MRPSueldoMensualMasCostoED").attr("readonly", "readonly");
                $("#MRPBonoVariableED").attr("readonly", "readonly");
                $("#MRPEsquemaContratacionED").attr("readonly", "readonly");
                $("#MRPCandidatoED").attr("readonly", "readonly");
                $("#MRPFechaRecepcionED").attr("readonly", "readonly");
                $("#MRPFechaIngresoED").attr("readonly", "readonly");
                $("#btnEditarDetallesRequisicion").attr("disabled", "disabled");
                $("#btnEditarDetallesRequisicion").hide();
                $("#btnEditarDetallesRequisicion").hide();
                var BotonEditarDetalles = document.querySelector("#btnEditarDetallesRequisicion");
                BotonEditarDetalles.setAttribute("disabled", "disabled");
                BotonEditarDetalles.setAttribute("style", "display:none");
            }
            else {
                $(".Cancelacion").hide();
                $("#MRPPuestoIdED").removeAttr("readonly");
                $("#MRPNumeroVacantesED").removeAttr("readonly");
                $("#MRPFechaElaboracionED").removeAttr("readonly");
                $("#MRPTipoVacanteED").removeAttr("readonly");
                $("#MRPTurnoIdED").removeAttr("readonly");
                $("#MRPRolarTurnoED").removeAttr("disabled");
                $("#MRPSucursalIdED").removeAttr("readonly");
                $("#MRPTiempoAlimentosED").removeAttr("readonly");
                $("#MRPMotivoVacanteED").removeAttr("readonly");
                $("#MRPMotivoDescripcionED").removeAttr("readonly");
                $("#MRPSexoIdED").removeAttr("readonly");
                $("#MRPEscolaridadIdED").removeAttr("readonly");
                $("#MRPEdadMinimaED").removeAttr("readonly");
                $("#MRPEdadMaximaED").removeAttr("readonly");
                $("#MRPTituloIndispensableED").removeAttr("disabled");
                $("#MRPCedulaIndispensableED").removeAttr("disabled");
                $("#MRPExperienciaIndispensableED").removeAttr("disabled");
                $("#MRPFuncionesPuestoED").removeAttr("readonly");
                $("#MRPConocimientosPuestoED").removeAttr("readonly");
                $('#MRPSueldoMensualInicialED').removeAttr("readonly");
                $('#MRPSueldoMensualPlantaED').removeAttr("readonly");
                $('#MRPSueldoMensualMasCostoED').removeAttr("readonly");
                $('#MRPBonoVariableED').removeAttr("readonly");
                $('#MRPCandidatoED').removeAttr("readonly");
                $('#MRPFechaIngresoED').removeAttr("readonly");
                $('#MRPFolioED').removeAttr("readonly");
                $('#MRPFechaRecepcionED').removeAttr("readonly");
                $("#btnEditarDetallesRequisicion").removeAttr("disabled");
                $("#btnEditarDetallesRequisicion").show();
                $("#btnMostrarSueldos").show();
                var BotonMostrarSueldos = document.querySelector("#btnAceptarRequisicion");
                BotonMostrarSueldos.removeAttribute("disabled");
                BotonMostrarSueldos.removeAttribute("style", "display:none");
            }
        },
        error: function (err) {
            console.log(err)
        }
    });
}

function ModificarDetalleRequisicion() {
    event.preventDefault();
    var x = $("#EditDetallesRequisicion").valid(); // ATENCIÓN AQUÍ, 
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
                var IdEditarDetalles = document.getElementById("IdED").value;
                var requisicion = {  // PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS
                    MRPNumeroVacantes: $("#MRPNumeroVacantesED").val(),
                    MRPSucursalId: $("#MRPSucursalIdED").val(),
                    MRPTipoVacante: $("#MRPTipoVacanteED").val(),
                    MRPRolarTurno: $("#MRPRolarTurnoED").val(),
                    MRPTiempoAlimentos: $("#MRPTiempoAlimentosED").val(),
                    MRPMotivoVacante: $("#MRPMotivoVacanteED").val(),
                    MRPMotivoDescripcion: $("#MRPMotivoDescripcionED").val(),
                    MRPSexoId: $("#MRPSexoIdED").val(),
                    MRPEdadMinima: $("#MRPEdadMinimaED").val(),
                    MRPEdadMaxima: $("#MRPEdadMaximaED").val(),
                    MRPEscolaridadId: $("#MRPEscolaridadIdED").val(),
                    MRPTituloIndispensable: $("#MRPTituloIndispensableED").val(),
                    MRPCedulaIndispensable: $("#MRPCedulaIndispensableED").val(),
                    MRPExperienciaIndispensable: $("#MRPExperienciaIndispensableED").val(),
                    MRPFuncionesPuesto: $("#MRPFuncionesPuestoED").val(),
                    MRPConocimientosPuesto: $("#MRPConocimientosPuestoED").val(),
                    MRPFechaElaboracion: $("#MRPFechaElaboracionED").val(),
                    MRPTurnoId: $("#MRPTurnoIdED").val(),
                    MRPPuestoId: $("#MRPPuestoIdED").val(),
                    MRPSueldoMensualInicial: $("#MRPSueldoMensualInicialED").val(),
                    MRPSueldoMensualPlanta: $("#MRPSueldoMensualPlantaED").val(),
                    MRPSueldoMensualMasCosto: $("#MRPSueldoMensualMasCostoED").val(),
                    MRPBonoVariable: $("#MRPBonoVariableED").val(),
                    MRPEsquemaContratacion: $("#MRPEsquemaContratacionED").val(),
                    MRPCandidato: $("#MRPCandidatoED").val(),
                    MRPFechaIngreso: $("#MRPFechaIngresoED").val(),
                    MRPFolio: $("#MRPFolioED").val(),
                }
                $.ajax({
                    type: "POST",
                    url: "/EditDetallesRequisicion",
                    data: { MRP: requisicion, Id: IdEditarDetalles }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("btnEditarDetallesRequisicion").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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


function GenerarPdf(Id) {
    event.preventDefault();
    Swal.fire({
        title: '¿Desea generar un archivo Pdf?',
        text: "El mismo archivo se guardará en C:\FormatosRequisicionPersonal",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: 'Cancelar',
        confirmButtonText: 'Aceptar'
    }).then((result) => {
        $.ajax({
            type: "GET",
            url: "/GenerarPdf/" + Id,
            success: function (response) {
                if (response) {
                    
                    var downloadLink;
                    var dataType = "application/pdf";
                    var filename = response;

                }
            },
            error: function (err) {
                console.log(err)
            }
        });      
        
    });
}

function OpenModalCancelacion(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/CancelarRequisicionById/" + Id,
        success: function (response) {
            $('#DetallesCancelacion').modal('show');
            $("#IdC").val(response.mrpId);
            $("#MRPPuestoIdC").val(response.puestoDescripcion);
            $("#MRPNumeroVacantesC").val(response.mrpNumeroVacantes);
            $("#MRPFechaElaboracionC").val(response.fe);
            $("#MRPTipoVacanteC").val(response.tipoVacante);
            $("#MRPTurnoIdC").val(response.turDescripcion);
            $("#MRPRolarTurnoC").val(response.turnoRolado);
            $("#MRPSucursalIdC").val(response.sucuNombre);
            $("#MRPTiempoAlimentosC").val(response.mrpTiempoAlimentos);
            $("#MRPMotivoVacanteC").val(response.motivoVacante);
            $("#MRPMotivoDescripcionC").val(response.mrpMotivoDescripcion);
            $("#MRPSexoIdC").val(response.sexDescripcion);
            $("#MRPEscolaridadIdC").val(response.escoDescripcion);
            $("#MRPEdadMinimaC").val(response.mrpEdadMinima);
            $("#MRPEdadMaximaC").val(response.mrpEdadMaxima);
            $("#MRPTituloIndispensableC").val(response.titulo);
            $("#MRPCedulaIndispensableC").val(response.cedula);
            $("#MRPExperienciaIndispensableC").val(response.experiencia);
            $("#MRPFuncionesPuestoC").val(response.mrpFuncionesPuesto);
            $("#MRPConocimientosPuestoC").val(response.mrpConocimientosPuesto);
            $("#MRPFolioC").val(response.mrpFolio);
            $("#MRPSueldoMensualInicialC").val(response.mrpSueldoMensualInicial);
            $("#MRPSueldoMensualPlantaC").val(response.mrpSueldoMensualPlanta);
            $("#MRPSueldoMensualMasCostoC").val(response.mrpSueldoMensualMasCosto);
            $("#MRPBonoVariableC").val(response.mrpBonoVariable);
            $("#MRPEsquemaContratacionC").val(response.mrpEsquemaContratacion);
            $("#MRPCandidatoC").val(response.mrpCandidato);
            $("#MRPFechaRecepcionC").val(response.fecharecepcioncorta);
            $("#MRPFechaIngresoC").val(response.ingreso);
            if (response.mrpEstatusId == 4) {
                $(".Cancelacion").show();
                $("#btnCancelarRequisicion").hide();
                var BotonCancelarRequisicion = document.querySelector("#btnCancelarRequisicion");
                BotonCancelarRequisicion.setAttribute("disabled", "disabled");
                BotonCancelarRequisicion.setAttribute("style", "display:none");
            }
            else {
                $(".Cancelacion").hide();
                $("#btnCancelarRequisicion").show();
                var BotonCancelarRequisicion = document.querySelector("#btnCancelarRequisicion");
                BotonCancelarRequisicion.removeAttribute("disabled");
                BotonCancelarRequisicion.removeAttribute("style", "display:none");
            }
        },
        error: function (err) {
            console.log(err)
        }
    });
}

function CancelarRequisicion() {
    var ValidacionCancelacion = document.getElementById("MotivoCancelacion").value;
    if (ValidacionCancelacion == "") {
        alert("Por favor ingresa el motivo por el cual se está cancelando la requisición");
    } else {
        event.preventDefault();
        var x = $("#DetailsCancelarRequisicion").valid(); // ATENCIÓN AQUÍ, 
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
                    var Id = document.getElementById("IdC").value;
                    var MotivoCancelacion = document.getElementById("MotivoCancelacion").value;
                    $.ajax({
                        type: "POST",
                        url: "/CancelarRequisicion",
                        data: { Id: Id, Motivo: MotivoCancelacion }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                        beforeSend: function () {
                            $("#btnCancelarRequisicion").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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
                                window.location.href = "/RequisicionPersonal";
                            });
                        }
                    });
                }
            });
        }
    }
}

$('#MRPDepaId').on('change', function () {
    $("#MRPAreaId").html("Seleccione una opción")
});
function SeleccionarDepartamento() {

    var Id = document.getElementById('MRPDepaId').value;
    $.ajax({
        type: "GET",
        url: "/AreaByDepa?Id=" + Id,
        success: function (data) {
            Areas = data.areasDep;
            Areas.forEach(function (Areas) {
                $("#MRPAreaId").append(
                    "<option value=" + Areas.areaId + ">" + Areas.areaDescripcion + "</option>"
                );
            })
        }
    });
}

$('#MRPAreaId').on('change', function () {
    $("#MRPPuestoId").html("Seleccione una opción")
});
function SeleccionarArea() {

    var Id = document.getElementById('MRPAreaId').value;
    $.ajax({
        type: "GET",
        url: "/PuestoByArea?Id=" + Id,
        success: function (data) {
            Puestos = data.puestos;
            Puestos.forEach(function (Puestos) {
                $("#MRPPuestoId").append(
                    "<option value=" + Puestos.puestoId + ">" + Puestos.puestoDescripcion + "</option>"
                );
            })
        }
    });
}

