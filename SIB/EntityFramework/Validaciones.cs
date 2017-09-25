using System;
using System.Data.Entity.Validation;
using System.Linq;

namespace SIB.EntityFramework
{
    partial class BibliotecaContext
    {
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                //Retorna los mensaje de errores como una lista de strings
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Se une la lista en un solo string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Se combina la excepcion con el los errores de validacion.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                //Lanza una excepcion con el mensaje mejorado.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }
}
