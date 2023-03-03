$(document).ready(function () {
    $("#SucursalDiv").hide();
    $("#SucursalDiv2").hide();
    $("#NumNomina").hide();
    $("#ReporteEmpId").hide();

    $("#TablaVacaciones").hide();
    $("#TablaComision").hide();
    $("#TablaPersonalSH").hide();
    $("#PermisoPersonalCH").hide();
    $("#Todos").hide();

    $('input[type="radio"]').click(function () {
        var inputValue = $(this).attr("value");
        var targetBox = $("." + inputValue);
        $(".box").not(targetBox).hide();
        $(targetBox).show();
    });

    AsignarPermisosReportes();
});

var global;
var BanderaParaControlador = "";
var BanderaVacaciones = false;
var BanderaComision = false;
var BanderaPersonalSH = false;
var BanderaPersonalCH = false;
var BanderaTodos = false;
var FechaInicialGlobal;
var FechaFinalGlobal;

function OpenModalVistaPrevia(IdSucursal, NoNomina, FechaInicio, FechaFinal, IdPermiso, TodasSucursales, TodosPermisos) {
    IdSucursal = document.getElementById('ReporteSucursalId').value;
    FechaInicio = document.getElementById('ReporteFechaDesde').value;
    FechaFinal = document.getElementById('ReporteFechaHasta').value;
    FechaInicialGlobal = FechaInicio;
    FechaFinalGlobal = FechaFinal;
    IdPermiso = document.getElementById('ReporteIncidenciaId').value;
    TodasSucursales = document.getElementById('ReporteCheckBoxAllSucursales').checked;
    TodosPermisos = document.getElementById('ReporteCheckBoxAllPermisos').checked;
    if (IdSucursal == "") {
        IdSucursal = 0;
    }
    NoNomina = document.getElementById('ReporteEmpId').value;
    if (NoNomina == "") {
        NoNomina = 0;
    }
    if (IdPermiso == "" && TodosPermisos != false) {
        IdPermiso = 0;
    }

    if (IdSucursal == 0 && NoNomina == 0 && TodasSucursales == false) {
        alert("Seleccione una sucursal o un empleado para continuar")
    } else {
        
        event.preventDefault();
        $.ajax({
            type: "GET",
            url: "/ReporteVistaPrevia/" + IdSucursal + "/" + NoNomina + "/" + FechaInicio + "/" + FechaFinal + "/" + IdPermiso + "/" + TodasSucursales + "/" + TodosPermisos,
            success: function (response) {
                 // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
                const separador = "-";
                var FechaInicioSeparada = FechaInicio.split(separador);
                var FechaFinalSeparada = FechaFinal.split(separador);
                var NFechaInicio = FechaInicioSeparada[2] + separador + FechaInicioSeparada[1] + separador + FechaInicioSeparada[0];
                var NFechaFinal = FechaFinalSeparada[2] + separador + FechaFinalSeparada[1] + separador + FechaFinalSeparada[0];
                var DatosDeReporte = response;

                document.getElementById('FechaDesde').innerHTML = "Desde: " +  NFechaInicio;
                document.getElementById('FechaHasta').innerHTML = "Hasta: " + NFechaFinal;
                document.getElementById('TReportes').innerHTML = "REPORTE DE " + DatosDeReporte.descripcionPermiso;
                var fecha = new Date(); //Fecha actual
                var mes = fecha.getMonth() + 1; //obteniendo mes
                var dia = fecha.getDate(); //obteniendo dia
                var ano = fecha.getFullYear(); //obteniendo año
                if (dia < 10)
                    dia = '0' + dia; //agrega cero si el menor de 10
                if (mes < 10)
                    mes = '0' + mes //agrega cero si el menor de 10

                
                document.getElementById('FechaDeHoy').innerHTML = "Fecha de hoy: " + dia + "-" + mes + "-" + ano;

                $('#btnExportarExcel').removeAttr("disabled");
                $('#btnExportarPDF').removeAttr("disabled");
                
                global = response.incidenciasBySucursal;
                var $TablaVistaPrevia;

                if (DatosDeReporte.descripcionPermiso == "VACACIONES") {
                    $TablaVistaPrevia = document.querySelector("#table3");
                } else if (DatosDeReporte.descripcionPermiso == "CAMBIOS DE HORARIO") {
                    $TablaVistaPrevia = document.querySelector("#table7");
                } else if (DatosDeReporte.descripcionPermiso == "PERMISOS CON GOCE" || DatosDeReporte.descripcionPermiso == "PERMISOS SIN GOCE") {
                    $TablaVistaPrevia = document.querySelector("#table6");
                } else if (DatosDeReporte.descripcionPermiso == "PERMISO EN COMISIÓN") {
                    $TablaVistaPrevia = document.querySelector("#table5");
                } else if (DatosDeReporte.descripcionPermiso == "Todos") {
                    $TablaVistaPrevia = document.querySelector("#table8");
                } else if (DatosDeReporte.descripcionPermiso == "ENTRADAS/SALIDAS") {
                    $TablaVistaPrevia = document.querySelector("#table9");
                }
                
                var $TablaDatos = document.querySelector("#table4");
                if ($TablaVistaPrevia.childElementCount != 0) {
                    while ($TablaVistaPrevia.firstChild) {
                        $TablaVistaPrevia.removeChild($TablaVistaPrevia.firstChild);
                    }
                    $TablaVistaPrevia = document.querySelector("#table3");
                }

                if ($TablaDatos.childElementCount != 0) {
                    while ($TablaDatos.firstChild) {
                        $TablaDatos.removeChild($TablaDatos.firstChild);
                    }
                    $TablaDatos = document.querySelector("#table4");
                }
                if ($TablaVistaPrevia.childElementCount == 0) {

                        if (DatosDeReporte.banderaFiltroPorPermiso != false) { // Filtrado por permiso
                            if (DatosDeReporte.incidenciasBySucursal[0].reInciInciId == 4) {// Vacaciones
                                // creamos el THead para la paginada
                                const $THead = document.createElement("thead");
                                const $TrHead = document.createElement("tr");
                                const $ThFolio = document.createElement("th");
                                const $ThNomina = document.createElement("th");
                                const $ThNombre = document.createElement("th");
                                const $ThSucursal = document.createElement("th");
                                const $ThFechaPermiso = document.createElement("th");
                                const $ThEstatusPermiso = document.createElement("th");
                                const $ThTipoPermiso = document.createElement("th"); // Hasta aquí son apartados generales
                                const $ThFechaInicio = document.createElement("th"); // Empiezan los apartados de cada tipo de permiso
                                const $ThFechaFinal = document.createElement("th");
                                const $ThFechaPresentacion = document.createElement("th");
                                const $ThDiasAusencia = document.createElement("th");
                                const $ThPersonaCubrira = document.createElement("th");

                                $ThFolio.textContent = "Folio del permiso";
                                $ThFolio.setAttribute("scope", "col");
                                $ThFolio.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThFolio);
                                $THead.appendChild($TrHead);

                                $ThNomina.textContent = "Número de nómina";
                                $ThNomina.setAttribute("scope", "col");
                                $ThNomina.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThNomina);
                                $THead.appendChild($TrHead);

                                $ThNombre.textContent = "Nombre del empleado";
                                $ThNombre.setAttribute("scope", "col");
                                $ThNombre.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThNombre);
                                $THead.appendChild($TrHead);

                                $ThSucursal.textContent = "Nombre de Sucursal";
                                $ThSucursal.setAttribute("scope", "col");
                                $ThSucursal.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThSucursal);
                                $THead.appendChild($TrHead);

                                $ThFechaPermiso.textContent = "Fecha del permiso";
                                $ThFechaPermiso.setAttribute("scope", "col");
                                $ThFechaPermiso.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThFechaPermiso);
                                $THead.appendChild($TrHead);

                                $ThEstatusPermiso.textContent = "Estatus del permiso";
                                $ThEstatusPermiso.setAttribute("scope", "col");
                                $ThEstatusPermiso.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThEstatusPermiso);
                                $THead.appendChild($TrHead);

                                $ThTipoPermiso.textContent = "Tipo de permiso";
                                $ThTipoPermiso.setAttribute("scope", "col");
                                $ThTipoPermiso.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThTipoPermiso);
                                $THead.appendChild($TrHead);

                                $ThFechaInicio.textContent = "Fecha de inicio de vacaciones";
                                $ThFechaInicio.setAttribute("scope", "col");
                                $ThFechaInicio.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThFechaInicio);
                                $THead.appendChild($TrHead);

                                $ThFechaFinal.textContent = "Fecha culminación de vacaciones";
                                $ThFechaFinal.setAttribute("scope", "col");
                                $ThFechaFinal.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThFechaFinal);
                                $THead.appendChild($TrHead);

                                $ThFechaPresentacion.textContent = "Fecha de presentación";
                                $ThFechaPresentacion.setAttribute("scope", "col");
                                $ThFechaPresentacion.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThFechaPresentacion);
                                $THead.appendChild($TrHead);
                                
                                $ThDiasAusencia.textContent = "Días de ausencia";
                                $ThDiasAusencia.setAttribute("scope", "col");
                                $ThDiasAusencia.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThDiasAusencia);
                                $THead.appendChild($TrHead);
                                

                                $ThPersonaCubrira.textContent = "Persona que cubrirá";
                                $ThPersonaCubrira.setAttribute("scope", "col");
                                $ThPersonaCubrira.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThPersonaCubrira);
                                $THead.appendChild($TrHead);

                                $TablaVistaPrevia.appendChild($THead);

                                const $TBody = document.createElement("tbody");

                                // creamos el THead para los datos
                                const $THeadD = document.createElement("thead");
                                const $TrHeadD = document.createElement("tr");
                                const $ThFolioD = document.createElement("th");
                                const $ThNominaD = document.createElement("th");
                                const $ThNombreD = document.createElement("th");
                                const $ThSucursalD = document.createElement("th");
                                const $ThFechaPermisoD = document.createElement("th");
                                const $ThEstatusPermisoD = document.createElement("th");
                                const $ThTipoPermisoD = document.createElement("th"); // Hasta aquí son apartados generales
                                const $ThFechaInicioD = document.createElement("th"); // Empiezan los apartados de cada tipo de permiso
                                const $ThFechaFinalD = document.createElement("th");
                                const $ThFechaPresentacionD = document.createElement("th");
                                const $ThDiasAusenciaD = document.createElement("th");
                                const $ThPersonaCubriraD = document.createElement("th");

                                $ThFolioD.textContent = "Folio del permiso";
                                $ThFolioD.setAttribute("scope", "col");
                                $ThFolioD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThFolioD);
                                $THeadD.appendChild($TrHeadD);

                                $ThNominaD.textContent = "Número de nómina";
                                $ThNominaD.setAttribute("scope", "col");
                                $ThNominaD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThNominaD);
                                $THeadD.appendChild($TrHeadD);

                                $ThNombreD.textContent = "Nombre del empleado";
                                $ThNombreD.setAttribute("scope", "col");
                                $ThNombreD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThNombreD);
                                $THeadD.appendChild($TrHeadD);

                                $ThSucursalD.textContent = "Nombre de Sucursal";
                                $ThSucursalD.setAttribute("scope", "col");
                                $ThSucursalD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThSucursalD);
                                $THeadD.appendChild($TrHeadD);

                                $ThFechaPermisoD.textContent = "Fecha del permiso";
                                $ThFechaPermisoD.setAttribute("scope", "col");
                                $ThFechaPermisoD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThFechaPermisoD);
                                $THeadD.appendChild($TrHeadD);

                                $ThEstatusPermisoD.textContent = "Estatus del permiso";
                                $ThEstatusPermisoD.setAttribute("scope", "col");
                                $ThEstatusPermisoD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThEstatusPermisoD);
                                $THeadD.appendChild($TrHeadD);

                                $ThTipoPermisoD.textContent = "Tipo de permiso";
                                $ThTipoPermisoD.setAttribute("scope", "col");
                                $ThTipoPermisoD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThTipoPermisoD);
                                $THeadD.appendChild($TrHeadD);

                                $ThFechaInicioD.textContent = "Fecha de inicio de vacaciones";
                                $ThFechaInicioD.setAttribute("scope", "col");
                                $ThFechaInicioD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThFechaInicioD);
                                $THeadD.appendChild($TrHeadD);

                                $ThFechaFinalD.textContent = "Fecha culminación de vacaciones";
                                $ThFechaFinalD.setAttribute("scope", "col");
                                $ThFechaFinalD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThFechaFinalD);
                                $THeadD.appendChild($TrHeadD);


                                $ThFechaPresentacionD.textContent = "Fecha de presentación";
                                $ThFechaPresentacionD.setAttribute("scope", "col");
                                $ThFechaPresentacionD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThFechaPresentacionD);
                                $THeadD.appendChild($TrHeadD);

                                $ThDiasAusenciaD.textContent = "Días de ausencia";
                                $ThDiasAusenciaD.setAttribute("scope", "col");
                                $ThDiasAusenciaD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThDiasAusenciaD);
                                $THeadD.appendChild($TrHeadD);


                                $ThPersonaCubriraD.textContent = "Persona que cubrirá";
                                $ThPersonaCubriraD.setAttribute("scope", "col");
                                $ThPersonaCubriraD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThPersonaCubriraD);
                                $THeadD.appendChild($TrHeadD);

                                $TablaDatos.appendChild($THeadD);

                                const $TBodyD = document.createElement("tbody");

                                /* ---------------- Aquí termina la creación del Header de la tabla. ---------------- */

                                for (var i = 0; i < DatosDeReporte.incidenciasBySucursal.length; i++) {
                                    // Creamos el Tr

                                    const $tr = document.createElement("tr");
                                    const $trD = document.createElement("tr");
                                    
                                    // Empezamos a crear los td generales. PAGINACIÓN

                                    const $tdFolio = document.createElement("td");
                                    $tdFolio.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciId;
                                    $tdFolio.id = "ReInciId";
                                    $tdFolio.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdFolio);

                                    const $tdNomina = document.createElement("td");
                                    $tdNomina.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNumero;
                                    $tdNomina.id = "ReInciEmpNomina";
                                    $tdNomina.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdNomina);

                                    const $tdNombre = document.createElement("td");
                                    $tdNombre.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNombreCompleto;
                                    $tdNombre.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdNombre);

                                    const $tdSucursal = document.createElement("td");
                                    $tdSucursal.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciSucuNombre;
                                    $tdSucursal.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdSucursal);

                                    const $tdFechaPermiso = document.createElement("td");
                                    var FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].fecha);
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
                                    $tdFechaPermiso.textContent = FShort;
                                    $tdFechaPermiso.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdFechaPermiso);

                                    const $tdEstatus = document.createElement("td");
                                    $tdEstatus.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEstatusDescripcion;
                                    $tdEstatus.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdEstatus);

                                    const $tdTipoPermiso = document.createElement("td");
                                    $tdTipoPermiso.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciInciDescripcion;
                                    $tdTipoPermiso.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdTipoPermiso);

                                    // Aquí terminan los encabezados generales

                                    // Empezamos a crear los particulares de cada permiso

                                    const $tdFechaInicio = document.createElement("td");
                                     FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciFechaInicio);
                                     DiaRecortado = FechaRecortada.getDate();
                                    if (DiaRecortado < 10) {
                                        DiaRecortado = "0" + DiaRecortado;
                                    }
                                     MesRecortado = FechaRecortada.getMonth();
                                    MesRecortado = MesRecortado + 1;
                                    if (MesRecortado < 10) {
                                        MesRecortado = "0" + MesRecortado;
                                    }
                                     FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                    $tdFechaInicio.textContent = FShort;
                                    $tdFechaInicio.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdFechaInicio);

                                    const $tdFechaFinal = document.createElement("td");
                                    FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciFechaFinal);
                                    DiaRecortado = FechaRecortada.getDate();
                                    if (DiaRecortado < 10) {
                                        DiaRecortado = "0" + DiaRecortado;
                                    }
                                    MesRecortado = FechaRecortada.getMonth();
                                    MesRecortado = MesRecortado + 1;
                                    if (MesRecortado < 10) {
                                        MesRecortado = "0" + MesRecortado;
                                    }
                                    FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                    $tdFechaFinal.textContent = FShort;
                                    $tdFechaFinal.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdFechaFinal);

                                    const $tdFechaPresentacion = document.createElement("td");
                                    FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciFechaPresentacion);
                                    DiaRecortado = FechaRecortada.getDate();
                                    if (DiaRecortado < 10) {
                                        DiaRecortado = "0" + DiaRecortado;
                                    }
                                    MesRecortado = FechaRecortada.getMonth();
                                    MesRecortado = MesRecortado + 1;
                                    if (MesRecortado < 10) {
                                        MesRecortado = "0" + MesRecortado;
                                    }
                                    FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                    $tdFechaPresentacion.textContent = FShort;
                                    $tdFechaPresentacion.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdFechaPresentacion);

                                    const $tdDiasAusencia = document.createElement("td");
                                    $tdDiasAusencia.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciDiasAusencia;
                                    $tdDiasAusencia.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdDiasAusencia);

                                    const $tdPersonaCubrira = document.createElement("td");
                                    $tdPersonaCubrira.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciPersonaCubrira;
                                    $tdPersonaCubrira.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdPersonaCubrira);

                                    $TBody.appendChild($tr);
                                    $TablaVistaPrevia.appendChild($TBody);

                                    // Empezamos a crear los td generales. DATOS ----------------------------------------------------------------------------------

                                    const $tdFolioD = document.createElement("td");
                                    $tdFolioD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciId;
                                    $tdFolioD.id = "ReInciId";
                                    $tdFolioD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdFolioD);

                                    const $tdNominaD = document.createElement("td");
                                    $tdNominaD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNumero;
                                    $tdNominaD.id = "ReInciEmpNomina";
                                    $tdNominaD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdNominaD);

                                    const $tdNombreD = document.createElement("td");
                                    $tdNombreD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNombreCompleto;
                                    $tdNombreD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdNombreD);

                                    const $tdSucursalD = document.createElement("td");
                                    $tdSucursalD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciSucuNombre;
                                    $tdSucursalD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdSucursalD);

                                    const $tdFechaPermisoD = document.createElement("td");
                                    FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].fecha);
                                    DiaRecortado = FechaRecortada.getDate();
                                    if (DiaRecortado < 10) {
                                        DiaRecortado = "0" + DiaRecortado;
                                    }
                                     MesRecortado = FechaRecortada.getMonth();
                                    MesRecortado = MesRecortado + 1;
                                    if (MesRecortado < 10) {
                                        MesRecortado = "0" + MesRecortado;
                                    }
                                     FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                    $tdFechaPermisoD.textContent = FShort;
                                    $tdFechaPermisoD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdFechaPermisoD);

                                    const $tdEstatusD = document.createElement("td");
                                    $tdEstatusD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEstatusDescripcion;
                                    $tdEstatusD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdEstatusD);

                                    const $tdTipoPermisoD = document.createElement("td");
                                    $tdTipoPermisoD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciInciDescripcion;
                                    $tdTipoPermisoD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdTipoPermisoD);

                                    // Aquí terminan los encabezados generales

                                    // Empezamos a crear los particulares de cada permiso

                                    const $tdFechaInicioD = document.createElement("td");
                                    FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciFechaInicio);
                                    DiaRecortado = FechaRecortada.getDate();
                                    if (DiaRecortado < 10) {
                                        DiaRecortado = "0" + DiaRecortado;
                                    }
                                    MesRecortado = FechaRecortada.getMonth();
                                    MesRecortado = MesRecortado + 1;
                                    if (MesRecortado < 10) {
                                        MesRecortado = "0" + MesRecortado;
                                    }
                                    FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                    $tdFechaInicioD.textContent = FShort;
                                    $tdFechaInicioD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdFechaInicioD);

                                    const $tdFechaFinalD = document.createElement("td");
                                    FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciFechaFinal);
                                    DiaRecortado = FechaRecortada.getDate();
                                    if (DiaRecortado < 10) {
                                        DiaRecortado = "0" + DiaRecortado;
                                    }
                                    MesRecortado = FechaRecortada.getMonth();
                                    MesRecortado = MesRecortado + 1;
                                    if (MesRecortado < 10) {
                                        MesRecortado = "0" + MesRecortado;
                                    }
                                    FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                    $tdFechaFinalD.textContent = FShort;
                                    $tdFechaFinalD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdFechaFinalD);

                                    const $tdFechaPresentacionD = document.createElement("td");
                                    FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciFechaPresentacion);
                                    DiaRecortado = FechaRecortada.getDate();
                                    if (DiaRecortado < 10) {
                                        DiaRecortado = "0" + DiaRecortado;
                                    }
                                    MesRecortado = FechaRecortada.getMonth();
                                    MesRecortado = MesRecortado + 1;
                                    if (MesRecortado < 10) {
                                        MesRecortado = "0" + MesRecortado;
                                    }
                                    FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                    $tdFechaPresentacionD.textContent = FShort;
                                    $tdFechaPresentacionD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdFechaPresentacionD);

                                    const $tdDiasAusenciaD = document.createElement("td");
                                    $tdDiasAusenciaD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciDiasAusencia;
                                    $tdDiasAusenciaD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdDiasAusenciaD);

                                    const $tdPersonaCubriraD = document.createElement("td");
                                    $tdPersonaCubriraD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciPersonaCubrira;
                                    $tdPersonaCubriraD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdPersonaCubriraD);

                                    $TBodyD.appendChild($trD);
                                    $TablaDatos.appendChild($TBodyD);

                                }
                                if (BanderaVacaciones == true) {
                                    $('#table3').DataTable().clear();
                                $('#table3').DataTable().destroy();
                                }

                                /*if (BanderaParaDataTable != true) {*/
                                    $('#table3').DataTable({
                                        "destroy": true,
                                        "searching": true,
                                        "lengthChange": false,
                                        "info": false,
                                        dom: 'Bfrtip',
                                        buttons: [
                                            {
                                                extend: 'pdfHtml5',
                                                orientation: 'landscape',
                                                pageSize: 'LEGAL'
                                            }
                                        ]


                                    });
                               /* }*/

                                BanderaVacaciones = true;
                                BanderaParaControlador = "Vacaciones";

                                $("#TablaVacaciones").show();
                                $("#TablaComision").hide();
                                $("#TablaPersonalSH").hide();
                                $("#PermisoPersonalCH").hide();
                                $("#Todos").hide();

                            } else if (DatosDeReporte.incidenciasBySucursal[0].reInciInciId == 11) { // Permisos en comisión

                                // Encabezados para la tabla paginada
                                const $THead = document.createElement("thead");
                                const $TrHead = document.createElement("tr");
                                const $ThFolio = document.createElement("th");
                                const $ThNomina = document.createElement("th");
                                const $ThNombre = document.createElement("th");
                                const $ThSucursal = document.createElement("th");
                                const $ThFechaPermiso = document.createElement("th");
                                const $ThEstatusPermiso = document.createElement("th");
                                const $ThTipoPermiso = document.createElement("th"); // Hasta aquí son apartados generales
                                const $ThAsunto = document.createElement("th"); // Empiezan los apartados de cada tipo de permiso
                                const $ThDestino = document.createElement("th");
                                const $ThTelDestino = document.createElement("th");
                                const $ThContacto1 = document.createElement("th");
                                const $ThNombreDestino = document.createElement("th");
                                const $ThContacto2 = document.createElement("th");
                                const $ThObservaciones = document.createElement("th");
                                const $ThHoraSalida = document.createElement("th");
                                const $ThHoraRegreso = document.createElement("th");

                                $ThFolio.textContent = "Folio del permiso";
                                $ThFolio.setAttribute("scope", "col");
                                $ThFolio.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThFolio);
                                $THead.appendChild($TrHead);

                                $ThNomina.textContent = "Número de nómina";
                                $ThNomina.setAttribute("scope", "col");
                                $ThNomina.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThNomina);
                                $THead.appendChild($TrHead);

                                $ThNombre.textContent = "Nombre del empleado";
                                $ThNombre.setAttribute("scope", "col");
                                $ThNombre.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThNombre);
                                $THead.appendChild($TrHead);

                                $ThSucursal.textContent = "Nombre de Sucursal";
                                $ThSucursal.setAttribute("scope", "col");
                                $ThSucursal.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThSucursal);
                                $THead.appendChild($TrHead);

                                $ThFechaPermiso.textContent = "Fecha del permiso";
                                $ThFechaPermiso.setAttribute("scope", "col");
                                $ThFechaPermiso.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThFechaPermiso);
                                $THead.appendChild($TrHead);

                                $ThEstatusPermiso.textContent = "Estatus del permiso";
                                $ThEstatusPermiso.setAttribute("scope", "col");
                                $ThEstatusPermiso.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThEstatusPermiso);
                                $THead.appendChild($TrHead);

                                $ThTipoPermiso.textContent = "Tipo de permiso";
                                $ThTipoPermiso.setAttribute("scope", "col");
                                $ThTipoPermiso.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThTipoPermiso);
                                $THead.appendChild($TrHead);

                                $ThAsunto.textContent = "Asunto";
                                $ThAsunto.setAttribute("scope", "col");
                                $ThAsunto.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThAsunto);
                                $THead.appendChild($TrHead);

                                $ThDestino.textContent = "Destino";
                                $ThDestino.setAttribute("scope", "col");
                                $ThDestino.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThDestino);
                                $THead.appendChild($TrHead);

                                $ThTelDestino.textContent = "Destino";
                                $ThTelDestino.setAttribute("scope", "col");
                                $ThTelDestino.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThTelDestino);
                                $THead.appendChild($TrHead);
                                

                                $ThContacto1.textContent = "Contacto 1";
                                $ThContacto1.setAttribute("scope", "col");
                                $ThContacto1.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThContacto1);
                                $THead.appendChild($TrHead);
                                

                                $ThNombreDestino.textContent = "Nombre del destino";
                                $ThNombreDestino.setAttribute("scope", "col");
                                $ThNombreDestino.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThNombreDestino);
                                $THead.appendChild($TrHead);
                                

                                $ThContacto2.textContent = "Contacto 2";
                                $ThContacto2.setAttribute("scope", "col");
                                $ThContacto2.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThContacto2);
                                $THead.appendChild($TrHead);

                                $ThObservaciones.textContent = "Observaciones";
                                $ThObservaciones.setAttribute("scope", "col");
                                $ThObservaciones.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThObservaciones);
                                $THead.appendChild($TrHead);

                                $ThHoraSalida.textContent = "Hora de salida";
                                $ThHoraSalida.setAttribute("scope", "col");
                                $ThHoraSalida.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThHoraSalida);
                                $THead.appendChild($TrHead);

                                $ThHoraRegreso.textContent = "Hora de regreso";
                                $ThHoraRegreso.setAttribute("scope", "col");
                                $ThHoraRegreso.setAttribute("nowrap", "nowrap");
                                $TrHead.appendChild($ThHoraRegreso);
                                $THead.appendChild($TrHead);

                                $TablaVistaPrevia.appendChild($THead);

                                const $TBody = document.createElement("tbody");

                                // Encabezados para la tabla de datos
                                const $THeadD = document.createElement("thead");
                                const $TrHeadD = document.createElement("tr");
                                const $ThFolioD = document.createElement("th");
                                const $ThNominaD = document.createElement("th");
                                const $ThNombreD = document.createElement("th");
                                const $ThSucursalD = document.createElement("th");
                                const $ThFechaPermisoD = document.createElement("th");
                                const $ThEstatusPermisoD = document.createElement("th");
                                const $ThTipoPermisoD = document.createElement("th"); // Hasta aquí son apartados generales
                                const $ThAsuntoD = document.createElement("th"); // Empiezan los apartados de cada tipo de permiso
                                const $ThDestinoD = document.createElement("th");
                                const $ThTelDestinoD = document.createElement("th");
                                const $ThContacto1D = document.createElement("th");
                                const $ThNombreDestinoD = document.createElement("th");
                                const $ThContacto2D = document.createElement("th");
                                const $ThObservacionesD = document.createElement("th");
                                const $ThHoraSalidaD = document.createElement("th");
                                const $ThHoraRegresoD = document.createElement("th");

                                $ThFolioD.textContent = "Folio del permiso";
                                $ThFolioD.setAttribute("scope", "col");
                                $ThFolioD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThFolioD);
                                $THeadD.appendChild($TrHeadD);

                                $ThNominaD.textContent = "Número de nómina";
                                $ThNominaD.setAttribute("scope", "col");
                                $ThNominaD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThNominaD);
                                $THeadD.appendChild($TrHeadD);

                                $ThNombreD.textContent = "Nombre del empleado";
                                $ThNombreD.setAttribute("scope", "col");
                                $ThNombreD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThNombreD);
                                $THeadD.appendChild($TrHeadD);

                                $ThSucursalD.textContent = "Nombre de Sucursal";
                                $ThSucursalD.setAttribute("scope", "col");
                                $ThSucursalD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThSucursalD);
                                $THeadD.appendChild($TrHeadD);

                                $ThFechaPermisoD.textContent = "Fecha del permiso";
                                $ThFechaPermisoD.setAttribute("scope", "col");
                                $ThFechaPermisoD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThFechaPermisoD);
                                $THeadD.appendChild($TrHeadD);

                                $ThEstatusPermisoD.textContent = "Estatus del permiso";
                                $ThEstatusPermisoD.setAttribute("scope", "col");
                                $ThEstatusPermisoD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThEstatusPermisoD);
                                $THeadD.appendChild($TrHeadD);

                                $ThTipoPermisoD.textContent = "Tipo de permiso";
                                $ThTipoPermisoD.setAttribute("scope", "col");
                                $ThTipoPermisoD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThTipoPermisoD);
                                $THeadD.appendChild($TrHeadD);

                                $ThAsuntoD.textContent = "Asunto";
                                $ThAsuntoD.setAttribute("scope", "col");
                                $ThAsuntoD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThAsuntoD);
                                $THeadD.appendChild($TrHeadD);

                                $ThDestinoD.textContent = "Destino";
                                $ThDestinoD.setAttribute("scope", "col");
                                $ThDestinoD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThDestinoD);
                                $THeadD.appendChild($TrHeadD);

                                $ThTelDestinoD.textContent = "Destino";
                                $ThTelDestinoD.setAttribute("scope", "col");
                                $ThTelDestinoD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThTelDestinoD);
                                $THeadD.appendChild($TrHeadD);

                                $ThContacto1D.textContent = "Contacto 1";
                                $ThContacto1D.setAttribute("scope", "col");
                                $ThContacto1D.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThContacto1D);
                                $THeadD.appendChild($TrHeadD);

                                $ThNombreDestinoD.textContent = "Nombre del destino";
                                $ThNombreDestinoD.setAttribute("scope", "col");
                                $ThNombreDestinoD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThNombreDestinoD);
                                $THeadD.appendChild($TrHeadD);

                                $ThContacto2D.textContent = "Contacto 2";
                                $ThContacto2D.setAttribute("scope", "col");
                                $ThContacto2D.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThContacto2D);
                                $THeadD.appendChild($TrHeadD);

                                $ThObservacionesD.textContent = "Observaciones";
                                $ThObservacionesD.setAttribute("scope", "col");
                                $ThObservacionesD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThObservacionesD);
                                $THeadD.appendChild($TrHeadD);

                                $ThHoraSalidaD.textContent = "Hora de salida";
                                $ThHoraSalidaD.setAttribute("scope", "col");
                                $ThHoraSalidaD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThHoraSalidaD);
                                $THeadD.appendChild($TrHeadD);

                                $ThHoraRegresoD.textContent = "Hora de regreso";
                                $ThHoraRegresoD.setAttribute("scope", "col");
                                $ThHoraRegresoD.setAttribute("nowrap", "nowrap");
                                $TrHeadD.appendChild($ThHoraRegresoD);
                                $THeadD.appendChild($TrHeadD);

                                $TablaDatos.appendChild($THeadD);

                                const $TBodyD = document.createElement("tbody");

                                /* ---------------- Aquí termina la creación del Header de la tabla. ---------------- */

                                for (var i = 0; i < DatosDeReporte.incidenciasBySucursal.length; i++) {
                                    // Creamos el Tr

                                    const $tr = document.createElement("tr");
                                    const $trD = document.createElement("tr");
                                    
                                    // Empezamos a crear los td generales. PAGINACION ----------------------------------------------------------------------------

                                    const $tdFolio = document.createElement("td");
                                    $tdFolio.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciId;
                                    $tdFolio.id = "ReInciId";
                                    $tdFolio.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdFolio);

                                    const $tdNomina = document.createElement("td");
                                    $tdNomina.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNumero;
                                    $tdNomina.id = "ReInciEmpNomina";
                                    $tdNomina.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdNomina);

                                    const $tdNombre = document.createElement("td");
                                    $tdNombre.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNombreCompleto;
                                    $tdNombre.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdNombre);

                                    const $tdSucursal = document.createElement("td");
                                    $tdSucursal.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciSucuNombre;
                                    $tdSucursal.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdSucursal);

                                    const $tdFechaPermiso = document.createElement("td");
                                    var FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].fecha);
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
                                    $tdFechaPermiso.textContent = FShort;
                                    $tdFechaPermiso.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdFechaPermiso);

                                    const $tdEstatus = document.createElement("td");
                                    $tdEstatus.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEstatusDescripcion;
                                    $tdEstatus.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdEstatus);

                                    const $tdTipoPermiso = document.createElement("td");
                                    $tdTipoPermiso.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciInciDescripcion;
                                    $tdTipoPermiso.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdTipoPermiso);

                                    // --------------- Creamos los apartados particulares de permiso en comisión -----------------

                                    const $tdAsunto = document.createElement("td");
                                    $tdAsunto.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciAsunto;
                                    $tdAsunto.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdAsunto);

                                    const $tdDestino = document.createElement("td");
                                    $tdDestino.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciDestino;
                                    $tdDestino.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdDestino);

                                    const $tdTelDestino = document.createElement("td");
                                    $tdTelDestino.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciTelDestino;
                                    $tdTelDestino.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdTelDestino);

                                    const $tdContacto1 = document.createElement("td");
                                    $tdContacto1.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciContacto1;
                                    $tdContacto1.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdContacto1);

                                    const $tdNombreDestino = document.createElement("td");
                                    $tdNombreDestino.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciNombreDestino;
                                    $tdNombreDestino.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdNombreDestino);

                                    const $tdContacto2 = document.createElement("td");
                                    $tdContacto2.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciContacto2;
                                    $tdContacto2.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdContacto2);

                                    const $tdObservaciones = document.createElement("td");
                                    $tdObservaciones.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciObservaciones;
                                    $tdObservaciones.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdObservaciones);

                                    const $tdHoraSalida = document.createElement("td");
                                    var HoraObjeto = DatosDeReporte.incidenciasBySucursal[i].detInciHoraSalida;
                                    var Hora = HoraObjeto.hours;
                                    if (Hora < 10) {
                                        Hora = "0" + Hora;
                                    }
                                    var Minutos = HoraObjeto.minutes;
                                    if (Minutos < 10) {
                                        Minutos = "0" + Minutos;
                                    }
                                    var Segundos = HoraObjeto.seconds;
                                    if (Segundos < 10) {
                                        Segundos = "0" + Segundos;
                                    }
                                    var Formato = Hora + ":" + Minutos + ":" + Segundos;

                                    $tdHoraSalida.textContent = Formato;
                                    $tdHoraSalida.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdHoraSalida);

                                    const $tdHoraRegreso = document.createElement("td");
                                    HoraObjeto = DatosDeReporte.incidenciasBySucursal[i].detInciHoraRegreso;
                                    Hora = HoraObjeto.hours;
                                    if (Hora < 10) {
                                        Hora = "0" + Hora;
                                    }
                                    Minutos = HoraObjeto.minutes;
                                    if (Minutos < 10) {
                                        Minutos = "0" + Minutos;
                                    }
                                    Segundos = HoraObjeto.seconds;
                                    if (Segundos < 10) {
                                        Segundos = "0" + Segundos;
                                    }
                                    Formato = Hora + ":" + Minutos + ":" + Segundos;
                                    $tdHoraRegreso.textContent = Formato;
                                    $tdHoraRegreso.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdHoraRegreso);

                                    $TBody.appendChild($tr);
                                    $TablaVistaPrevia.appendChild($TBody);

                                    // Empezamos a crear los td generales. DATOS ----------------------------------------------------------------------------

                                    const $tdFolioD = document.createElement("td");
                                    $tdFolioD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciId;
                                    $tdFolioD.id = "ReInciId";
                                    $tdFolioD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdFolioD);

                                    const $tdNominaD = document.createElement("td");
                                    $tdNominaD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNumero;
                                    $tdNominaD.id = "ReInciEmpNomina";
                                    $tdNominaD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdNominaD);

                                    const $tdNombreD = document.createElement("td");
                                    $tdNombreD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNombreCompleto;
                                    $tdNombreD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdNombreD);

                                    const $tdSucursalD = document.createElement("td");
                                    $tdSucursalD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciSucuNombre;
                                    $tdSucursalD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdSucursalD);

                                    const $tdFechaPermisoD = document.createElement("td");
                                    FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].fecha);
                                    DiaRecortado = FechaRecortada.getDate();
                                    if (DiaRecortado < 10) {
                                        DiaRecortado = "0" + DiaRecortado;
                                    }
                                    MesRecortado = FechaRecortada.getMonth();
                                    MesRecortado = MesRecortado + 1;
                                    if (MesRecortado < 10) {
                                        MesRecortado = "0" + MesRecortado;
                                    }
                                    FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                    $tdFechaPermisoD.textContent = FShort;
                                    $tdFechaPermisoD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdFechaPermisoD);

                                    const $tdEstatusD = document.createElement("td");
                                    $tdEstatusD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEstatusDescripcion;
                                    $tdEstatusD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdEstatusD);

                                    const $tdTipoPermisoD = document.createElement("td");
                                    $tdTipoPermisoD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciInciDescripcion;
                                    $tdTipoPermisoD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdTipoPermisoD);

                                    // --------------- Creamos los apartados particulares de permiso en comisión -----------------

                                    const $tdAsuntoD = document.createElement("td");
                                    $tdAsuntoD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciAsunto;
                                    $tdAsuntoD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdAsuntoD);

                                    const $tdDestinoD = document.createElement("td");
                                    $tdDestinoD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciDestino;
                                    $tdDestinoD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdDestinoD);

                                    const $tdTelDestinoD = document.createElement("td");
                                    $tdTelDestinoD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciTelDestino;
                                    $tdTelDestinoD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdTelDestinoD);

                                    const $tdContacto1D = document.createElement("td");
                                    $tdContacto1D.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciContacto1;
                                    $tdContacto1D.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdContacto1D);

                                    const $tdNombreDestinoD = document.createElement("td");
                                    $tdNombreDestinoD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciNombreDestino;
                                    $tdNombreDestinoD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdNombreDestinoD);

                                    const $tdContacto2D = document.createElement("td");
                                    $tdContacto2D.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciContacto2;
                                    $tdContacto2D.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdContacto2D);

                                    const $tdObservacionesD = document.createElement("td");
                                    $tdObservacionesD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciObservaciones;
                                    $tdObservacionesD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdObservacionesD);

                                    const $tdHoraSalidaD = document.createElement("td");
                                    var HoraObjeto = DatosDeReporte.incidenciasBySucursal[i].detInciHoraSalida;
                                    var Hora = HoraObjeto.hours;
                                    if (Hora < 10) {
                                        Hora = "0" + Hora;
                                    }
                                    var Minutos = HoraObjeto.minutes;
                                    if (Minutos < 10) {
                                        Minutos = "0" + Minutos;
                                    }
                                    var Segundos = HoraObjeto.seconds;
                                    if (Segundos < 10) {
                                        Segundos = "0" + Segundos;
                                    }
                                    var Formato = Hora + ":" + Minutos + ":" + Segundos;

                                    $tdHoraSalidaD.textContent = Formato;
                                    $tdHoraSalidaD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdHoraSalidaD);

                                    const $tdHoraRegresoD = document.createElement("td");
                                    HoraObjeto = DatosDeReporte.incidenciasBySucursal[i].detInciHoraRegreso;
                                    Hora = HoraObjeto.hours;
                                    if (Hora < 10) {
                                        Hora = "0" + Hora;
                                    }
                                    Minutos = HoraObjeto.minutes;
                                    if (Minutos < 10) {
                                        Minutos = "0" + Minutos;
                                    }
                                    Segundos = HoraObjeto.seconds;
                                    if (Segundos < 10) {
                                        Segundos = "0" + Segundos;
                                    }
                                    Formato = Hora + ":" + Minutos + ":" + Segundos;
                                    $tdHoraRegresoD.textContent = Formato;
                                    $tdHoraRegresoD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdHoraRegresoD);



                                    $TBodyD.appendChild($trD);
                                    $TablaDatos.appendChild($TBodyD);

                                }

                                if (BanderaComision == true) {
                                    $('#table5').DataTable().clear();
                                    $('#table5').DataTable().destroy();
                                }

                                /*if (BanderaParaDataTable != true) {*/
                                $('#table5').DataTable({
                                    "destroy": true,
                                    "searching": true,
                                    "lengthChange": false,
                                    "info": false,
                                    dom: 'Bfrtip',
                                    buttons: [
                                        {
                                            extend: 'pdfHtml5',
                                            orientation: 'landscape',
                                            pageSize: 'LEGAL'
                                        }
                                    ]


                                });

                                BanderaComision = true;
                                BanderaParaControlador = "Comisión";

                                $("#TablaVacaciones").hide();
                                $("#TablaComision").show();
                                $("#TablaPersonalSH").hide();
                                $("#PermisoPersonalCH").hide();
                                $("#Todos").hide();



                            } else if (DatosDeReporte.incidenciasBySucursal[0].reInciInciId == 5 || DatosDeReporte.incidenciasBySucursal[0].reInciInciId == 6 || DatosDeReporte.incidenciasBySucursal[0].reInciInciId == 7 || DatosDeReporte.incidenciasBySucursal[0].reInciInciId == 8) {
                                // Permiso personal 
                                if (DatosDeReporte.incidenciasBySucursal[0].reInciInciId == 6 || DatosDeReporte.incidenciasBySucursal[0].reInciInciId == 7) { // Permisos SIN horario

                                    // creamos el THead Paginación
                                    const $THead = document.createElement("thead");
                                    const $TrHead = document.createElement("tr");
                                    const $ThFolio = document.createElement("th");
                                    const $ThNomina = document.createElement("th");
                                    const $ThNombre = document.createElement("th");
                                    const $ThSucursal = document.createElement("th");
                                    const $ThFechaPermiso = document.createElement("th");
                                    const $ThEstatusPermiso = document.createElement("th");
                                    const $ThTipoPermiso = document.createElement("th"); // Hasta aquí son apartados generales
                                    const $ThFechaDetallePermiso = document.createElement("th"); // Empiezan los apartados de cada tipo de permiso
                                    const $ThMotivo = document.createElement("th");
                                    const $ThMedidaAccion = document.createElement("th");

                                    $ThFolio.textContent = "Folio del permiso";
                                    $ThFolio.setAttribute("scope", "col");
                                    $ThFolio.setAttribute("nowrap", "nowrap");
                                    $TrHead.appendChild($ThFolio);
                                    $THead.appendChild($TrHead);

                                    $ThNomina.textContent = "Número de nómina";
                                    $ThNomina.setAttribute("scope", "col");
                                    $ThNomina.setAttribute("nowrap", "nowrap");
                                    $TrHead.appendChild($ThNomina);
                                    $THead.appendChild($TrHead);

                                    $ThNombre.textContent = "Nombre del empleado";
                                    $ThNombre.setAttribute("scope", "col");
                                    $ThNombre.setAttribute("nowrap", "nowrap");
                                    $TrHead.appendChild($ThNombre);
                                    $THead.appendChild($TrHead);

                                    $ThSucursal.textContent = "Nombre de Sucursal";
                                    $ThSucursal.setAttribute("scope", "col");
                                    $ThSucursal.setAttribute("nowrap", "nowrap");
                                    $TrHead.appendChild($ThSucursal);
                                    $THead.appendChild($TrHead);

                                    $ThFechaPermiso.textContent = "Fecha del permiso";
                                    $ThFechaPermiso.setAttribute("scope", "col");
                                    $ThFechaPermiso.setAttribute("nowrap", "nowrap");
                                    $TrHead.appendChild($ThFechaPermiso);
                                    $THead.appendChild($TrHead);

                                    $ThEstatusPermiso.textContent = "Estatus del permiso";
                                    $ThEstatusPermiso.setAttribute("scope", "col");
                                    $ThEstatusPermiso.setAttribute("nowrap", "nowrap");
                                    $TrHead.appendChild($ThEstatusPermiso);
                                    $THead.appendChild($TrHead);

                                    $ThTipoPermiso.textContent = "Tipo de permiso";
                                    $ThTipoPermiso.setAttribute("scope", "col");
                                    $ThTipoPermiso.setAttribute("nowrap", "nowrap");
                                    $TrHead.appendChild($ThTipoPermiso);
                                    $THead.appendChild($TrHead);

                                    $ThFechaDetallePermiso.textContent = "Fecha para el permiso";
                                    $ThFechaDetallePermiso.setAttribute("scope", "col");
                                    $ThFechaDetallePermiso.setAttribute("nowrap", "nowrap");
                                    $TrHead.appendChild($ThFechaDetallePermiso);
                                    $THead.appendChild($TrHead);

                                    $ThMotivo.textContent = "Motivo";
                                    $ThMotivo.setAttribute("scope", "col");
                                    $ThMotivo.setAttribute("nowrap", "nowrap");
                                    $TrHead.appendChild($ThMotivo);
                                    $THead.appendChild($TrHead);

                                    $ThMedidaAccion.textContent = "Medida de acción";
                                    $ThMedidaAccion.setAttribute("scope", "col");
                                    $ThMedidaAccion.setAttribute("nowrap", "nowrap");
                                    $TrHead.appendChild($ThMedidaAccion);
                                    $THead.appendChild($TrHead);

                                    $TablaVistaPrevia.appendChild($THead);

                                    const $TBody = document.createElement("tbody");

                                    // creamos el THead Datos
                                    const $THeadD = document.createElement("thead");
                                    const $TrHeadD = document.createElement("tr");
                                    const $ThFolioD = document.createElement("th");
                                    const $ThNominaD = document.createElement("th");
                                    const $ThNombreD = document.createElement("th");
                                    const $ThSucursalD = document.createElement("th");
                                    const $ThFechaPermisoD = document.createElement("th");
                                    const $ThEstatusPermisoD = document.createElement("th");
                                    const $ThTipoPermisoD = document.createElement("th"); // Hasta aquí son apartados generales
                                    const $ThFechaDetallePermisoD = document.createElement("th"); // Empiezan los apartados de cada tipo de permiso
                                    const $ThMotivoD = document.createElement("th");
                                    const $ThMedidaAccionD = document.createElement("th");

                                    $ThFolioD.textContent = "Folio del permiso";
                                    $ThFolioD.setAttribute("scope", "col");
                                    $ThFolioD.setAttribute("nowrap", "nowrap");
                                    $TrHeadD.appendChild($ThFolioD);
                                    $THeadD.appendChild($TrHeadD);

                                    $ThNominaD.textContent = "Número de nómina";
                                    $ThNominaD.setAttribute("scope", "col");
                                    $ThNominaD.setAttribute("nowrap", "nowrap");
                                    $TrHeadD.appendChild($ThNominaD);
                                    $THeadD.appendChild($TrHeadD);

                                    $ThNombreD.textContent = "Nombre del empleado";
                                    $ThNombreD.setAttribute("scope", "col");
                                    $ThNombreD.setAttribute("nowrap", "nowrap");
                                    $TrHeadD.appendChild($ThNombreD);
                                    $THeadD.appendChild($TrHeadD);

                                    $ThSucursalD.textContent = "Nombre de Sucursal";
                                    $ThSucursalD.setAttribute("scope", "col");
                                    $ThSucursalD.setAttribute("nowrap", "nowrap");
                                    $TrHeadD.appendChild($ThSucursalD);
                                    $THeadD.appendChild($TrHeadD);

                                    $ThFechaPermisoD.textContent = "Fecha del permiso";
                                    $ThFechaPermisoD.setAttribute("scope", "col");
                                    $ThFechaPermisoD.setAttribute("nowrap", "nowrap");
                                    $TrHeadD.appendChild($ThFechaPermisoD);
                                    $THeadD.appendChild($TrHeadD);

                                    $ThEstatusPermisoD.textContent = "Estatus del permiso";
                                    $ThEstatusPermisoD.setAttribute("scope", "col");
                                    $ThEstatusPermisoD.setAttribute("nowrap", "nowrap");
                                    $TrHeadD.appendChild($ThEstatusPermisoD);
                                    $THeadD.appendChild($TrHeadD);

                                    $ThTipoPermisoD.textContent = "Tipo de permiso";
                                    $ThTipoPermisoD.setAttribute("scope", "col");
                                    $ThTipoPermisoD.setAttribute("nowrap", "nowrap");
                                    $TrHeadD.appendChild($ThTipoPermisoD);
                                    $THeadD.appendChild($TrHeadD);

                                    $ThFechaDetallePermisoD.textContent = "Fecha para el permiso";
                                    $ThFechaDetallePermisoD.setAttribute("scope", "col");
                                    $ThFechaDetallePermisoD.setAttribute("nowrap", "nowrap");
                                    $TrHeadD.appendChild($ThFechaDetallePermisoD);
                                    $THeadD.appendChild($TrHeadD);

                                    $ThMotivoD.textContent = "Motivo";
                                    $ThMotivoD.setAttribute("scope", "col");
                                    $ThMotivoD.setAttribute("nowrap", "nowrap");
                                    $TrHeadD.appendChild($ThMotivoD);
                                    $THeadD.appendChild($TrHeadD);
                                    
                                    $ThMedidaAccionD.textContent = "Medida de acción";
                                    $ThMedidaAccionD.setAttribute("scope", "col");
                                    $ThMedidaAccionD.setAttribute("nowrap", "nowrap");
                                    $TrHeadD.appendChild($ThMedidaAccionD);
                                    $THeadD.appendChild($TrHeadD);

                                    $TablaDatos.appendChild($THeadD);

                                    const $TBodyD = document.createElement("tbody");

                                    /* ---------------- Aquí termina la creación del Header de la tabla. ---------------- */

                                for (var i = 0; i < DatosDeReporte.incidenciasBySucursal.length; i++) {
                                    // Creamos el Tr

                                    const $tr = document.createElement("tr");
                                    const $trD = document.createElement("tr");
                                    
                                    // Empezamos a crear los td generales. PAGINACION --------------------------------------------------------------------------------------------

                                    const $tdFolio = document.createElement("td");
                                    $tdFolio.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciId;
                                    $tdFolio.id = "ReInciId";
                                    $tdFolio.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdFolio);

                                    const $tdNomina = document.createElement("td");
                                    $tdNomina.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNumero;
                                    $tdNomina.id = "ReInciEmpNomina";
                                    $tdNomina.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdNomina);

                                    const $tdNombre = document.createElement("td");
                                    $tdNombre.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNombreCompleto;
                                    $tdNombre.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdNombre);

                                    const $tdSucursal = document.createElement("td");
                                    $tdSucursal.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciSucuNombre;
                                    $tdSucursal.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdSucursal);

                                    const $tdFechaPermiso = document.createElement("td");
                                    var FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].fecha);
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
                                    $tdFechaPermiso.textContent = FShort;
                                    $tdFechaPermiso.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdFechaPermiso);

                                    const $tdEstatus = document.createElement("td");
                                    $tdEstatus.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEstatusDescripcion;
                                    $tdEstatus.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdEstatus);

                                    const $tdTipoPermiso = document.createElement("td");
                                    $tdTipoPermiso.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciInciDescripcion;
                                    $tdTipoPermiso.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdTipoPermiso);

                                    // Aquí terminan los encabezados generales

                                    // Empezamos a crear los particulares de cada permiso

                                    const $tdFechaDetallePermiso = document.createElement("td");
                                     FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciDetFecha);
                                     DiaRecortado = FechaRecortada.getDate();
                                    if (DiaRecortado < 10) {
                                        DiaRecortado = "0" + DiaRecortado;
                                    }
                                     MesRecortado = FechaRecortada.getMonth();
                                    MesRecortado = MesRecortado + 1;
                                    if (MesRecortado < 10) {
                                        MesRecortado = "0" + MesRecortado;
                                    }
                                     FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                    $tdFechaDetallePermiso.textContent = FShort;
                                    $tdFechaDetallePermiso.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdFechaDetallePermiso);
                                    
                                    const $tdMotivo = document.createElement("td");
                                    $tdMotivo.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciMotivo;
                                    $tdMotivo.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdMotivo);

                                    const $tdMedidaAccion = document.createElement("td");
                                    $tdMedidaAccion.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciMedidaAccion;
                                    $tdMedidaAccion.setAttribute("nowrap", "nowrap");
                                    $tr.appendChild($tdMedidaAccion);

                                    $TBody.appendChild($tr);
                                    $TablaVistaPrevia.appendChild($TBody);

                                    // Empezamos a crear los td generales. DATOS --------------------------------------------------------------------------------------------

                                    const $tdFolioD = document.createElement("td");
                                    $tdFolioD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciId;
                                    $tdFolioD.id = "ReInciId";
                                    $tdFolioD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdFolioD);

                                    const $tdNominaD = document.createElement("td");
                                    $tdNominaD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNumero;
                                    $tdNominaD.id = "ReInciEmpNomina";
                                    $tdNominaD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdNominaD);

                                    const $tdNombreD = document.createElement("td");
                                    $tdNombreD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNombreCompleto;
                                    $tdNombreD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdNombreD);

                                    const $tdSucursalD = document.createElement("td");
                                    $tdSucursalD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciSucuNombre;
                                    $tdSucursalD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdSucursalD);

                                    const $tdFechaPermisoD = document.createElement("td");
                                    FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].fecha);
                                    DiaRecortado = FechaRecortada.getDate();
                                    if (DiaRecortado < 10) {
                                        DiaRecortado = "0" + DiaRecortado;
                                    }
                                    MesRecortado = FechaRecortada.getMonth();
                                    MesRecortado = MesRecortado + 1;
                                    if (MesRecortado < 10) {
                                        MesRecortado = "0" + MesRecortado;
                                    }
                                    FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                    $tdFechaPermisoD.textContent = FShort;
                                    $tdFechaPermisoD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdFechaPermisoD);

                                    const $tdEstatusD = document.createElement("td");
                                    $tdEstatusD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEstatusDescripcion;
                                    $tdEstatusD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdEstatusD);

                                    const $tdTipoPermisoD = document.createElement("td");
                                    $tdTipoPermisoD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciInciDescripcion;
                                    $tdTipoPermisoD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdTipoPermisoD);


                                    // Aquí terminan los encabezados generales

                                    // Empezamos a crear los particulares de cada permiso

                                    const $tdFechaDetallePermisoD = document.createElement("td");
                                    FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciDetFecha);
                                    DiaRecortado = FechaRecortada.getDate();
                                    if (DiaRecortado < 10) {
                                        DiaRecortado = "0" + DiaRecortado;
                                    }
                                    MesRecortado = FechaRecortada.getMonth();
                                    MesRecortado = MesRecortado + 1;
                                    if (MesRecortado < 10) {
                                        MesRecortado = "0" + MesRecortado;
                                    }
                                    FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                    $tdFechaDetallePermisoD.textContent = FShort;
                                    $tdFechaDetallePermisoD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdFechaDetallePermisoD);

                                    const $tdMotivoD = document.createElement("td");
                                    $tdMotivoD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciMotivo;
                                    $tdMotivoD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdMotivoD);

                                    const $tdMedidaAccionD = document.createElement("td");
                                    $tdMedidaAccionD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciMedidaAccion;
                                    $tdMedidaAccionD.setAttribute("nowrap", "nowrap");
                                    $trD.appendChild($tdMedidaAccionD);

                                    $TBodyD.appendChild($trD);
                                    $TablaDatos.appendChild($TBodyD);

                                    }


                                    if (BanderaPersonalSH == true) {
                                        $('#table6').DataTable().clear();
                                        $('#table6').DataTable().destroy();
                                    }

                                    /*if (BanderaParaDataTable != true) {*/
                                    $('#table6').DataTable({
                                        "destroy": true,
                                        "searching": true,
                                        "lengthChange": false,
                                        "info": false,
                                        dom: 'Bfrtip',
                                        buttons: [
                                            {
                                                extend: 'pdfHtml5',
                                                orientation: 'landscape',
                                                pageSize: 'LEGAL'
                                            }
                                        ]


                                    });

                                    BanderaPersonalSH = true;
                                    BanderaParaControlador = "PersonalSH";

                                    $("#TablaVacaciones").hide();
                                    $("#TablaComision").hide();
                                    $("#TablaPersonalSH").show();
                                    $("#PermisoPersonalCH").hide();
                                    $("#Todos").hide();

                                } else { // Permisos CON horarios

                                    // ---------------- 
                                    if (DatosDeReporte.descripcionPermiso == "ENTRADAS/SALIDAS") {

                                        // creamos el THead
                                        const $THead = document.createElement("thead");
                                        const $TrHead = document.createElement("tr");
                                        const $ThFolio = document.createElement("th");
                                        const $ThNomina = document.createElement("th");
                                        const $ThNombre = document.createElement("th");
                                        const $ThSucursal = document.createElement("th");
                                        const $ThFechaPermiso = document.createElement("th");
                                        const $ThEstatusPermiso = document.createElement("th");
                                        const $ThTipoPermiso = document.createElement("th"); // Hasta aquí son apartados generales
                                        const $ThFechaDetallePermiso = document.createElement("th"); // Empiezan los apartados de cada tipo de permiso
                                        const $ThFechaFinDetallePermiso = document.createElement("th");
                                        const $ThHoraEntradaTrabajo = document.createElement("th");
                                        const $ThHoraSalidaTrabajo = document.createElement("th");
                                        const $ThHoraSalidaComida = document.createElement("th");
                                        const $ThHoraRegresoComida = document.createElement("th");
                                        const $ThMotivo = document.createElement("th");
                                        const $ThMedidaAccion = document.createElement("th");

                                        $ThFolio.textContent = "Folio del permiso";
                                        $ThFolio.setAttribute("scope", "col");
                                        $ThFolio.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThFolio);
                                        $THead.appendChild($TrHead);

                                        $ThNomina.textContent = "Número de nómina";
                                        $ThNomina.setAttribute("scope", "col");
                                        $ThNomina.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThNomina);
                                        $THead.appendChild($TrHead);

                                        $ThNombre.textContent = "Nombre del empleado";
                                        $ThNombre.setAttribute("scope", "col");
                                        $ThNombre.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThNombre);
                                        $THead.appendChild($TrHead);

                                        $ThSucursal.textContent = "Nombre de Sucursal";
                                        $ThSucursal.setAttribute("scope", "col");
                                        $ThSucursal.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThSucursal);
                                        $THead.appendChild($TrHead);

                                        $ThFechaPermiso.textContent = "Fecha del permiso";
                                        $ThFechaPermiso.setAttribute("scope", "col");
                                        $ThFechaPermiso.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThFechaPermiso);
                                        $THead.appendChild($TrHead);

                                        $ThEstatusPermiso.textContent = "Estatus del permiso";
                                        $ThEstatusPermiso.setAttribute("scope", "col");
                                        $ThEstatusPermiso.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThEstatusPermiso);
                                        $THead.appendChild($TrHead);

                                        $ThTipoPermiso.textContent = "Tipo de permiso";
                                        $ThTipoPermiso.setAttribute("scope", "col");
                                        $ThTipoPermiso.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThTipoPermiso);
                                        $THead.appendChild($TrHead);

                                        $ThFechaDetallePermiso.textContent = "Fecha para el permiso";
                                        $ThFechaDetallePermiso.setAttribute("scope", "col");
                                        $ThFechaDetallePermiso.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThFechaDetallePermiso);
                                        $THead.appendChild($TrHead);
                                        
                                        $ThFechaFinDetallePermiso.textContent = "Fecha final del permiso";
                                        $ThFechaFinDetallePermiso.setAttribute("scope", "col");
                                        $ThFechaFinDetallePermiso.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThFechaFinDetallePermiso);
                                        $THead.appendChild($TrHead);
                                       
                                        $ThHoraEntradaTrabajo.textContent = "Hora de entrada";
                                        $ThHoraEntradaTrabajo.setAttribute("scope", "col");
                                        $ThHoraEntradaTrabajo.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThHoraEntradaTrabajo);
                                        $THead.appendChild($TrHead);
                                        
                                        $ThHoraSalidaTrabajo.textContent = "Hora de salida";
                                        $ThHoraSalidaTrabajo.setAttribute("scope", "col");
                                        $ThHoraSalidaTrabajo.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThHoraSalidaTrabajo);
                                        $THead.appendChild($TrHead);
                                        
                                        $ThHoraSalidaComida.textContent = "Hora de salida a comer";
                                        $ThHoraSalidaComida.setAttribute("scope", "col");
                                        $ThHoraSalidaComida.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThHoraSalidaComida);
                                        $THead.appendChild($TrHead);
                                        
                                        $ThHoraRegresoComida.textContent = "Hora de regreso de comer";
                                        $ThHoraRegresoComida.setAttribute("scope", "col");
                                        $ThHoraRegresoComida.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThHoraRegresoComida);
                                        $THead.appendChild($TrHead);
                                       
                                        $ThMotivo.textContent = "Motivo";
                                        $ThMotivo.setAttribute("scope", "col");
                                        $ThMotivo.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThMotivo);
                                        $THead.appendChild($TrHead);

                                        $ThMedidaAccion.textContent = "Medida de acción";
                                        $ThMedidaAccion.setAttribute("scope", "col");
                                        $ThMedidaAccion.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThMedidaAccion);
                                        $THead.appendChild($TrHead);

                                        $TablaVistaPrevia.appendChild($THead);

                                        const $TBody = document.createElement("tbody");

                                        /* -----------------------------------------------------DATOS-------------------------------------------------------------*/

                                        // creamos el THead
                                        const $THeadD = document.createElement("thead");
                                        const $TrHeadD = document.createElement("tr");
                                        const $ThFolioD = document.createElement("th");
                                        const $ThNominaD = document.createElement("th");
                                        const $ThNombreD = document.createElement("th");
                                        const $ThSucursalD = document.createElement("th");
                                        const $ThFechaPermisoD = document.createElement("th");
                                        const $ThEstatusPermisoD = document.createElement("th");
                                        const $ThTipoPermisoD = document.createElement("th"); // Hasta aquí son apartados generales
                                        const $ThFechaDetallePermisoD = document.createElement("th"); // Empiezan los apartados de cada tipo de permiso
                                        const $ThFechaFinDetallePermisoD = document.createElement("th");
                                        const $ThHoraEntradaTrabajoD = document.createElement("th");
                                        const $ThHoraSalidaTrabajoD = document.createElement("th");
                                        const $ThHoraSalidaComidaD = document.createElement("th");
                                        const $ThHoraRegresoComidaD = document.createElement("th");
                                        const $ThMotivoD = document.createElement("th");
                                        const $ThMedidaAccionD = document.createElement("th");

                                        $ThFolioD.textContent = "Folio del permiso";
                                        $ThFolioD.setAttribute("scope", "col");
                                        $ThFolioD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThFolioD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThNominaD.textContent = "Número de nómina";
                                        $ThNominaD.setAttribute("scope", "col");
                                        $ThNominaD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThNominaD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThNombreD.textContent = "Nombre del empleado";
                                        $ThNombreD.setAttribute("scope", "col");
                                        $ThNombreD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThNombreD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThSucursalD.textContent = "Nombre de Sucursal";
                                        $ThSucursalD.setAttribute("scope", "col");
                                        $ThSucursalD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThSucursalD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThFechaPermisoD.textContent = "Fecha del permiso";
                                        $ThFechaPermisoD.setAttribute("scope", "col");
                                        $ThFechaPermisoD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThFechaPermisoD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThEstatusPermisoD.textContent = "Estatus del permiso";
                                        $ThEstatusPermisoD.setAttribute("scope", "col");
                                        $ThEstatusPermisoD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThEstatusPermisoD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThTipoPermisoD.textContent = "Tipo de permiso";
                                        $ThTipoPermisoD.setAttribute("scope", "col");
                                        $ThTipoPermisoD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThTipoPermisoD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThFechaDetallePermisoD.textContent = "Fecha para el permiso";
                                        $ThFechaDetallePermisoD.setAttribute("scope", "col");
                                        $ThFechaDetallePermisoD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThFechaDetallePermisoD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThFechaFinDetallePermisoD.textContent = "Fecha final del permiso";
                                        $ThFechaFinDetallePermisoD.setAttribute("scope", "col");
                                        $ThFechaFinDetallePermisoD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThFechaFinDetallePermisoD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThHoraEntradaTrabajoD.textContent = "Hora de entrada";
                                        $ThHoraEntradaTrabajoD.setAttribute("scope", "col");
                                        $ThHoraEntradaTrabajoD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThHoraEntradaTrabajoD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThHoraSalidaTrabajoD.textContent = "Hora de salida";
                                        $ThHoraSalidaTrabajoD.setAttribute("scope", "col");
                                        $ThHoraSalidaTrabajoD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThHoraSalidaTrabajoD);
                                        $THead.appendChild($TrHeadD);

                                        $ThHoraSalidaComidaD.textContent = "Hora de salida a comer";
                                        $ThHoraSalidaComidaD.setAttribute("scope", "col");
                                        $ThHoraSalidaComidaD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThHoraSalidaComidaD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThHoraRegresoComidaD.textContent = "Hora de regreso de comer";
                                        $ThHoraRegresoComidaD.setAttribute("scope", "col");
                                        $ThHoraRegresoComidaD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThHoraRegresoComidaD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThMotivoD.textContent = "Motivo";
                                        $ThMotivoD.setAttribute("scope", "col");
                                        $ThMotivoD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThMotivoD);
                                        $THeadD.appendChild($TrHeadD);


                                        $ThMedidaAccionD.textContent = "Medida de acción";
                                        $ThMedidaAccionD.setAttribute("scope", "col");
                                        $ThMedidaAccionD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThMedidaAccionD);
                                        $THeadD.appendChild($TrHeadD);

                                        
                                        $TablaDatos.appendChild($THeadD);

                                        const $TBodyD = document.createElement("tbody");

                                        /* ---------------- Aquí termina la creación del Header de la tabla. ---------------- */

                                        for (var i = 0; i < DatosDeReporte.incidenciasBySucursal.length; i++) {
                                            // Creamos el Tr

                                            const $tr = document.createElement("tr");
                                            const $trD = document.createElement("tr");

                                            // Empezamos a crear los td generales. PAGINACIÓN --------------------------------------------------------------------------

                                            const $tdFolio = document.createElement("td");
                                            $tdFolio.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciId;
                                            $tdFolio.id = "ReInciId";
                                            $tdFolio.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdFolio);

                                            const $tdNomina = document.createElement("td");
                                            $tdNomina.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNumero;
                                            $tdNomina.id = "ReInciEmpNomina";
                                            $tdNomina.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdNomina);

                                            const $tdNombre = document.createElement("td");
                                            $tdNombre.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNombreCompleto;
                                            $tdNombre.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdNombre);

                                            const $tdSucursal = document.createElement("td");
                                            $tdSucursal.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciSucuNombre;
                                            $tdSucursal.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdSucursal);

                                            const $tdFechaPermiso = document.createElement("td");
                                            var FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].fecha);
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
                                            $tdFechaPermiso.textContent = FShort;
                                            $tdFechaPermiso.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdFechaPermiso);

                                            const $tdEstatus = document.createElement("td");
                                            $tdEstatus.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEstatusDescripcion;
                                            $tdEstatus.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdEstatus);

                                            const $tdTipoPermiso = document.createElement("td");
                                            $tdTipoPermiso.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciInciDescripcion;
                                            $tdTipoPermiso.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdTipoPermiso);

                                            // Aquí terminan los encabezados generales

                                            // Empezamos a crear los particulares de cada permiso

                                            const $tdFechaDetallePermiso = document.createElement("td");
                                            FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciDetFecha);
                                            DiaRecortado = FechaRecortada.getDate();
                                            if (DiaRecortado < 10) {
                                                DiaRecortado = "0" + DiaRecortado;
                                            }
                                            MesRecortado = FechaRecortada.getMonth();
                                            MesRecortado = MesRecortado + 1;
                                            if (MesRecortado < 10) {
                                                MesRecortado = "0" + MesRecortado;
                                            }
                                            FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                            $tdFechaDetallePermiso.textContent = FShort;
                                            $tdFechaDetallePermiso.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdFechaDetallePermiso);
                                            // ----------------------------------------------------------------------------------------------------

                                            const $tdFechaFinDetallePermiso = document.createElement("td");
                                            $tdFechaFinDetallePermiso.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciFechaFinPermisoPersonal;
                                            $tdFechaFinDetallePermiso.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdFechaFinDetallePermiso);

                                            const $tdHoraEntradaTrabajo = document.createElement("td");
                                            $tdHoraEntradaTrabajo.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciHoraIngresoPermiso;
                                            $tdHoraEntradaTrabajo.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdHoraEntradaTrabajo);

                                            const $tdHoraSalidaTrabajo = document.createElement("td");
                                            $tdHoraSalidaTrabajo.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciHoraSalidaPermiso;
                                            $tdHoraSalidaTrabajo.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdHoraSalidaTrabajo);

                                            const $tdHoraSalidaComida = document.createElement("td");
                                            $tdHoraSalidaComida.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciHoraSalidaComida;
                                            $tdHoraSalidaComida.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdHoraSalidaComida);

                                            const $tdHoraRegresoComida = document.createElement("td");
                                            $tdHoraRegresoComida.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciHoraRegresoComida;
                                            $tdHoraRegresoComida.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdHoraRegresoComida);

                                            // -------------------------------------------------------------------------------------------------
                                            const $tdMotivo = document.createElement("td");
                                            $tdMotivo.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciMotivo;
                                            $tdMotivo.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdMotivo);

                                            const $tdMedidaAccion = document.createElement("td");
                                            $tdMedidaAccion.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciMedidaAccion;
                                            $tdMedidaAccion.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdMedidaAccion);

                                            

                                            $TBody.appendChild($tr);
                                            $TablaVistaPrevia.appendChild($TBody);

                                            // Empezamos a crear los td generales. DATOS --------------------------------------------------------------------------

                                            const $tdFolioD = document.createElement("td");
                                            $tdFolioD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciId;
                                            $tdFolioD.id = "ReInciId";
                                            $tdFolioD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdFolioD);

                                            const $tdNominaD = document.createElement("td");
                                            $tdNominaD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNumero;
                                            $tdNominaD.id = "ReInciEmpNomina";
                                            $tdNominaD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdNominaD);

                                            const $tdNombreD = document.createElement("td");
                                            $tdNombreD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNombreCompleto;
                                            $tdNombreD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdNombreD);

                                            const $tdSucursalD = document.createElement("td");
                                            $tdSucursalD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciSucuNombre;
                                            $tdSucursalD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdSucursalD);

                                            const $tdFechaPermisoD = document.createElement("td");
                                            FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].fecha);
                                            var DiaRecortado = FechaRecortada.getDate();
                                            if (DiaRecortado < 10) {
                                                DiaRecortado = "0" + DiaRecortado;
                                            }
                                            MesRecortado = FechaRecortada.getMonth();
                                            MesRecortado = MesRecortado + 1;
                                            if (MesRecortado < 10) {
                                                MesRecortado = "0" + MesRecortado;
                                            }
                                            FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                            $tdFechaPermisoD.textContent = FShort;
                                            $tdFechaPermisoD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdFechaPermisoD);

                                            const $tdEstatusD = document.createElement("td");
                                            $tdEstatusD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEstatusDescripcion;
                                            $tdEstatusD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdEstatusD);

                                            const $tdTipoPermisoD = document.createElement("td");
                                            $tdTipoPermisoD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciInciDescripcion;
                                            $tdTipoPermisoD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdTipoPermisoD);

                                            // Aquí terminan los encabezados generales

                                            // Empezamos a crear los particulares de cada permiso

                                            const $tdFechaDetallePermisoD = document.createElement("td");
                                            FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciDetFecha);
                                            DiaRecortado = FechaRecortada.getDate();
                                            if (DiaRecortado < 10) {
                                                DiaRecortado = "0" + DiaRecortado;
                                            }
                                            MesRecortado = FechaRecortada.getMonth();
                                            MesRecortado = MesRecortado + 1;
                                            if (MesRecortado < 10) {
                                                MesRecortado = "0" + MesRecortado;
                                            }
                                            FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                            $tdFechaDetallePermisoD.textContent = FShort;
                                            $tdFechaDetallePermisoD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdFechaDetallePermisoD);
                                            //---------------------
                                          
                                            const $tdFechaFinDetallePermisoD = document.createElement("td");
                                            $tdFechaFinDetallePermisoD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciFechaFinPermisoPersonal;
                                            $tdFechaFinDetallePermisoD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdFechaFinDetallePermisoD);

                                            const $tdHoraEntradaTrabajoD = document.createElement("td");
                                            $tdHoraEntradaTrabajoD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciHoraIngresoPermiso;
                                            $tdHoraEntradaTrabajoD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdHoraEntradaTrabajoD);

                                            const $tdHoraSalidaTrabajoD = document.createElement("td");
                                            $tdHoraSalidaTrabajoD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciHoraSalidaPermiso;
                                            $tdHoraSalidaTrabajoD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdHoraSalidaTrabajoD);

                                            const $tdHoraSalidaComidaD = document.createElement("td");
                                            $tdHoraSalidaComidaD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciHoraSalidaComida;
                                            $tdHoraSalidaComidaD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdHoraSalidaComidaD);

                                            const $tdHoraRegresoComidaD = document.createElement("td");
                                            $tdHoraRegresoComidaD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciHoraRegresoComida;
                                            $tdHoraRegresoComidaD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdHoraRegresoComidaD);
                                            //---------------------
                                            const $tdMotivoD = document.createElement("td");
                                            $tdMotivoD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciMotivo;
                                            $tdMotivoD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdMotivoD);

                                            const $tdMedidaAccionD = document.createElement("td");
                                            $tdMedidaAccionD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciMedidaAccion;
                                            $tdMedidaAccionD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdMedidaAccionD);

                                            

                                        }

                                        if (BanderaPersonalCH == true) {
                                            $('#table9').DataTable().clear();
                                            $('#table9').DataTable().destroy();
                                        }

                                        /*if (BanderaParaDataTable != true) {*/
                                        $('#table9').DataTable({
                                            "destroy": true,
                                            "searching": true,
                                            "lengthChange": false,
                                            "info": false,
                                            dom: 'Bfrtip',
                                            buttons: [
                                                {
                                                    extend: 'pdfHtml5',
                                                    orientation: 'landscape',
                                                    pageSize: 'LEGAL'
                                                }
                                            ]

                                        });

                                        BanderaPersonalCH = true;
                                        BanderaParaControlador = "Entradas/Salidas";

                                        $("#TablaVacaciones").hide();
                                        $("#TablaComision").hide();
                                        $("#TablaPersonalSH").hide();
                                        $("#PermisoPersonalCH").show();
                                        $("#Todos").hide();

                                    }
   // -----------------------------------------------------------------------------------------------------------------------------------------------
                                    else {

                                        // creamos el THead
                                        const $THead = document.createElement("thead");
                                        const $TrHead = document.createElement("tr");
                                        const $ThFolio = document.createElement("th");
                                        const $ThNomina = document.createElement("th");
                                        const $ThNombre = document.createElement("th");
                                        const $ThSucursal = document.createElement("th");
                                        const $ThFechaPermiso = document.createElement("th");
                                        const $ThEstatusPermiso = document.createElement("th");
                                        const $ThTipoPermiso = document.createElement("th"); // Hasta aquí son apartados generales
                                        const $ThFechaDetallePermiso = document.createElement("th"); // Empiezan los apartados de cada tipo de permiso
                                        const $ThMotivo = document.createElement("th");
                                        const $ThMedidaAccion = document.createElement("th");
                                        const $ThHorario = document.createElement("th");

                                        $ThFolio.textContent = "Folio del permiso";
                                        $ThFolio.setAttribute("scope", "col");
                                        $ThFolio.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThFolio);
                                        $THead.appendChild($TrHead);

                                        $ThNomina.textContent = "Número de nómina";
                                        $ThNomina.setAttribute("scope", "col");
                                        $ThNomina.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThNomina);
                                        $THead.appendChild($TrHead);

                                        $ThNombre.textContent = "Nombre del empleado";
                                        $ThNombre.setAttribute("scope", "col");
                                        $ThNombre.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThNombre);
                                        $THead.appendChild($TrHead);

                                        $ThSucursal.textContent = "Nombre de Sucursal";
                                        $ThSucursal.setAttribute("scope", "col");
                                        $ThSucursal.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThSucursal);
                                        $THead.appendChild($TrHead);

                                        $ThFechaPermiso.textContent = "Fecha del permiso";
                                        $ThFechaPermiso.setAttribute("scope", "col");
                                        $ThFechaPermiso.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThFechaPermiso);
                                        $THead.appendChild($TrHead);

                                        $ThEstatusPermiso.textContent = "Estatus del permiso";
                                        $ThEstatusPermiso.setAttribute("scope", "col");
                                        $ThEstatusPermiso.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThEstatusPermiso);
                                        $THead.appendChild($TrHead);

                                        $ThTipoPermiso.textContent = "Tipo de permiso";
                                        $ThTipoPermiso.setAttribute("scope", "col");
                                        $ThTipoPermiso.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThTipoPermiso);
                                        $THead.appendChild($TrHead);

                                        $ThFechaDetallePermiso.textContent = "Fecha para el permiso";
                                        $ThFechaDetallePermiso.setAttribute("scope", "col");
                                        $ThFechaDetallePermiso.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThFechaDetallePermiso);
                                        $THead.appendChild($TrHead);

                                        $ThMotivo.textContent = "Motivo";
                                        $ThMotivo.setAttribute("scope", "col");
                                        $ThMotivo.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThMotivo);
                                        $THead.appendChild($TrHead);

                                        $ThMedidaAccion.textContent = "Medida de acción";
                                        $ThMedidaAccion.setAttribute("scope", "col");
                                        $ThMedidaAccion.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThMedidaAccion);
                                        $THead.appendChild($TrHead);

                                        $ThHorario.textContent = "Horario";
                                        $ThHorario.setAttribute("scope", "col");
                                        $ThHorario.setAttribute("nowrap", "nowrap");
                                        $TrHead.appendChild($ThHorario);
                                        $THead.appendChild($TrHead);

                                        $TablaVistaPrevia.appendChild($THead);

                                        const $TBody = document.createElement("tbody");

                                        /* -----------------------------------------------------DATOS-------------------------------------------------------------*/

                                        // creamos el THead
                                        const $THeadD = document.createElement("thead");
                                        const $TrHeadD = document.createElement("tr");
                                        const $ThFolioD = document.createElement("th");
                                        const $ThNominaD = document.createElement("th");
                                        const $ThNombreD = document.createElement("th");
                                        const $ThSucursalD = document.createElement("th");
                                        const $ThFechaPermisoD = document.createElement("th");
                                        const $ThEstatusPermisoD = document.createElement("th");
                                        const $ThTipoPermisoD = document.createElement("th"); // Hasta aquí son apartados generales
                                        const $ThFechaDetallePermisoD = document.createElement("th"); // Empiezan los apartados de cada tipo de permiso
                                        const $ThMotivoD = document.createElement("th");
                                        const $ThMedidaAccionD = document.createElement("th");
                                        const $ThHorarioD = document.createElement("th");

                                        $ThFolioD.textContent = "Folio del permiso";
                                        $ThFolioD.setAttribute("scope", "col");
                                        $ThFolioD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThFolioD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThNominaD.textContent = "Número de nómina";
                                        $ThNominaD.setAttribute("scope", "col");
                                        $ThNominaD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThNominaD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThNombreD.textContent = "Nombre del empleado";
                                        $ThNombreD.setAttribute("scope", "col");
                                        $ThNombreD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThNombreD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThSucursalD.textContent = "Nombre de Sucursal";
                                        $ThSucursalD.setAttribute("scope", "col");
                                        $ThSucursalD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThSucursalD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThFechaPermisoD.textContent = "Fecha del permiso";
                                        $ThFechaPermisoD.setAttribute("scope", "col");
                                        $ThFechaPermisoD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThFechaPermisoD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThEstatusPermisoD.textContent = "Estatus del permiso";
                                        $ThEstatusPermisoD.setAttribute("scope", "col");
                                        $ThEstatusPermisoD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThEstatusPermisoD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThTipoPermisoD.textContent = "Tipo de permiso";
                                        $ThTipoPermisoD.setAttribute("scope", "col");
                                        $ThTipoPermisoD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThTipoPermisoD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThFechaDetallePermisoD.textContent = "Fecha para el permiso";
                                        $ThFechaDetallePermisoD.setAttribute("scope", "col");
                                        $ThFechaDetallePermisoD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThFechaDetallePermisoD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThMotivoD.textContent = "Motivo";
                                        $ThMotivoD.setAttribute("scope", "col");
                                        $ThMotivoD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThMotivoD);
                                        $THeadD.appendChild($TrHeadD);


                                        $ThMedidaAccionD.textContent = "Medida de acción";
                                        $ThMedidaAccionD.setAttribute("scope", "col");
                                        $ThMedidaAccionD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThMedidaAccionD);
                                        $THeadD.appendChild($TrHeadD);

                                        $ThHorarioD.textContent = "Horario";
                                        $ThHorarioD.setAttribute("scope", "col");
                                        $ThHorarioD.setAttribute("nowrap", "nowrap");
                                        $TrHeadD.appendChild($ThHorarioD);
                                        $THeadD.appendChild($TrHeadD);

                                        $TablaDatos.appendChild($THeadD);

                                        const $TBodyD = document.createElement("tbody");

                                        /* ---------------- Aquí termina la creación del Header de la tabla. ---------------- */

                                        for (var i = 0; i < DatosDeReporte.incidenciasBySucursal.length; i++) {
                                            // Creamos el Tr

                                            const $tr = document.createElement("tr");
                                            const $trD = document.createElement("tr");

                                            // Empezamos a crear los td generales. PAGINACIÓN --------------------------------------------------------------------------

                                            const $tdFolio = document.createElement("td");
                                            $tdFolio.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciId;
                                            $tdFolio.id = "ReInciId";
                                            $tdFolio.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdFolio);

                                            const $tdNomina = document.createElement("td");
                                            $tdNomina.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNumero;
                                            $tdNomina.id = "ReInciEmpNomina";
                                            $tdNomina.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdNomina);

                                            const $tdNombre = document.createElement("td");
                                            $tdNombre.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNombreCompleto;
                                            $tdNombre.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdNombre);

                                            const $tdSucursal = document.createElement("td");
                                            $tdSucursal.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciSucuNombre;
                                            $tdSucursal.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdSucursal);

                                            const $tdFechaPermiso = document.createElement("td");
                                            var FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].fecha);
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
                                            $tdFechaPermiso.textContent = FShort;
                                            $tdFechaPermiso.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdFechaPermiso);

                                            const $tdEstatus = document.createElement("td");
                                            $tdEstatus.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEstatusDescripcion;
                                            $tdEstatus.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdEstatus);

                                            const $tdTipoPermiso = document.createElement("td");
                                            $tdTipoPermiso.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciInciDescripcion;
                                            $tdTipoPermiso.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdTipoPermiso);

                                            // Aquí terminan los encabezados generales

                                            // Empezamos a crear los particulares de cada permiso

                                            const $tdFechaDetallePermiso = document.createElement("td");
                                            FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciDetFecha);
                                            DiaRecortado = FechaRecortada.getDate();
                                            if (DiaRecortado < 10) {
                                                DiaRecortado = "0" + DiaRecortado;
                                            }
                                            MesRecortado = FechaRecortada.getMonth();
                                            MesRecortado = MesRecortado + 1;
                                            if (MesRecortado < 10) {
                                                MesRecortado = "0" + MesRecortado;
                                            }
                                            FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                            $tdFechaDetallePermiso.textContent = FShort;
                                            $tdFechaDetallePermiso.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdFechaDetallePermiso);

                                            const $tdMotivo = document.createElement("td");
                                            $tdMotivo.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciMotivo;
                                            $tdMotivo.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdMotivo);

                                            const $tdMedidaAccion = document.createElement("td");
                                            $tdMedidaAccion.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciMedidaAccion;
                                            $tdMedidaAccion.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdMedidaAccion);

                                            const $tdHorario = document.createElement("td");
                                            $tdHorario.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciHorarioDetalles;
                                            $tdHorario.setAttribute("nowrap", "nowrap");
                                            $tr.appendChild($tdHorario);

                                            $TBody.appendChild($tr);
                                            $TablaVistaPrevia.appendChild($TBody);

                                            // Empezamos a crear los td generales. DATOS --------------------------------------------------------------------------

                                            const $tdFolioD = document.createElement("td");
                                            $tdFolioD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciId;
                                            $tdFolioD.id = "ReInciId";
                                            $tdFolioD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdFolioD);

                                            const $tdNominaD = document.createElement("td");
                                            $tdNominaD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNumero;
                                            $tdNominaD.id = "ReInciEmpNomina";
                                            $tdNominaD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdNominaD);

                                            const $tdNombreD = document.createElement("td");
                                            $tdNombreD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNombreCompleto;
                                            $tdNombreD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdNombreD);

                                            const $tdSucursalD = document.createElement("td");
                                            $tdSucursalD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciSucuNombre;
                                            $tdSucursalD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdSucursalD);

                                            const $tdFechaPermisoD = document.createElement("td");
                                            FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].fecha);
                                            var DiaRecortado = FechaRecortada.getDate();
                                            if (DiaRecortado < 10) {
                                                DiaRecortado = "0" + DiaRecortado;
                                            }
                                            MesRecortado = FechaRecortada.getMonth();
                                            MesRecortado = MesRecortado + 1;
                                            if (MesRecortado < 10) {
                                                MesRecortado = "0" + MesRecortado;
                                            }
                                            FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                            $tdFechaPermisoD.textContent = FShort;
                                            $tdFechaPermisoD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdFechaPermisoD);

                                            const $tdEstatusD = document.createElement("td");
                                            $tdEstatusD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEstatusDescripcion;
                                            $tdEstatusD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdEstatusD);

                                            const $tdTipoPermisoD = document.createElement("td");
                                            $tdTipoPermisoD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciInciDescripcion;
                                            $tdTipoPermisoD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdTipoPermisoD);

                                            // Aquí terminan los encabezados generales

                                            // Empezamos a crear los particulares de cada permiso

                                            const $tdFechaDetallePermisoD = document.createElement("td");
                                            FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciDetFecha);
                                            DiaRecortado = FechaRecortada.getDate();
                                            if (DiaRecortado < 10) {
                                                DiaRecortado = "0" + DiaRecortado;
                                            }
                                            MesRecortado = FechaRecortada.getMonth();
                                            MesRecortado = MesRecortado + 1;
                                            if (MesRecortado < 10) {
                                                MesRecortado = "0" + MesRecortado;
                                            }
                                            FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                            $tdFechaDetallePermisoD.textContent = FShort;
                                            $tdFechaDetallePermisoD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdFechaDetallePermisoD);

                                            const $tdMotivoD = document.createElement("td");
                                            $tdMotivoD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciMotivo;
                                            $tdMotivoD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdMotivoD);

                                            const $tdMedidaAccionD = document.createElement("td");
                                            $tdMedidaAccionD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciMedidaAccion;
                                            $tdMedidaAccionD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdMedidaAccionD);

                                            const $tdHorarioD = document.createElement("td");
                                            $tdHorarioD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciHorarioDetalles;
                                            $tdHorarioD.setAttribute("nowrap", "nowrap");
                                            $trD.appendChild($tdHorarioD);

                                            $TBodyD.appendChild($trD);
                                            $TablaDatos.appendChild($TBodyD);

                                        }

                                        if (BanderaPersonalCH == true) {
                                            $('#table7').DataTable().clear();
                                            $('#table7').DataTable().destroy();
                                        }

                                        /*if (BanderaParaDataTable != true) {*/
                                        $('#table7').DataTable({
                                            "destroy": true,
                                            "searching": true,
                                            "lengthChange": false,
                                            "info": false,
                                            dom: 'Bfrtip',
                                            buttons: [
                                                {
                                                    extend: 'pdfHtml5',
                                                    orientation: 'landscape',
                                                    pageSize: 'LEGAL'
                                                }
                                            ]

                                        });

                                        BanderaPersonalCH = true;
                                        BanderaParaControlador = "PersonalCH";

                                        $("#TablaVacaciones").hide();
                                        $("#TablaComision").hide();
                                        $("#TablaPersonalSH").hide();
                                        $("#PermisoPersonalCH").show();
                                        $("#Todos").hide();

                                    }
                                    }

                            } 
                        } else { // Todos los permisos
                            // creamos el THead para PAGINACION
                            const $THead = document.createElement("thead");
                            const $TrHead = document.createElement("tr");
                            const $ThFolio = document.createElement("th");
                            const $ThNomina = document.createElement("th");
                            const $ThNombre = document.createElement("th");
                            const $ThSucursal = document.createElement("th");
                            const $ThFechaPermiso = document.createElement("th");
                            const $ThEstatusPermiso = document.createElement("th");
                            const $ThTipoPermiso = document.createElement("th"); // Hasta aquí son apartados generales
                            const $ThFechaInicio = document.createElement("th"); // Empiezan los apartados de vacaciones
                            const $ThFechaFinal = document.createElement("th");
                            const $ThFechaPresentacion = document.createElement("th");
                            const $ThDiasAusencia = document.createElement("th");
                            const $ThPersonaCubrira = document.createElement("th");
                            const $ThAsunto = document.createElement("th"); // Empiezan los apartados de comisión
                            const $ThDestino = document.createElement("th");
                            const $ThTelDestino = document.createElement("th");
                            const $ThContacto1 = document.createElement("th");
                            const $ThNombreDestino = document.createElement("th");
                            const $ThContacto2 = document.createElement("th");
                            const $ThObservaciones = document.createElement("th");
                            const $ThHoraSalida = document.createElement("th");
                            const $ThHoraRegreso = document.createElement("th");
                            const $ThFechaDetallePermiso = document.createElement("th"); // Empiezan los apartados de permiso personal 
                            const $ThMotivo = document.createElement("th");
                            const $ThMedidaAccion = document.createElement("th");
                            const $ThHorario = document.createElement("th");

                            $ThFolio.textContent = "Folio del permiso";
                            $ThFolio.setAttribute("scope", "col");
                            $ThFolio.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThFolio);
                            $THead.appendChild($TrHead);

                            $ThNomina.textContent = "Número de nómina";
                            $ThNomina.setAttribute("scope", "col");
                            $ThNomina.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThNomina);
                            $THead.appendChild($TrHead);

                            $ThNombre.textContent = "Nombre del empleado";
                            $ThNombre.setAttribute("scope", "col");
                            $ThNombre.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThNombre);
                            $THead.appendChild($TrHead);

                            $ThSucursal.textContent = "Nombre de Sucursal";
                            $ThSucursal.setAttribute("scope", "col");
                            $ThSucursal.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThSucursal);
                            $THead.appendChild($TrHead);

                            $ThFechaPermiso.textContent = "Fecha del permiso";
                            $ThFechaPermiso.setAttribute("scope", "col");
                            $ThFechaPermiso.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThFechaPermiso);
                            $THead.appendChild($TrHead);

                            $ThEstatusPermiso.textContent = "Estatus del permiso";
                            $ThEstatusPermiso.setAttribute("scope", "col");
                            $ThEstatusPermiso.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThEstatusPermiso);
                            $THead.appendChild($TrHead);

                            $ThTipoPermiso.textContent = "Tipo de permiso";
                            $ThTipoPermiso.setAttribute("scope", "col");
                            $ThTipoPermiso.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThTipoPermiso);
                            $THead.appendChild($TrHead);

                            $ThFechaInicio.textContent = "Fecha de inicio de vacaciones";
                            $ThFechaInicio.setAttribute("scope", "col");
                            $ThFechaInicio.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThFechaInicio);
                            $THead.appendChild($TrHead);

                            $ThFechaFinal.textContent = "Fecha culminación de vacaciones";
                            $ThFechaFinal.setAttribute("scope", "col");
                            $ThFechaFinal.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThFechaFinal);
                            $THead.appendChild($TrHead);

                            $ThFechaPresentacion.textContent = "Fecha de presentación";
                            $ThFechaPresentacion.setAttribute("scope", "col");
                            $ThFechaPresentacion.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThFechaPresentacion);
                            $THead.appendChild($TrHead);

                            $ThDiasAusencia.textContent = "Días de ausencia";
                            $ThDiasAusencia.setAttribute("scope", "col");
                            $ThDiasAusencia.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThDiasAusencia);
                            $THead.appendChild($TrHead);

                            $ThPersonaCubrira.textContent = "Persona que cubrirá";
                            $ThPersonaCubrira.setAttribute("scope", "col");
                            $ThPersonaCubrira.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThPersonaCubrira);
                            $THead.appendChild($TrHead);
                            
                            $ThAsunto.textContent = "Asunto";
                            $ThAsunto.setAttribute("scope", "col");
                            $ThAsunto.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThAsunto);
                            $THead.appendChild($TrHead);

                            $ThDestino.textContent = "Destino";
                            $ThDestino.setAttribute("scope", "col");
                            $ThDestino.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThDestino);
                            $THead.appendChild($TrHead);

                            $ThTelDestino.textContent = "Destino";
                            $ThTelDestino.setAttribute("scope", "col");
                            $ThTelDestino.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThTelDestino);
                            $THead.appendChild($TrHead);

                            $ThContacto1.textContent = "Contacto 1";
                            $ThContacto1.setAttribute("scope", "col");
                            $ThContacto1.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThContacto1);
                            $THead.appendChild($TrHead);

                            $ThNombreDestino.textContent = "Nombre del destino";
                            $ThNombreDestino.setAttribute("scope", "col");
                            $ThNombreDestino.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThNombreDestino);
                            $THead.appendChild($TrHead);

                            $ThContacto2.textContent = "Contacto 2";
                            $ThContacto2.setAttribute("scope", "col");
                            $ThContacto2.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThContacto2);
                            $THead.appendChild($TrHead);

                            $ThObservaciones.textContent = "Observaciones";
                            $ThObservaciones.setAttribute("scope", "col");
                            $ThObservaciones.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThObservaciones);
                            $THead.appendChild($TrHead);

                            $ThHoraSalida.textContent = "Hora de salida";
                            $ThHoraSalida.setAttribute("scope", "col");
                            $ThHoraSalida.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThHoraSalida);
                            $THead.appendChild($TrHead);

                            $ThHoraRegreso.textContent = "Hora de regreso";
                            $ThHoraRegreso.setAttribute("scope", "col");
                            $ThHoraRegreso.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThHoraRegreso);
                            $THead.appendChild($TrHead);

                            $ThFechaDetallePermiso.textContent = "Fecha para el permiso";
                            $ThFechaDetallePermiso.setAttribute("scope", "col");
                            $ThFechaDetallePermiso.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThFechaDetallePermiso);
                            $THead.appendChild($TrHead);

                            $ThMotivo.textContent = "Motivo";
                            $ThMotivo.setAttribute("scope", "col");
                            $ThMotivo.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThMotivo);
                            $THead.appendChild($TrHead);

                            $ThMedidaAccion.textContent = "Medida de acción";
                            $ThMedidaAccion.setAttribute("scope", "col");
                            $ThMedidaAccion.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThMedidaAccion);
                            $THead.appendChild($TrHead);

                            $THead.appendChild($TrHead);

                            $ThHorario.textContent = "Horario en el permiso";
                            $ThHorario.setAttribute("scope", "col");
                            $ThHorario.setAttribute("nowrap", "nowrap");
                            $TrHead.appendChild($ThHorario);
                            $THead.appendChild($TrHead);

                            $TablaVistaPrevia.appendChild($THead);

                            const $TBody = document.createElement("tbody");

                            // creamos el THead para DATOS
                            const $THeadD = document.createElement("thead");
                            const $TrHeadD = document.createElement("tr");
                            const $ThFolioD = document.createElement("th");
                            const $ThNominaD = document.createElement("th");
                            const $ThNombreD = document.createElement("th");
                            const $ThSucursalD = document.createElement("th");
                            const $ThFechaPermisoD = document.createElement("th");
                            const $ThEstatusPermisoD = document.createElement("th");
                            const $ThTipoPermisoD = document.createElement("th"); // Hasta aquí son apartados generales
                            const $ThFechaInicioD = document.createElement("th"); // Empiezan los apartados de vacaciones
                            const $ThFechaFinalD = document.createElement("th");
                            const $ThFechaPresentacionD = document.createElement("th");
                            const $ThDiasAusenciaD = document.createElement("th");
                            const $ThPersonaCubriraD = document.createElement("th");
                            const $ThAsuntoD = document.createElement("th"); // Empiezan los apartados de comisión
                            const $ThDestinoD = document.createElement("th");
                            const $ThTelDestinoD = document.createElement("th");
                            const $ThContacto1D = document.createElement("th");
                            const $ThNombreDestinoD = document.createElement("th");
                            const $ThContacto2D = document.createElement("th");
                            const $ThObservacionesD = document.createElement("th");
                            const $ThHoraSalidaD = document.createElement("th");
                            const $ThHoraRegresoD = document.createElement("th");
                            const $ThFechaDetallePermisoD = document.createElement("th"); // Empiezan los apartados de permiso personal 
                            const $ThMotivoD = document.createElement("th");
                            const $ThMedidaAccionD = document.createElement("th");
                            const $ThHorarioD = document.createElement("th");

                            $ThFolioD.textContent = "Folio del permiso";
                            $ThFolioD.setAttribute("scope", "col");
                            $ThFolioD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThFolioD);
                            $THeadD.appendChild($TrHeadD);

                            $ThNominaD.textContent = "Número de nómina";
                            $ThNominaD.setAttribute("scope", "col");
                            $ThNominaD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThNominaD);
                            $THeadD.appendChild($TrHeadD);

                            $ThNombreD.textContent = "Nombre del empleado";
                            $ThNombreD.setAttribute("scope", "col");
                            $ThNombreD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThNombreD);
                            $THeadD.appendChild($TrHeadD);

                            $ThSucursalD.textContent = "Nombre de Sucursal";
                            $ThSucursalD.setAttribute("scope", "col");
                            $ThSucursalD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThSucursalD);
                            $THeadD.appendChild($TrHeadD);

                            $ThFechaPermisoD.textContent = "Fecha del permiso";
                            $ThFechaPermisoD.setAttribute("scope", "col");
                            $ThFechaPermisoD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThFechaPermisoD);
                            $THeadD.appendChild($TrHeadD);

                            $ThEstatusPermisoD.textContent = "Estatus del permiso";
                            $ThEstatusPermisoD.setAttribute("scope", "col");
                            $ThEstatusPermisoD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThEstatusPermisoD);
                            $THeadD.appendChild($TrHeadD);

                            $ThTipoPermisoD.textContent = "Tipo de permiso";
                            $ThTipoPermisoD.setAttribute("scope", "col");
                            $ThTipoPermisoD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThTipoPermisoD);
                            $THeadD.appendChild($TrHeadD);

                            $ThFechaInicioD.textContent = "Fecha de inicio de vacaciones";
                            $ThFechaInicioD.setAttribute("scope", "col");
                            $ThFechaInicioD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThFechaInicioD);
                            $THeadD.appendChild($TrHeadD);

                            $ThFechaFinalD.textContent = "Fecha culminación de vacaciones";
                            $ThFechaFinalD.setAttribute("scope", "col");
                            $ThFechaFinalD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThFechaFinalD);
                            $THeadD.appendChild($TrHeadD);

                            $ThFechaPresentacionD.textContent = "Fecha de presentación";
                            $ThFechaPresentacionD.setAttribute("scope", "col");
                            $ThFechaPresentacionD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThFechaPresentacionD);
                            $THeadD.appendChild($TrHeadD);

                            $ThDiasAusenciaD.textContent = "Días de ausencia";
                            $ThDiasAusenciaD.setAttribute("scope", "col");
                            $ThDiasAusenciaD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThDiasAusenciaD);
                            $THeadD.appendChild($TrHeadD);

                            $ThPersonaCubriraD.textContent = "Persona que cubrirá";
                            $ThPersonaCubriraD.setAttribute("scope", "col");
                            $ThPersonaCubriraD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThPersonaCubriraD);
                            $THeadD.appendChild($TrHeadD);

                            $ThAsuntoD.textContent = "Asunto";
                            $ThAsuntoD.setAttribute("scope", "col");
                            $ThAsuntoD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThAsuntoD);
                            $THeadD.appendChild($TrHeadD);

                            $ThDestinoD.textContent = "Destino";
                            $ThDestinoD.setAttribute("scope", "col");
                            $ThDestinoD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThDestinoD);
                            $THeadD.appendChild($TrHeadD);

                            $ThTelDestinoD.textContent = "Destino";
                            $ThTelDestinoD.setAttribute("scope", "col");
                            $ThTelDestinoD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThTelDestinoD);
                            $THeadD.appendChild($TrHeadD);

                            $ThContacto1D.textContent = "Contacto 1";
                            $ThContacto1D.setAttribute("scope", "col");
                            $ThContacto1D.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThContacto1D);
                            $THeadD.appendChild($TrHeadD);

                            $ThNombreDestinoD.textContent = "Nombre del destino";
                            $ThNombreDestinoD.setAttribute("scope", "col");
                            $ThNombreDestinoD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThNombreDestinoD);
                            $THeadD.appendChild($TrHeadD);

                            $ThContacto2D.textContent = "Contacto 2";
                            $ThContacto2D.setAttribute("scope", "col");
                            $ThContacto2D.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThContacto2D);
                            $THeadD.appendChild($TrHeadD);

                            $ThObservacionesD.textContent = "Observaciones";
                            $ThObservacionesD.setAttribute("scope", "col");
                            $ThObservacionesD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThObservacionesD);
                            $THeadD.appendChild($TrHeadD);

                            $ThHoraSalidaD.textContent = "Hora de salida";
                            $ThHoraSalidaD.setAttribute("scope", "col");
                            $ThHoraSalidaD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThHoraSalidaD);
                            $THeadD.appendChild($TrHeadD);

                            $ThHoraRegresoD.textContent = "Hora de regreso";
                            $ThHoraRegresoD.setAttribute("scope", "col");
                            $ThHoraRegresoD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThHoraRegresoD);
                            $THeadD.appendChild($TrHeadD);

                            $ThFechaDetallePermisoD.textContent = "Fecha para el permiso";
                            $ThFechaDetallePermisoD.setAttribute("scope", "col");
                            $ThFechaDetallePermisoD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThFechaDetallePermisoD);
                            $THeadD.appendChild($TrHeadD);

                            $ThMotivoD.textContent = "Motivo";
                            $ThMotivoD.setAttribute("scope", "col");
                            $ThMotivoD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThMotivoD);
                            $THeadD.appendChild($TrHeadD);

                            $ThMedidaAccionD.textContent = "Medida de acción";
                            $ThMedidaAccionD.setAttribute("scope", "col");
                            $ThMedidaAccionD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThMedidaAccionD);
                            $THeadD.appendChild($TrHeadD);

                            $ThHorarioD.textContent = "Horario en el permiso";
                            $ThHorarioD.setAttribute("scope", "col");
                            $ThHorarioD.setAttribute("nowrap", "nowrap");
                            $TrHeadD.appendChild($ThHorarioD);
                            $THeadD.appendChild($TrHeadD);

                            $TablaDatos.appendChild($THeadD);

                            const $TBodyD = document.createElement("tbody");

                            const nulo = "";

                            for (var i = 0; i < DatosDeReporte.incidenciasBySucursal.length; i++) {
                                // Creamos el Tr

                                const $tr = document.createElement("tr");
                                const $trD = document.createElement("tr");
                                
                                // Empezamos a crear los td generales. PAGINACIÓN --------------------------------------------------------------------------------------

                                const $tdFolio = document.createElement("td");
                                $tdFolio.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciId;
                                $tdFolio.id = "ReInciId";
                                $tdFolio.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdFolio);

                                const $tdNomina = document.createElement("td");
                                $tdNomina.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNumero;
                                $tdNomina.id = "ReInciEmpNomina";
                                $tdNomina.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdNomina);

                                const $tdNombre = document.createElement("td");
                                $tdNombre.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNombreCompleto;
                                $tdNombre.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdNombre);

                                const $tdSucursal = document.createElement("td");
                                $tdSucursal.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciSucuNombre;
                                $tdSucursal.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdSucursal);

                                const $tdFechaPermiso = document.createElement("td");
                                var FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].fecha);
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
                                $tdFechaPermiso.textContent = FShort;
                                $tdFechaPermiso.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdFechaPermiso);

                                const $tdEstatus = document.createElement("td");
                                $tdEstatus.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEstatusDescripcion;
                                $tdEstatus.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdEstatus);

                                const $tdTipoPermiso = document.createElement("td");
                                $tdTipoPermiso.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciInciDescripcion;
                                $tdTipoPermiso.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdTipoPermiso);

                                // Aquí terminan los encabezados generales

                                // Empezamos a crear los particulares de cada permiso

                                const $tdFechaInicio = document.createElement("td");
                                FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciFechaInicio);
                                DiaRecortado = FechaRecortada.getDate();
                                if (DiaRecortado < 10) {
                                    DiaRecortado = "0" + DiaRecortado;
                                }
                                MesRecortado = FechaRecortada.getMonth();
                                MesRecortado = MesRecortado + 1;
                                if (MesRecortado < 10) {
                                    MesRecortado = "0" + MesRecortado;
                                }
                                FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                if (FShort == "01-01-1") {
                                    $tdFechaInicio.textContent = nulo;
                                } else {
                                    $tdFechaInicio.textContent = FShort;
                                }
                                $tdFechaInicio.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdFechaInicio);

                                const $tdFechaFinal = document.createElement("td");
                                FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciFechaFinal);
                                DiaRecortado = FechaRecortada.getDate();
                                if (DiaRecortado < 10) {
                                    DiaRecortado = "0" + DiaRecortado;
                                }
                                MesRecortado = FechaRecortada.getMonth();
                                MesRecortado = MesRecortado + 1;
                                if (MesRecortado < 10) {
                                    MesRecortado = "0" + MesRecortado;
                                }
                                FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                if (FShort == "01-01-1") {
                                    $tdFechaFinal.textContent = nulo;
                                } else {
                                    $tdFechaFinal.textContent = FShort;
                                }
                                $tdFechaFinal.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdFechaFinal);

                                const $tdFechaPresentacion = document.createElement("td");
                                FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciFechaPresentacion);
                                DiaRecortado = FechaRecortada.getDate();
                                if (DiaRecortado < 10) {
                                    DiaRecortado = "0" + DiaRecortado;
                                }
                                MesRecortado = FechaRecortada.getMonth();
                                MesRecortado = MesRecortado + 1;
                                if (MesRecortado < 10) {
                                    MesRecortado = "0" + MesRecortado;
                                }
                                FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                if (FShort == "01-01-1") {
                                    $tdFechaPresentacion.textContent = nulo;
                                } else {
                                    $tdFechaPresentacion.textContent = FShort;
                                }
                                
                                $tdFechaPresentacion.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdFechaPresentacion);

                                const $tdDiasAusencia = document.createElement("td");
                                if (DatosDeReporte.incidenciasBySucursal[i].detInciDiasAusencia == 0) {
                                    $tdDiasAusencia.textContent = nulo;
                                } else {
                                    $tdDiasAusencia.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciDiasAusencia;
                                }
                                
                                $tdDiasAusencia.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdDiasAusencia);

                                const $tdPersonaCubrira = document.createElement("td");
                                $tdPersonaCubrira.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciPersonaCubrira;
                                $tdPersonaCubrira.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdPersonaCubrira);

                                // --------------- Creamos los apartados particulares de permiso en comisión -----------------

                                const $tdAsunto = document.createElement("td");
                                $tdAsunto.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciAsunto;
                                $tdAsunto.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdAsunto);

                                const $tdDestino = document.createElement("td");
                                $tdDestino.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciDestino;
                                $tdDestino.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdDestino);

                                const $tdTelDestino = document.createElement("td");
                                $tdTelDestino.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciTelDestino;
                                $tdTelDestino.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdTelDestino);

                                const $tdContacto1 = document.createElement("td");
                                $tdContacto1.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciContacto1;
                                $tdContacto1.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdContacto1);

                                const $tdNombreDestino = document.createElement("td");
                                $tdNombreDestino.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciNombreDestino;
                                $tdNombreDestino.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdNombreDestino);

                                const $tdContacto2 = document.createElement("td");
                                $tdContacto2.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciContacto2;
                                $tdContacto2.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdContacto2);

                                const $tdObservaciones = document.createElement("td");
                                $tdObservaciones.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciObservaciones;
                                $tdObservaciones.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdObservaciones);

                                const $tdHoraSalida = document.createElement("td");
                                var HoraObjeto = DatosDeReporte.incidenciasBySucursal[i].detInciHoraSalida;
                                var Hora = HoraObjeto.hours;
                                if (Hora < 10) {
                                    Hora = "0" + Hora;
                                }
                                var Minutos = HoraObjeto.minutes;
                                if (Minutos < 10) {
                                    Minutos = "0" + Minutos;
                                }
                                var Segundos = HoraObjeto.seconds;
                                if (Segundos < 10) {
                                    Segundos = "0" + Segundos;
                                }
                                var Formato = Hora + ":" + Minutos + ":" + Segundos;

                                if (Formato == "00:00:00") {
                                    $tdHoraSalida.textContent = nulo;
                                } else {
                                    $tdHoraSalida.textContent = Formato;
                                }

                                $tdHoraSalida.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdHoraSalida);

                                const $tdHoraRegreso = document.createElement("td");
                                HoraObjeto = DatosDeReporte.incidenciasBySucursal[i].detInciHoraRegreso;
                                Hora = HoraObjeto.hours;
                                if (Hora < 10) {
                                    Hora = "0" + Hora;
                                }
                                Minutos = HoraObjeto.minutes;
                                if (Minutos < 10) {
                                    Minutos = "0" + Minutos;
                                }
                                Segundos = HoraObjeto.seconds;
                                if (Segundos < 10) {
                                    Segundos = "0" + Segundos;
                                }
                                Formato = Hora + ":" + Minutos + ":" + Segundos;
                                if (Formato == "00:00:00") {
                                    $tdHoraRegreso.textContent = nulo;
                                } else {
                                    $tdHoraRegreso.textContent = Formato;
                                }
                                
                                $tdHoraRegreso.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdHoraRegreso);

                                // Empezamos a crear los particulares de permiso personal --------------------------------------------

                                const $tdFechaDetallePermiso = document.createElement("td");
                                FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciDetFecha);
                                DiaRecortado = FechaRecortada.getDate();
                                if (DiaRecortado < 10) {
                                    DiaRecortado = "0" + DiaRecortado;
                                }
                                MesRecortado = FechaRecortada.getMonth();
                                MesRecortado = MesRecortado + 1;
                                if (MesRecortado < 10) {
                                    MesRecortado = "0" + MesRecortado;
                                }
                                FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                if (FShort == "01-01-1") {
                                    $tdFechaDetallePermiso.textContent = nulo;
                                } else {
                                    $tdFechaDetallePermiso.textContent = FShort;
                                }
                                $tdFechaDetallePermiso.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdFechaDetallePermiso);

                                const $tdMotivo = document.createElement("td");
                                $tdMotivo.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciMotivo;
                                $tdMotivo.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdMotivo);

                                const $tdMedidaAccion = document.createElement("td");
                                $tdMedidaAccion.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciMedidaAccion;
                                $tdMedidaAccion.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdMedidaAccion);

                                const $tdHorario = document.createElement("td");
                                if (DatosDeReporte.incidenciasBySucursal[i].detInciHorarioDetalles == "De Lunes a Viernes de: 00:00:00 a: 00:00:00 y los sábados de : 00:00:00 a: 00:00:00") {
                                    $tdHorario.textContent = nulo;
                                } else {
                                    $tdHorario.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciHorarioDetalles;
                                }
                              
                                $tdHorario.setAttribute("nowrap", "nowrap");
                                $tr.appendChild($tdHorario);

                                $TBody.appendChild($tr);
                                $TablaVistaPrevia.appendChild($TBody);

                                // Empezamos a crear los td generales. DATOS --------------------------------------------------------------------------------------

                                const $tdFolioD = document.createElement("td");
                                $tdFolioD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciId;
                                $tdFolioD.id = "ReInciId";
                                $tdFolioD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdFolioD);

                                const $tdNominaD = document.createElement("td");
                                $tdNominaD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNumero;
                                $tdNominaD.id = "ReInciEmpNomina";
                                $tdNominaD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdNominaD);

                                const $tdNombreD = document.createElement("td");
                                $tdNombreD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEmpNombreCompleto;
                                $tdNombreD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdNombreD);

                                const $tdSucursalD = document.createElement("td");
                                $tdSucursalD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciSucuNombre;
                                $tdSucursalD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdSucursalD);

                                const $tdFechaPermisoD = document.createElement("td");
                                FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].fecha);
                                DiaRecortado = FechaRecortada.getDate();
                                if (DiaRecortado < 10) {
                                    DiaRecortado = "0" + DiaRecortado;
                                }
                                MesRecortado = FechaRecortada.getMonth();
                                MesRecortado = MesRecortado + 1;
                                if (MesRecortado < 10) {
                                    MesRecortado = "0" + MesRecortado;
                                }
                                FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                $tdFechaPermisoD.textContent = FShort;
                                $tdFechaPermisoD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdFechaPermisoD);

                                const $tdEstatusD = document.createElement("td");
                                $tdEstatusD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciEstatusDescripcion;
                                $tdEstatusD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdEstatusD);

                                const $tdTipoPermisoD = document.createElement("td");
                                $tdTipoPermisoD.textContent = DatosDeReporte.incidenciasBySucursal[i].reInciInciDescripcion;
                                $tdTipoPermisoD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdTipoPermisoD);

                                // Aquí terminan los encabezados generales

                                // Empezamos a crear los particulares de cada permiso

                                const $tdFechaInicioD = document.createElement("td");
                                FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciFechaInicio);
                                DiaRecortado = FechaRecortada.getDate();
                                if (DiaRecortado < 10) {
                                    DiaRecortado = "0" + DiaRecortado;
                                }
                                MesRecortado = FechaRecortada.getMonth();
                                MesRecortado = MesRecortado + 1;
                                if (MesRecortado < 10) {
                                    MesRecortado = "0" + MesRecortado;
                                }
                                FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                if (FShort == "01-01-1") {
                                    $tdFechaInicioD.textContent = nulo;
                                } else {
                                    $tdFechaInicioD.textContent = FShort;
                                }
                                $tdFechaInicioD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdFechaInicioD);

                                const $tdFechaFinalD = document.createElement("td");
                                FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciFechaFinal);
                                DiaRecortado = FechaRecortada.getDate();
                                if (DiaRecortado < 10) {
                                    DiaRecortado = "0" + DiaRecortado;
                                }
                                MesRecortado = FechaRecortada.getMonth();
                                MesRecortado = MesRecortado + 1;
                                if (MesRecortado < 10) {
                                    MesRecortado = "0" + MesRecortado;
                                }
                                FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                if (FShort == "01-01-1") {
                                    $tdFechaFinalD.textContent = nulo;
                                } else {
                                    $tdFechaFinalD.textContent = FShort;
                                }
                                $tdFechaFinalD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdFechaFinalD);

                                const $tdFechaPresentacionD = document.createElement("td");
                                FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciFechaPresentacion);
                                DiaRecortado = FechaRecortada.getDate();
                                if (DiaRecortado < 10) {
                                    DiaRecortado = "0" + DiaRecortado;
                                }
                                MesRecortado = FechaRecortada.getMonth();
                                MesRecortado = MesRecortado + 1;
                                if (MesRecortado < 10) {
                                    MesRecortado = "0" + MesRecortado;
                                }
                                FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                if (FShort == "01-01-1") {
                                    $tdFechaPresentacionD.textContent = nulo;
                                } else {
                                    $tdFechaPresentacionD.textContent = FShort;
                                }

                                $tdFechaPresentacionD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdFechaPresentacionD);

                                const $tdDiasAusenciaD = document.createElement("td");
                                if (DatosDeReporte.incidenciasBySucursal[i].detInciDiasAusencia == 0) {
                                    $tdDiasAusenciaD.textContent = nulo;
                                } else {
                                    $tdDiasAusenciaD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciDiasAusencia;
                                }

                                $tdDiasAusenciaD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdDiasAusenciaD);

                                const $tdPersonaCubriraD = document.createElement("td");
                                $tdPersonaCubriraD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciPersonaCubrira;
                                $tdPersonaCubriraD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdPersonaCubriraD);

                                // --------------- Creamos los apartados particulares de permiso en comisión -----------------

                                const $tdAsuntoD = document.createElement("td");
                                $tdAsuntoD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciAsunto;
                                $tdAsuntoD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdAsuntoD);

                                const $tdDestinoD = document.createElement("td");
                                $tdDestinoD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciDestino;
                                $tdDestinoD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdDestinoD);

                                const $tdTelDestinoD = document.createElement("td");
                                $tdTelDestinoD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciTelDestino;
                                $tdTelDestinoD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdTelDestinoD);

                                const $tdContacto1D = document.createElement("td");
                                $tdContacto1D.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciContacto1;
                                $tdContacto1D.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdContacto1D);

                                const $tdNombreDestinoD = document.createElement("td");
                                $tdNombreDestinoD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciNombreDestino;
                                $tdNombreDestinoD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdNombreDestinoD);

                                const $tdContacto2D = document.createElement("td");
                                $tdContacto2D.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciContacto2;
                                $tdContacto2D.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdContacto2D);

                                const $tdObservacionesD = document.createElement("td");
                                $tdObservacionesD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciObservaciones;
                                $tdObservacionesD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdObservacionesD);

                                const $tdHoraSalidaD = document.createElement("td");
                                HoraObjeto = DatosDeReporte.incidenciasBySucursal[i].detInciHoraSalida;
                                Hora = HoraObjeto.hours;
                                if (Hora < 10) {
                                    Hora = "0" + Hora;
                                }
                                Minutos = HoraObjeto.minutes;
                                if (Minutos < 10) {
                                    Minutos = "0" + Minutos;
                                }
                                Segundos = HoraObjeto.seconds;
                                if (Segundos < 10) {
                                    Segundos = "0" + Segundos;
                                }
                                Formato = Hora + ":" + Minutos + ":" + Segundos;

                                if (Formato == "00:00:00") {
                                    $tdHoraSalidaD.textContent = nulo;
                                } else {
                                    $tdHoraSalidaD.textContent = Formato;
                                }

                                $tdHoraSalidaD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdHoraSalidaD);

                                const $tdHoraRegresoD = document.createElement("td");
                                HoraObjeto = DatosDeReporte.incidenciasBySucursal[i].detInciHoraRegreso;
                                Hora = HoraObjeto.hours;
                                if (Hora < 10) {
                                    Hora = "0" + Hora;
                                }
                                Minutos = HoraObjeto.minutes;
                                if (Minutos < 10) {
                                    Minutos = "0" + Minutos;
                                }
                                Segundos = HoraObjeto.seconds;
                                if (Segundos < 10) {
                                    Segundos = "0" + Segundos;
                                }
                                Formato = Hora + ":" + Minutos + ":" + Segundos;
                                if (Formato == "00:00:00") {
                                    $tdHoraRegresoD.textContent = nulo;
                                } else {
                                    $tdHoraRegresoD.textContent = Formato;
                                }

                                $tdHoraRegresoD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdHoraRegresoD);

                                // Empezamos a crear los particulares de permiso personal --------------------------------------------

                                const $tdFechaDetallePermisoD = document.createElement("td");
                                FechaRecortada = new Date(DatosDeReporte.incidenciasBySucursal[i].detInciDetFecha);
                                DiaRecortado = FechaRecortada.getDate();
                                if (DiaRecortado < 10) {
                                    DiaRecortado = "0" + DiaRecortado;
                                }
                                MesRecortado = FechaRecortada.getMonth();
                                MesRecortado = MesRecortado + 1;
                                if (MesRecortado < 10) {
                                    MesRecortado = "0" + MesRecortado;
                                }
                                FShort = DiaRecortado + "-" + MesRecortado + "-" + FechaRecortada.getFullYear();
                                if (FShort == "01-01-1") {
                                    $tdFechaDetallePermisoD.textContent = nulo;
                                } else {
                                    $tdFechaDetallePermisoD.textContent = FShort;
                                }
                                $tdFechaDetallePermisoD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdFechaDetallePermisoD);


                                const $tdMotivoD = document.createElement("td");
                                $tdMotivoD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciMotivo;
                                $tdMotivoD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdMotivoD);

                                const $tdMedidaAccionD = document.createElement("td");
                                $tdMedidaAccionD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciMedidaAccion;
                                $tdMedidaAccionD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdMedidaAccionD);

                                const $tdHorarioD = document.createElement("td");
                                if (DatosDeReporte.incidenciasBySucursal[i].detInciHorarioDetalles == "De Lunes a Viernes de: 00:00:00 a: 00:00:00 y los sábados de : 00:00:00 a: 00:00:00") {
                                    $tdHorarioD.textContent = nulo;
                                } else {
                                    $tdHorarioD.textContent = DatosDeReporte.incidenciasBySucursal[i].detInciHorarioDetalles;
                                }

                                $tdHorarioD.setAttribute("nowrap", "nowrap");
                                $trD.appendChild($tdHorarioD);

                                $TBodyD.appendChild($trD);
                                $TablaDatos.appendChild($TBodyD);

                            }

                            if (BanderaTodos == true) {
                                $('#table8').DataTable().clear();
                                $('#table8').DataTable().destroy();
                            }
                            
                            $('#table8').DataTable({
                                "destroy": true,
                                "searching": true,
                                "lengthChange": false,
                                "info": false,
                                dom: 'Bfrtip',
                                buttons: [
                                    {
                                        extend: 'pdfHtml5',
                                        orientation: 'landscape',
                                        pageSize: 'LEGAL'
                                    }
                                ]
                            });

                            BanderaParaDataTable = true;
                            BanderaParaControlador = "Todos";

                            $("#TablaVacaciones").hide();
                            $("#TablaComision").hide();
                            $("#TablaPersonalSH").hide();
                            $("#PermisoPersonalCH").hide();
                            $("#Todos").show();

                            /* ---------------- Aquí termina la creación del apartado de todos los permisos de la tabla. ---------------- */
                        }
                    
                }
            },
            error: function (err) {
                console.log(err)
            }
        });
    }
    
}

function ExportarExcel() {
    var fecha = new Date(); //Fecha actual
    var mes = fecha.getMonth() + 1; //obteniendo mes
    var dia = fecha.getDate(); //obteniendo dia
    var ano = fecha.getFullYear(); //obteniendo año
    if (dia < 10)
        dia = '0' + dia; //agrega cero si el menor de 10
    if (mes < 10)
        mes = '0' + mes //agrega cero si el menor de 10
          
    var filename = "ReportePermiso" + dia + "-" + mes + "-" + ano;
    var downloadLink;
    var dataType = 'application/vnd.ms-excel';
    var tableSelect = document.getElementById('table4');
    var html = tableSelect.outerHTML.replace(/ /g, '%20'); // Tabla de datos
    var encabezadoSelect = document.getElementById('tablaencabezado');
    var html2 = encabezadoSelect.outerHTML.replace(/ /g, '%20'); // Tabla de encabezado
    
    while (html.indexOf('á') != -1) html = html.replace('á', '&aacute;');
    while (html.indexOf('Á') != -1) html = html.replace('Á', '&Aacute;');
    while (html.indexOf('é') != -1) html = html.replace('é', '&eacute;');
    while (html.indexOf('É') != -1) html = html.replace('É', '&Eacute;');
    while (html.indexOf('í') != -1) html = html.replace('í', '&iacute;');
    while (html.indexOf('Í') != -1) html = html.replace('Í', '&Iacute;');
    while (html.indexOf('ó') != -1) html = html.replace('ó', '&oacute;');
    while (html.indexOf('Ó') != -1) html = html.replace('Ó', '&Oacute;');
    while (html.indexOf('ú') != -1) html = html.replace('ú', '&uacute;');
    while (html.indexOf('Ú') != -1) html = html.replace('Ú', '&Uacute;');
    while (html.indexOf('º') != -1) html = html.replace('º', '&ordm;');
    while (html.indexOf('ñ') != -1) html = html.replace('ñ', '&ntilde;');
    while (html.indexOf('Ñ') != -1) html = html.replace('Ñ', '&Ntilde;');

    while (html2.indexOf('á') != -1) html2 = html2.replace('á', '&aacute;');
    while (html2.indexOf('Á') != -1) html2 = html2.replace('Á', '&Aacute;');
    while (html2.indexOf('é') != -1) html2 = html2.replace('é', '&eacute;');
    while (html2.indexOf('É') != -1) html2 = html2.replace('É', '&Eacute;');
    while (html2.indexOf('í') != -1) html2 = html2.replace('í', '&iacute;');
    while (html2.indexOf('Í') != -1) html2 = html2.replace('Í', '&Iacute;');
    while (html2.indexOf('ó') != -1) html2 = html2.replace('ó', '&oacute;');
    while (html2.indexOf('Ó') != -1) html2 = html2.replace('Ó', '&Oacute;');
    while (html2.indexOf('ú') != -1) html2 = html2.replace('ú', '&uacute;');
    while (html2.indexOf('Ú') != -1) html2 = html2.replace('Ú', '&Uacute;');
    while (html2.indexOf('º') != -1) html2 = html2.replace('º', '&ordm;');
    while (html2.indexOf('ñ') != -1) html2 = html2.replace('ñ', '&ntilde;');
    while (html2.indexOf('Ñ') != -1) html2 = html2.replace('Ñ', '&Ntilde;');

    // nombre de archivo
    filename = filename ? filename + '.xls' : 'excel_data.xls';

    // referencia agregada
    downloadLink = document.createElement("a");
    document.body.appendChild(downloadLink);

    if (navigator.msSaveBlob /*navigator.msSaveOrOpenBlob*/) {
        var blob = new Blob(['\ufeff', html2 + html], {
            type: dataType
        });
        window.navigator.msSaveOrOpenBlob(blob, filename);
    } else {
        // link de archivo
        downloadLink.href = 'data:' + dataType + ', ' + html2 + html;

        //el nombre archivo a link
        downloadLink.download = filename;

        //ejecutando la descarga
        downloadLink.click();
    }
    alert("Archivo de Excel descargado. El mismo se encuentra en la carpeta de descargas del equipo");
    
}


function ExportarPDF() {
    event.preventDefault();
    var Datos = global;
    var Filtro = BanderaParaControlador;
    var FechaIni = FechaInicialGlobal;
    var FechaFin = FechaFinalGlobal;
                $.ajax({
                    type: "POST",
                    url: "/ReportePdf",
                    data: { data: Datos, Bandera:Filtro, FechaPrincipio: FechaIni, FechaFinalizacion: FechaFin }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("#btnExportarPDF").prop("disabled", false); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
                        Swal.fire({
                            title: 'Generando Pdf...',
                            allowEscapeKey: true,
                            allowOutsideClick: true,
                            showConfirmButton: false,
                            onOpen: () => {
                                Swal.showLoading();
                            }
                        });
                    },
                    complete: function (NombreArchivo) {
                        swal({
                            type: 'success',
                            title: '¡Listo!.',
                            text: "Se ha guardado con éxito el Pdf en C:\\ReportesNucleoRH"
                        }).then((result) => {
                            window.open("/DescargarPdf/"+ NombreArchivo.responseText, "_blank");
                        });
                    }
                });
}

function ExportarPdf() {
    var Datos = global;
    var Filtro = BanderaParaControlador;
    var FechaInicioPdf = document.getElementById('ReporteFechaDesde').value;
    var FechaFinalPdf = document.getElementById('ReporteFechaHasta').value;
    var Objeto = {
        Registros : Datos,
        Fil: Filtro,
        FIP: FechaInicioPdf,
        FFP: FechaFinalPdf
        
    }
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "https://dental.nucleodediagnostico.mx/api",
        headers: {'Content-Type':'application/json'},
        data: JSON.stringify({ Objeto }),
        success: function (response) {


        },
        error: function (err) {
            console.log(err)
        }
    });
}


function AsignarPermisosReportes() {

    $.ajax({
        type: "GET",
        url: "/PermisosReportes/",
        success: function (data) {

            if (data.banderaPermisos == true) {
                alert("No tienes permiso para ver esta página. Redireccionando a Home");
                window.location.href = "/home";
            } else {
                

                // Validación para URPMOSTRAR
                if (data.listaPermisos[0].urpMostrar != true) {

                    window.location.href = "/home";

                }
            }
        }
    });
}