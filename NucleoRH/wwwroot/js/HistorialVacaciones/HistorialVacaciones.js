$('#table3').DataTable({
    "searching": true,
    "lengthChange": false,
    "info": false,
});

//$('#table5').DataTable({
//    "searching": false,
//    "lengthChange": false,
//    "info": false,
//    'pagination': false,
//});

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


// ------------ FUNCION AGREGAR ----------------

function AddHistorialVacaciones() {
    event.preventDefault();
    var x = $("#AddHistorialVacaciones").valid(); // ATENCIÓN AQUÍ, 
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
                var historial = {  // PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS
                    
                    
                    HVVacacionesPendientesEjercicioActual: $("#HVVacacionesPendientesEjercicioActual").val(),
                    HVVacacionesPendientesEjerciciosPasados: $("#HVVacacionesPendientesEjerciciosPasados").val(),
                    HVReInciId: $("#HVReInciId").val(),
                    
                }
                var NumeroEmpleado = document.getElementById('HVEmpId').value;
                $.ajax({
                    type: "POST",
                    url: "/AddHistorialVacaciones",
                    data: { vaca: historial, IdEmpleado: NumeroEmpleado }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("#btnAddHistorialVacaciones").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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

function OpenModalDetails(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/HistorialById/" + Id,
        success: function (response) {
            $('#Details').modal('show');
            $('#Historial').modal('hide');
            $("#IdRegistroDetalle").val(response.hvId);
            $("#HVEmpIdD").val(response.nombreCompleto),
            $("#HVAntiguedadIdD").val(response.antiguedadCorrespondiente),
            $("#HVDiasSolicitadosD").val(response.hvDiasSolicitados),
            $("#HVEjercicioD").val(response.hvEjercicio),
            $("#HVFechaSolicitudD").val(response.fs),
            $("#HVFechaInicioD").val(response.fi),
            $("#HVFechaCulminacionD").val(response.fc),
            $("#HVFechaPresentacionD").val(response.fp),
            $("#HVVacacionesPendientesEjercicioActualD").val(response.hvVacacionesPendientesEjercicioActual),
            $("#HVVacacionesPendientesEjerciciosPasadosD").val(response.hvVacacionesPendientesEjerciciosPasados),
            $("#HVReInciIdD").val(response.hvReInciId),
            $("#HVPuestoIdD").val(response.puestoConversion),
            $("#HVSucursalIdD").val(response.sucu)

            // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
        },
        error: function (err) {
            console.log(err)
        }
    });
}

function VolverAHistorial() {

    let Id = document.getElementById('IdRegistroDetalle').value;
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/VolverAHistorialById/" + Id,
        success: function (response) {
            $('#Details').modal('hide');
            var IdEmpleado = response;
            OpenHistorialVacaciones(IdEmpleado);
            

            // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
        },
        error: function (err) {
            console.log(err)
        }
    });
}

var tablaHistorialEmpleado;

function OpenHistorialVacaciones(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/HistorialFiltradoById/" + Id,
        success: function (response) {
            tablaHistorialEmpleado = response;
            $('#Historial').modal('show');

            var $HistorialFiltrado = document.querySelector("#HistorialFiltrado");


            if ($HistorialFiltrado.childElementCount != 0) {
                while ($HistorialFiltrado.firstChild) {
                    $HistorialFiltrado.removeChild($HistorialFiltrado.firstChild);
                }
                /*$HistorialFiltrado.childElementCount = 0;*/
                $HistorialFiltrado = document.querySelector("#HistorialFiltrado");

            }
            if ($HistorialFiltrado.childElementCount == 0) {

                for (var i = 0; i < tablaHistorialEmpleado.length; i++) {

                    //Creamos el tr
                    const $tr = document.createElement("tr");
                    $tr.id = "Fila";

                    // creamos el td de Id y lo adjuntamos a tr
                    var $tdId = document.createElement("td"); // El textContent del td es el Id
                    $tdId.textContent = tablaHistorialEmpleado[i].hvId;
                    $tdId.id = "IdHV";
                    $tdId.setAttribute("name", "IdHV[]");
                    $tdId.setAttribute("style", "display:none");
                    $tdId.className = "IdHV";
                    $tr.appendChild($tdId);

                    // creamos el td de Id y lo adjuntamos a tr
                    var $tdEjercicio = document.createElement("td"); // El textContent del td es el Id
                    $tdEjercicio.textContent = tablaHistorialEmpleado[i].hvEjercicio;
                    $tdEjercicio.className = "HVEjercicio";
                    $tr.appendChild($tdEjercicio);


                    // creamos el td de Id y lo adjuntamos a tr
                    var $tdDiasSolicitados = document.createElement("td"); // El textContent del td es el Id
                    $tdDiasSolicitados.textContent = tablaHistorialEmpleado[i].hvDiasSolicitados;
                    $tdDiasSolicitados.className = "HVDiasSolicitados";
                    $tr.appendChild($tdDiasSolicitados);

                    // creamos el td de Id y lo adjuntamos a tr
                    var $tdFechaSolicitud = document.createElement("td"); // El textContent del td es el Id
                    var FechaRecortada = new Date(tablaHistorialEmpleado[i].hvFechaSolicitud);
                    var DiaRecortado = FechaRecortada.getDate();
                    if (DiaRecortado < 10) {
                        DiaRecortado = "0" + DiaRecortado;
                    }
                    var MesRecortado = FechaRecortada.getMonth();
                    MesRecortado = MesRecortado + 1;
                    if (MesRecortado < 10) {
                        MesRecortado = "0" + MesRecortado;
                    }
                    var FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                    $tdFechaSolicitud.textContent = FShort;
                    $tdFechaSolicitud.className = "HVFechaSolicitud";
                    $tr.appendChild($tdFechaSolicitud);

                    // creamos el td de Id y lo adjuntamos a tr
                    var $tdSaldo = document.createElement("td"); // El textContent del td es el Id
                    $tdSaldo.textContent = tablaHistorialEmpleado[i].hvSaldoVacaciones;
                    $tdSaldo.className = "HVSaldo";
                    $tr.appendChild($tdSaldo);



                    // tablaHistorialEmpleado[i].hvId;

                    // <a href="" data-toggle="tooltip" data-placement="top" title="Details" onclick="OpenHistorialVacaciones(@ins.EmpId)"><i class="material-icons text-info">search</i></a>
                    var Id = tablaHistorialEmpleado[i].hvId;
                    var $tdAcciones = document.createElement("td");
                    var $tdAHref = document.createElement("td"); // Función
                    $tdAHref.setAttribute("data-toggle", "tooltip");
                    $tdAHref.setAttribute("data-placement", "top");
                    $tdAHref.setAttribute("title", "Details");
                    $tdAHref.setAttribute("onclick", "OpenModalDetails" + "(" + tablaHistorialEmpleado[i].hvId + ")");
                    var $tdi = document.createElement("i"); // ícono
                    $tdi.setAttribute("class", "material-icons text-info");
                    $tdi.textContent = "search";
                    $tdi.setAttribute("value", tablaHistorialEmpleado[i].hvId);

                    $tdAHref.appendChild($tdi);
                    $tdAcciones.appendChild($tdAHref);
                    $tr.appendChild($tdAcciones);

                    $HistorialFiltrado.appendChild($tr);

                }


            } 


            // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
        },
        error: function (err) {
            console.log(err)
        }
    });
}

$(document).ready(function () {
    AsignarPermisosHistorial();
});

function AsignarPermisosHistorial() {

    $.ajax({
        type: "GET",
        url: "/PermisosHistorial/",
        success: function (data) {
            if (data.banderaPermisos == true) {
                alert("No tienes permiso para ver esta página. Redireccionando a Home");
                window.location.href = "/home";
            } else {
                // Validación para URPCREAR
                if (data.listaPermisos[0].urpCrear == true) {
                    $("#btnAgregarHistorial").show();

                } else {
                    $("#btnAgregarHistorial").hide();
                }

                // Validación para URPMOSTRAR
                if (data.listaPermisos[0].urpMostrar != true) {

                    window.location.href = "/home";

                }

            }

        }
    });
}