$(document).ready(function () {
    AsignarPermisos();
});

function AsignarPermisos() {
    const Menu = "Menu";
    const SubMenu = "SubMenu";
    $.ajax({
        type: "GET",
        url: "/PermisosPorAutorizacion/",
        success: function (data) {

            if (data.banderaRegistrosVacios == true) {
                $("#Menu2").hide();
                $("#Menu3").hide();
                $("#Menu4").hide();
                $("#Menu5").hide();
                $("#Menu7").hide();
            } else {
                for (var i = 0; i < data.listaValores.length; i++) {
                    var ResultadoModulo = Menu + data.listaValores[i].idMenu;
                    if (data.listaValores[i].authorizacion == true) {
                        $("#" + ResultadoModulo).show();

                    } else {
                        $("#" + ResultadoModulo).hide();
                    }
                }

                for (var i = 0; i < data.listaValores.length; i++) {
                    var ResultadoSubModulo = SubMenu + data.listaValores[i].idSubMenu;
                    if (data.listaValores[i].authorizacion == true) {
                        $("#" + ResultadoSubModulo).show();

                    } else {
                        $("#" + ResultadoSubModulo).hide();
                    }
                }
            }

            var taquitoAlPastor = 4;

            document.getElementById("EmpleadoLogeado").textContent = data.empNombreCompleto;
            document.getElementById("Nomina").textContent = data.empNumero;
            
        }
    });
}