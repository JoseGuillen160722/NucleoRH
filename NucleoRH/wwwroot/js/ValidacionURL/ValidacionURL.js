//window.onload = function () {
//    ValidarURL();
//}

//function OpenModalEdit(Id) {
//    event.preventDefault();
//    $.ajax({
//        type: "GET",
//        url: "/MostrarIncidenciaById/" + Id,
//        success: function (response) {
//            $('#Edit').modal('show');
//            $("#Id").val(response.reInciId);
//            $('#ReInciEmpIdE').val(response.idEmpleado);
//            $('#FechaE').val(response.fechaCortada);
//            $('#ReInciInciIdE').val(response.reInciInciId);
//            $('#ReInciEstatusIdE').val(response.reInciEstatusId); // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
//            // Aquí se muestran o se ocultan los DIV de las incidencias dependiendo de cuál se tenga que mostrar
            
//        },
//        error: function (err) {
//            console.log(err)
//        }
//    });


//}


//function ValidarURL(a0s9d8f7g6, Fkch23, gs54gf) {
//    const valores = window.location.search;
//    if (valores != "") {
//        const separador1 = "&";
//        const separador2 = "=";
//        var ListaValores = valores.split(separador1);
//        var IdUsuarioJunto = ListaValores[1];
//        var FolioJunto = ListaValores[2];
//        var BanderaJunta = ListaValores[3];
//        var SeparacionUsuario = IdUsuarioJunto.split(separador2);
//        var SeparacionFolio = FolioJunto.split(separador2);
//        var SeparacionBandera = BanderaJunta.split(separador2);
//        IdUsuario = SeparacionUsuario[1];
//        var Folio = SeparacionFolio[1];
//        Bandera = SeparacionBandera[1];
//        Id = Folio;
        
//        var BanderaGlobal = false;

//        $.ajax({
//            type: "GET",
//            url: "/ValidacionIncidenciaById/" + Id + "/" + IdUsuario + "/" + Bandera,
//            success: function (response) {

                  
//                if (response.redflag == false) {
//                    alert("Datos no correspondientes a la incidencia \n");
//                    window.location.href = "/home";
//                } else {
//                    if (Bandera == "3424hjlk234") { // Aceptada
//                        OpenModalEdit(Folio);
//                        /*EditRegistroIncidencias();*/
//                    } else if (Bandera == "jfnROs34") { // Rechazada
//                        OpenModalEdit(Folio);
//                    } else if (Bandera == "4RT55cgd6FOR") { // Detalles
                        
//                    }
//                }
//            },
//            error: function (err) {
//                console.log(err)
//            }

//        });
        
//    } else {
//        alert("Página inaccesible \n");
//        window.location.href = "/home";
//    }

//}