﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIB.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BibliotecaEntities : DbContext
    {
        public BibliotecaEntities()
            : base("name=BibliotecaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Alumnos> Alumnos { get; set; }
        public virtual DbSet<Autores> Autores { get; set; }
        public virtual DbSet<Editorial> Editorial { get; set; }
        public virtual DbSet<Libros> Libros { get; set; }
        public virtual DbSet<Maestros> Maestros { get; set; }
        public virtual DbSet<Permisos> Permisos { get; set; }
        public virtual DbSet<Prestamos> Prestamos { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<UsuarioPermisos> UsuarioPermisos { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
    }
}
