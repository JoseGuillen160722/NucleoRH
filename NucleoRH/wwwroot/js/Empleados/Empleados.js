$('#table3').DataTable({
    "searching": true,
    "lengthChange": false,
    "info": false,
});

// ------------ FUNCION AGREGAR ----------------

function AddEmpleados() {
    event.preventDefault();
    var x = $("#AddEmpleados").valid(); // ATENCIÓN AQUÍ, 
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
                var emp = {  // PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS
                    EmpNumero: $("#EmpNumero").val(),
                    EmpNombre: $("#EmpNombre").val(),
                    EmpPaterno: $("#EmpPaterno").val(),
                    EmpMaterno: $("#EmpMaterno").val(),
                    EmpTelefono: $("#EmpTelefono").val(),
                    EmpCelular: $("#EmpCelular").val(),
                    EmpEmail: $("#EmpEmail").val(),
                    EmpFechaIngreso: $("#EmpFechaIngreso").val(),
                    EmpRfc: $("#EmpRfc").val(),
                    EmpCurp: $("#EmpCurpA").val(),
                    EmpImss: $("#EmpImss").val(),
                    EmpEscoId: $("#EmpEscoId").val(),
                    EmpEdocId: $("#EmpEdocId").val(),
                    EmpSexId: $("#EmpSexId").val(),
                    EmpEstId: $("#EmpEstId").val(),
                    EmpComentarios: $("#EmpComentarios").val(),
                    EmpPatId: $("#EmpPatId").val(),
                    EmpPuestoId: $("#EmpPuestoId").val(),
                    EmpSucuId: $("#EmpSucuId").val(),
                    EmpJornaId: $("#EmpJornaId").val(),
                    EmpTrabTipoId: $("#EmpTrabTipoId").val(),
                    EmpTurId: $("#EmpTurId").val(),
                    EmpNacFecha: $("#EmpNacFecha").val(),
                    EmpNacMunicId: $("#EmpNacMunicId").val(),
                    EmpVacId: $("#EmpVacId").val(),
                    EmpUserId: $("#EmpUserId").val(),
                    EmpAlias: $("#EmpAlias").val()
                }
                $.ajax({
                    type: "POST",
                    url: "/AddEmpleados",
                    data: { empleados: emp }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("#btnAddEmpleados").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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
function DeleteEmpleados(Id) {
    event.preventDefault();
    Swal.fire({
        title: '¿Está seguro?',
        text: "¡SE BORRARÁ EL REGISTRO!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Borrar',
        cancelButtonText: 'Cancelar',
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "POST",
                url: "/DeleteEmpleados?Id=" + Id, // COMO SIEMPRE, REVISA EL NOMBRE DE TU FUNCIÓN Y DE TU VARIABLE ESTABLECIDOS EN EL CONTROLADOR Y EN EL HTML
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
        url: "/EmpleadosById/" + Id,
        success: function (response) {

            $('#Edit').modal('show');
            $("#IdE").val(response.empId);
            $('#EmpNumeroE').val(response.empNumero);
            $('#EmpNombreE').val(response.empNombre);
            $('#EmpPaternoE').val(response.empPaterno);
            $('#EmpMaternoE').val(response.empMaterno);
            $('#EmpTelefonoE').val(response.empTelefono);
            $('#EmpCelularE').val(response.empCelular);
            $('#EmpEmailE').val(response.empEmail);
            $('#EmpFechaIngresoE').val(response.empFechaIngreso);
            $('#EmpRfcE').val(response.empRfc);
            $('#EmpCurpE').val(response.empCurp);
            $('#EmpImssE').val(response.empImss);
            $('#EmpEscoIdE').val(response.empEscoId);
            $('#EmpEdocIdE').val(response.empEdocId);
            $('#EmpSexIdE').val(response.empSexId);
            $('#EmpEstIdE').val(response.empEstId);
            $('#EmpComentariosE').val(response.empComentarios);
            $('#EmpPatIdE').val(response.empPatId);
            $('#EmpPuestoIdE').val(response.empPuestoId);
            $('#EmpSucuIdE').val(response.empSucuId);
            $('#EmpJornaIdE').val(response.empJornaId);
            $('#EmpTrabTipoIdE').val(response.empTrabTipoId);
            $('#EmpTurIdE').val(response.empTurId);
            $('#EmpNacFechaE').val(response.empNacFecha);
            $('#EmpNacMunicIdE').val(response.empNacMunicId);
            $('#EmpVacIdE').val(response.empVacId);
            $('#EmpUserIdE').val(response.empUserId);
            $('#EmpAliasE').val(response.empAlias);
            $('#DomiEstaIdE').val(response.domiMunicEstaId);

            if (response.empEstId == 2) {
                $('#EmpNumeroE').attr("readonly", "readonly");
                $('#EmpNombreE').attr("readonly", "readonly");
                $('#EmpPaternoE').attr("readonly", "readonly");
                $('#EmpMaternoE').attr("readonly", "readonly");
                $('#EmpTelefonoE').attr("readonly", "readonly");
                $('#EmpCelularE').attr("readonly", "readonly");
                $('#EmpEmailE').attr("readonly", "readonly");
                $('#EmpFechaIngresoE').attr("readonly", "readonly");
                $('#EmpRfcE').attr("readonly", "readonly");
                $('#EmpCurpE').attr("readonly", "readonly");
                $('#EmpImssE').attr("readonly", "readonly");
                $('#EmpEscoIdE').attr("readonly", "readonly");
                $('#EmpEdocIdE').attr("readonly", "readonly");
                $('#EmpSexIdE').attr("readonly", "readonly");
                $('#EmpComentariosE').attr("readonly", "readonly");
                $('#EmpPatIdE').attr("readonly", "readonly");
                $('#EmpPuestoIdE').attr("readonly", "readonly");
                $('#EmpSucuIdE').attr("readonly", "readonly");
                $('#EmpJornaIdE').attr("readonly", "readonly");
                $('#EmpTrabTipoIdE').attr("readonly", "readonly");
                $('#EmpTurIdE').attr("readonly", "readonly");
                $('#EmpNacFechaE').attr("readonly", "readonly");
                $('#EmpNacMunicIdE').attr("readonly", "readonly");
                $('#EmpVacIdE').attr("readonly", "readonly");
                $('#EmpUserIdE').attr("readonly", "readonly");
                $('#EmpAliasE').attr("readonly", "readonly");
                $('#DomiEstaIdE').attr("readonly", "readonly");
            } else {
                $('#EmpNumeroE').removeAttr("readonly");
                $('#EmpNombreE').removeAttr("readonly");
                $('#EmpPaternoE').removeAttr("readonly");
                $('#EmpMaternoE').removeAttr("readonly");
                $('#EmpTelefonoE').removeAttr("readonly");
                $('#EmpCelularE').removeAttr("readonly");
                $('#EmpEmailE').removeAttr("readonly");
                $('#EmpFechaIngresoE').removeAttr("readonly");
                $('#EmpRfcE').removeAttr("readonly");
                $('#EmpCurpE').removeAttr("readonly");
                $('#EmpImssE').removeAttr("readonly");
                $('#EmpEscoIdE').removeAttr("readonly");
                $('#EmpEdocIdE').removeAttr("readonly");
                $('#EmpSexIdE').removeAttr("readonly");
                $('#EmpComentariosE').removeAttr("readonly");
                $('#EmpPatIdE').removeAttr("readonly");
                $('#EmpPuestoIdE').removeAttr("readonly");
                $('#EmpSucuIdE').removeAttr("readonly");
                $('#EmpJornaIdE').removeAttr("readonly");
                $('#EmpTrabTipoIdE').removeAttr("readonly");
                $('#EmpTurIdE').removeAttr("readonly");
                $('#EmpNacFechaE').removeAttr("readonly");
                $('#EmpNacMunicIdE').removeAttr("readonly");
                $('#EmpVacIdE').removeAttr("readonly");
                $('#EmpUserIdE').removeAttr("readonly");
                $('#EmpAliasE').removeAttr("readonly");
                $('#DomiEstaIdE').removeAttr("readonly");
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
function EditEmpleados() {
    event.preventDefault();
    var x = $("#EditEmpleados").valid(); // Edita el nombre
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
                var depa_ = { // Edita los nombres de las variables
                    EmpId: $("#IdE").val(),
                    EmpNumero: $("#EmpNumeroE").val(),
                    EmpNombre: $("#EmpNombreE").val(),
                    EmpPaterno: $("#EmpPaternoE").val(),
                    EmpMaterno: $("#EmpMaternoE").val(),
                    EmpTelefono: $("#EmpTelefonoE").val(),
                    EmpCelular: $("#EmpCelularE").val(),
                    EmpEmail: $("#EmpEmailE").val(),
                    EmpFechaIngreso: $("#EmpFechaIngresoE").val(),
                    EmpCurp: $("#EmpCurpE").val(),
                    EmpRfc: $("#EmpRfcE").val(),
                    EmpImss: $("#EmpImssE").val(),
                    EmpEscoId: $("#EmpEscoIdE").val(),
                    EmpEdocId: $("#EmpEdocIdE").val(),
                    EmpSexId: $("#EmpSexIdE").val(),
                    EmpEstId: $("#EmpEstIdE").val(),
                    EmpComentarios: $("#EmpComentariosE").val(),
                    EmpPatId: $("#EmpPatIdE").val(),
                    EmpPuestoId: $("#EmpPuestoIdE").val(),
                    EmpSucuId: $("#EmpSucuIdE").val(),
                    EmpJornaId: $("#EmpJornaIdE").val(),
                    EmpTrabTipoId: $("#EmpTrabTipoIdE").val(),
                    EmpTurId: $("#EmpTurIdE").val(),
                    EmpNacFecha: $("#EmpNacFechaE").val(),
                    EmpNacMunicId: $("#EmpNacMunicIdE").val(),
                    EmpVacId: $("#EmpVacIdE").val(),
                    EmpAlias: $("#EmpAliasE").val()
                }
                $.ajax({
                    type: "POST",
                    url: "/EditEmpleados", // Edita el nombre
                    data: { empleados: depa_ },
                    beforeSend: function () {
                        $("#btnEditEmpleados").prop("disabled", true); // Edita el nombre
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

// FUNCIÓN DE SELECT DEPENDIENTE DE OTRO SELECT ------------------------------------------------------------

$('#DomiEstaId').on('change', function () {
    $("#EmpNacMunicIdId").html("Seleccione una opción")
});
function SeleccionarEstado() {

    var Id = document.getElementById('DomiEstaId').value;
    $.ajax({
        type: "GET",
        url: "/SeleccionarEstado?Id=" + Id,
        success: function (data) {
            MunicipioEstado = data.municipioEstado; // Cuando se mande llamar el objeto se empieza a nombrar con MINÚSCULLAS, por ejemplo si tu objeto se llama MunicipioEstado, se llamará: municipioEstado
            MunicipioEstado.forEach(function (MunicipioEstado) {
                $("#EmpNacMunicId").append(
                    "<option value=" + MunicipioEstado.domiMunicId + ">" + MunicipioEstado.domiMunicDescripcion + "</option>"
                );
            })
        }
    });
}

// FUNCIÓN DE SELECT DEPENDIENTE DE OTRO SELECT ------------------------------------------------------------

$('#DomiEstaIdE').on('change', function () {
    $("#EmpNacMunicIdIdE").html("Seleccione una opción")
});
function SeleccionarEstadoE() {

    var Id = document.getElementById('DomiEstaIdE').value;
    $.ajax({
        type: "GET",
        url: "/SeleccionarEstado?Id=" + Id,
        success: function (data) {
            MunicipioEstado = data.municipioEstado;
            $("#EmpNacMunicIdE").empty();
            MunicipioEstado.forEach(function (MunicipioEstado) {
                $("#EmpNacMunicIdE").append(
                    "<option value=" + MunicipioEstado.domiMunicId + ">" + MunicipioEstado.domiMunicDescripcion + "</option>"
                );
            })
        }
    });
}

function OpenModalDetails(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/EmpleadosById/" + Id,
        success: function (response) {
            $('#Details').modal('show');
            $("#IdD").val(response.empId);
            $('#EmpNumeroD').val(response.empNumero);
            $('#EmpNombreD').val(response.empNombre);
            $('#EmpPaternoD').val(response.empPaterno);
            $('#EmpMaternoD').val(response.empMaterno);
            $('#EmpTelefonoD').val(response.empTelefono);
            $('#EmpCelularD').val(response.empCelular);
            $('#EmpEmailD').val(response.empEmail);
            $('#EmpFechaIngresoD').val(response.empFechaIngreso);
            $('#EmpRfcD').val(response.empRfc);
            $('#EmpCurpD').val(response.empCurp);
            $('#EmpImssD').val(response.empImss);
            $('#EmpEscoIdD').val(response.empEscoId);
            $('#EmpEdocIdD').val(response.empEdocId);
            $('#EmpSexIdD').val(response.empSexId);
            $('#EmpEstIdD').val(response.empEstId);
            $('#EmpComentariosD').val(response.empComentarios);
            $('#EmpPatIdD').val(response.empPatId);
            $('#EmpPuestoIdD').val(response.empPuestoId);
            $('#EmpSucuIdD').val(response.empSucuId);
            $('#EmpJornaIdD').val(response.empJornaId);
            $('#EmpTrabTipoIdD').val(response.empTrabTipoId);
            $('#EmpTurIdD').val(response.empTurId);
            $('#EmpNacFechaD').val(response.empNacFecha);
            $('#EmpNacMunicIdD').val(response.empNacMunicId);
            $('#EmpVacIdD').val(response.empVacId);
            $('#EmpAliasD').val(response.empAlias);

            // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
        },
        error: function (err) {
            console.log(err)
        }
    });
}

function OpenModalCredenciales(Id) {
    event.preventDefault();
    $.ajax({
        type: "GET",
        url: "/EmpleadosById/" + Id,
        success: function (response) {
            $('#Credenciales').modal('show');
            $("#Id").val(response.empId);
            $('#NoNomina').val(response.empNumero);
           

            // LOS NOMBRES DE LAS VARIABLES, NO LOS OLVIDES
        },
        error: function (err) {
            console.log(err)
        }
    });
}


// ------------ FUNCION AGREGAR ----------------

function AddUsuarios() {
    event.preventDefault();

    var x = $("#AddUsuario").valid(); // ATENCIÓN AQUÍ, 

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
                var usuario = {
                    UserName: $("#NoNomina").val(), // PRESTA SUMA ATENCIÓN EN LOS NOMBRES DE LAS VARIABLES, DEBEN DE SER LOS MISMOS
                    Email: $("#UserCorreo").val(),
                    PasswordHash: $("#UserPassword").val()
                }
                $.ajax({
                    type: "POST",
                    url: "/AddUsuarios",
                    data: { user: usuario }, // EL PRIMER NOMBRE ES LA VARIABLE QUE ALMACENA TODO EN EL CONTROLADOR, LA SEGUNDA ES LA QUE ESTÁ DECLARADA EN LA NOTA ANTERIOR
                    beforeSend: function () {
                        $("#btnAddUsuario").prop("disabled", true); // ES EL NOMBRE DE TU BOTÓN EN LA PARTE DE HTML, LOS NOMBRES DEBEN DE SER IGUALES
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
    AsignarPermisosEmpleados();
});

function AsignarPermisosEmpleados() {

    $.ajax({
        type: "GET",
        url: "/PermisosEmpleados/",
        success: function (data) {
            if (data.banderaPermisos == true) {
                alert("No tienes permiso para ver esta página. Redireccionando a Home");
                window.location.href = "/home";
            } else {
                // Validación para URPCREAR
                if (data.listaPermisos[0].urpCrear == true) {
                    $("#btnAgregarEmpleado").show();
                    $(".AgregarCredenciales").show();

                } else {
                    $("#btnAgregarEmpleado").hide();
                    $(".AgregarCredenciales").hide();
                }

                // Validación para URPMOSTRAR
                if (data.listaPermisos[0].urpMostrar != true) {

                    window.location.href = "/home";

                }

                // Validación para URPMODIFICAR
                if (data.listaPermisos[0].urpModificar == true) {
                    $(".EditarEmpleado").show();

                } else {
                    $(".EditarEmpleado").hide();
                }

                // Validación para URPELIMINAR
                if (data.listaPermisos[0].urpEliminar == true) {
                    $(".EliminarEmpleado").show();

                } else {
                    $(".EliminarEmpleado").hide();
                }

            }
        }
    });
}