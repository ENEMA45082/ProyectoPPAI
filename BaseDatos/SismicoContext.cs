using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite; // Asegúrate de incluir esta directiva using
using ProyectoPPAI.Clases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace ProyectoPPAI.BaseDatos
{
    // DbContext principal para manejar la base de datos
    public class SismicoContext : DbContext
    {
        // DbSets para las tablas principales
        public DbSet<EventoSismicoBD> EventosSismicos { get; set; }
        public DbSet<EstacionSismologicaBD> EstacionesSismologicas { get; set; }
        public DbSet<SismografoBD> Sismografos { get; set; }
        public DbSet<SerieTemporalBD> SeriesTemporales { get; set; }
        public DbSet<MuestraSismicaBD> MuestrasSismicas { get; set; }
        public DbSet<DetalleMuestraSismicaBD> DetallesMuestras { get; set; }
        public DbSet<CambioEstadoBD> CambiosEstado { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configurar SQLite con ruta completa
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var dbPath = Path.Combine(baseDirectory, "BaseDatos", "eventos_sismicos.db");
            var connectionString = $"Data Source={dbPath}";
            
            // Logging para verificar la ruta
            Console.WriteLine($"=== CONFIGURACIÓN DE BASE DE DATOS ===");
            Console.WriteLine($"Directorio base: {baseDirectory}");
            Console.WriteLine($"Ruta completa BD: {dbPath}");
            Console.WriteLine($"¿Archivo existe?: {File.Exists(dbPath)}");
            
            // Crear directorio si no existe
            var dbDirectory = Path.GetDirectoryName(dbPath);
            if (!Directory.Exists(dbDirectory))
            {
                Directory.CreateDirectory(dbDirectory);
                Console.WriteLine($"Directorio creado: {dbDirectory}");
            }
            
            optionsBuilder.UseSqlite(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar relaciones explícitamente
            modelBuilder.Entity<SismografoBD>()
                .HasOne(s => s.Estacion)
                .WithMany(e => e.Sismografos)
                .HasForeignKey(s => s.EstacionId);

            modelBuilder.Entity<SerieTemporalBD>()
                .HasOne(st => st.EventoSismico)
                .WithMany(e => e.SeriesTemporales)
                .HasForeignKey(st => st.EventoSismicoId);

            modelBuilder.Entity<SerieTemporalBD>()
                .HasOne(st => st.Sismografo)
                .WithMany(s => s.SeriesTemporales)
                .HasForeignKey(st => st.SismografoId);

            modelBuilder.Entity<MuestraSismicaBD>()
                .HasOne(m => m.SerieTemporal)
                .WithMany(st => st.Muestras)
                .HasForeignKey(m => m.SerieTemporalId);

            modelBuilder.Entity<DetalleMuestraSismicaBD>()
                .HasOne(d => d.Muestra)
                .WithMany(m => m.Detalles)
                .HasForeignKey(d => d.MuestraId);

            modelBuilder.Entity<CambioEstadoBD>()
                .HasOne(c => c.EventoSismico)
                .WithMany(e => e.CambiosEstado)
                .HasForeignKey(c => c.EventoSismicoId);

            // Configurar campo UsuarioModificacion como nullable
            modelBuilder.Entity<CambioEstadoBD>()
                .Property(c => c.UsuarioModificacion)
                .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }
    }

    // Entidades simplificadas para Entity Framework
    [Table("EventosSismicos")]
    public class EventoSismicoBD
    {
        [Key]
        public int Id { get; set; }
        
        public DateTime FechaHoraOcurrencia { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public double LatitudEpicentro { get; set; }
        public double LongitudEpicentro { get; set; }
        public double LatitudHipocentro { get; set; }
        public double LongitudHipocentro { get; set; }
        public double ValorMagnitud { get; set; }
        public string DescripcionMagnitud { get; set; } = "";
        public string EstadoActual { get; set; } = "";
        public string Clasificacion { get; set; } = "";
        public string OrigenGeneracion { get; set; } = "";
        public string DescripcionOrigen { get; set; } = "";
        public string Alcance { get; set; } = "";
        public string DescripcionAlcance { get; set; } = "";
        
        // Navegación
        public virtual ICollection<SerieTemporalBD> SeriesTemporales { get; set; } = new List<SerieTemporalBD>();
        public virtual ICollection<CambioEstadoBD> CambiosEstado { get; set; } = new List<CambioEstadoBD>();
    }

    [Table("EstacionesSismologicas")]
    public class EstacionSismologicaBD
    {
        [Key]
        public int Id { get; set; }
        
        public string Nombre { get; set; } = "";
        public string Codigo { get; set; } = "";
        
        // Navegación
        public virtual ICollection<SismografoBD> Sismografos { get; set; } = new List<SismografoBD>();
    }

    [Table("Sismografos")]
    public class SismografoBD
    {
        [Key]
        public int Id { get; set; }
        
        public DateTime FechaAdquisicion { get; set; }
        public string Identificador { get; set; } = "";
        public string NumeroSerie { get; set; } = "";
        
        // Claves foráneas
        public int EstacionId { get; set; }
        
        // Navegación
        public virtual EstacionSismologicaBD Estacion { get; set; } = null!;
        public virtual ICollection<SerieTemporalBD> SeriesTemporales { get; set; } = new List<SerieTemporalBD>();
    }

    [Table("SeriesTemporales")]
    public class SerieTemporalBD
    {
        [Key]
        public int Id { get; set; }
        
        public string CondicionAlarma { get; set; } = "";
        public DateTime FechaHoraInicioRegistroMuestras { get; set; }
        public DateTime FechaHoraRegistro { get; set; }
        public double FrecuenciaMuestreo { get; set; }
        
        // Claves foráneas
        public int EventoSismicoId { get; set; }
        public int SismografoId { get; set; }
        
        // Navegación
        public virtual EventoSismicoBD EventoSismico { get; set; } = null!;
        public virtual SismografoBD Sismografo { get; set; } = null!;
        public virtual ICollection<MuestraSismicaBD> Muestras { get; set; } = new List<MuestraSismicaBD>();
    }

    [Table("MuestrasSismicas")]
    public class MuestraSismicaBD
    {
        [Key]
        public int Id { get; set; }
        
        public DateTime FechaHoraMuestra { get; set; }
        public string DetalleMuestra { get; set; } = "";
        
        // Clave foránea
        public int SerieTemporalId { get; set; }
        
        // Navegación
        public virtual SerieTemporalBD SerieTemporal { get; set; } = null!;
        public virtual ICollection<DetalleMuestraSismicaBD> Detalles { get; set; } = new List<DetalleMuestraSismicaBD>();
    }

    [Table("DetallesMuestras")]
    public class DetalleMuestraSismicaBD
    {
        [Key]
        public int Id { get; set; }
        
        public double Valor { get; set; }
        public string TipoDato { get; set; } = "";
        public string DescripcionTipoDato { get; set; } = "";
        
        // Clave foránea
        public int MuestraId { get; set; }
        
        // Navegación
        public virtual MuestraSismicaBD Muestra { get; set; } = null!;
    }

    [Table("CambiosEstado")]
    public class CambioEstadoBD
    {
        [Key]
        public int Id { get; set; }
        
        public DateTime FechaHoraCambio { get; set; }
        public string EstadoAnterior { get; set; } = "";
        public string EstadoNuevo { get; set; } = "";
        public string? UsuarioModificacion { get; set; }
        
        // Clave foránea
        public int EventoSismicoId { get; set; }
        
        // Navegación
        public virtual EventoSismicoBD EventoSismico { get; set; } = null!;
    }
}