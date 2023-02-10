$('#table3').DataTable({
    "searching": true,
    "lengthChange": false,
    "info": false,
});




// ------------ FUNCION GENERAL DE AGREGAR POR VALIDACION ----------------





function AddURP() {

   
    var urpsubmodulosCatalogos = document.querySelectorAll(".urpsubmodulos");
    var urpcrearCatalogos = document.querySelectorAll(".urpcrear");
    var urpmostrarCatalogos = document.querySelectorAll(".urpmostrar");
    var urpmodificarCatalogos = document.querySelectorAll(".urpmodificar");
    var urpeliminarCatalogos = document.querySelectorAll(".urpeliminar");

    // Cada campo es convertido a un array bidimensional
    var urpsubmodulosValoresCatalogos = [];
    for (var x = 0; x < urpsubmodulosCatalogos.length; x++) {
        urpsubmodulosValoresCatalogos.push(urpsubmodulosCatalogos[x].innerText);
    }
    var urpcrearValoresCatalogos = [];
    for (var x = 0; x < urpcrearCatalogos.length; x++) {
        urpcrearValoresCatalogos.push(urpcrearCatalogos[x].checked);
    }
    var urpmostrarValoresCatalogos = [];
    for (var x = 0; x < urpmostrarCatalogos.length; x++) {
        urpmostrarValoresCatalogos.push(urpmostrarCatalogos[x].checked);
    }
    var urpmodificarValoresCatalogos = [];
    for (var x = 0; x < urpmodificarCatalogos.length; x++) {
        urpmodificarValoresCatalogos.push(urpmodificarCatalogos[x].checked);
    }
    var urpeliminarValoresCatalogos = [];
    for (var x = 0; x < urpeliminarCatalogos.length; x++) {
        urpeliminarValoresCatalogos.push(urpeliminarCatalogos[x].checked);
    }


    
    var ArregloFinalCatalogos = []; // <---- SE MANDA A LLAMAR EN EL MÉTODO

    for (var x = 0; x < urpsubmodulosValoresCatalogos.length; x++) {
        var Arreglo = [];
        Arreglo.Id = urpsubmodulosValoresCatalogos[x];
        Arreglo.Crear = urpcrearValoresCatalogos[x];
        Arreglo.Mostrar = urpmostrarValoresCatalogos[x];
        Arreglo.Modificar = urpmodificarValoresCatalogos[x];
        Arreglo.Eliminar = urpeliminarValoresCatalogos[x];
        ArregloFinalCatalogos.push(Arreglo);
        
    }

     // console.log(ArregloFinalCatalogos) // <- Devuelve el arreglo de la tabla correspondiente a Catálogos. En el cual se toman los arreglos de los datos de la tabla y solamente se meten en un único for

    // --------------- Apartado para el submódulo de Empleados ----------------

    var urpsubmodulosEmpleados = document.querySelectorAll(".urpsubmodulosEmpleados");
    var urpcrearEmpleados = document.querySelectorAll(".urpcrearEmpleados");
    var urpmostrarEmpleados = document.querySelectorAll(".urpmostrarEmpleados");
    var urpmodificarEmpleados = document.querySelectorAll(".urpmodificarEmpleados");
    var urpeliminarEmpleados = document.querySelectorAll(".urpeliminarEmpleados");

    // Cada campo es convertido a un array bidimensional
    var urpsubmodulosValoresEmpleados = [];
    for (var x = 0; x < urpsubmodulosEmpleados.length; x++) {
        urpsubmodulosValoresEmpleados.push(urpsubmodulosEmpleados[x].innerText);
    }
    var urpcrearValoresEmpleados = [];
    for (var x = 0; x < urpcrearEmpleados.length; x++) {
        urpcrearValoresEmpleados.push(urpcrearEmpleados[x].checked);
    }
    var urpmostrarValoresEmpleados = [];
    for (var x = 0; x < urpmostrarEmpleados.length; x++) {
        urpmostrarValoresEmpleados.push(urpmostrarEmpleados[x].checked);
    }
    var urpmodificarValoresEmpleados = [];
    for (var x = 0; x < urpmodificarEmpleados.length; x++) {
        urpmodificarValoresEmpleados.push(urpmodificarEmpleados[x].checked);
    }
    var urpeliminarValoresEmpleados = [];
    for (var x = 0; x < urpeliminarEmpleados.length; x++) {
        urpeliminarValoresEmpleados.push(urpeliminarEmpleados[x].checked);
    }

    var ArregloFinalEmpleados = [];   // <---- SE MANDA A LLAMAR EN EL MÉTODO

    for (var i = 0; i < urpsubmodulosValoresEmpleados.length; i++) {
        var Arreglo = [];
        Arreglo.Id = urpsubmodulosValoresEmpleados[i];
        Arreglo.Crear = urpcrearValoresEmpleados[i];
        Arreglo.Mostrar = urpmostrarValoresEmpleados[i];
        Arreglo.Modificar = urpmodificarValoresEmpleados[i];
        Arreglo.Eliminar = urpeliminarValoresEmpleados[i];
        ArregloFinalEmpleados.push(Arreglo);
    }

   //  console.log(ArregloFinalEmpleados) // Lo mismo, pero para empleados

     // --------------- Apartado para el submódulo de Capital Humano ----------------

    var urpsubmodulosCapitalHumano = document.querySelectorAll(".urpsubmodulosCapitalHumano");
    var urpcrearCapitalHumano = document.querySelectorAll(".urpcrearCapitalHumano");
    var urpmostrarCapitalHumano = document.querySelectorAll(".urpmostrarCapitalHumano");
    var urpmodificarCapitalHumano = document.querySelectorAll(".urpmodificarCapitalHumano");
    var urpeliminarCapitalHumano = document.querySelectorAll(".urpeliminarCapitalHumano");

    // Cada campo es convertido a un array bidimensional
    var urpsubmodulosValoresCapitalHumano = [];
    for (var x = 0; x < urpsubmodulosCapitalHumano.length; x++) {
        urpsubmodulosValoresCapitalHumano.push(urpsubmodulosCapitalHumano[x].innerText); 
    }
    var urpcrearValoresCapitalHumano = [];
    for (var x = 0; x < urpcrearCapitalHumano.length; x++) {
        urpcrearValoresCapitalHumano.push(urpcrearCapitalHumano[x].checked);
    }
    var urpmostrarValoresCapitalHumano = [];
    for (var x = 0; x < urpmostrarCapitalHumano.length; x++) {
        urpmostrarValoresCapitalHumano.push(urpmostrarCapitalHumano[x].checked);
    }
    var urpmodificarValoresCapitalHumano = [];
    for (var x = 0; x < urpmodificarCapitalHumano.length; x++) {
        urpmodificarValoresCapitalHumano.push(urpmodificarCapitalHumano[x].checked);
    }
    var urpeliminarValoresCapitalHumano = [];
    for (var x = 0; x < urpeliminarCapitalHumano.length; x++) {
        urpeliminarValoresCapitalHumano.push(urpeliminarCapitalHumano[x].checked);
    }

    var ArregloFinalCapitalHumano = []; // < ----SE MANDA A LLAMAR EN EL MÉTODO

    for (var i = 0; i < urpsubmodulosValoresCapitalHumano.length; i++) {
        var Arreglo = [];
        Arreglo.Id = urpsubmodulosValoresCapitalHumano[i];
        Arreglo.Crear = urpcrearValoresCapitalHumano[i];
        Arreglo.Mostrar = urpmostrarValoresCapitalHumano[i];
        Arreglo.Modificar = urpmodificarValoresCapitalHumano[i];
        Arreglo.Eliminar = urpeliminarValoresCapitalHumano[i];
        ArregloFinalCapitalHumano.push(Arreglo);
    }

   //  console.log(ArregloFinalCapitalHumano) // <--- Ahora para Capital Humano

    // --------------- Apartado para el submódulo de Administración ----------------

    var urpsubmodulosAdmin = document.querySelectorAll(".urpsubmodulosAdmin");
    var urpcrearAdmin = document.querySelectorAll(".urpcrearAdmin");
    var urpmostrarAdmin = document.querySelectorAll(".urpmostrarAdmin");
    var urpmodificarAdmin = document.querySelectorAll(".urpmodificarAdmin");
    var urpeliminarAdmin = document.querySelectorAll(".urpeliminarAdmin");

    // Cada campo es convertido a un array bidimensional
    var urpsubmodulosValoresAdmin = [];
    for (var x = 0; x < urpsubmodulosAdmin.length; x++) {
        urpsubmodulosValoresAdmin.push(urpsubmodulosAdmin[x].innerText); 
    }
    var urpcrearValoresAdmin = [];
    for (var x = 0; x < urpcrearAdmin.length; x++) {
        urpcrearValoresAdmin.push(urpcrearAdmin[x].checked);
    }
    var urpmostrarValoresAdmin = [];
    for (var x = 0; x < urpmostrarAdmin.length; x++) {
        urpmostrarValoresAdmin.push(urpmostrarAdmin[x].checked);
    }
    var urpmodificarValoresAdmin = [];
    for (var x = 0; x < urpmodificarAdmin.length; x++) {
        urpmodificarValoresAdmin.push(urpmodificarAdmin[x].checked);
    }
    var urpeliminarValoresAdmin = [];
    for (var x = 0; x < urpeliminarAdmin.length; x++) {
        urpeliminarValoresAdmin.push(urpeliminarAdmin[x].checked);
    }

    var ArregloFinalAdmin = []; // < ----SE MANDA A LLAMAR EN EL MÉTODO

    for (var i = 0; i < urpsubmodulosValoresAdmin.length; i++) {
        var Arreglo = [];
        Arreglo.Id = urpsubmodulosValoresAdmin[i];
        Arreglo.Crear = urpcrearValoresAdmin[i];
        Arreglo.Mostrar = urpmostrarValoresAdmin[i];
        Arreglo.Modificar = urpmodificarValoresAdmin[i];
        Arreglo.Eliminar = urpeliminarValoresAdmin[i];
        ArregloFinalAdmin.push(Arreglo);
    }

   // console.log(ArregloFinalAdmin) // <--- MUESTRA CÓMO QUEDA EL ARREGLO

    // --------------- Apartado para el submódulo de Reportes ----------------

    var urpsubmodulosReportes = document.querySelectorAll(".urpsubmodulosReportes");
    var urpcrearReportes = document.querySelectorAll(".urpcrearReportes");
    var urpmostrarReportes = document.querySelectorAll(".urpmostrarReportes");
    var urpmodificarReportes = document.querySelectorAll(".urpmodificarReportes");
    var urpeliminarReportes = document.querySelectorAll(".urpeliminarReportes");

    // Cada campo es convertido a un array bidimensional
    var urpsubmodulosValoresReportes = [];
    for (var x = 0; x < urpsubmodulosReportes.length; x++) {
        urpsubmodulosValoresReportes.push(urpsubmodulosReportes[x].innerText); // inserta pero truena como mis rodillas
    }
    var urpcrearValoresReportes = [];
    for (var x = 0; x < urpcrearReportes.length; x++) {
        urpcrearValoresReportes.push(urpcrearReportes[x].checked);
    }
    var urpmostrarValoresReportes = [];
    for (var x = 0; x < urpmostrarReportes.length; x++) {
        urpmostrarValoresReportes.push(urpmostrarReportes[x].checked);
    }
    var urpmodificarValoresReportes = [];
    for (var x = 0; x < urpmodificarReportes.length; x++) {
        urpmodificarValoresReportes.push(urpmodificarReportes[x].checked);
    }
    var urpeliminarValoresReportes = [];
    for (var x = 0; x < urpeliminarReportes.length; x++) {
        urpeliminarValoresReportes.push(urpeliminarReportes[x].checked);
    }

    var ArregloFinalReportes = []; // < ----SE MANDA A LLAMAR EN EL MÉTODO

    for (var i = 0; i < urpsubmodulosValoresReportes.length; i++) {
        var Arreglo = [];
        Arreglo.Id = urpsubmodulosValoresReportes[i];
        Arreglo.Crear = urpcrearValoresReportes[i];
        Arreglo.Mostrar = urpmostrarValoresReportes[i];
        Arreglo.Modificar = urpmodificarValoresReportes[i];
        Arreglo.Eliminar = urpeliminarValoresReportes[i];
        ArregloFinalReportes.push(Arreglo);
    }

   //  console.log(ArregloFinalReportes) // <---- Muestra en consola los arreglos ordenados en una matriz

    

     var ArregloDeTablas = [].concat(ArregloFinalCatalogos, ArregloFinalEmpleados, ArregloFinalCapitalHumano, ArregloFinalAdmin, ArregloFinalReportes);


     //console.log(ArregloDeTablas)

    event.preventDefault();
    var x = $("#AddURP").valid(); // ATENCIÓN AQUÍ,

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
                var urp = {
                    URPUserId: $("#URPUserId").val(), // PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS
                    URPRoleId: $("#URPRoleId").val(),
                    URPModuloId: $("#URPModuloId").val(),
                    //URPSubModuloId: $("#URPSubModuloId").val(), // A partir de este dato, se hacen las inserciones con el array decladado arriba.
                    //URPCrear: $("#URPCrear").val(),
                    //URPModificar: $("#URPModificar").val(),
                    //URPMostrar: $("#URPMostrar").val(),
                    //URPEliminar: $("#URPEliminar").val()
                    
                }
                var Arreglo = { values: ArregloDeTablas}
                $.ajax({
                    type: "POST",
                    url: "/AddURP",
                    data: {
                        URP: urp, URPSubIdCatalogos: urpsubmodulosValoresCatalogos, URPCrearCatalogos: urpcrearValoresCatalogos, URPMostrarCatalogos: urpmostrarValoresCatalogos, URPModificarCatalogos: urpmodificarValoresCatalogos, URPEliminarCatalogos: urpmodificarValoresCatalogos, URPSubModulosEmpleados: urpsubmodulosValoresEmpleados, URPCrearEmpleados: urpcrearValoresEmpleados, URPMostrarEmpleados: urpmostrarValoresEmpleados, URPModificarEmpleados: urpmodificarValoresEmpleados, URPEliminarEmpleados: urpeliminarValoresEmpleados, URPSubModulosCH: urpsubmodulosValoresCapitalHumano, URPCrearCH: urpcrearValoresCapitalHumano, URPMostrarCH: urpmostrarValoresCapitalHumano, URPModificarCH: urpmodificarValoresCapitalHumano, URPEliminarCH: urpeliminarValoresCapitalHumano, URPSubModulosAdmin: urpsubmodulosValoresAdmin, URPCrearAdmin: urpcrearValoresAdmin, URPMostrarAdmin: urpmostrarValoresAdmin, URPModificarAdmin: urpmodificarValoresAdmin, URPEliminarAdmin: urpeliminarValoresAdmin, URPSubModulosReportes: urpsubmodulosValoresReportes, URPCrearReportes: urpcrearValoresReportes, URPMostrarReportes: urpmostrarValoresReportes, URPModificarReportes: urpmodificarValoresReportes, URPEliminarReportes: urpcrearValoresReportes }, 
                    // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("#btnAddURP").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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
        url: "/URPById/" + Id,
        success: function (response) {
            tablaPermisos = response;
            const Catalogos = tablaPermisos.filter(modulo => modulo.idModulo === 7);
            const Empleados = tablaPermisos.filter(modulo => modulo.idModulo === 5);
            const CH = tablaPermisos.filter(modulo => modulo.idModulo === 4);
            const Admin = tablaPermisos.filter(modulo => modulo.idModulo === 2);
            const Reportes = tablaPermisos.filter(modulo => modulo.idModulo === 3);
            $('#Edit').modal('show');
            $("#Id").val(response.urpId);
            $("#URPUserIdE").val(response[0].idUsuario); // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
            

            // ------------------------- Creación de tabla de catálogos para su edición -------------------------

            var $CuerpoTablaCatalogos = document.querySelector("#MiTablaCatalogos");

            if ($CuerpoTablaCatalogos.childElementCount == 0) {
                //Obtenemos todos los tr de la tabla y los almacenamos en una variable tipo arreglo 


                // recorrer todo el arreglo
                for (var i = 0; i < Catalogos.length; i++) {


                    //Creamos el tr
                    const $tr = document.createElement("tr");
                    $tr.id = "Fila";

                    // creamos el td de Id y lo adjuntamos a tr
                    var $tdId = document.createElement("td"); // El textContent del td es el Id
                    $tdId.textContent = Catalogos[i].idPermiso;
                    $tdId.id = "IdCatalogo";
                    $tdId.setAttribute("name", "IdCatalogo[]");
                    $tdId.setAttribute("style", "display:none");
                    $tdId.className = "IdCatalogo";
                    $tr.appendChild($tdId);
                    //creamos el td del id del submódulo y lo adjuntamos al tr
                    var $tdSubId = document.createElement("td");
                    $tdSubId.textContent = Catalogos[i].nombreSubModulo;
                    $tdSubId.id = "SubModuloCatalogos";
                    $tdSubId.setAttribute("name", "SubModuloCatalogos[]");
                    $tdSubId.className = "SubModuloCatalogos";
                    $tr.appendChild($tdSubId);
                    // el td del apartado de crear
                    var $tdUrpCrear = document.createElement("td");
                    $tdUrpCrear.textContent = Catalogos[i].temporalUrpCrear;
                    var $tdCrear = document.createElement("td");
                    var $CheckboxCrear = document.createElement("input");
                    $CheckboxCrear.type = "checkbox";
                    $CheckboxCrear.checked = false;
                    $CheckboxCrear.id = "CheckboxCrearCatalogo";
                    $CheckboxCrear.setAttribute("name", "CheckboxCrearCatalogo[]")
                    $CheckboxCrear.className = "CheckboxCrearCatalogo";
                    if ($tdUrpCrear.textContent == "true") {
                        $CheckboxCrear.checked = true;
                    } else {
                        $CheckboxCrear.checked = false;
                    }
                    $tdCrear.appendChild($CheckboxCrear);
                    $tr.appendChild($tdCrear);
                    // el td para el apartado de mostrar
                    var $tdUrpMostrar = document.createElement("td");
                    $tdUrpMostrar.textContent = Catalogos[i].temporalUrpMostrar;
                    var $tdMostrar = document.createElement("td");
                    var $CheckboxMostrar = document.createElement("input");
                    $CheckboxMostrar.type = "checkbox";
                    $CheckboxMostrar.checked = false;
                    $CheckboxMostrar.id = "CheckboxMostrarCatalogo";
                    $CheckboxMostrar.setAttribute("name", "CheckboxMostrarCatalogo[]");
                    $CheckboxMostrar.className = "CheckboxMostrarCatalogo";
                    if ($tdUrpMostrar.textContent == "true") {
                        $CheckboxMostrar.checked = true;
                    } else {
                        $CheckboxMostrar.checked = false;
                    }
                    $tdMostrar.appendChild($CheckboxMostrar);
                    $tr.appendChild($tdMostrar);
                    // el td para el apartado de modificar 
                    var $tdUrpModificar = document.createElement("td");
                    $tdUrpModificar.textContent = Catalogos[i].temporalUrpModificar;
                    var $tdModificar = document.createElement("td");
                    var $CheckboxModificar = document.createElement("input");
                    $CheckboxModificar.type = "checkbox";
                    $CheckboxModificar.checked = false;
                    $CheckboxModificar.setAttribute("name", "CheckboxModificarCatalogo[]");
                    $CheckboxModificar.className = "CheckboxModificarCatalogo";
                    $CheckboxModificar.id = "CheckboxModificarCatalogo";
                    if ($tdUrpModificar.textContent == "true") {
                        $CheckboxModificar.checked = true;
                    } else {
                        $CheckboxModificar.checked = false;
                    }
                    $tdModificar.appendChild($CheckboxModificar);
                    $tr.appendChild($tdModificar);
                    // el td para el apartado de eliminar
                    var $tdUrpEliminar = document.createElement("td");
                    $tdUrpEliminar.textContent = Catalogos[i].temporalUrpEliminar;
                    var $tdEliminar = document.createElement("td");
                    var $CheckboxEliminar = document.createElement("input");
                    $CheckboxEliminar.type = "checkbox";
                    $CheckboxEliminar.checked = false;
                    $CheckboxEliminar.setAttribute("name", "CheckboxEliminarCatalogo[]");
                    $CheckboxEliminar.className = "CheckboxEliminarCatalogo";
                    $CheckboxEliminar.id = "CheckboxEliminarCatalogo";
                    if ($tdUrpEliminar.textContent == "true") {
                        $CheckboxEliminar.checked = true;
                    } else {
                        $CheckboxEliminar.checked = false;
                    }
                    $tdEliminar.appendChild($CheckboxEliminar);
                    $tr.appendChild($tdEliminar);
                    // finalmente agregamos el tr al cuerpo de la tabla
                    $CuerpoTablaCatalogos.appendChild($tr);
                    // y el ciclo se repite hasta que termina de recorrer todo el arreglo


                }

                // ----------------------------- GENERACIÓN DE TABLA DE EMPLEADOS PARA SU EDICIÓN -------------------------------

                const $CuerpoTablaEmpleados = document.querySelector("#MiTablaEmpleados");
                // recorrer todo el arreglo
                for (var i = 0; i < Empleados.length; i++) {
                    //Creamos el tr
                    const $tr = document.createElement("tr");
                    // creamos el td de Id y lo adjuntamos a tr
                    var $tdId = document.createElement("td"); // El textContent del td es el Id
                    $tdId.textContent = Empleados[i].idPermiso;
                    $tdId.id = "IdEmpleados";
                    $tdId.setAttribute("name", "IdEmpleados[]");
                    $tdId.setAttribute("style", "display:none");
                    $tdId.className = "IdEmpleados";
                    $tr.appendChild($tdId);
                    //creamos el td del id del submódulo y lo adjuntamos al tr
                    var $tdSubId = document.createElement("td");
                    $tdSubId.textContent = Empleados[i].nombreSubModulo;
                    $tdSubId.id = "SubModuloEmpleados";
                    $tdSubId.setAttribute("name", "SubModuloEmpleados[]");
                    $tdSubId.className = "SubModuloEmpleados";
                    $tr.appendChild($tdSubId);
                    // el td del apartado de crear
                    var $tdUrpCrear = document.createElement("td");
                    $tdUrpCrear.textContent = Empleados[i].temporalUrpCrear;
                    var $tdCrear = document.createElement("td");
                    var $CheckboxCrear = document.createElement("input");
                    $CheckboxCrear.type = "checkbox";
                    $CheckboxCrear.checked = false;
                    $CheckboxCrear.id = "CheckboxCrearEmpleados";
                    $CheckboxCrear.setAttribute("name", "CheckboxCrearEmpleados[]");
                    $CheckboxCrear.className = "CheckboxCrearEmpleados";
                    if ($tdUrpCrear.textContent == "true") {
                        $CheckboxCrear.checked = true;
                    } else {
                        $CheckboxCrear.checked = false;
                    }
                    $tdCrear.appendChild($CheckboxCrear);
                    $tr.appendChild($tdCrear);
                    // el td para el apartado de mostrar
                    var $tdUrpMostrar = document.createElement("td");
                    $tdUrpMostrar.textContent = Empleados[i].temporalUrpMostrar;
                    var $tdMostrar = document.createElement("td");
                    var $CheckboxMostrar = document.createElement("input");
                    $CheckboxMostrar.type = "checkbox";
                    $CheckboxMostrar.checked = false;
                    $CheckboxMostrar.id = "CheckboxMostrarEmpleados";
                    $CheckboxMostrar.setAttribute("name", "CheckboxMostrarEmpleados[]");
                    $CheckboxMostrar.className = "CheckboxMostrarEmpleados";
                    if ($tdUrpMostrar.textContent == "true") {
                        $CheckboxMostrar.checked = true;
                    } else {
                        $CheckboxMostrar.checked = false;
                    }
                    $tdMostrar.appendChild($CheckboxMostrar);
                    $tr.appendChild($tdMostrar);
                    // el td para el apartado de modificar 
                    var $tdUrpModificar = document.createElement("td");
                    $tdUrpModificar.textContent = Empleados[i].temporalUrpModificar;
                    var $tdModificar = document.createElement("td");
                    var $CheckboxModificar = document.createElement("input");
                    $CheckboxModificar.type = "checkbox";
                    $CheckboxModificar.checked = false;
                    $CheckboxModificar.id = "CheckboxModificarEmpleados";
                    $CheckboxModificar.setAttribute("name", "CheckboxModificarEmpleados[]");
                    $CheckboxModificar.className = "CheckboxModificarEmpleados";
                    if ($tdUrpModificar.textContent == "true") {
                        $CheckboxModificar.checked = true;
                    } else {
                        $CheckboxModificar.checked = false;
                    }
                    $tdModificar.appendChild($CheckboxModificar);
                    $tr.appendChild($tdModificar);
                    // el td para el apartado de eliminar
                    var $tdUrpEliminar = document.createElement("td");
                    $tdUrpEliminar.textContent = Empleados[i].temporalUrpEliminar;
                    var $tdEliminar = document.createElement("td");
                    var $CheckboxEliminar = document.createElement("input");
                    $CheckboxEliminar.type = "checkbox";
                    $CheckboxEliminar.checked = false;
                    $CheckboxEliminar.id = "CheckboxEliminarEmpleados";
                    $CheckboxEliminar.setAttribute("name", "CheckboxEliminarEmpleados[]");
                    $CheckboxEliminar.className = "CheckboxEliminarEmpleados";
                    if ($tdUrpEliminar.textContent == "true") {
                        $CheckboxEliminar.checked = true;
                    } else {
                        $CheckboxEliminar.checked = false;
                    }
                    $tdEliminar.appendChild($CheckboxEliminar);
                    $tr.appendChild($tdEliminar);
                    // finalmente agregamos el tr al cuerpo de la tabla
                    $CuerpoTablaEmpleados.appendChild($tr);
                    // y el ciclo se repite hasta que termina de recorrer todo el arreglo
                }

                // ----------------------------- GENERACIÓN DE TABLA DE CAPITAL HUMANO PARA SU EDICIÓN -------------------------------

                const $CuerpoTablaCH = document.querySelector("#MiTablaCH");
                // recorrer todo el arreglo
                for (var i = 0; i < CH.length; i++) {
                    //Creamos el tr
                    const $tr = document.createElement("tr");
                    // creamos el td de Id y lo adjuntamos a tr
                    var $tdId = document.createElement("td"); // El textContent del td es el Id
                    $tdId.textContent = CH[i].idPermiso;
                    $tdId.id = "IdCH";
                    $tdId.setAttribute("name", "IdCH[]");
                    $tdId.setAttribute("style", "display:none");
                    $tdId.className = "IdCH";
                    $tr.appendChild($tdId);
                    //creamos el td del id del submódulo y lo adjuntamos al tr
                    var $tdSubId = document.createElement("td");
                    $tdSubId.textContent = Admin[i].nombreSubModulo;
                    $tdSubId.id = "SubModuloCH";
                    $tdSubId.setAttribute("name", "SubModuloCH[]");
                    $tdSubId.className = "SubModuloCH";
                    $tr.appendChild($tdSubId);
                    // el td del apartado de crear
                    var $tdUrpCrear = document.createElement("td");
                    $tdUrpCrear.textContent = CH[i].temporalUrpCrear;
                    var $tdCrear = document.createElement("td");
                    var $CheckboxCrear = document.createElement("input");
                    $CheckboxCrear.type = "checkbox";
                    $CheckboxCrear.checked = false;
                    $CheckboxCrear.id = "CheckboxCrearCH";
                    $CheckboxCrear.setAttribute("name", "CheckboxCrearCH[]");
                    $CheckboxCrear.className = "CheckboxCrearCH";
                    if ($tdUrpCrear.textContent == "true") {
                        $CheckboxCrear.checked = true;
                    } else {
                        $CheckboxCrear.checked = false;
                    }
                    $tdCrear.appendChild($CheckboxCrear);
                    $tr.appendChild($tdCrear);
                    // el td para el apartado de mostrar
                    var $tdUrpMostrar = document.createElement("td");
                    $tdUrpMostrar.textContent = CH[i].temporalUrpMostrar;
                    var $tdMostrar = document.createElement("td");
                    var $CheckboxMostrar = document.createElement("input");
                    $CheckboxMostrar.type = "checkbox";
                    $CheckboxMostrar.checked = false;
                    $CheckboxMostrar.id = "CheckboxMostrarCH";
                    $CheckboxMostrar.setAttribute("name", "CheckboxMostrarCH[]");
                    $CheckboxMostrar.className = "CheckboxMostrarCH";
                    if ($tdUrpMostrar.textContent == "true") {
                        $CheckboxMostrar.checked = true;
                    } else {
                        $CheckboxMostrar.checked = false;
                    }
                    $tdMostrar.appendChild($CheckboxMostrar);
                    $tr.appendChild($tdMostrar);
                    // el td para el apartado de modificar 
                    var $tdUrpModificar = document.createElement("td");
                    $tdUrpModificar.textContent = CH[i].temporalUrpModificar;
                    var $tdModificar = document.createElement("td");
                    var $CheckboxModificar = document.createElement("input");
                    $CheckboxModificar.type = "checkbox";
                    $CheckboxModificar.checked = false;
                    $CheckboxModificar.id = "CheckboxModificarCH";
                    $CheckboxModificar.setAttribute("name", "CheckboxModificarCH[]");
                    $CheckboxModificar.className = "CheckboxModificarCH";
                    if ($tdUrpModificar.textContent == "true") {
                        $CheckboxModificar.checked = true;
                    } else {
                        $CheckboxModificar.checked = false;
                    }
                    $tdModificar.appendChild($CheckboxModificar);
                    $tr.appendChild($tdModificar);
                    // el td para el apartado de eliminar
                    var $tdUrpEliminar = document.createElement("td");
                    $tdUrpEliminar.textContent = CH[i].temporalUrpEliminar;
                    var $tdEliminar = document.createElement("td");
                    var $CheckboxEliminar = document.createElement("input");
                    $CheckboxEliminar.type = "checkbox";
                    $CheckboxEliminar.checked = false;
                    $CheckboxEliminar.id = "CheckboxEliminarCH";
                    $CheckboxEliminar.setAttribute("name", "CheckboxEliminarCH[]");
                    $CheckboxEliminar.className = "CheckboxEliminarCH";
                    if ($tdUrpEliminar.textContent == "true") {
                        $CheckboxEliminar.checked = true;
                    } else {
                        $CheckboxEliminar.checked = false;
                    }
                    $tdEliminar.appendChild($CheckboxEliminar);
                    $tr.appendChild($tdEliminar);
                    // finalmente agregamos el tr al cuerpo de la tabla
                    $CuerpoTablaCH.appendChild($tr);
                    // y el ciclo se repite hasta que termina de recorrer todo el arreglo
                }

                // ----------------------------- GENERACIÓN DE TABLA DE ADMINISTRACIÓN PARA SU EDICIÓN -------------------------------

                const $CuerpoTablaAdmin = document.querySelector("#MiTablaAdmin");
                // recorrer todo el arreglo
                for (var i = 0; i < Admin.length; i++) {
                    //Creamos el tr
                    const $tr = document.createElement("tr");
                    // creamos el td de Id y lo adjuntamos a tr
                    var $tdId = document.createElement("td"); // El textContent del td es el Id
                    $tdId.textContent = Admin[i].idPermiso;
                    $tdId.id = "IdAdmin";
                    $tdId.setAttribute("name", "IdAdmin[]");
                    $tdId.setAttribute("style", "display:none");
                    $tdId.className = "IdAdmin";
                    $tr.appendChild($tdId);
                    //creamos el td del id del submódulo y lo adjuntamos al tr
                    var $tdSubId = document.createElement("td");
                    $tdSubId.textContent = Admin[i].nombreSubModulo;
                    $tdSubId.id = "SubModuloAdmin";
                    $tdSubId.setAttribute("name", "SubModuloAdmin[]");
                    $tdSubId.className = "SubModuloAdmin";
                    $tr.appendChild($tdSubId);
                    // el td del apartado de crear
                    var $tdUrpCrear = document.createElement("td");
                    $tdUrpCrear.textContent = Admin[i].temporalUrpCrear;
                    var $tdCrear = document.createElement("td");
                    var $CheckboxCrear = document.createElement("input");
                    $CheckboxCrear.type = "checkbox";
                    $CheckboxCrear.checked = false;
                    $CheckboxCrear.id = "CheckboxCrearAdmin";
                    $CheckboxCrear.setAttribute("name", "CheckboxCrearAdmin[]");
                    $CheckboxCrear.className = "CheckboxCrearAdmin";
                    if ($tdUrpCrear.textContent == "true") {
                        $CheckboxCrear.checked = true;
                    } else {
                        $CheckboxCrear.checked = false;
                    }
                    $tdCrear.appendChild($CheckboxCrear);
                    $tr.appendChild($tdCrear);
                    // el td para el apartado de mostrar
                    var $tdUrpMostrar = document.createElement("td");
                    $tdUrpMostrar.textContent = Admin[i].temporalUrpMostrar;
                    var $tdMostrar = document.createElement("td");
                    var $CheckboxMostrar = document.createElement("input");
                    $CheckboxMostrar.type = "checkbox";
                    $CheckboxMostrar.checked = false;
                    $CheckboxMostrar.id = "CheckboxMostrarAdmin";
                    $CheckboxMostrar.setAttribute("name", "CheckboxMostrarAdmin[]");
                    $CheckboxMostrar.className = "CheckboxMostrarAdmin";
                    if ($tdUrpMostrar.textContent == "true") {
                        $CheckboxMostrar.checked = true;
                    } else {
                        $CheckboxMostrar.checked = false;
                    }
                    $tdMostrar.appendChild($CheckboxMostrar);
                    $tr.appendChild($tdMostrar);
                    // el td para el apartado de modificar 
                    var $tdUrpModificar = document.createElement("td");
                    $tdUrpModificar.textContent = Admin[i].temporalUrpModificar;
                    var $tdModificar = document.createElement("td");
                    var $CheckboxModificar = document.createElement("input");
                    $CheckboxModificar.type = "checkbox";
                    $CheckboxModificar.checked = false;
                    $CheckboxModificar.id = "CheckboxModificarAdmin";
                    $CheckboxModificar.setAttribute("name", "CheckboxModificarAdmin[]");
                    $CheckboxModificar.className = "CheckboxModificarAdmin";
                    if ($tdUrpModificar.textContent == "true") {
                        $CheckboxModificar.checked = true;
                    } else {
                        $CheckboxModificar.checked = false;
                    }
                    $tdModificar.appendChild($CheckboxModificar);
                    $tr.appendChild($tdModificar);
                    // el td para el apartado de eliminar
                    var $tdUrpEliminar = document.createElement("td");
                    $tdUrpEliminar.textContent = Admin[i].temporalUrpEliminar;
                    var $tdEliminar = document.createElement("td");
                    var $CheckboxEliminar = document.createElement("input");
                    $CheckboxEliminar.type = "checkbox";
                    $CheckboxEliminar.checked = false;
                    $CheckboxEliminar.id = "CheckboxEliminarAdmin";
                    $CheckboxEliminar.setAttribute("name", "CheckboxEliminarAdmin[]");
                    $CheckboxEliminar.className = "CheckboxEliminarAdmin";
                    if ($tdUrpEliminar.textContent == "true") {
                        $CheckboxEliminar.checked = true;
                    } else {
                        $CheckboxEliminar.checked = false;
                    }
                    $tdEliminar.appendChild($CheckboxEliminar);
                    $tr.appendChild($tdEliminar);
                    // finalmente agregamos el tr al cuerpo de la tabla
                    $CuerpoTablaAdmin.appendChild($tr);
                    // y el ciclo se repite hasta que termina de recorrer todo el arreglo
                }

                // ----------------------------- GENERACIÓN DE TABLA DE REPORTES PARA SU EDICIÓN -------------------------------

                const $CuerpoTablaReportes = document.querySelector("#MiTablaReportes");
                // recorrer todo el arreglo
                for (var i = 0; i < Reportes.length; i++) {
                    //Creamos el tr
                    const $tr = document.createElement("tr");
                    // creamos el td de Id y lo adjuntamos a tr
                    var $tdId = document.createElement("td"); // El textContent del td es el Id
                    $tdId.textContent = Reportes[i].idPermiso;
                    $tdId.id = "IdReportes";
                    $tdId.setAttribute("name", "IdReportes[]");
                    $tdId.setAttribute("style", "display:none");
                    $tdId.className = "IdReportes";
                    $tr.appendChild($tdId);
                    //creamos el td del id del submódulo y lo adjuntamos al tr
                    var $tdSubId = document.createElement("td");
                    $tdSubId.textContent = Reportes[i].nombreSubModulo;
                    $tdSubId.id = "SubModuloReportes";
                    $tdSubId.setAttribute("name", "SubModuloReportes[]");
                    $tdSubId.className = "SubModuloReportes";
                    $tr.appendChild($tdSubId);
                    // el td del apartado de crear
                    var $tdUrpCrear = document.createElement("td");
                    $tdUrpCrear.textContent = Reportes[i].temporalUrpCrear;
                    var $tdCrear = document.createElement("td");
                    var $CheckboxCrear = document.createElement("input");
                    $CheckboxCrear.type = "checkbox";
                    $CheckboxCrear.checked = false;
                    $CheckboxCrear.id = "CheckboxCrearReportes";
                    $CheckboxCrear.setAttribute("name", "CheckboxCrearReportes[]");
                    $CheckboxCrear.className = "CheckboxCrearReportes";
                    if ($tdUrpCrear.textContent == "true") {
                        $CheckboxCrear.checked = true;
                    } else {
                        $CheckboxCrear.checked = false;
                    }
                    $tdCrear.appendChild($CheckboxCrear);
                    $tr.appendChild($tdCrear);
                    // el td para el apartado de mostrar
                    var $tdUrpMostrar = document.createElement("td");
                    $tdUrpMostrar.textContent = Reportes[i].temporalUrpMostrar;
                    var $tdMostrar = document.createElement("td");
                    var $CheckboxMostrar = document.createElement("input");
                    $CheckboxMostrar.type = "checkbox";
                    $CheckboxMostrar.checked = false;
                    $CheckboxMostrar.id = "CheckboxMostrarReportes";
                    $CheckboxMostrar.setAttribute("name", "CheckboxMostrarReportes[]");
                    $CheckboxMostrar.className = "CheckboxMostrarReportes";
                    if ($tdUrpMostrar.textContent == "true") {
                        $CheckboxMostrar.checked = true;
                    } else {
                        $CheckboxMostrar.checked = false;
                    }
                    $tdMostrar.appendChild($CheckboxMostrar);
                    $tr.appendChild($tdMostrar);
                    // el td para el apartado de modificar 
                    var $tdUrpModificar = document.createElement("td");
                    $tdUrpModificar.textContent = Reportes[i].temporalUrpModificar;
                    var $tdModificar = document.createElement("td");
                    var $CheckboxModificar = document.createElement("input");
                    $CheckboxModificar.type = "checkbox";
                    $CheckboxModificar.checked = false;
                    $CheckboxModificar.id = "CheckboxModificarReportes";
                    $CheckboxModificar.setAttribute("name", "CheckboxModificarReportes[]");
                    $CheckboxModificar.className = "CheckboxModificarReportes";
                    if ($tdUrpModificar.textContent == "true") {
                        $CheckboxModificar.checked = true;
                    } else {
                        $CheckboxModificar.checked = false;
                    }
                    $tdModificar.appendChild($CheckboxModificar);
                    $tr.appendChild($tdModificar);
                    // el td para el apartado de eliminar
                    var $tdUrpEliminar = document.createElement("td");
                    $tdUrpEliminar.textContent = Reportes[i].temporalUrpEliminar;
                    var $tdEliminar = document.createElement("td");
                    var $CheckboxEliminar = document.createElement("input");
                    $CheckboxEliminar.type = "checkbox";
                    $CheckboxEliminar.checked = false;
                    $CheckboxEliminar.id = "CheckboxEliminarReportes";
                    $CheckboxEliminar.setAttribute("name", "CheckboxEliminarReportes[]");
                    $CheckboxEliminar.className = "CheckboxEliminarReportes";
                    if ($tdUrpEliminar.textContent == "true") {
                        $CheckboxEliminar.checked = true;
                    } else {
                        $CheckboxEliminar.checked = false;
                    }
                    $tdEliminar.appendChild($CheckboxEliminar);
                    $tr.appendChild($tdEliminar);
                    // finalmente agregamos el tr al cuerpo de la tabla
                    $CuerpoTablaReportes.appendChild($tr);
                    // y el ciclo se repite hasta que termina de recorrer todo el arreglo
                }
            }
         
               
                
            

            

           

        },
        error: function (err) {
            console.log(err)
        }
    });
}



var tablaPermisos;


// ------------------------ FUNCIÓN DE EDICIÓN DE DATOS ------------------------
// NOTA: NO OLVIDES REVISAR BIEN EL NOMBRE DE TUS VARIABLES
function EditURP() {


    // VARIABLES DE CATÁLOGOS
    var IdCatalogosEdicion = document.querySelectorAll(".IdCatalogo");
    var SubModulosCatalogosEdicion = document.querySelectorAll(".SubModuloCatalogos");
    var CrearCatalogosEdicion = document.querySelectorAll(".CheckboxCrearCatalogo");
    var MostrarCatalogosEdicion = document.querySelectorAll(".CheckboxMostrarCatalogo");
    var ModificarCatalogosEdicion = document.querySelectorAll(".CheckboxModificarCatalogo");
    var EliminarCatalogosEdicion = document.querySelectorAll(".CheckboxEliminarCatalogo");

    // VARIABLES DE EMPLEADOS
    var IdEmpleadosEdicion = document.querySelectorAll(".IdEmpleados");
    var SubModulosEmpleadosEdicion = document.querySelectorAll(".SubModuloEmpleados");
    var CrearEmpleadosEdicion = document.querySelectorAll(".CheckboxCrearEmpleados");
    var MostrarEmpleadosEdicion = document.querySelectorAll(".CheckboxMostrarEmpleados");
    var ModificarEmpleadosEdicion = document.querySelectorAll(".CheckboxModificarEmpleados");
    var EliminarEmpleadosEdicion = document.querySelectorAll(".CheckboxEliminarEmpleados");

    // VARIABLES DE CAPITAL HUMANO
    var IdCHEdicion = document.querySelectorAll(".IdCH");
    var SubModulosCHEdicion = document.querySelectorAll(".SubModuloCH");
    var CrearCHEdicion = document.querySelectorAll(".CheckboxCrearCH");
    var MostrarCHEdicion = document.querySelectorAll(".CheckboxMostrarCH");
    var ModificarCHEdicion = document.querySelectorAll(".CheckboxModificarCH");
    var EliminarCHEdicion = document.querySelectorAll(".CheckboxEliminarCH");

    // VARIABLES DE ADMINISTRACIÓN
    var IdAdminEdicion = document.querySelectorAll(".IdAdmin");
    var SubModulosAdminEdicion = document.querySelectorAll(".SubModuloAdmin");
    var CrearAdminEdicion = document.querySelectorAll(".CheckboxCrearAdmin");
    var MostrarAdminEdicion = document.querySelectorAll(".CheckboxMostrarAdmin");
    var ModificarAdminEdicion = document.querySelectorAll(".CheckboxModificarAdmin");
    var EliminarAdminEdicion = document.querySelectorAll(".CheckboxEliminarAdmin");

    // VARIABLES DE REPORTES
    var IdReportesEdicion = document.querySelectorAll(".IdReportes");
    var SubModulosReportesEdicion = document.querySelectorAll(".SubModuloReportes");
    var CrearReportesEdicion = document.querySelectorAll(".CheckboxCrearReportes");
    var MostrarReportesEdicion = document.querySelectorAll(".CheckboxMostrarReportes");
    var ModificarReportesEdicion = document.querySelectorAll(".CheckboxModificarReportes");
    var EliminarReportesEdicion = document.querySelectorAll(".CheckboxEliminarReportes");

    // Cada campo es convertido a un array bidimensional QUE CORRESPONDE A CATÁLOGOS
    var IdCatalogos = [];
    for (var x = 0; x < IdCatalogosEdicion.length; x++) {
        IdCatalogos.push(IdCatalogosEdicion[x].innerText);
    }
    var SubModuloCatalogos = [];
    for (var x = 0; x < SubModulosCatalogosEdicion.length; x++) {
        SubModuloCatalogos.push(SubModulosCatalogosEdicion[x].innerText);
    }
    var CatalogosCrear = [];
    for (var x = 0; x < CrearCatalogosEdicion.length; x++) {
        CatalogosCrear.push(CrearCatalogosEdicion[x].checked);
    }
    var CatalogosMostrar = [];
    for (var x = 0; x < MostrarCatalogosEdicion.length; x++) {
        CatalogosMostrar.push(MostrarCatalogosEdicion[x].checked);
    }
    var CatalogosModificar = [];
    for (var x = 0; x < ModificarCatalogosEdicion.length; x++) {
        CatalogosModificar.push(ModificarCatalogosEdicion[x].checked);
    }
    var CatalogosEliminar = [];
    for (var x = 0; x < EliminarCatalogosEdicion.length; x++) {
        CatalogosEliminar.push(EliminarCatalogosEdicion[x].checked);
    }

    // Apartado de arreglos de EMPLEADOS

    var IdEmpleados = [];
    for (var x = 0; x < IdEmpleadosEdicion.length; x++) {
        IdEmpleados.push(IdEmpleadosEdicion[x].innerText);
    }
    var SubModuloEmpleados = [];
    for (var x = 0; x < SubModulosEmpleadosEdicion.length; x++) {
        SubModuloEmpleados.push(SubModulosEmpleadosEdicion[x].innerText);
    }
    var EmpleadosCrear = [];
    for (var x = 0; x < CrearEmpleadosEdicion.length; x++) {
        EmpleadosCrear.push(CrearEmpleadosEdicion[x].checked);
    }
    var EmpleadosMostrar = [];
    for (var x = 0; x < MostrarEmpleadosEdicion.length; x++) {
        EmpleadosMostrar.push(MostrarEmpleadosEdicion[x].checked);
    }
    var EmpleadosModificar = [];
    for (var x = 0; x < ModificarEmpleadosEdicion.length; x++) {
        EmpleadosModificar.push(ModificarEmpleadosEdicion[x].checked);
    }
    var EmpleadosEliminar = [];
    for (var x = 0; x < EliminarEmpleadosEdicion.length; x++) {
        EmpleadosEliminar.push(EliminarEmpleadosEdicion[x].checked);
    }

    // APARTADO DE ARREGLOS DE CAPITAL HUMANO

    var IdCH = [];
    for (var x = 0; x < IdCHEdicion.length; x++) {
        IdCH.push(IdCHEdicion[x].innerText);
    }
    var SubModuloCH = [];
    for (var x = 0; x < SubModulosCHEdicion.length; x++) {
        SubModuloCH.push(SubModulosCHEdicion[x].innerText);
    }
    var CHCrear = [];
    for (var x = 0; x < CrearCHEdicion.length; x++) {
        CHCrear.push(CrearCHEdicion[x].checked);
    }
    var CHMostrar = [];
    for (var x = 0; x < MostrarCHEdicion.length; x++) {
       CHMostrar.push(MostrarCHEdicion[x].checked);
    }
    var CHModificar = [];
    for (var x = 0; x < ModificarCHEdicion.length; x++) {
        CHModificar.push(ModificarCHEdicion[x].checked);
    }
    var CHEliminar = [];
    for (var x = 0; x < EliminarCHEdicion.length; x++) {
        CHEliminar.push(EliminarCHEdicion[x].checked);
    }

    // APARTADO DE ARREGLOS DE ADMINISTRACION

    var IdAdmin = [];
    for (var x = 0; x < IdAdminEdicion.length; x++) {
        IdAdmin.push(IdAdminEdicion[x].innerText);
    }
    var SubModuloAdmin = [];
    for (var x = 0; x < SubModulosAdminEdicion.length; x++) {
        SubModuloAdmin.push(SubModulosAdminEdicion[x].innerText);
    }
    var AdminCrear = [];
    for (var x = 0; x < CrearAdminEdicion.length; x++) {
        AdminCrear.push(CrearAdminEdicion[x].checked);
    }
    var AdminMostrar = [];
    for (var x = 0; x < MostrarAdminEdicion.length; x++) {
        AdminMostrar.push(MostrarAdminEdicion[x].checked);
    }
    var AdminModificar = [];
    for (var x = 0; x < ModificarAdminEdicion.length; x++) {
        AdminModificar.push(ModificarAdminEdicion[x].checked);
    }
    var AdminEliminar = [];
    for (var x = 0; x < EliminarAdminEdicion.length; x++) {
        AdminEliminar.push(EliminarAdminEdicion[x].checked);
    }

    // APARTADO DE ARREGLOS DE REPORTES

    var IdReportes = [];
    for (var x = 0; x < IdReportesEdicion.length; x++) {
        IdReportes.push(IdReportesEdicion[x].innerText);
    }
    var SubModuloReportes = [];
    for (var x = 0; x < SubModulosReportesEdicion.length; x++) {
        SubModuloReportes.push(SubModulosReportesEdicion[x].innerText);
    }
    var ReportesCrear = [];
    for (var x = 0; x < CrearReportesEdicion.length; x++) {
        ReportesCrear.push(CrearReportesEdicion[x].checked);
    }
    var ReportesMostrar = [];
    for (var x = 0; x < MostrarReportesEdicion.length; x++) {
        ReportesMostrar.push(MostrarReportesEdicion[x].checked);
    }
    var ReportesModificar = [];
    for (var x = 0; x < ModificarReportesEdicion.length; x++) {
        ReportesModificar.push(ModificarReportesEdicion[x].checked);
    }
    var ReportesEliminar = [];
    for (var x = 0; x < EliminarReportesEdicion.length; x++) {
        ReportesEliminar.push(EliminarReportesEdicion[x].checked);
    }

    event.preventDefault();
   

    var x = $("#EditURP").valid(); // Edita el nombre
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
                var urp = { // Edita los nombres de las variables
                   /* URPId : $("#Id").val(),*/
                    URPUserId: $("#URPUserIdE").val(), // PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS
                    URPRoleId: $("#URPRoleIdE").val(),
                    //URPModuloId: $("#URPModuloIdE").val(),
                    //URPSubModuloId: $("#URPSubModuloIdE").val(), // A partir de este dato, se hacen las inserciones con el array decladado arriba.
                    //URPCrear: $("#URPCrearE").val(),
                    //URPModificar: $("#URPModificarE").val(),
                    //URPMostrar: $("#URPMostrarE").val(),
                    //URPEliminar: $("#URPEliminarE").val()
                }
                $.ajax({
                    type: "POST",
                    url: "/EditURP", // Edita el nombre
                    data: { URP: urp, IdCat: IdCatalogos, SubMCat: SubModuloCatalogos, CatCrear: CatalogosCrear, CatMostrar: CatalogosMostrar, CatModificar: CatalogosModificar, CatEliminar: CatalogosEliminar, IdEmp: IdEmpleados, SubMEmp: SubModuloEmpleados, EmpCrear: EmpleadosCrear, EmpMostrar: EmpleadosMostrar, EmpModificar: EmpleadosModificar, EmpEliminar: EmpleadosEliminar, IdCapH: IdCH, SubMCapH: SubModuloCH, CapHCrear: CHCrear, CapHMostrar: CHMostrar, CapHModificar: CHModificar, CapHEliminar: CHEliminar, AdminId: IdAdmin, AdminSubModulo: SubModuloAdmin, CrearAdmin: AdminCrear, MostrarAdmin: AdminMostrar, ModificarAdmin: AdminModificar, EliminarAdmin: AdminEliminar, ReportesId: IdReportes, ReportesSubModulos: SubModuloReportes, RepCrear: ReportesCrear, RepMostrar: ReportesMostrar, RepModificar: ReportesModificar, RepEliminar: ReportesEliminar },
                    beforeSend: function () {
                        $("#btnEditURP").prop("disabled", true); // Edita el nombre
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

// SELECCIONAR TODO CATALOGOS

$(document).ready(function () {
    selected = true;
    $('#SeleccionarTodoCatalogos').val(this.checked);
    $('#SeleccionarTodoCatalogos').change(function () {
        if (this.checked) {
            $('#mi-tabla input[type=checkbox]').prop("checked", true);
            $('#SeleccionarTodoCatalogos').val('Deseleccionar');
        } else {
            $('#mi-tabla input[type=checkbox]').prop("checked", false);
            $('#SeleccionarTodoCatalogos').val('Seleccionar');
        }
        selected = !selected;
    });
});

// SELECCIONAR TODO EMPLEADOS

$(document).ready(function () {
    selected = true;
    $('#SeleccionarTodoEmpleados').val(this.checked);
    $('#SeleccionarTodoEmpleados').change(function () {
        if (this.checked) {
            $('#mi-tablaEmpleados input[type=checkbox]').prop("checked", true);
            $('#SeleccionarTodoEmpleados').val('Deseleccionar');
        } else {
            $('#mi-tablaEmpleados input[type=checkbox]').prop("checked", false);
            $('#SeleccionarTodoEmpleados').val('Seleccionar');
        }
        selected = !selected;
    });
});

// SELECCIONAR TODO CAPITAL HUMANO

$(document).ready(function () {
    selected = true;
    $('#SeleccionarTodoCH').val(this.checked);
    $('#SeleccionarTodoCH').change(function () {
        if (this.checked) {
            $('#mi-tablaCH input[type=checkbox]').prop("checked", true);
            $('#SeleccionarTodoCH').val('Deseleccionar');
        } else {
            $('#mi-tablaCH input[type=checkbox]').prop("checked", false);
            $('#SeleccionarTodoCH').val('Seleccionar');
        }
        selected = !selected;
    });
});

// SELECCIONAR TODO ADMIN

$(document).ready(function () {
    selected = true;
    $('#SeleccionarTodoAdmin').val(this.checked);
    $('#SeleccionarTodoAdmin').change(function () {
        if (this.checked) {
            $('#mi-tablaAdmin input[type=checkbox]').prop("checked", true);
            $('#SeleccionarTodoAdmin').val('Deseleccionar');
        } else {
            $('#mi-tablaAdmin input[type=checkbox]').prop("checked", false);
            $('#SeleccionarTodoAdmin').val('Seleccionar');
        }
        selected = !selected;
    });
});

// SELECCIONAR TODO REPORTES

$(document).ready(function () {
    selected = true;
    $('#SeleccionarTodoReportes').val(this.checked);
    $('#SeleccionarTodoReportes').change(function () {
        if (this.checked) {
            $('#mi-tablaReportes input[type=checkbox]').prop("checked", true);
            $('#SeleccionarTodoReportes').val('Deseleccionar');
        } else {
            $('#mi-tablaReportes input[type=checkbox]').prop("checked", false);
            $('#SeleccionarTodoReportes').val('Seleccionar');
        }
        selected = !selected;
    });
});


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
    AsignarPermisosPermisos();
});

function AsignarPermisosPermisos() {

    $.ajax({
        type: "GET",
        url: "/PermisosPermisos/",
        success: function (data) {
            if (data.banderaPermisos == true) {
                alert("No tienes permiso para ver esta página. Redireccionando a Home");
                window.location.href = "/home";
            } else {
                // Validación para URPCREAR
                if (data.listaPermisos[0].urpCrear == true) {
                    $("#btnAgregarPermisos").show();

                } else {
                    $("#btnAgregarPermisos").hide();
                }

                // Validación para URPMOSTRAR
                if (data.listaPermisos[0].urpMostrar != true) {

                    window.location.href = "/home";

                }

                // Validación para URPMODIFICAR
                if (data.listaPermisos[0].urpModificar == true) {
                    $(".EditarPermisos").show();

                } else {
                    $(".EditarPermisos").hide();
                }
            }
            

            
        }
    });
}