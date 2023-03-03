using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace NucleoRH.Models
{
    public partial class Nucleo_RHContext : IdentityDbContext
    {
        public Nucleo_RHContext()
        {
        }

        public Nucleo_RHContext(DbContextOptions<Nucleo_RHContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CatAreas> CatAreas { get; set; }
        public virtual DbSet<CatChecadores> CatChecadores { get; set; }
        public virtual DbSet<CatDepartamentos> CatDepartamentos { get; set; }
        public virtual DbSet<CatDomiciliosColonias> CatDomiciliosColonias { get; set; }
        public virtual DbSet<CatDomiciliosEstados> CatDomiciliosEstados { get; set; }
        public virtual DbSet<CatDomiciliosMunicipios> CatDomiciliosMunicipios { get; set; }
        public virtual DbSet<CatEmpleados> CatEmpleados { get; set; }
        public virtual DbSet<CatEscolaridades> CatEscolaridades { get; set; }
        public virtual DbSet<CatEstadosCiviles> CatEstadosCiviles { get; set; }
        public virtual DbSet<CatEstatus> CatEstatus { get; set; }
        public virtual DbSet<CatFestivos> CatFestivos { get; set; }
        public virtual DbSet<CatHorarios> CatHorarios { get; set; }
        public virtual DbSet<CatIncidencias> CatIncidencias { get; set; }
        public virtual DbSet<CatJornadasLaborales> CatJornadasLaborales { get; set; }
        public virtual DbSet<CatPatrones> CatPatrones { get; set; }
        public virtual DbSet<CatPeriodos> CatPeriodos { get; set; }
        public virtual DbSet<CatPlantillas> CatPlantillas { get; set; }
        public virtual DbSet<CatPuestos> CatPuestos { get; set; }
        public virtual DbSet<CatSexos> CatSexos { get; set; }
        public virtual DbSet<CatSucursales> CatSucursales { get; set; }
        public virtual DbSet<CatTrabajadorTipos> CatTrabajadorTipos { get; set; }
        public virtual DbSet<CatTurnosLaborales> CatTurnosLaborales { get; set; }
        public virtual DbSet<CatVacunacion> CatVacunacion { get; set; }
        public virtual DbSet<CfgParametrosSistema> CfgParametrosSistema { get; set; }
        public virtual DbSet<MovEmpleadosAsistencias> MovEmpleadosAsistencias { get; set; }
        public virtual DbSet<MovEmpleadosDomicilios> MovEmpleadosDomicilios { get; set; }
        public virtual DbSet<MovEmpleadosIncidencias> MovEmpleadosIncidencias { get; set; }
        public virtual DbSet<MovEmpleadosSueldos> MovEmpleadosSueldos { get; set; }
        public virtual DbSet<CatUsuariosPermisos> CatUsuariosPermisos { get; set; }
        public virtual DbSet<CatModulos>  CatModulos { get; set; }
        public virtual DbSet<CatRegistroIncidencias> CatRegistroIncidencias { get; set; }
        public virtual DbSet<CatDetalleIncidencias> CatDetalleIncidencias { get; set; }
        public virtual DbSet<CatAntiguedad> CatAntiguedad { get; set; }
        public virtual DbSet<CatHistorialVacaciones> CatHistorialVacaciones { get; set; }
        public virtual DbSet<CatSaldoDeVacaciones> CatSaldoDeVacaciones { get; set; }
        public virtual DbSet<CatSubModulos> CatSubModulos { get; set; }
        public virtual DbSet<CatFlujos> CatFlujos { get; set; }
        public virtual DbSet<CatSolicitudIncidencias> CatSolicitudIncidencias { get; set; }
        public virtual DbSet<CatDetalleFlujo> CatDetalleFlujo { get; set; }
        public virtual DbSet<CatBitacoraIncidencias> CatBitacoraIncidencias { get; set; }
        public virtual DbSet<MovEmpleadosHorarios> MovEmpleadosHorarios { get; set; }
        public virtual DbSet<MovRequisicionPersonal> MovRequisicionPersonal { get; set; }
        public virtual DbSet<CatBitacoraRequisicionPersonal> CatBitacoraRequisicionPersonal { get; set; }
        public virtual DbSet<CatEmpTrazabilidad> CatEmpTrazabilidad { get; set; }
        public virtual DbSet<PuestosSP> PuestosSP { get; set; }
        public virtual DbSet<MovPermisosPorTiempo> MovPermisosPorTiempo { get; set; }



        // -------------

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=SRVNDBD4\\SQLNDBDSIAN;Initial Catalog=Nucleo_RH;Persist Security Info=True;User Id=SIAN;Password=S14n2017");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CatAreas>(entity =>
            {
                entity.HasKey(e => e.AreaId);

                entity.ToTable("cat_Areas");

                entity.Property(e => e.AreaId).HasColumnName("areaID");

                entity.Property(e => e.AreaDepaId).HasColumnName("areaDepaID");

                entity.Property(e => e.AreaDescripcion)
                    .HasColumnName("areaDescripcion")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.AreaDepa)
                    .WithMany(p => p.CatAreas)
                    .HasForeignKey(d => d.AreaDepaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cat_Areas_cat_Departamentos");
            });

            modelBuilder.Entity<CatChecadores>(entity =>
            {
                entity.HasKey(e => e.ChecId);

                entity.ToTable("cat_Checadores");

                entity.Property(e => e.ChecId).HasColumnName("checID");

                entity.Property(e => e.ChecDescripcion)
                    .HasColumnName("checDescripcion")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ChecIp)
                    .HasColumnName("checIP")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ChecMinutosDescarga).HasColumnName("checMinutosDescarga");

                entity.Property(e => e.ChecPathDescarga)
                    .HasColumnName("checPathDescarga")
                    .IsUnicode(false);

                entity.Property(e => e.ChecSucuId).HasColumnName("checSucuID");

                entity.Property(e => e.ChecUltimaDescarga)
                    .HasColumnName("checUltimaDescarga")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.ChecSucu)
                    .WithMany(p => p.CatChecadores)
                    .HasForeignKey(d => d.ChecSucuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cat_Checadores_cat_Sucursales");
            });

            modelBuilder.Entity<CatDepartamentos>(entity =>
            {
                entity.HasKey(e => e.DepaId);

                entity.ToTable("cat_Departamentos");

                entity.Property(e => e.DepaId).HasColumnName("depaID");

                entity.Property(e => e.DepaDescripcion)
                    .HasColumnName("depaDescripcion")
                    .HasMaxLength(100)
                    .IsUnicode(false);

            });

            modelBuilder.Entity<CatDomiciliosColonias>(entity =>
            {
                entity.HasKey(e => e.DomiColoId);

                entity.ToTable("cat_DomiciliosColonias");

                entity.Property(e => e.DomiColoId).HasColumnName("domiColoID");

                entity.Property(e => e.DomiColoCp).HasColumnName("domiColoCP");

                entity.Property(e => e.DomiColoDescripcion)
                    .HasColumnName("domiColoDescripcion")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DomiMunicId).HasColumnName("domiMunicID");

                entity.HasOne(d => d.DomiMunic)
                    .WithMany(p => p.CatDomiciliosColonias)
                    .HasForeignKey(d => d.DomiMunicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cat_DomiciliosColonias_cat_DomiciliosMunicipios");
            });

            modelBuilder.Entity<CatDomiciliosEstados>(entity =>
            {
                entity.HasKey(e => e.DomiEstaId);

                entity.ToTable("cat_DomiciliosEstados");

                entity.Property(e => e.DomiEstaId).HasColumnName("domiEstaID");

                entity.Property(e => e.DomiEstaAbrev)
                    .HasColumnName("domiEstaAbrev")
                    .HasMaxLength(2)
                    .IsFixedLength();

                entity.Property(e => e.DomiEstaDescripcion)
                    .HasColumnName("domiEstaDescripcion")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CatDomiciliosMunicipios>(entity =>
            {
                entity.HasKey(e => e.DomiMunicId);

                entity.ToTable("cat_DomiciliosMunicipios");

                entity.Property(e => e.DomiMunicId).HasColumnName("domiMunicID");

                entity.Property(e => e.DomiMunicDescripcion)
                    .HasColumnName("domiMunicDescripcion")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DomiMunicEstaId).HasColumnName("domiMunicEstaID");

                entity.HasOne(d => d.DomiMunicEsta)
                    .WithMany(p => p.CatDomiciliosMunicipios)
                    .HasForeignKey(d => d.DomiMunicEstaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cat_DomiciliosMunicipios_cat_DomiciliosEstados");
            });

            modelBuilder.Entity<CatEmpleados>(entity =>
            {
                entity.HasKey(e => e.EmpId);

                entity.ToTable("cat_Empleados");

                entity.Property(e => e.EmpId).HasColumnName("empID");

                entity.Property(e => e.EmpCelular)
                    .HasColumnName("empCelular")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.EmpComentarios)
                    .HasColumnName("empComentarios")
                    .IsUnicode(false);

                entity.Property(e => e.EmpCurp)
                    .HasColumnName("empCURP")
                    .HasMaxLength(18)
                    .IsUnicode(false);

                entity.Property(e => e.EmpEdocId).HasColumnName("empEdocID");

                entity.Property(e => e.EmpEmail)
                    .HasColumnName("empEmail")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmpEscoId).HasColumnName("empEscoID");

                entity.Property(e => e.EmpEstId).HasColumnName("empEstID");

                entity.Property(e => e.EmpFechaIngreso)
                    .HasColumnName("empFechaIngreso")
                    .HasColumnType("date");

                entity.Property(e => e.EmpImss)
                    .HasColumnName("empIMSS")
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.EmpJornaId).HasColumnName("empJornaID");

                entity.Property(e => e.EmpMaterno)
                    .HasColumnName("empMaterno")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmpNacFecha)
                    .HasColumnName("empNacFecha")
                    .HasColumnType("date");

                entity.Property(e => e.EmpNacMunicId).HasColumnName("empNacMunicID");

                entity.Property(e => e.EmpNombre)
                    .HasColumnName("empNombre")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmpNumero).HasColumnName("empNumero");

                entity.Property(e => e.EmpPatId).HasColumnName("empPatID");

                entity.Property(e => e.EmpPaterno)
                    .HasColumnName("empPaterno")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmpPuestoId).HasColumnName("empPuestoID");

                entity.Property(e => e.EmpRfc)
                    .HasColumnName("empRFC")
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.EmpSexId).HasColumnName("empSexID");

                entity.Property(e => e.EmpSucuId).HasColumnName("empSucuID");

                entity.Property(e => e.EmpTelefono)
                    .HasColumnName("empTelefono")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.EmpTrabTipoId).HasColumnName("empTrabTipoID");

                entity.Property(e => e.EmpTurId).HasColumnName("empTurID");

                entity.Property(e => e.EmpVacId).HasColumnName("empVacID");

                entity.Property(e => e.EmpUserId).HasColumnName("empUserID");

                entity.Property(e => e.EmpNombreCompleto).HasComputedColumnSql("empNombre + ' ' + empPaterno + ' ' + empMaterno");

                entity.Property(e => e.EmpAlias).HasColumnName("empAlias");

                entity.Property(e => e.EmpVacaDias).HasColumnName("empVacaDias");

                entity.HasOne(d => d.EmpEdoc)
                    .WithMany(p => p.CatEmpleados)
                    .HasForeignKey(d => d.EmpEdocId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cat_Empleados_cat_EstadosCiviles");

                entity.HasOne(d => d.EmpEsco)
                    .WithMany(p => p.CatEmpleados)
                    .HasForeignKey(d => d.EmpEscoId)
                    .HasConstraintName("FK_cat_Empleados_cat_Escolaridades");

                entity.HasOne(d => d.EmpEst)
                    .WithMany(p => p.CatEmpleados)
                    .HasForeignKey(d => d.EmpEstId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cat_Empleados_cat_Estatus");

                entity.HasOne(d => d.EmpJorna)
                    .WithMany(p => p.CatEmpleados)
                    .HasForeignKey(d => d.EmpJornaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cat_Empleados_cat_JornadasLaborales");

                entity.HasOne(d => d.EmpNacMunic)
                    .WithMany(p => p.CatEmpleados)
                    .HasForeignKey(d => d.EmpNacMunicId)
                    .HasConstraintName("FK_cat_Empleados_cat_DomiciliosMunicipios");

                entity.HasOne(d => d.EmpPat)
                    .WithMany(p => p.CatEmpleados)
                    .HasForeignKey(d => d.EmpPatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cat_Empleados_cat_Patrones");

                entity.HasOne(d => d.EmpPuesto)
                    .WithMany(p => p.CatEmpleados)
                    .HasForeignKey(d => d.EmpPuestoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cat_Empleados_cat_Puestos");

                entity.HasOne(d => d.EmpSex)
                    .WithMany(p => p.CatEmpleados)
                    .HasForeignKey(d => d.EmpSexId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cat_Empleados_cat_Sexos");

                entity.HasOne(d => d.EmpSucu)
                    .WithMany(p => p.CatEmpleados)
                    .HasForeignKey(d => d.EmpSucuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cat_Empleados_cat_Sucursales");

                entity.HasOne(d => d.EmpTrabTipo)
                    .WithMany(p => p.CatEmpleados)
                    .HasForeignKey(d => d.EmpTrabTipoId)
                    .HasConstraintName("FK_cat_Empleados_cat_TrabajadorTipos");

                entity.HasOne(d => d.EmpTur)
                    .WithMany(p => p.CatEmpleados)
                    .HasForeignKey(d => d.EmpTurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cat_Empleados_cat_TurnosLaborales");

                entity.HasOne(d => d.EmpVac)
                    .WithMany(p => p.CatEmpleados)
                    .HasForeignKey(d => d.EmpVacId)
                    .HasConstraintName("FK_cat_Empleados_cat_Vacunacion");
            });

            modelBuilder.Entity<CatEscolaridades>(entity =>
            {
                entity.HasKey(e => e.EscoId);

                entity.ToTable("cat_Escolaridades");

                entity.Property(e => e.EscoId).HasColumnName("escoID");

                entity.Property(e => e.EscoDescripcion)
                    .HasColumnName("escoDescripcion")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatEstadosCiviles>(entity =>
            {
                entity.HasKey(e => e.EdocId);

                entity.ToTable("cat_EstadosCiviles");

                entity.Property(e => e.EdocId).HasColumnName("edocID");

                entity.Property(e => e.EdocDescripcion)
                    .HasColumnName("edocDescripcion")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatEstatus>(entity =>
            {
                entity.HasKey(e => e.EstId);

                entity.ToTable("cat_Estatus");

                entity.Property(e => e.EstId).HasColumnName("estID");

                entity.Property(e => e.EstDescripcion)
                    .HasColumnName("estDescripcion")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatFestivos>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("cat_Festivos");

                entity.Property(e => e.FestDescripcion)
                    .HasColumnName("festDescripcion")
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.FestFechaDesde)
                    .HasColumnName("festFechaDesde")
                    .HasColumnType("date");

                entity.Property(e => e.FestFechaHasta)
                    .HasColumnName("festFechaHasta")
                    .HasColumnType("date");

                entity.Property(e => e.FestGuardia).HasColumnName("festGuardia");

                entity.Property(e => e.FestId)
                    .HasColumnName("festID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FestObservaciones).HasColumnName("festObservaciones");
            });

            modelBuilder.Entity<CatHorarios>(entity =>
            {
                entity.HasKey(e => e.HoraId);

                entity.ToTable("cat_Horarios");

                entity.Property(e => e.HoraId).HasColumnName("horaID");

                entity.Property(e => e.HoraComidaEntrada).HasColumnName("horaComidaEntrada");

                entity.Property(e => e.HoraComidaSalida).HasColumnName("horaComidaSalida");

                entity.Property(e => e.HoraEntrada).HasColumnName("horaEntrada");

                entity.Property(e => e.HoraSabadoComidaEntrada).HasColumnName("horaSabadoComidaEntrada");

                entity.Property(e => e.HoraSabadoComidaSalida).HasColumnName("horaSabadoComidaSalida");

                entity.Property(e => e.HoraSabadoEntrada).HasColumnName("horaSabadoEntrada");

                entity.Property(e => e.HoraSabadoSalida).HasColumnName("horaSabadoSalida");

                entity.Property(e => e.HoraSalida).HasColumnName("horaSalida");
            });

            modelBuilder.Entity<CatIncidencias>(entity =>
            {
                entity.HasKey(e => e.InciId);

                entity.ToTable("cat_Incidencias");

                entity.Property(e => e.InciId).HasColumnName("inciID");

                entity.Property(e => e.InciDescripcion)
                    .HasColumnName("inciDescripcion")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatJornadasLaborales>(entity =>
            {
                entity.HasKey(e => e.JornaId);

                entity.ToTable("cat_JornadasLaborales");

                entity.Property(e => e.JornaId).HasColumnName("jornaID");

                entity.Property(e => e.JornaDescripcion)
                    .HasColumnName("jornaDescripcion")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatPatrones>(entity =>
            {
                entity.HasKey(e => e.PatId);

                entity.ToTable("cat_Patrones");

                entity.Property(e => e.PatId).HasColumnName("patID");

                entity.Property(e => e.PatAbrev)
                    .HasColumnName("patAbrev")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PatColoId).HasColumnName("patColoID");

                entity.Property(e => e.PatDescripcion)
                    .HasColumnName("patDescripcion")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.PatRegistro)
                    .HasColumnName("patRegistro")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PatRfc)
                    .HasColumnName("patRFC")
                    .HasMaxLength(18)
                    .IsUnicode(false);

                entity.HasOne(d => d.PatColo)
                    .WithMany(p => p.CatPatrones)
                    .HasForeignKey(d => d.PatColoId)
                    .HasConstraintName("FK_cat_Patrones_cat_DomiciliosColonias");
            });

            modelBuilder.Entity<CatPeriodos>(entity =>
            {
                entity.HasKey(e => e.PerId);

                entity.ToTable("cat_Periodos");

                entity.Property(e => e.PerCerrado)
                    .HasColumnName("perCerrado")
                    .HasColumnType("date");

                entity.Property(e => e.PerFechaDesde)
                    .HasColumnName("perFechaDesde")
                    .HasColumnType("date");

                entity.Property(e => e.PerFechaHasta)
                    .HasColumnName("perFechaHasta")
                    .HasColumnType("date");

                entity.Property(e => e.PerId)
                    .HasColumnName("perID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PerNum).HasColumnName("perNum");
            });

            modelBuilder.Entity<CatPlantillas>(entity =>
            {
                entity.HasKey(e => e.PlantiId);

                entity.ToTable("cat_Plantillas");

                entity.Property(e => e.PlantiDepaId).HasColumnName("plantiDepaID");

                entity.HasOne(d => d.PlanDepa)
                    .WithMany(p => p.CatPlantillas)
                    .HasForeignKey(d => d.PlantiDepaId)
                    .HasConstraintName("FK_cat_Plantilla_cat_Departamentos");


                entity.Property(e => e.PlantiId)
                    .HasColumnName("plantiID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PlantiPuestoId).HasColumnName("plantiPuestoID");

                entity.HasOne(d => d.PlanPuesto)
                    .WithMany(p => p.CatPlantillas)
                    .HasForeignKey(d => d.PlantiPuestoId)
                    .HasConstraintName("FK_cat_Plantilla_cat_Puestos");

                entity.Property(e => e.PlantiSucuId).HasColumnName("plantiSucuID");

                entity.HasOne(d => d.PlanSucu)
                    .WithMany(p => p.CatPlantillas)
                    .HasForeignKey(d => d.PlantiSucuId)
                    .HasConstraintName("FK_cat_Plantilla_cat_Sucursales");
            });

            modelBuilder.Entity<CatPuestos>(entity =>
            {
                entity.HasKey(e => e.PuestoId);

                entity.ToTable("cat_Puestos");

                entity.Property(e => e.PuestoId).HasColumnName("puestoID");

                entity.Property(e => e.PuestoAreaId).HasColumnName("puestoAreaID");

                entity.Property(e => e.PuestoDescripcion)
                    .HasColumnName("puestoDescripcion")
                    .HasMaxLength(150)
                    .IsUnicode(false);


                

                entity.Property(e => e.PuestoJerarquiaSuperiorPuestoId).HasColumnName("puestoJerarquiaSuperiorPuestoID");

                entity.HasOne(d => d.PuestoArea)
                    .WithMany(p => p.CatPuestos)
                    .HasForeignKey(d => d.PuestoAreaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cat_Puestos_cat_Areas");

                entity.HasOne(d => d.PuestoJefe)
                    .WithMany(p => p.CatPuestosSuperiores)
                    .HasForeignKey(d => d.PuestoJerarquiaSuperiorPuestoId)
                    .HasConstraintName("FK_cat_Puestos_cat_Puestos");
            });

            modelBuilder.Entity<CatSexos>(entity =>
            {
                entity.HasKey(e => e.SexId);

                entity.ToTable("cat_Sexos");

                entity.Property(e => e.SexId).HasColumnName("sexID");

                entity.Property(e => e.SexDescripcion)
                    .HasColumnName("sexDescripcion")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatSucursales>(entity =>
            {
                entity.HasKey(e => e.SucuId);

                entity.ToTable("cat_Sucursales");

                entity.Property(e => e.SucuId).HasColumnName("SucuID");

                entity.Property(e => e.SucuEmail)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.SucuNcorto)
                    .HasColumnName("SucuNCorto")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SucuNombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SucuPatId).HasColumnName("SucuPatID");

                entity.HasOne(d => d.SucuPat)
                    .WithMany(p => p.CatSucursales)
                    .HasForeignKey(d => d.SucuPatId)
                    .HasConstraintName("FK_cat_Sucursales_cat_Patrones");
            });

            modelBuilder.Entity<CatTrabajadorTipos>(entity =>
            {
                entity.HasKey(e => e.TrabTipoId);

                entity.ToTable("cat_TrabajadorTipos");

                entity.Property(e => e.TrabTipoId).HasColumnName("trabTipoID");

                entity.Property(e => e.TrabTipoDescripcion)
                    .HasColumnName("trabTipoDescripcion")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatTurnosLaborales>(entity =>
            {
                entity.HasKey(e => e.TurId);

                entity.ToTable("cat_TurnosLaborales");

                entity.Property(e => e.TurId).HasColumnName("turID");

                entity.Property(e => e.TurDescripcion)
                    .HasColumnName("turDescripcion")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatVacunacion>(entity =>
            {
                entity.HasKey(e => e.VacId)
                    .HasName("PK_cat_VacunacionTipos");

                entity.ToTable("cat_Vacunacion");

                entity.Property(e => e.VacId).HasColumnName("vacID");

                entity.Property(e => e.VacDescripcion)
                    .HasColumnName("vacDescripcion")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CfgParametrosSistema>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("cfg_ParametrosSistema");

                entity.Property(e => e.ParamGafetteFrente)
                    .HasColumnName("paramGafetteFrente")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ParamGafetteReverso)
                    .HasColumnName("paramGafetteReverso")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ParamId)
                    .HasColumnName("paramID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ParamMinutosComida).HasColumnName("paramMinutosComida");

                entity.Property(e => e.ParamMinutosTolerancia).HasColumnName("paramMinutosTolerancia");
            });

            modelBuilder.Entity<MovEmpleadosAsistencias>(entity =>
            {
                entity.HasKey(e => e.EmpAsisId);

                entity.ToTable("mov_EmpleadosAsistencias");

                entity.Property(e => e.EmpAsisId).HasColumnName("empAsisID");

                entity.Property(e => e.EmpAsisChecId).HasColumnName("empAsisChecID");

                entity.Property(e => e.EmpAsisEmpId).HasColumnName("empAsisEmpID");

                entity.Property(e => e.EmpAsisFecha)
                    .HasColumnName("empAsisFecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.EmpAsisHoraId).HasColumnName("empAsisHoraID");

                entity.HasOne(d => d.EmpAsisChec)
                    .WithMany(p => p.MovEmpleadosAsistencias)
                    .HasForeignKey(d => d.EmpAsisChecId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mov_EmpleadosAsistencias_cat_Checadores");

                entity.HasOne(d => d.EmpAsisEmp)
                    .WithMany(p => p.MovEmpleadosAsistencias)
                    .HasForeignKey(d => d.EmpAsisEmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mov_EmpleadosAsistencias_cat_Empleados");

                entity.HasOne(d => d.EmpAsisHora)
                    .WithMany(p => p.MovEmpleadosAsistencias)
                    .HasForeignKey(d => d.EmpAsisHoraId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mov_EmpleadosAsistencias_cat_Horarios");
            });

            modelBuilder.Entity<MovEmpleadosDomicilios>(entity =>
            {
                entity.HasKey(e => e.DomiId);

                entity.ToTable("mov_EmpleadosDomicilios");

                entity.Property(e => e.DomiId).HasColumnName("domiID");

                entity.Property(e => e.DomiCalle)
                    .HasColumnName("domiCalle")
                    .HasMaxLength(50);

                entity.Property(e => e.DomiColoId).HasColumnName("domiColoID");

                entity.Property(e => e.DomiEmpId).HasColumnName("domiEmpID");

                entity.Property(e => e.DomiNumExt).HasColumnName("domiNumExt");

                entity.Property(e => e.DomiNumInt)
                    .HasColumnName("domiNumInt")
                    .HasMaxLength(10);
            });

           

            modelBuilder.Entity<MovEmpleadosIncidencias>(entity =>
            {
                entity.HasKey(e => e.EmpInciId);

                entity.ToTable("mov_EmpleadosIncidencias");

                entity.Property(e => e.EmpInciId).HasColumnName("empInciID");

                entity.Property(e => e.EmpInciEmpId).HasColumnName("empInciEmpID");

                entity.Property(e => e.EmpInciFechaDesde)
                    .HasColumnName("empInciFechaDesde")
                    .HasColumnType("datetime");

                entity.Property(e => e.EmpInciFechaHasta)
                    .HasColumnName("empInciFechaHasta")
                    .HasColumnType("datetime");

                entity.Property(e => e.EmpInciHoraId).HasColumnName("empInciHoraID");

                entity.Property(e => e.EmpInciInciId).HasColumnName("empInciInciID");

                entity.Property(e => e.EmpInciObs)
                    .HasColumnName("empInciObs")
                    .IsUnicode(false);

                entity.HasOne(d => d.EmpInciEmp)
                    .WithMany(p => p.MovEmpleadosIncidencias)
                    .HasForeignKey(d => d.EmpInciEmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mov_EmpleadosIncidencias_cat_Empleados");

                entity.HasOne(d => d.EmpInciHora)
                    .WithMany(p => p.MovEmpleadosIncidencias)
                    .HasForeignKey(d => d.EmpInciHoraId)
                    .HasConstraintName("FK_mov_EmpleadosIncidencias_cat_Horarios");

                entity.HasOne(d => d.EmpInciInci)
                    .WithMany(p => p.MovEmpleadosIncidencias)
                    .HasForeignKey(d => d.EmpInciInciId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mov_EmpleadosIncidencias_cat_Incidencias");
            });

            modelBuilder.Entity<MovEmpleadosSueldos>(entity =>
            {
                entity.HasKey(e => e.EmpSdoId);

                entity.ToTable("mov_EmpleadosSueldos");

                entity.HasIndex(e => e.EmpSdoEmpId)
                    .HasName("IX_mov_EmpleadosSueldos");

                entity.Property(e => e.EmpSdoId).HasColumnName("empSdoID");

                entity.Property(e => e.EmpSdoApoyoViviTrans)
                    .HasColumnName("empSdoApoyoViviTrans")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.EmpSdoBono)
                    .HasColumnName("empSdoBono")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.EmpSdoDespensa)
                    .HasColumnName("empSdoDespensa")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.EmpSdoEmpId).HasColumnName("empSdoEmpID");

                entity.Property(e => e.EmpSdoFechaAlta)
                    .HasColumnName("empSdoFechaAlta")
                    .HasColumnType("datetime");

                entity.Property(e => e.EmpSdoPremioAsis)
                    .HasColumnName("empSdoPremioAsis")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.EmpSdoPremioPunt)
                    .HasColumnName("empSdoPremioPunt")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.EmpSdoSdi)
                    .HasColumnName("empSdoSDI")
                    .HasColumnType("numeric(18, 0)");

                entity.Property(e => e.EmpSdoSueldoDia)
                    .HasColumnName("empSdoSueldoDia")
                    .HasColumnType("numeric(18, 0)");

                entity.HasOne(d => d.EmpSdoEmp)
                    .WithMany(p => p.MovEmpleadosSueldos)
                    .HasForeignKey(d => d.EmpSdoEmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mov_EmpleadosSueldos_cat_Empleados1");
            });

            modelBuilder.Entity<CatRegistroIncidencias>(entity =>
            {
                entity.HasKey(e => e.ReInciId);

                entity.ToTable("CatRegistroIncidencias");

                entity.Property(e => e.ReInciId).HasColumnName("reInciId");

                entity.Property(e => e.ReInciEmpId).HasColumnName("reInciEmpId");

                entity.Property(e => e.Fecha).HasColumnName("fecha");

                entity.Property(e => e.ReInciInciId).HasColumnName("reInciInciId");

                entity.Property(e => e.ReInciEstatusId).HasColumnName("reInciEstatusId");

                entity.Property(e => e.ReInciEstatusFlujo).HasColumnName("reInciEstatusFlujo");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.RegistroIncidencias)
                    .HasForeignKey(d => d.ReInciEmpId)
                    .HasConstraintName("FK_registroIncidencias_cat_Empleados");

                entity.HasOne(d => d.Inci)
                    .WithMany(p => p.RegistroIncidencias)
                    .HasForeignKey(d => d.ReInciInciId)
                    .HasConstraintName("FK_registroIncidencias_cat_Incidencias");

                entity.HasOne(d => d.Estatus)
                    .WithMany(p => p.CatRegistroIncidencias)
                    .HasForeignKey(d => d.ReInciEstatusId)
                    .HasConstraintName("FK_cat_RegistroIncidencias_cat_Estatus");

                entity.HasOne(d => d.DetFlujo)
                    .WithMany(p => p.ReInci)
                    .HasForeignKey(d => d.ReInciEstatusFlujo)
                    .HasConstraintName("FK_cat_RegistroIncidencias_cat_DetalleFlujo");



            });

            modelBuilder.Entity<CatDetalleIncidencias>(entity =>
            {
                entity.HasKey(e => e.DetInciId);

                entity.ToTable("CatDetalleIncidencias");

                entity.Property(e => e.DetInciId).HasColumnName("detInciID");

                entity.Property(e => e.DetInciReInciId).HasColumnName("detInciReInciID");

                entity.Property(e => e.DetFecha).HasColumnName("detFecha");

                entity.Property(e => e.DetInciHorarioId).HasColumnName("detInciHorarioID");

                entity.Property(e => e.MedidaAccion)
                    .HasColumnName("medidaAccion");

                entity.Property(e => e.Motivo)
                    .HasColumnName("motivo");
                
                entity.Property(e => e.Asunto)
                    .HasColumnName("asunto");

                entity.Property(e => e.Destino)
                    .HasColumnName("destino");

                entity.Property(e => e.TelDestino)
                    .HasColumnName("telDestino")
                    .HasMaxLength(12);

                entity.Property(e => e.Contacto1)
                    .HasColumnName("contacto1");

                entity.Property(e => e.NombreDestino)
                    .HasColumnName("nombreDestino");

                entity.Property(e => e.Contacto2)
                    .HasColumnName("contacto2");

                entity.Property(e => e.Observaciones)
                    .HasColumnName("observaciones");

                entity.Property(e => e.HoraSalida)
                    .HasColumnName("horaSalida");

                entity.Property(e => e.HoraRegreso)
                    .HasColumnName("horaRegreso");

                entity.Property(e => e.FechaInicio)
                    .HasColumnName("fechaInicio");

                entity.Property(e => e.FechaFinal)
                    .HasColumnName("fechaFinal");

                entity.Property(e => e.FechaPresentacion)
                    .HasColumnName("fechaPresentacion");

                entity.Property(e => e.DiasAusencia)
                    .HasColumnName("diasAusencia");

                entity.Property(e => e.PersonaCubrira)
                    .HasColumnName("personaCubrira");

                entity.Property(e => e.DetInciFechaFinPermisoPersonal)
                    .HasColumnName("detInciFechaFinPermisoPersonal");

                entity.Property(e => e.DetInciHoraIngreso)
                    .HasColumnName("detInciHoraIngreso");

                entity.Property(e => e.DetInciHoraSalida)
                    .HasColumnName("detInciHoraSalida");

                entity.Property(e => e.DetInciHoraSalidaComida)
                    .HasColumnName("detInciHoraSalidaComida");

                entity.Property(e => e.DetInciHoraRegresoComida)
                    .HasColumnName("detInciHoraRegresoComida");

                entity.HasOne(d => d.ReInci)
                    .WithMany(p => p.DetalleIncidencias)
                    .HasForeignKey(d => d.DetInciReInciId)
                    .HasConstraintName("FK_cat_DetalleIncidencias_cat_RegistroIncidencias");

                entity.HasOne(d => d.Hora)
                    .WithMany(p => p.DetallesIncidencias)
                    .HasForeignKey(d => d.DetInciHorarioId)
                    .HasConstraintName("FK_cat_DetalleIncidencias_cat_Horarios");


            });

            modelBuilder.Entity<CatAntiguedad>(entity =>
            {
                entity.HasKey(e => e.AntiId);

                entity.ToTable("cat_Antiguedad");

                entity.Property(e => e.AntiId).HasColumnName("antiID");

                entity.Property(e => e.AntiAniosDesde).HasColumnName("antiAniosDesde");

                entity.Property(e => e.AntiAniosHasta).HasColumnName("antiAniosHasta");

                //entity.Property(e => e.AntiAnios)
                //    .HasColumnName("antiAnios")
                //    .HasMaxLength(100)
                //    .IsUnicode(false);

                entity.Property(e => e.AntiDias)
                    .HasColumnName("antiDias");

            });


        modelBuilder.Entity<CatHistorialVacaciones>(entity =>
        {
            entity.HasKey(e => e.HVId);

            entity.ToTable("cat_HistorialVacaciones");

            entity.Property(e => e.HVEmpId).HasColumnName("hVEmpID");

            entity.Property(e => e.HVAntiguedadId).HasColumnName("hVEAntiguedadID");

            entity.Property(e => e.HVDiasSolicitados).HasColumnName("hVDiasSolicitados");

            entity.Property(e => e.HVEjercicio).HasColumnName("hVEjercicio");

            entity.Property(e => e.HVFechaSolicitud)
                .HasColumnName("hvFechaSolicitud");
                        
            entity.Property(e => e.HVFechaInicio)
                .HasColumnName("hVFechaInicio");

            entity.Property(e => e.HVFechaCulminacion)
                .HasColumnName("hVFechaCulminacion");

            entity.Property(e => e.HVFechaPresentacion)
                .HasColumnName("hVFechaPresentacion");

            entity.Property(e => e.HVVacacionesPendientesEjercicioActual)
                .HasColumnName("hVVacacionesPendientesEjercicioActual");

            entity.Property(e => e.HVVacacionesPendientesEjerciciosPasados)
                .HasColumnName("hVVacacionesPendientesEjerciciosPasados");

            entity.Property(e => e.HVReInciId)
                .HasColumnName("hVReInciId");

            entity.Property(e => e.HVPuestoId)
                .HasColumnName("hVPuestoId");

            entity.Property(e => e.HVSucursalId).HasColumnName("hVSucursalId");

            entity.Property(e => e.HVSaldoVacaciones).HasColumnName("hVSaldoVacaciones");

            entity.HasOne(d => d.HVEmp)
                .WithMany(p => p.HistorialVacaciones)
                .HasForeignKey(d => d.HVEmpId)
                .HasConstraintName("FK_cat_HistorialVacaciones_cat_CatEmpleados");

            entity.HasOne(d => d.HVAnti)
                .WithMany(p => p.HistorialVacaciones)
                .HasForeignKey(d => d.HVAntiguedadId)
                .HasConstraintName("FK_cat_HistorialVacaciones_cat_Antiguedad");

            entity.HasOne(d => d.HVReInci)
               .WithMany(p => p.HistorialVacaciones)
               .HasForeignKey(d => d.HVReInciId)
               .HasConstraintName("FK_cat_HistorialVacaciones_cat_RegistroIncidencias");

            entity.HasOne(d => d.HVPuestos)
               .WithMany(p => p.HistorialVacaciones)
               .HasForeignKey(d => d.HVPuestoId)
               .HasConstraintName("FK_cat_HistorialVacaciones_cat_Puestos");

            entity.HasOne(d => d.HVSucursales)
               .WithMany(p => p.HistorialVacaciones)
               .HasForeignKey(d => d.HVSucursalId)
               .HasConstraintName("FK_cat_HistorialVacaciones_cat_Sucursales");

        });

            modelBuilder.Entity<CatSaldoDeVacaciones>(entity =>
            {
                entity.HasKey(e => e.SVId);

                entity.ToTable("cat_SaldoDeVacaciones");

                entity.Property(e => e.SVId).HasColumnName("sVId");

                entity.Property(e => e.SVEmpId).HasColumnName("sVEmpId");

                entity.Property(e => e.SVEjercicio).HasColumnName("sVEjercicio");

                entity.Property(e => e.SVPeriodoId).HasColumnName("sVPeriodoId");

                entity.Property(e => e.SVFechaRegistro).HasColumnName("sVFechaRegistro");

                entity.Property(e => e.SVAniosAntiguedad).HasColumnName("sVAniosAntiguedad");

                entity.Property(e => e.SVAntiId).HasColumnName("sVAntiId");

                entity.Property(e => e.SVDiasDisfrutados).HasColumnName("sVDiasDisfrutados");

                entity.Property(e => e.SVDiasRestantes).HasColumnName("sVDiasRestantes");

                entity.HasOne(d => d.SVEmpleados)
               .WithMany(p => p.SaldoDeVacaciones)
               .HasForeignKey(d => d.SVEmpId)
               .HasConstraintName("FK_cat_SaldoDeVacaciones_cat_Empleados");

                entity.HasOne(d => d.SVPeriodos)
               .WithMany(p => p.SaldoDeVacaciones)
               .HasForeignKey(d => d.SVPeriodoId)
               .HasConstraintName("FK_cat_SaldoDeVacaciones_cat_Periodos");

                entity.HasOne(d => d.SVAntiguedad)
               .WithMany(p => p.SaldoDeVacaciones)
               .HasForeignKey(d => d.SVAntiId)
               .HasConstraintName("FK_cat_SaldoDeVacaciones_cat_Antiguedad");

            });

            modelBuilder.Entity<CatModulos>(entity =>
            {
                entity.HasKey(e => e.ModuloId);

                entity.ToTable("cat_Modulos");

                entity.Property(e => e.ModuloId).HasColumnName("moduloId");

                entity.Property(e => e.ModuloNombre)
                    .HasColumnName("moduloNombre")
                    .HasMaxLength(100)
                    .IsUnicode(false);
               
            });

            

            modelBuilder.Entity<CatSubModulos>(entity =>
            {
                entity.HasKey(e => e.SubMId);

                entity.ToTable("cat_SubModulos");

                entity.Property(e => e.SubMId).HasColumnName("subMId");

                entity.Property(e => e.SubMModuloId).HasColumnName("subMModuloId");

                entity.Property(e => e.SubMName)
                    .HasColumnName("subMName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Modulos)
                .WithMany(p => p.SubModulos)
                .HasForeignKey(d => d.SubMModuloId)
                .HasConstraintName("FK_cat_SubModulos_cat_Modulos");

            });

            modelBuilder.Entity<CatFlujos>(entity => 
            {
                entity.HasKey(e => e.FlujoId);

                entity.ToTable("cat_Flujos");

                entity.Property(e => e.FlujoId).HasColumnName("flujoId");

                entity.Property(e => e.FlujoDescripcion)
                .HasColumnName("flujoDescripcion")
                .HasMaxLength(100)
                .IsUnicode(false);
            });

            modelBuilder.Entity<CatSolicitudIncidencias>(entity =>
            {
                entity.HasKey(e => e.SolInciId);

                entity.ToTable("cat_SolicitudIncidencias");

                entity.Property(e => e.SolInciId).HasColumnName("solInciId");

                entity.Property(e => e.SolInciReInciId).HasColumnName("solInciReInciId");

                entity.Property(e => e.SolInciEmpId).HasColumnName("solInciEmpId");

                entity.Property(e => e.SolInciFechaRegistro).HasColumnName("solInciFechaRegistro");

                entity.Property(e => e.SolInciPuestoSuperior).HasColumnName("solInciPuestoSuperior");

                entity.Property(e => e.SolInciPerfiles).HasColumnName("solInciPerfiles").HasMaxLength(100)
                .IsUnicode(false);

                entity.Property(e => e.SolInciFlujoId).HasColumnName("solInciFlujoId");

                entity.HasOne(d => d.Empleados)
                .WithMany(p => p.SolicitudIncidencias)
                .HasForeignKey(d => d.SolInciEmpId)
                .HasConstraintName("FK_cat_SolicitudIncidencias_cat_Empleados");

                entity.HasOne(d => d.ReInci)
                .WithMany(p => p.SolicitudIncidencias)
                .HasForeignKey(d => d.SolInciReInciId)
                .HasConstraintName("FK_cat_SolicitudIncidencias_cat_RegistroIncidencias");

                entity.HasOne(d => d.Flujos)
                .WithMany(p => p.SolicitudIncidencias)
                .HasForeignKey(d => d.SolInciFlujoId)
                .HasConstraintName("FK_cat_SolicitudIncidencias_cat_Flujos");

            });

            modelBuilder.Entity<CatDetalleFlujo>(entity =>
            {
                entity.HasKey(e => e.DetFlujoId);

                entity.ToTable("cat_DetalleFlujo");

                entity.Property(e => e.DetFlujoId).HasColumnName("detFlujoId");

                entity.Property(e => e.DetFlujoFlujoId).HasColumnName("detFlujoFlujoId");

                entity.Property(e => e.DetFlujoDescripcion)
                .HasColumnName("detFlujoDescripcion")
                .HasMaxLength(100)
                .IsUnicode(false);

                entity.Property(e => e.DetFlujoOrden).HasColumnName("detFlujoOrden");

                entity.Property(e => e.DetFlujoPerfiles)
                .HasColumnName("detFlujoPerfiles")
                .IsUnicode(false);

                entity.Property(e => e.DetFlujoCorreoDestino)
                .HasColumnName("detFlujoCorreoDestino")
                .IsUnicode(false);

                entity.Property(e => e.DetFlujoEmail)
                .HasColumnName("detFlujoEmail")
                .IsUnicode(false);

                entity.HasOne(d => d.Flujo)
               .WithMany(p => p.DetalleFlujos)
               .HasForeignKey(d => d.DetFlujoFlujoId)
               .HasConstraintName("FK_cat_DetalleFlujos_cat_Flujos");
            });

            modelBuilder.Entity<CatBitacoraIncidencias>(entity =>
            {
                entity.HasKey(e => e.BitInciId);

                entity.ToTable("cat_BitacoraIncidencias");

                entity.Property(e => e.BitInciId).HasColumnName("bitInciId");

                entity.Property(e => e.BitInciReInciId).HasColumnName("bitInciReInciId");

                entity.Property(e => e.BitInciDetFlujoId).HasColumnName("bitInciDetFlujoId");

                entity.Property(e => e.BitInciUserId)
                .HasColumnName("bitInciUserId")
                .HasMaxLength(100)
                .IsUnicode(false);

                entity.Property(e => e.BitInciObservaciones)
                .HasColumnName("bitInciObservaciones")
                .IsUnicode(false);

                entity.Property(e => e.BitInciFecha).HasColumnName("bitInciFecha");

                entity.HasOne(d => d.ReInci)
               .WithMany(p => p.BitacoraIncidencias)
               .HasForeignKey(d => d.BitInciReInciId)
               .HasConstraintName("FK_cat_BitacoraIncidencias_cat_RegistroIncidencias");

                entity.HasOne(d => d.DetFlujos)
               .WithMany(p => p.BitacoraIncidencias)
               .HasForeignKey(d => d.BitInciDetFlujoId)
               .HasConstraintName("FK_cat_BitacoraIncidencias_cat_DetalleFlujos");
            });

            modelBuilder.Entity<CatEmpTrazabilidad>(entity =>
            {
                entity.HasKey(e => e.EmpTrazaId);

                entity.ToTable("cat_EmpTrazabilidad");

                entity.Property(e => e.EmpTrazaId).HasColumnName("empTrazaId");

                entity.Property(e => e.EmpTrazaEmpId).HasColumnName("empTrazaEmpId");

                entity.Property(e => e.EmpTrazaPassword).HasColumnName("empTrazaPassword");



                entity.HasOne(d => d.Empleados)
                    .WithMany(p => p.CatEmpTrazabilidads)
                    .HasForeignKey(d => d.EmpTrazaEmpId)
                    .HasConstraintName("FK_cat_EmpTrazabilidad_cat_Empleados");


            });

            modelBuilder.Entity<MovEmpleadosHorarios>(entity =>
            {
                entity.HasKey(e => e.EmpHoraId);

                entity.ToTable("mov_EmpleadosHorarios");

                entity.Property(e => e.EmpHoraId).HasColumnName("empHoraId");

                entity.Property(e => e.EmpHoraFechaRegistro).HasColumnName("empHoraFechaRegistro");

                entity.Property(e => e.EmpHoraEmpId).HasColumnName("empHoraEmpId");

                entity.Property(e => e.EmpHoraHorId).HasColumnName("empHoraHorId");

                entity.Property(e => e.EmpHoraFechaDesde).HasColumnName("empHoraFechaDesde");

                entity.Property(e => e.EmpHoraFechaHasta).HasColumnName("empHoraFechaHasta");

                entity.Property(e => e.EmpHoraTipoHorario).HasColumnName("empHoraTipoHorario");



                entity.HasOne(d => d.HoraEmp)
                    .WithMany(p => p.MovEmpleadosHorarios)
                    .HasForeignKey(d => d.EmpHoraEmpId)
                    .HasConstraintName("FK_mov_EmpleadosHorarios_cat_Empleados");

                entity.HasOne(d => d.HoraHor)
                    .WithMany(p => p.MovEmpleadosHorarios)
                    .HasForeignKey(d => d.EmpHoraHorId)
                    .HasConstraintName("FK_mov_EmpleadosHorarios_cat_Horarios");


            });

            modelBuilder.Entity<CatUsuariosPermisos>(entity =>
            {
                entity.HasKey(e => e.URPId);

                entity.ToTable("cat_UsuariosPermisos");

                entity.Property(e => e.URPId).HasColumnName("uRPId");

                entity.Property(e => e.URPUserId)
                    .HasColumnName("uRPUserId")
                    .HasMaxLength(100)
                    .IsUnicode(false);
                
                entity.Property(e => e.URPModuloId).HasColumnName("uRPModuloId");

                entity.Property(e => e.URPCrear).HasColumnName("uRPCrear");

                entity.Property(e => e.URPMostrar).HasColumnName("uRPMostrar");

                entity.Property(e => e.URPModificar).HasColumnName("uRPModificar");

                entity.Property(e => e.URPEliminar).HasColumnName("uRPEliminar");

                entity.Property(e => e.URPSubModuloId).HasColumnName("uRPSubModuloId");

                entity.Property(e => e.URPAutorizado).HasColumnName("uRPAutorizado");

                entity.HasOne(d => d.Modulo)
               .WithMany(p => p.UsuariosPermisos)
               .HasForeignKey(d => d.URPModuloId)
               .HasConstraintName("FK_cat_UsuariosPermisos_cat_Modulos");

                entity.HasOne(d => d.SubModulo)
               .WithMany(p => p.CatUsuariosPermisos)
               .HasForeignKey(d => d.URPSubModuloId)
               .HasConstraintName("FK_cat_UsuariosPermisos_cat_SubModulos");

            });

            modelBuilder.Entity<MovRequisicionPersonal>(entity =>
            {
                entity.HasKey(e => e.MRPId);

                entity.ToTable("mov_RequisicionPersonal");

                entity.Property(e => e.MRPId).HasColumnName("mRPId");

                entity.Property(e => e.MRPNumeroVacantes).HasColumnName("mRPNumeroVacantes");

                entity.Property(e => e.MRPFechaElaboracion).HasColumnName("mRPFechaElaboracion");

                entity.Property(e => e.MRPFechaRecepcion).HasColumnName("mRPFechaRecepcion");

                entity.Property(e => e.MRPFolio).HasColumnName("mRPFolio");

                entity.Property(e => e.MRPPuestoId).HasColumnName("mRPPuestoId");

                entity.Property(e => e.MRPTipoVacante).HasColumnName("mRPTipoVacante");

                entity.Property(e => e.MRPTurnoId).HasColumnName("mRPTurnoId");

                entity.Property(e => e.MRPRolarTurno).HasColumnName("mRPRolarTurno");

                entity.Property(e => e.MRPTiempoAlimentos).HasColumnName("mRPTiempoAlimentos");

                entity.Property(e => e.MRPMotivoVacante).HasColumnName("mRPMotivoVacante");

                entity.Property(e => e.MRPMotivoDescripcion).HasColumnName("mRPMotivoDescripcion");

                entity.Property(e => e.MRPSexoId).HasColumnName("mRPSexoId");

                entity.Property(e => e.MRPEdadMinima).HasColumnName("mRPEdadMinima");

                entity.Property(e => e.MRPEdadMaxima).HasColumnName("mRPEdadMaxima");

                entity.Property(e => e.MRPEscolaridadId).HasColumnName("mRPEscolaridadId");

                entity.Property(e => e.MRPTituloIndispensable).HasColumnName("mRPTituloIndispensable");

                entity.Property(e => e.MRPCedulaIndispensable).HasColumnName("mRPCedulaIndispensable");

                entity.Property(e => e.MRPFuncionesPuesto).HasColumnName("mRPFuncionesPuesto");

                entity.Property(e => e.MRPConocimientosPuesto).HasColumnName("mRPConocimientosPuesto");

                entity.Property(e => e.MRPSueldoMensualInicial).HasColumnName("mRPSueldoMensualInicial");

                entity.Property(e => e.MRPSueldoMensualPlanta).HasColumnName("mRPSueldoMensualPlanta");

                entity.Property(e => e.MRPSueldoMensualMasCosto).HasColumnName("mRPSueldoMensualMasCosto");

                entity.Property(e => e.MRPBonoVariable).HasColumnName("mRPBonoVariable");

                entity.Property(e => e.MRPEsquemaContratacion).HasColumnName("mRPEsquemaContratacion");

                entity.Property(e => e.MRPCandidato).HasColumnName("mRPCandidato");

                entity.Property(e => e.MRPFechaIngreso).HasColumnName("mRPFechaIngreso");

                entity.Property(e => e.MRPSucursalId).HasColumnName("mRPSucursalId");

                entity.Property(e => e.MRPExperienciaIndispensable).HasColumnName("mRPExperienciaIndispensable");

                entity.Property(e => e.MRPFlujoId).HasColumnName("mRPFlujoId");

                entity.Property(e => e.MRPEstatusId).HasColumnName("mRPEstatusId");

                entity.Property(e => e.MRPUserId).HasColumnName("mRPUserId");

                entity.Property(e => e.MRPEmpId).HasColumnName("mRPEmpId");

                entity.HasOne(d => d.Puestos)
               .WithMany(p => p.RequisicionPersonal)
               .HasForeignKey(d => d.MRPPuestoId)
               .HasConstraintName("FK_mov_RequisicionPersonal_cat_Puestos");

                entity.HasOne(d => d.Turnos)
               .WithMany(p => p.MovRequisicionPersonal)
               .HasForeignKey(d => d.MRPTurnoId)
               .HasConstraintName("FK_mov_RequisicionPersonal_cat_Turnos");

                entity.HasOne(d => d.Sexos)
               .WithMany(p => p.MovRequisicionPersonals)
               .HasForeignKey(d => d.MRPSexoId)
               .HasConstraintName("FK_mov_RequisicionPersonal_cat_Sexos");

                entity.HasOne(d => d.Escolaridad)
               .WithMany(p => p.MovRequisicionPersonals)
               .HasForeignKey(d => d.MRPEscolaridadId)
               .HasConstraintName("FK_mov_RequisicionPersonal_cat_Escolaridades");

                entity.HasOne(d => d.DetalleFlujo)
              .WithMany(p => p.RequisicionPersonalRP)
              .HasForeignKey(d => d.MRPFlujoId)
              .HasConstraintName("FK_mov_RequisicionPersonal_cat_DetalleFlujo");

                entity.HasOne(d => d.Sucursales)
               .WithMany(p => p.RequisicionPersonal)
               .HasForeignKey(d => d.MRPSucursalId)
               .HasConstraintName("FK_mov_RequisicionPersonal_cat_Sucursales");

                entity.HasOne(d => d.Estatus)
               .WithMany(p => p.MovRequisicionPersonal)
               .HasForeignKey(d => d.MRPEstatusId)
               .HasConstraintName("FK_mov_RequisicionPersonal_cat_Estatus");

                entity.HasOne(d => d.Empleados)
               .WithMany(p => p.MovRequisicionPersonal)
               .HasForeignKey(d => d.MRPEmpId)
               .HasConstraintName("FK_mov_RequisicionPersonal_cat_Empleados");

            });

            modelBuilder.Entity<CatBitacoraRequisicionPersonal>(entity =>
            {
                entity.HasKey(e => e.BitRPId);

                entity.ToTable("cat_BitacoraRequisicionPersonal");

                entity.Property(e => e.BitRPId).HasColumnName("bitRPId");

                entity.Property(e => e.BitRPRPId).HasColumnName("bitRPRPId");

                entity.Property(e => e.BitRPDetFlujoId).HasColumnName("bitRPDetFlujoId");

                entity.Property(e => e.BitRPUserId)
                .HasColumnName("bitRPUserId")
                .HasMaxLength(100)
                .IsUnicode(false);

                entity.Property(e => e.BitRPObservaciones)
                .HasColumnName("bitRPObservaciones")
                .IsUnicode(false);

                entity.Property(e => e.BitRPFecha).HasColumnName("bitRPFecha");

                entity.HasOne(d => d.MovRequisicionPersonal)
               .WithMany(p => p.CatBitacoraRequisicionPersonal)
               .HasForeignKey(d => d.BitRPRPId)
               .HasConstraintName("FK_cat_BitacoraRequisicionPersonal_mov_RequisicionPersonal");

                entity.HasOne(d => d.CatDetalleFlujo)
               .WithMany(p => p.CatBitacoraRequisicionPersonal)
               .HasForeignKey(d => d.BitRPDetFlujoId)
               .HasConstraintName("FK_cat_BitacoraRequisicionPersonal_cat_DetalleFlujos");
            });

            modelBuilder.Entity<PuestosSP>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<MovPermisosPorTiempo>(entity =>
            {
                entity.HasKey(e => e.PPTId);

                entity.ToTable("mov_PermisosPorTiempo");

                entity.Property(e => e.PPTId).HasColumnName("pPTId");

                entity.Property(e => e.PPTEmpId).HasColumnName("pPTEmpId");

                entity.Property(e => e.PPTFechaInicio).HasColumnName("pPTFechaInicio");

                entity.Property(e => e.PPTFechaFinal).HasColumnName("pPTFechaFinal");

                entity.Property(e => e.PPTHoraEntrada).HasColumnName("pPTHoraEntrada");

                entity.Property(e => e.PPTHoraSalida).HasColumnName("pPTHoraSalida");

                entity.Property(e => e.PPTHoraSalidaComida).HasColumnName("pPTHoraSalidaComida");

                entity.Property(e => e.PPTHoraRegresoComida).HasColumnName("pPTHoraRegresoComida");

                entity.Property(e => e.PPTEstatusId).HasColumnName("pPTEstatusId");

                entity.Property(e => e.PPTReInciId).HasColumnName("pPTReInciId");

                entity.HasOne(d => d.Empleados)
               .WithMany(p => p.MovPermisosPorTiempo)
               .HasForeignKey(d => d.PPTEmpId)
               .HasConstraintName("FK_mov_PermisosPorTiempo_cat_Empleados");

                entity.HasOne(d => d.Estatus)
               .WithMany(p => p.MovPermisosPorTiempos)
               .HasForeignKey(d => d.PPTEstatusId)
               .HasConstraintName("FK_mov_PermisosPorTiempo_cat_Estatus");

                entity.HasOne(d => d.ReInci)
               .WithMany(p => p.MovPermisosPorTiempo)
               .HasForeignKey(d => d.PPTReInciId)
               .HasConstraintName("FK_mov_PermisosPorTiempo_cat_RegistroIncidencias");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
