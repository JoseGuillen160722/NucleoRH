using System;
using System.Collections.Generic;

namespace NucleoRH.Models
{
    public class CatDetalleIncidencias
    {
        public int DetInciId { get; set; }
        public int DetInciReInciId { get; set; }
        public DateTime DetFecha { get; set; }
        public int DetInciHorarioId { get; set; }
        public string MedidaAccion { get; set; }
        public string Motivo { get; set; }
        public string Asunto { get; set; }
        public string Destino { get; set; }
        public string TelDestino { get; set; }
        public string Contacto1 { get; set; }
        public string NombreDestino { get; set; }
        public string Contacto2 { get; set; }
        public string Observaciones { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public TimeSpan HoraRegreso { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public DateTime FechaPresentacion { get; set; }
        public int DiasAusencia { get; set; }
        public string PersonaCubrira { get; set; }
        public DateTime? DetInciFechaFinPermisoPersonal { get; set; }
        public TimeSpan? DetInciHoraIngreso { get; set; }
        public TimeSpan? DetInciHoraSalida { get; set; }
        public TimeSpan? DetInciHoraSalidaComida { get; set; }
        public TimeSpan? DetInciHoraRegresoComida { get; set; }

        public virtual CatRegistroIncidencias ReInci { get; set; }
        public virtual CatHorarios Hora { get; set; }

    }
}
