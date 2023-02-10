using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NucleoRH.Migrations
{
    public partial class UpdateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            //migrationBuilder.CreateTable(
            //    name: "cat_Departamentos",
            //    columns: table => new
            //    {
            //        depaID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        depaDescripcion = table.Column<string>(unicode: false, maxLength: 100, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_Departamentos", x => x.depaID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_DomiciliosEstados",
            //    columns: table => new
            //    {
            //        domiEstaID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        domiEstaDescripcion = table.Column<string>(maxLength: 50, nullable: true),
            //        domiEstaAbrev = table.Column<string>(fixedLength: true, maxLength: 2, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_DomiciliosEstados", x => x.domiEstaID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_Escolaridades",
            //    columns: table => new
            //    {
            //        escoID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        escoDescripcion = table.Column<string>(unicode: false, maxLength: 15, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_Escolaridades", x => x.escoID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_EstadosCiviles",
            //    columns: table => new
            //    {
            //        edocID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        edocDescripcion = table.Column<string>(unicode: false, maxLength: 15, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_EstadosCiviles", x => x.edocID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_Estatus",
            //    columns: table => new
            //    {
            //        estID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        estDescripcion = table.Column<string>(unicode: false, maxLength: 15, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_Estatus", x => x.estID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_Festivos",
            //    columns: table => new
            //    {
            //        festID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        festDescripcion = table.Column<string>(unicode: false, maxLength: 35, nullable: true),
            //        festFechaDesde = table.Column<DateTime>(type: "date", nullable: true),
            //        festFechaHasta = table.Column<DateTime>(type: "date", nullable: true),
            //        festGuardia = table.Column<int>(nullable: true),
            //        festObservaciones = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_Horarios",
            //    columns: table => new
            //    {
            //        horaID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        horaEntrada = table.Column<TimeSpan>(nullable: true),
            //        horaComidaSalida = table.Column<TimeSpan>(nullable: true),
            //        horaComidaEntrada = table.Column<TimeSpan>(nullable: true),
            //        horaSalida = table.Column<TimeSpan>(nullable: true),
            //        horaSabadoEntrada = table.Column<TimeSpan>(nullable: true),
            //        horaSabadoComidaSalida = table.Column<TimeSpan>(nullable: true),
            //        horaSabadoComidaEntrada = table.Column<TimeSpan>(nullable: true),
            //        horaSabadoSalida = table.Column<TimeSpan>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_Horarios", x => x.horaID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_Incidencias",
            //    columns: table => new
            //    {
            //        inciID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        inciDescripcion = table.Column<string>(unicode: false, maxLength: 20, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_Incidencias", x => x.inciID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_JornadasLaborales",
            //    columns: table => new
            //    {
            //        jornaID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        jornaDescripcion = table.Column<string>(unicode: false, maxLength: 15, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_JornadasLaborales", x => x.jornaID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_Periodos",
            //    columns: table => new
            //    {
            //        perID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        perNum = table.Column<int>(nullable: false),
            //        perFechaDesde = table.Column<DateTime>(type: "date", nullable: true),
            //        perFechaHasta = table.Column<DateTime>(type: "date", nullable: true),
            //        perCerrado = table.Column<DateTime>(type: "date", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_Plantillas",
            //    columns: table => new
            //    {
            //        plantiID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        plantiDepaID = table.Column<int>(nullable: false),
            //        plantiSucuID = table.Column<int>(nullable: false),
            //        plantiPuestoID = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_Sexos",
            //    columns: table => new
            //    {
            //        sexID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        sexDescripcion = table.Column<string>(unicode: false, maxLength: 20, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_Sexos", x => x.sexID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_TrabajadorTipos",
            //    columns: table => new
            //    {
            //        trabTipoID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        trabTipoDescripcion = table.Column<string>(unicode: false, maxLength: 20, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_TrabajadorTipos", x => x.trabTipoID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_TurnosLaborales",
            //    columns: table => new
            //    {
            //        turID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        turDescripcion = table.Column<string>(unicode: false, maxLength: 15, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_TurnosLaborales", x => x.turID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_Vacunacion",
            //    columns: table => new
            //    {
            //        vacID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        vacDescripcion = table.Column<string>(unicode: false, maxLength: 50, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_VacunacionTipos", x => x.vacID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cfg_ParametrosSistema",
            //    columns: table => new
            //    {
            //        paramID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        paramMinutosTolerancia = table.Column<int>(nullable: true),
            //        paramMinutosComida = table.Column<int>(nullable: true),
            //        paramGafetteFrente = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
            //        paramGafetteReverso = table.Column<string>(unicode: false, maxLength: 250, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //    });

            //migrationBuilder.CreateTable(
            //    name: "mov_EmpleadosDomicilios",
            //    columns: table => new
            //    {
            //        domiID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        domiEmpID = table.Column<int>(nullable: true),
            //        domiCalle = table.Column<string>(maxLength: 50, nullable: true),
            //        domiNumExt = table.Column<int>(nullable: true),
            //        domiNumInt = table.Column<string>(maxLength: 10, nullable: true),
            //        domiColoID = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_mov_EmpleadosDomicilios", x => x.domiID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "mov_EmpleadosHorarios",
            //    columns: table => new
            //    {
            //        empHoraID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        empHoraEmpID = table.Column<int>(nullable: false),
            //        empHoraHorID = table.Column<int>(nullable: false),
            //        empHoraFechaDesde = table.Column<DateTime>(type: "datetime", nullable: true),
            //        empHoraFechaHasta = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //    });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateTable(
            //    name: "cat_Areas",
            //    columns: table => new
            //    {
            //        areaID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        areaDescripcion = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
            //        areaDepaID = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_Areas", x => x.areaID);
            //        table.ForeignKey(
            //            name: "FK_cat_Areas_cat_Departamentos",
            //            column: x => x.areaDepaID,
            //            principalTable: "cat_Departamentos",
            //            principalColumn: "depaID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_DomiciliosMunicipios",
            //    columns: table => new
            //    {
            //        domiMunicID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        domiMunicDescripcion = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
            //        domiMunicEstaID = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_DomiciliosMunicipios", x => x.domiMunicID);
            //        table.ForeignKey(
            //            name: "FK_cat_DomiciliosMunicipios_cat_DomiciliosEstados",
            //            column: x => x.domiMunicEstaID,
            //            principalTable: "cat_DomiciliosEstados",
            //            principalColumn: "domiEstaID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_Puestos",
            //    columns: table => new
            //    {
            //        puestoID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        puestoDescripcion = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
            //        puestoAreaID = table.Column<int>(nullable: false),
            //        puestoJerarquiaSuperiorPuestoID = table.Column<int>(nullable: true),
            //        puestoJerarquiaOrden = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_Puestos", x => x.puestoID);
            //        table.ForeignKey(
            //            name: "FK_cat_Puestos_cat_Areas",
            //            column: x => x.puestoAreaID,
            //            principalTable: "cat_Areas",
            //            principalColumn: "areaID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_DomiciliosColonias",
            //    columns: table => new
            //    {
            //        domiColoID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        domiColoCP = table.Column<int>(nullable: true),
            //        domiColoDescripcion = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
            //        domiMunicID = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_DomiciliosColonias", x => x.domiColoID);
            //        table.ForeignKey(
            //            name: "FK_cat_DomiciliosColonias_cat_DomiciliosMunicipios",
            //            column: x => x.domiMunicID,
            //            principalTable: "cat_DomiciliosMunicipios",
            //            principalColumn: "domiMunicID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_Patrones",
            //    columns: table => new
            //    {
            //        patID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        patAbrev = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
            //        patDescripcion = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
            //        patRegistro = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
            //        patRFC = table.Column<string>(unicode: false, maxLength: 18, nullable: true),
            //        patColoID = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_Patrones", x => x.patID);
            //        table.ForeignKey(
            //            name: "FK_cat_Patrones_cat_DomiciliosColonias",
            //            column: x => x.patColoID,
            //            principalTable: "cat_DomiciliosColonias",
            //            principalColumn: "domiColoID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_Sucursales",
            //    columns: table => new
            //    {
            //        SucuID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SucuNCorto = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
            //        SucuNombre = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
            //        SucuPatID = table.Column<int>(nullable: true),
            //        SucuEmail = table.Column<string>(unicode: false, maxLength: 150, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_Sucursales", x => x.SucuID);
            //        table.ForeignKey(
            //            name: "FK_cat_Sucursales_cat_Patrones",
            //            column: x => x.SucuPatID,
            //            principalTable: "cat_Patrones",
            //            principalColumn: "patID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_Checadores",
            //    columns: table => new
            //    {
            //        checID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        checDescripcion = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
            //        checSucuID = table.Column<int>(nullable: false),
            //        checIP = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
            //        checPathDescarga = table.Column<string>(unicode: false, nullable: true),
            //        checMinutosDescarga = table.Column<int>(nullable: true),
            //        checUltimaDescarga = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_Checadores", x => x.checID);
            //        table.ForeignKey(
            //            name: "FK_cat_Checadores_cat_Sucursales",
            //            column: x => x.checSucuID,
            //            principalTable: "cat_Sucursales",
            //            principalColumn: "SucuID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "cat_Empleados",
            //    columns: table => new
            //    {
            //        empID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        empNumero = table.Column<int>(nullable: true),
            //        empNombre = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
            //        empPaterno = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
            //        empMaterno = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
            //        empTelefono = table.Column<decimal>(type: "numeric(18, 0)", nullable: true),
            //        empCelular = table.Column<decimal>(type: "numeric(18, 0)", nullable: true),
            //        empEmail = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
            //        empFechaIngreso = table.Column<DateTime>(type: "date", nullable: true),
            //        empRFC = table.Column<string>(unicode: false, maxLength: 13, nullable: true),
            //        empCURP = table.Column<string>(unicode: false, maxLength: 18, nullable: true),
            //        empIMSS = table.Column<string>(unicode: false, maxLength: 11, nullable: true),
            //        empEscoID = table.Column<int>(nullable: true),
            //        empEdocID = table.Column<int>(nullable: false),
            //        empSexID = table.Column<int>(nullable: false),
            //        empEstID = table.Column<int>(nullable: false),
            //        empComentarios = table.Column<string>(unicode: false, nullable: true),
            //        empPatID = table.Column<int>(nullable: false),
            //        empPuestoID = table.Column<int>(nullable: false),
            //        empSucuID = table.Column<int>(nullable: false),
            //        empJornaID = table.Column<int>(nullable: false),
            //        empTrabTipoID = table.Column<int>(nullable: true),
            //        empTurID = table.Column<int>(nullable: false),
            //        empNacFecha = table.Column<DateTime>(type: "date", nullable: true),
            //        empNacMunicID = table.Column<int>(nullable: true),
            //        empVacID = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_cat_Empleados", x => x.empID);
            //        table.ForeignKey(
            //            name: "FK_cat_Empleados_cat_EstadosCiviles",
            //            column: x => x.empEdocID,
            //            principalTable: "cat_EstadosCiviles",
            //            principalColumn: "edocID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_cat_Empleados_cat_Escolaridades",
            //            column: x => x.empEscoID,
            //            principalTable: "cat_Escolaridades",
            //            principalColumn: "escoID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_cat_Empleados_cat_Estatus",
            //            column: x => x.empEstID,
            //            principalTable: "cat_Estatus",
            //            principalColumn: "estID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_cat_Empleados_cat_JornadasLaborales",
            //            column: x => x.empJornaID,
            //            principalTable: "cat_JornadasLaborales",
            //            principalColumn: "jornaID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_cat_Empleados_cat_DomiciliosMunicipios",
            //            column: x => x.empNacMunicID,
            //            principalTable: "cat_DomiciliosMunicipios",
            //            principalColumn: "domiMunicID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_cat_Empleados_cat_Patrones",
            //            column: x => x.empPatID,
            //            principalTable: "cat_Patrones",
            //            principalColumn: "patID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_cat_Empleados_cat_Puestos",
            //            column: x => x.empPuestoID,
            //            principalTable: "cat_Puestos",
            //            principalColumn: "puestoID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_cat_Empleados_cat_Sexos",
            //            column: x => x.empSexID,
            //            principalTable: "cat_Sexos",
            //            principalColumn: "sexID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_cat_Empleados_cat_Sucursales",
            //            column: x => x.empSucuID,
            //            principalTable: "cat_Sucursales",
            //            principalColumn: "SucuID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_cat_Empleados_cat_TrabajadorTipos",
            //            column: x => x.empTrabTipoID,
            //            principalTable: "cat_TrabajadorTipos",
            //            principalColumn: "trabTipoID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_cat_Empleados_cat_TurnosLaborales",
            //            column: x => x.empTurID,
            //            principalTable: "cat_TurnosLaborales",
            //            principalColumn: "turID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_cat_Empleados_cat_Vacunacion",
            //            column: x => x.empVacID,
            //            principalTable: "cat_Vacunacion",
            //            principalColumn: "vacID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "mov_EmpleadosAsistencias",
            //    columns: table => new
            //    {
            //        empAsisID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        empAsisChecID = table.Column<int>(nullable: false),
            //        empAsisEmpID = table.Column<int>(nullable: false),
            //        empAsisHoraID = table.Column<int>(nullable: false),
            //        empAsisFecha = table.Column<DateTime>(type: "datetime", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_mov_EmpleadosAsistencias", x => x.empAsisID);
            //        table.ForeignKey(
            //            name: "FK_mov_EmpleadosAsistencias_cat_Checadores",
            //            column: x => x.empAsisChecID,
            //            principalTable: "cat_Checadores",
            //            principalColumn: "checID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_mov_EmpleadosAsistencias_cat_Empleados",
            //            column: x => x.empAsisEmpID,
            //            principalTable: "cat_Empleados",
            //            principalColumn: "empID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_mov_EmpleadosAsistencias_cat_Horarios",
            //            column: x => x.empAsisHoraID,
            //            principalTable: "cat_Horarios",
            //            principalColumn: "horaID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "mov_EmpleadosIncidencias",
            //    columns: table => new
            //    {
            //        empInciID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        empInciEmpID = table.Column<int>(nullable: false),
            //        empInciInciID = table.Column<int>(nullable: false),
            //        empInciFechaDesde = table.Column<DateTime>(type: "datetime", nullable: true),
            //        empInciFechaHasta = table.Column<DateTime>(type: "datetime", nullable: true),
            //        empInciHoraID = table.Column<int>(nullable: true),
            //        empInciObs = table.Column<string>(unicode: false, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_mov_EmpleadosIncidencias", x => x.empInciID);
            //        table.ForeignKey(
            //            name: "FK_mov_EmpleadosIncidencias_cat_Empleados",
            //            column: x => x.empInciEmpID,
            //            principalTable: "cat_Empleados",
            //            principalColumn: "empID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_mov_EmpleadosIncidencias_cat_Horarios",
            //            column: x => x.empInciHoraID,
            //            principalTable: "cat_Horarios",
            //            principalColumn: "horaID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_mov_EmpleadosIncidencias_cat_Incidencias",
            //            column: x => x.empInciInciID,
            //            principalTable: "cat_Incidencias",
            //            principalColumn: "inciID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "mov_EmpleadosSueldos",
            //    columns: table => new
            //    {
            //        empSdoID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        empSdoEmpID = table.Column<int>(nullable: false),
            //        empSdoFechaAlta = table.Column<DateTime>(type: "datetime", nullable: true),
            //        empSdoSueldoDia = table.Column<decimal>(type: "numeric(18, 0)", nullable: true),
            //        empSdoSDI = table.Column<decimal>(type: "numeric(18, 0)", nullable: true),
            //        empSdoPremioAsis = table.Column<decimal>(type: "numeric(18, 0)", nullable: true),
            //        empSdoPremioPunt = table.Column<decimal>(type: "numeric(18, 0)", nullable: true),
            //        empSdoDespensa = table.Column<decimal>(type: "numeric(18, 0)", nullable: true),
            //        empSdoApoyoViviTrans = table.Column<decimal>(type: "numeric(18, 0)", nullable: true),
            //        empSdoBono = table.Column<decimal>(type: "numeric(18, 0)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_mov_EmpleadosSueldos", x => x.empSdoID);
            //        table.ForeignKey(
            //            name: "FK_mov_EmpleadosSueldos_cat_Empleados1",
            //            column: x => x.empSdoEmpID,
            //            principalTable: "cat_Empleados",
            //            principalColumn: "empID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_Areas_areaDepaID",
            //    table: "cat_Areas",
            //    column: "areaDepaID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_Checadores_checSucuID",
            //    table: "cat_Checadores",
            //    column: "checSucuID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_DomiciliosColonias_domiMunicID",
            //    table: "cat_DomiciliosColonias",
            //    column: "domiMunicID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_DomiciliosMunicipios_domiMunicEstaID",
            //    table: "cat_DomiciliosMunicipios",
            //    column: "domiMunicEstaID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_Empleados_empEdocID",
            //    table: "cat_Empleados",
            //    column: "empEdocID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_Empleados_empEscoID",
            //    table: "cat_Empleados",
            //    column: "empEscoID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_Empleados_empEstID",
            //    table: "cat_Empleados",
            //    column: "empEstID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_Empleados_empJornaID",
            //    table: "cat_Empleados",
            //    column: "empJornaID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_Empleados_empNacMunicID",
            //    table: "cat_Empleados",
            //    column: "empNacMunicID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_Empleados_empPatID",
            //    table: "cat_Empleados",
            //    column: "empPatID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_Empleados_empPuestoID",
            //    table: "cat_Empleados",
            //    column: "empPuestoID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_Empleados_empSexID",
            //    table: "cat_Empleados",
            //    column: "empSexID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_Empleados_empSucuID",
            //    table: "cat_Empleados",
            //    column: "empSucuID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_Empleados_empTrabTipoID",
            //    table: "cat_Empleados",
            //    column: "empTrabTipoID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_Empleados_empTurID",
            //    table: "cat_Empleados",
            //    column: "empTurID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_Empleados_empVacID",
            //    table: "cat_Empleados",
            //    column: "empVacID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_Patrones_patColoID",
            //    table: "cat_Patrones",
            //    column: "patColoID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_Puestos_puestoAreaID",
            //    table: "cat_Puestos",
            //    column: "puestoAreaID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_cat_Sucursales_SucuPatID",
            //    table: "cat_Sucursales",
            //    column: "SucuPatID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_mov_EmpleadosAsistencias_empAsisChecID",
            //    table: "mov_EmpleadosAsistencias",
            //    column: "empAsisChecID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_mov_EmpleadosAsistencias_empAsisEmpID",
            //    table: "mov_EmpleadosAsistencias",
            //    column: "empAsisEmpID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_mov_EmpleadosAsistencias_empAsisHoraID",
            //    table: "mov_EmpleadosAsistencias",
            //    column: "empAsisHoraID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_mov_EmpleadosIncidencias_empInciEmpID",
            //    table: "mov_EmpleadosIncidencias",
            //    column: "empInciEmpID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_mov_EmpleadosIncidencias_empInciHoraID",
            //    table: "mov_EmpleadosIncidencias",
            //    column: "empInciHoraID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_mov_EmpleadosIncidencias_empInciInciID",
            //    table: "mov_EmpleadosIncidencias",
            //    column: "empInciInciID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_mov_EmpleadosSueldos",
            //    table: "mov_EmpleadosSueldos",
            //    column: "empSdoEmpID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            //migrationBuilder.DropTable(
            //    name: "cat_Festivos");

            //migrationBuilder.DropTable(
            //    name: "cat_Periodos");

            //migrationBuilder.DropTable(
            //    name: "cat_Plantillas");

            //migrationBuilder.DropTable(
            //    name: "cfg_ParametrosSistema");

            //migrationBuilder.DropTable(
            //    name: "mov_EmpleadosAsistencias");

            //migrationBuilder.DropTable(
            //    name: "mov_EmpleadosDomicilios");

            //migrationBuilder.DropTable(
            //    name: "mov_EmpleadosHorarios");

            //migrationBuilder.DropTable(
            //    name: "mov_EmpleadosIncidencias");

            //migrationBuilder.DropTable(
            //    name: "mov_EmpleadosSueldos");

            //migrationBuilder.DropTable(
            //    name: "AspNetRoles");

            //migrationBuilder.DropTable(
            //    name: "AspNetUsers");

            //migrationBuilder.DropTable(
            //    name: "cat_Checadores");

            //migrationBuilder.DropTable(
            //    name: "cat_Horarios");

            //migrationBuilder.DropTable(
            //    name: "cat_Incidencias");

            //migrationBuilder.DropTable(
            //    name: "cat_Empleados");

            //migrationBuilder.DropTable(
            //    name: "cat_EstadosCiviles");

            //migrationBuilder.DropTable(
            //    name: "cat_Escolaridades");

            //migrationBuilder.DropTable(
            //    name: "cat_Estatus");

            //migrationBuilder.DropTable(
            //    name: "cat_JornadasLaborales");

            //migrationBuilder.DropTable(
            //    name: "cat_Puestos");

            //migrationBuilder.DropTable(
            //    name: "cat_Sexos");

            //migrationBuilder.DropTable(
            //    name: "cat_Sucursales");

            //migrationBuilder.DropTable(
            //    name: "cat_TrabajadorTipos");

            //migrationBuilder.DropTable(
            //    name: "cat_TurnosLaborales");

            //migrationBuilder.DropTable(
            //    name: "cat_Vacunacion");

            //migrationBuilder.DropTable(
            //    name: "cat_Areas");

            //migrationBuilder.DropTable(
            //    name: "cat_Patrones");

            //migrationBuilder.DropTable(
            //    name: "cat_Departamentos");

            //migrationBuilder.DropTable(
            //    name: "cat_DomiciliosColonias");

            //migrationBuilder.DropTable(
            //    name: "cat_DomiciliosMunicipios");

            //migrationBuilder.DropTable(
            //    name: "cat_DomiciliosEstados");
        }
    }
}
