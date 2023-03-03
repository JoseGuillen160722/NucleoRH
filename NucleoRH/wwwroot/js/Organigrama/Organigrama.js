function SeleccionarJerarquia() {
    var Id = document.getElementById('PuestoId').value;
    $.ajax({
        type: "GET",
        url: "/SeleccionarJerarquia?Id=" + Id,
        success: function (data) {
            var Registros = data;
            let tree;
            let params;
            if (Registros.puestoSuperior == "SIN ESPECIFICAR") {
                tree = {
                   2: {},
                };
                params = {
                    2: {
                        trad: Registros.puestoDescripcion, styles: { 'background-color': '#012639', 'color': 'white', 'z-index': '100!important' }
                    },
                };

                for (var i = 0; i < Registros.jerarquiasInferiores.length; i++) {
                    params[i + 3] = {
                        trad: Registros.jerarquiasInferiores[i].puestoDescripcion, styles: { 'background-color': '#007DC4', 'color': 'white', 'z-index': '100!important' }
                    };
                    let fader = 2;
                    let y = i + 1
                    if (y > 4 && y <= 8) {
                        fader = 3 + i - 4;
                        tree[2][fader] = {};
                        tree[2][fader][i + 3] = "";
                    } else if (y > 8 && y <= 12) {
                        fader = 3 + i - 4;
                        tree[2][3 + i - 8][fader] = {};
                        tree[2][3 + i - 8][fader][i + 3] = "";
                    } else if (y > 12 && y <= 16) {
                        fader = 3 + i - 4;
                        tree[2][3 + i - 12][3 + i - 8][fader] = {};
                        tree[2][3 + i - 12][3 + i - 8][fader][i + 3] = "";
                    } else if (y > 16 && y <= 20) {
                        fader = 3 + i - 4;
                        tree[2][3 + i - 16][3 + i - 12][3 + i - 8][fader] = {};
                        tree[2][3 + i - 16][3 + i - 12][3 + i - 8][fader][i + 3] = "";
                    } else if (y > 20 && y <= 24) {
                        fader = 3 + i - 4;
                        tree[2][3 + i - 20][3 + i - 16][3 + i - 12][3 + i - 8][fader] = {};
                        tree[2][3 + i - 20][3 + i - 16][3 + i - 12][3 + i - 8][fader][i + 3] = "";
                    } else if (y > 24 && y <= 28) {
                        fader = 3 + i - 4;
                        tree[2][3 + i - 24][3 + i - 20][3 + i - 16][3 + i - 12][3 + i - 8][fader] = {};
                        tree[2][3 + i - 24][3 + i - 20][3 + i - 16][3 + i - 12][3 + i - 8][fader][i + 3] = "";
                    } else {
                        tree[fader][i + 3] = "";
                    }
                }
            } else {
                tree = {
                    1: {
                        2: {},
                    },
                };
                params = {
                    1: { trad: Registros.puestoSuperior, styles: { 'background-color': '#c7e3ef', 'z-index': '100!important' } },
                    2: {
                        trad: Registros.puestoDescripcion, styles: { 'background-color': '#012639', 'color': 'white', 'z-index': '100!important' }
                    },
                };

                for (var i = 0; i < Registros.jerarquiasInferiores.length; i++) {
                    params[i + 3] = {
                        trad: Registros.jerarquiasInferiores[i].puestoDescripcion, styles: { 'background-color': '#007DC4', 'color': 'white', 'z-index': '100!important' }
                    };
                    let fader = 2;
                    let y = i + 1
                    if (y > 4 && y <= 8) {
                        fader = 3 + i - 4;
                        tree[1][2][fader] = {};
                        tree[1][2][fader][i + 3] = "";
                    } else if (y > 8 && y <= 12) {
                        fader = 3 + i - 4;
                        tree[1][2][3 + i - 8][fader] = {};
                        tree[1][2][3 + i - 8][fader][i + 3] = "";
                    } else if (y > 12 && y <= 16) {
                        fader = 3 + i - 4;
                        tree[1][2][3 + i - 12][3 + i - 8][fader] = {};
                        tree[1][2][3 + i - 12][3 + i - 8][fader][i + 3] = "";
                    } else if (y > 16 && y <= 20) {
                        fader = 3 + i - 4;
                        tree[1][2][3 + i - 16][3 + i - 12][3 + i - 8][fader] = {};
                        tree[1][2][3 + i - 16][3 + i - 12][3 + i - 8][fader][i + 3] = "";
                    } else if (y > 20 && y <= 24) {
                        fader = 3 + i - 4;
                        tree[1][2][3 + i - 20][3 + i - 16][3 + i - 12][3 + i - 8][fader] = {};
                        tree[1][2][3 + i - 20][3 + i - 16][3 + i - 12][3 + i - 8][fader][i + 3] = "";
                    } else if (y > 24 && y <= 28) {
                        fader = 3 + i - 4;
                        tree[1][2][3 + i - 24][3 + i - 20][3 + i - 16][3 + i - 12][3 + i - 8][fader] = {};
                        tree[1][2][3 + i - 24][3 + i - 20][3 + i - 16][3 + i - 12][3 + i - 8][fader][i + 3] = "";
                    } else {
                        tree[1][fader][i + 3] = "";
                    }
                }
            }

            treeMaker.default(tree, {
                id: 'my_tree',
                card_click: function (element) {
                    console.log(element);
                    var Puesto = element.innerText;
                    $.ajax({
                        type: "GET",
                        url: "/PuestoByName?Puesto=" + Puesto,
                        success: function (response) {
                            $("#PuestoId").val(response.puestoId);
                            SeleccionarJerarquia();
                        },
                        error: function (err) {
                            console.log(err)
                        }
                    });
                },
                treeParams: params,
                'link_width': '4px',
                'link_color': '#2199e8',
            });
        }
    });
}

//$('#AreaId').on('change', function () {
//    $("#PuestoId").html("Seleccione una opción")
//});
//function RellenarPuestos() {

//    var Id = document.getElementById('AreaId').value;
//    $.ajax({
//        type: "GET",
//        url: "/RellenarPuestos?Id=" + Id,
//        success: function (data) {
//            Puestos = data.puestos;
//            Puestos.forEach(function (Puestos) {
//                $("#PuestoId").append(
//                    "<option value=" + Puestos.puestoId + ">" + Puestos.puestoDescripcion + "</option>"
//                );
//            })
//        }
//    });
//}

$(document).ready(function () {
    AsignarPermisosOrganigrama();
});

function AsignarPermisosOrganigrama() {

    $.ajax({
        type: "GET",
        url: "/PermisosOrganigrama/",
        success: function (data) {

            if (data.banderaPermisos == true) {
                alert("No tienes permiso para ver esta página. Redireccionando a Home");
                window.location.href = "/home";
            } else {
                // Validación para URPCREAR
                if (data.listaPermisos[0].urpCrear == true) {
                    $("#PuestoId").show();

                } else {
                    $("#PuestoId").hide();
                }

                // Validación para URPMOSTRAR
                if (data.listaPermisos[0].urpMostrar != true) {

                    window.location.href = "/home";

                }

                
            }


        }
    });
}