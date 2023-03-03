function VerDetalladoCosto(e) {
    event.preventDefault();
    var id = $(e).closest('tr').attr('id');

    console.log(id)
    var concepto = id.includes('Concepto');
    if (concepto) {
        id = id.replace('Concepto','');
    } else
        id = id.replace('Gasto','');

    var NoSemana = $("#NoSemana").val();
    var idGranja = $("#idGranja").val();
    $.ajax({
        type: 'GET',
        url: 'Home/GetDetalladoCosto',
        data: { id: id, esConcepto: concepto, NoSemana: NoSemana, Granja: idGranja },
        success: function (result) {
            console.log(result);
            $("#tbDetalleCosto").empty();
            var total = 0;
            $(result).each(function () {
                total += this.costo;
                var descripcion = "";
                //Tipo 1: Requisicion, 2:SalidaAlimento, 3: Gasto Directo, 4:Requisicion servicio
                switch (this.tipo) {
                    case 1:
                        descripcion = '<a href="/Requisiciones/Detalles/' + this.uuid + '" target="_blank">Requisición Folio: ' + this.folio + '</a>';
                        break;
                    case 2:
                        descripcion = '<a href="#">Salida Alimento de Camarón (' + this.cantidadAlimento + ' kg de ' + this.alimento + ')</a>';
                        break;
                    case 3:
                        descripcion = '<a href="/GastoDirecto/Detalles?Id=' + this.uuid +'" target="_blank">Gasto Directo</a>';
                        break;
                    case 4:
                        descripcion = '<a href="#" >Requisición de Servicio</a>';
                        break;
                    default:
                } 

                var tr = '<tr><td>' + descripcion + '</td><td class="text-right">$' + FormatNumber(this.costo, 2) + '</td></tr>';
                $("#tbDetalleCosto").append(tr);
            });
            var trx = '<tr><td class="text-right">Total</td><td class="text-right">$' + FormatNumber(total, 2) + '</td></tr>';
            $("#tbDetalleCosto").append(trx);
            $("#modalDetallesCostos").modal("show");
        },
        error: function (error) {
            console.log("------ ERROR DEL SISTEMA ------");
            console.log(error);
            toastr.error("Ocurrió un error al obtener los datos.");
        }
    });
}


$(document).ready(function () {
    GetDataIicial();
});

function GetDataIicial() {
    var NoSemana = $("#NoSemana").val();
    var idGranja = $("#idGranja").val();

    $.ajax({
        type: 'GET',
        url: 'Home/GetDatosSemana',
        data: { NoSemana: NoSemana, Granja: idGranja },
        success: function (result) {
            console.log(result);
            $('#tbSemanal').empty();    

            $(result).each(function () { 

                var porcentaje = 0; 
                if (this.gastoEstimado > 0) {
                    porcentaje = this.totalReal / this.gastoEstimado * 100;
                }

                var color = "";
                if (porcentaje <= 69)
                    color = "#7EFF4D";
                else if (porcentaje > 69 && porcentaje <= 89)
                    color = "#FAFF4D";
                else if (porcentaje > 89) {
                    color = "#FD7979";
                    porcentaje -= 100;
                }
                  
                var diferencia = this.gastoEstimado - this.totalReal;
                var btn = '<a href="" data-toggle="tooltip" data-placement="top" title="Ver detalles Semana" onclick="verDetallesSemana(' + this.noSemana + ')"><i class="material-icons text-primary">remove_red_eye</i></a>';
                var tr = '<tr style="background-color:' + color + '"><td class="text-center">' + this.semana + '</td><td  class="text-right pr-10">' + nf.format(this.gastoEstimado) + '</td><td  class="text-right pr-10">' + nf.format(this.totalReal) + '</td><td class="text-right pr-10">' + nf.format(diferencia) + '</td><td>' + btn + '</td></tr>';

                $('#tbSemanal').append(tr);
            })

            //var link = '<a target="_blank" rel="noopener noreferrer" href="' + result.urlProduccion + '"><i class="material-icons text-primary">remove_red_eye</i></a>';

            //if (result.urlProduccion == "no") {
            //    var link = '<a href="#"><i class="material-icons text-danger" data-toggle="tooltip" data-placement="top" title="Sin datos para mostrar">remove_red_eye</i></a>';
            //    //DESCOMENTAR CUANDO HAYA DATOS EN DETALLE PRODUCCION
            //   /* toastr.info("No hay datos para mostrar")*/
            //}


            //$("#poblacionSuma").text(result.sumaPob.toFixed(2))
            //$("#charolaSuma").text(result.sumaCha.toFixed(2))
            //$("#hectareas").text(result.totalHectareas)

            ////POBLACIONAL
            //var sumaKgEntreHa = (result.kgVb + result.kgSp) / result.totalHectareas;
            //var sumaRendimientoKg = result.promRendimiento + sumaKgEntreHa;

            //var kgPorHectareaPob = (result.sumaPob / result.totalHectareas) + sumaKgEntreHa;
            //var costoUnidadPorKgPob = result.totalReal / (sumaRendimientoKg * result.totalHectareas);
             
            //$("#kgPorHectareaPob").text(sumaRendimientoKg.toFixed(2));
            //$("#kgPorHectareaPob").attr('title', kgPorHectareaPob.toFixed(2));
            //if (result.produccionSemana)
            //    $("#costoUnidadPorKgPob").text(nf.format(costoUnidadPorKgPob));
            //else
            //    $("#costoUnidadPorKgPob").text("No se cuenta con el registro de producción en planta");

            ////CHAROLA 
            //var sumaRendimientoKgCha = result.promRendimientoCha + sumaKgEntreHa;

            //var kgPorHectareaCha = (result.sumaCha / result.totalHectareas) + sumaKgEntreHa;
            //var costoUnidadPorKgCha = result.totalReal / (sumaRendimientoKgCha * result.totalHectareas);


            //$("#kgPorHectareaCha").text(sumaRendimientoKgCha.toFixed(2))

            //$("#costoUnidadPorKgCha").text(nf.format(costoUnidadPorKgCha))

            //$("#pesoProm").text(result.pesoProm.toFixed(4))
            //$("#pesoProy").text(result.pesoProy.toFixed(4))
            //$("#bioProm").text(result.bioProm.toFixed(4))
            //$("#bioProy").text(result.bioProy.toFixed(4))

            //if (idGranja != -1)
            //    $("#linkDetalle").html(link)
            //else
            //    $("#linkDetalle").html("")
            $("#linkDetalle").html(link)

        },
        error: function (error) {
            console.log("------ ERROR DEL SISTEMA ------");
            console.log(error);
            toastr.error("Ocurrió un error al obtener los registros de propuestas producción.");
        }
    });
}
function GetDataPropuestas() {
    event.preventDefault();
    GetDataIicial();  
}


function verDetallesSemana(NoSemana) {
    event.preventDefault();
    var idGranja = $("#idGranja").val();
    $.ajax({
        type: 'GET',
        url: 'Home/GetDatosPresupuestovsReal',
        data: { NoSemana: NoSemana, Granja: idGranja },
        success: function (result) {
            console.log(result);
            $('#tbDetalleSemana').empty();

            var realSemana = 0;
            var proySemana = 0;
            var realAcumulado = 0;
            var proyAcumulado = 0;
            var proyTemporada = 0;

            var totales = [];
            $(result).each(function () {

                var acumuladoDiferencia = this.gastoEstimadoAcumulado - this.totalRealAcumulado;
                var temporadaDiferencia = this.gastoEstimadoTemporada - this.totalRealTemporada;

                var acumuladoPorcentaje = 0;
                var temporadaPorcentaje = 0;
                var semanaTemporada = 0;


                if (this.gastoEstimadoAcumulado != 0)
                    acumuladoPorcentaje = this.totalRealAcumulado / this.gastoEstimadoAcumulado * 100;

                if (this.gastoEstimadoTemporada != 0)
                    temporadaPorcentaje = this.totalRealTemporada / this.gastoEstimadoTemporada * 100;

                if (this.gastoEstimadoTemporada != 0)
                    semanaTemporada = this.totalRealSemana / this.gastoEstimadoTemporada * 100;

                var colorAcumulado = "Green"; 
                var colorTemporada = "Green"; 

                if (this.totalRealSemana == 0) {
                    semanaTemporada = 0;
                }

                if (acumuladoDiferencia <= 0)
                    colorAcumulado = "red";

                if (temporadaDiferencia <= 0)
                    colorTemporada = "red";
                // TR FINAL COMPLETO

                var tr = '<tr><td>' + this.nombreGasto + '</td><td class="text-right">' + nf.format(this.totalRealSemana) + '</td><td class="text-right">' + nf.format(this.gastoEstimadoSemana) + '</td>' +

                    '<td  class="text-right">' + nf.format(this.totalRealAcumulado) + '</td><td class="text-right">' + nf.format(this.gastoEstimadoAcumulado) + '</td><td style="color:' + colorAcumulado + '"  class="text-right"> ' + acumuladoPorcentaje.toFixed(2) + '% </td>' +
                    '<td class="text-right">' + nf.format(this.gastoEstimadoTemporada) + '</td><td style="color:' + colorTemporada + '"  class="text-right"> ' + temporadaPorcentaje.toFixed(2) + '% </td></tr>';

                
                var tipoTotal;
                if (this.idConcepto != null) {
                    //Se modifica los conceptos
                    var row = $("#Concepto" + this.idConcepto);
                    row.find('.SemanaReal').text(nf.format(this.totalRealSemana));
                    row.find('.SemanaProyectado').text(nf.format(this.gastoEstimadoSemana));
                    row.find('.AcumuladoReal').text(nf.format(this.totalRealAcumulado));
                    row.find('.AcumuladoProyectado').text(nf.format(this.gastoEstimadoAcumulado));
                    //row.find('.AcumuladoPorcentaje').text(acumuladoPorcentaje.toFixed(2) + '%');
                    row.find('.AcumuladoPorcentaje').text(nf.format(acumuladoDiferencia));
                    row.find('.AcumuladoPorcentaje').css('color', colorAcumulado);
                    row.find('.TemporadaProyectado').text(nf.format(this.gastoEstimadoTemporada));
                    //row.find('.TemporadaPorcentaje').text(temporadaPorcentaje.toFixed(2) + '%');
                    row.find('.TemporadaPorcentaje').text(nf.format(temporadaDiferencia));
                    row.find('.TemporadaPorcentaje').css('color', colorTemporada);

                    tipoTotal = row.attr('total');

                } else {
                    //Se modifica los gastos
                    var row = $("#Gasto" + this.idGasto);
                    row.find('.SemanaReal').text(nf.format(this.totalRealSemana));
                    row.find('.SemanaProyectado').text(nf.format(this.gastoEstimadoSemana));
                    row.find('.AcumuladoReal').text(nf.format(this.totalRealAcumulado));
                    row.find('.AcumuladoProyectado').text(nf.format(this.gastoEstimadoAcumulado));
                    //row.find('.AcumuladoPorcentaje').text(acumuladoPorcentaje.toFixed(2) + '%');
                    row.find('.AcumuladoPorcentaje').text(nf.format(acumuladoDiferencia));
                    row.find('.AcumuladoPorcentaje').css('color', colorAcumulado);
                    row.find('.TemporadaProyectado').text(nf.format(this.gastoEstimadoTemporada));
                    //row.find('.TemporadaPorcentaje').text(temporadaPorcentaje.toFixed(2) + '%');
                    row.find('.TemporadaPorcentaje').text(nf.format(temporadaDiferencia));
                    row.find('.TemporadaPorcentaje').css('color', colorTemporada);

                    tipoTotal = row.attr('total'); 
                }

                console.log(tipoTotal);
                var totalActual = totales.filter(x => x.nombre == tipoTotal)[0];
                console.log(totalActual);

                if (totalActual != null) {
                    totalActual.SemanaReal += this.totalRealSemana;
                    totalActual.SemanaProyectado += this.gastoEstimadoSemana;
                    totalActual.AcumuladoReal += this.totalRealAcumulado;
                    totalActual.AcumuladoProyectado += this.gastoEstimadoAcumulado;
                    totalActual.TemporadaProyectado += this.gastoEstimadoTemporada;
                    totalActual.AcumuladoPorcentaje += acumuladoDiferencia;
                    totalActual.TemporadaPorcentaje += temporadaDiferencia;
                }
                else {
                    var objTotal = {
                        nombre: tipoTotal,
                        "SemanaReal": this.totalRealSemana,
                        "SemanaProyectado": this.gastoEstimadoSemana,
                        "AcumuladoReal": this.totalRealAcumulado,
                        "AcumuladoPorcentaje": acumuladoDiferencia,
                        "AcumuladoProyectado": this.gastoEstimadoAcumulado,
                        "TemporadaProyectado": this.gastoEstimadoTemporada,
                        "TemporadaPorcentaje": temporadaDiferencia,
                    };
                    totales.push(objTotal);
                }
              
            })

            var totalProyectado = 0;
            var totalReal = 0;
            $(totales).each(function (index, t) {
                var rowTotal = $("#" + t.nombre);

                totalProyectado += t.AcumuladoProyectado;
                totalReal += t.AcumuladoReal;

                var colorVarianzaAcumulado = "Green";
                var colorVarianzaTemporada = "Green";
                if (t.AcumuladoPorcentaje<0) {
                    colorVarianzaAcumulado = "red"
                }
                if (t.TemporadaPorcentaje < 0) {
                    colorVarianzaTemporada = "red"
                }
                rowTotal.find('.SemanaReal').text(nf.format(t.SemanaReal));
                rowTotal.find('.SemanaProyectado').text(nf.format(t.SemanaProyectado));
                rowTotal.find('.AcumuladoReal').text(nf.format(t.AcumuladoReal));
                rowTotal.find('.AcumuladoProyectado').text(nf.format(t.AcumuladoProyectado));  
                rowTotal.find('.AcumuladoPorcentaje').text(nf.format(t.AcumuladoPorcentaje));
                rowTotal.find('.AcumuladoPorcentaje').css('color', colorVarianzaAcumulado);
                rowTotal.find('.TemporadaProyectado').text(nf.format(t.TemporadaProyectado));  
                rowTotal.find('.TemporadaPorcentaje').text(nf.format(t.TemporadaPorcentaje));
                rowTotal.find('.TemporadaPorcentaje').css('color', colorVarianzaTemporada);

            }); 
            console.log("totalProyectado ====>",totalProyectado);
            console.log("totalReal ====>", totalReal);
            var titulo = $("#NoSemana option:selected").text();
            $('#tituloDetallesSemana').text('Detalles de la : ' + titulo);
            $('#detallesSemana').modal('show');
        },
        error: function (error) {
            console.log("------ ERROR DEL SISTEMA ------");
            console.log(error);
            toastr.error("Ocurrió un error al obtener los registros de propuestas producción.");
        }
    });
}












function verDetallesSemanaBAK(NoSemana) {
    event.preventDefault();
    var idGranja = $("#idGranja").val();
    $.ajax({
        type: 'GET',
        url: 'Home/GetDatosPresupuestovsReal',
        data: { NoSemana: NoSemana, Granja: idGranja },
        success: function (result) {
            console.log(result);
            $('#tbDetalleSemana').empty();

            var realSemana = 0;
            var proySemana = 0;
            var realAcumulado = 0;
            var proyAcumulado = 0;
            var proyTemporada = 0;

            $(result).each(function () {

                var acumuladoPorcentaje = 0;
                var temporadaPorcentaje = 0; 
                var semanaTemporada = 0;

                if (this.gastoEstimadoAcumulado != 0) 
                    acumuladoPorcentaje = this.totalRealAcumulado / this.gastoEstimadoAcumulado * 100;

                if (this.gastoEstimadoTemporada != 0)
                    temporadaPorcentaje = this.totalRealTemporada / this.gastoEstimadoTemporada * 100;

                if (this.gastoEstimadoTemporada != 0) 
                    semanaTemporada = this.totalRealSemana / this.gastoEstimadoTemporada * 100;

                var colorAcumulado = "";
                if (acumuladoPorcentaje <= 75)
                    colorAcumulado = "green";
                else if (acumuladoPorcentaje > 75 && acumuladoPorcentaje <= 100)
                    colorAcumulado = "orange";
                else if (acumuladoPorcentaje > 100)
                    colorAcumulado = "red";

                var colorTemporada = "";
                if (semanaTemporada <= 75)
                    colorTemporada = "green";
                else if (semanaTemporada > 75 && semanaTemporada <= 100)
                    colorTemporada = "orange";
                else if (semanaTemporada > 100)
                    colorTemporada = "red";


                if (this.totalRealSemana == 0) {
                    semanaTemporada = 0;
                }
                // TR FINAL COMPLETO

                var tr = '<tr><td>' + this.nombreGasto + '</td><td class="text-right">' + nf.format(this.totalRealSemana) + '</td><td class="text-right">' + nf.format(this.gastoEstimadoSemana) + '</td>' +

                    '<td  class="text-right">' + nf.format(this.totalRealAcumulado) + '</td><td class="text-right">' + nf.format(this.gastoEstimadoAcumulado) + '</td><td style="color:' + colorAcumulado + '"  class="text-right"> ' + acumuladoPorcentaje.toFixed(2) + '% </td><td class="text-right">' + nf.format(this.gastoEstimadoTemporada) + '</td><td style="color:' + colorTemporada + '"  class="text-right"> ' + temporadaPorcentaje.toFixed(2) + '% </td></tr>';

                // TR POR MIENTRAS
                //var tr = '<tr><td>' + this.nombreGasto + '</td>' +
                //    '<td class="text-right">' + nf.format(this.totalRealSemana) + '</td><td class="text-right">' + nf.format(this.gastoEstimadoTemporada) + '</td><td style="color:' + colorTemporada + '"  class="text-right"> ' + semanaTemporada.toFixed(2) + '% </td></tr>';

                realSemana += this.totalRealSemana;
                proySemana += this.gastoEstimadoSemana;
                realAcumulado += this.totalRealAcumulado;
                proyAcumulado += this.gastoEstimadoAcumulado;
                proyTemporada += this.gastoEstimadoTemporada;
                $('#tbDetalleSemana').append(tr);
            })


            var tf = '<tr><td></td><td class="text-right">' + nf.format(realSemana) + '</td><td class="text-right">' + nf.format(proySemana) + '</td><td class="text-right">' + nf.format(realAcumulado) + '</td><td class="text-right">' + nf.format(proyAcumulado) + '</td><td></td>' +
                '<td class="text-right">' + nf.format(proyTemporada) + '</td><td></td></tr>';
            $('#tbDetalleSemana').append(tf);

            var titulo = $("#NoSemana option:selected").text();
            $('#tituloDetallesSemana').text('Detalles de la : ' + titulo);
            $('#detallesSemana').modal('show');
        },
        error: function (error) {
            console.log("------ ERROR DEL SISTEMA ------");
            console.log(error);
            toastr.error("Ocurrió un error al obtener los registros de propuestas producción.");
        }
    });
}