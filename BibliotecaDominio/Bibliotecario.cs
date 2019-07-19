using BibliotecaDominio.IRepositorio;
using System;

namespace BibliotecaDominio
{
    public class Bibliotecario
    {
        public const string EL_LIBRO_NO_SE_ENCUENTRA_DISPONIBLE = "El libro no se encuentra disponible";
        private bool esPalindromo = false;
        private IRepositorioLibro libroRepositorio;
        private IRepositorioPrestamo prestamoRepositorio;

        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        /// <param name="libroRepositorio"></param>
        /// <param name="prestamoRepositorio"></param>
        public Bibliotecario(IRepositorioLibro libroRepositorio, IRepositorioPrestamo prestamoRepositorio)
        {
            this.libroRepositorio = libroRepositorio;
            this.prestamoRepositorio = prestamoRepositorio;
        }

        /// <summary>
        /// Ejecuta el préstamo de un libro.
        /// </summary>
        /// <param name="isbn">ISBN Unico del libro</param>
        /// <param name="nombreUsuario">Nombre del usuario que realiza el prestamo</param>
        public void Prestar(string isbn, string nombreUsuario)
        {
            #region Variables

            DateTime fechaEntregaMaxima = DateTime.Now;
            string respuesta = string.Empty;
            int sumaIsbn = 0;
            
            #endregion

            //Se obtiene el libro del repositorio para verificar existencia.
            var libroPrestamo = libroRepositorio.ObtenerPorIsbn(isbn);

            if (libroPrestamo != null)
            {
                //Se verifica si puede ser prestado o no el libro.
                if (!EsPrestado(isbn))
                {
                    //Se valida si el ISBN es palíndromo o no
                    esPalindromo = EsPalindromo(isbn);

                    if (esPalindromo)
                        respuesta = "los libros palíndromos solo se pueden utilizar en la biblioteca";
                    else
                    {
                        //Se realiza la suma de los numeros
                        for (int i = 0; i < isbn.Length; i++)
                        {
                            if (Char.IsNumber(isbn[i]))
                                sumaIsbn += Convert.ToInt32(isbn[i].ToString());
                        }

                        //Validar sumatoria de los dígitos del ISBN
                        if (sumaIsbn > 30)
                        {
                            fechaEntregaMaxima.AddDays(16);

                            if (fechaEntregaMaxima.DayOfWeek == DayOfWeek.Sunday)
                                fechaEntregaMaxima.AddDays(1);
                        }
                    }

                    //Instancia objeto libro
                    Prestamo prestamoLibro = new Prestamo(DateTime.Now, libroPrestamo, fechaEntregaMaxima, nombreUsuario);

                    //Se agrega préstamo a repositorio
                    prestamoRepositorio.Agregar(prestamoLibro);
                }
                else
                   throw new Exception("El libro no se encuentra disponible");
            }
            else
                respuesta = "El libro solicitado no se encuentra registrado";
        }

        /// <summary>
        /// Verifica si el ISBN del libro es palíndromo
        /// </summary>
        /// <param name="isbn">ISBN Unico del libro</param>
        /// <returns></returns>
        private bool EsPalindromo(string isbn)
        {
            if (isbn.Length <= 2) return true;
            else
                return isbn[0] == isbn[isbn.Length - 1] ? EsPalindromo(isbn.Substring(1, isbn.Length - 2)) : false;
        }

        /// <summary>
        /// Verifica si un libro se encuentra prestado o no
        /// </summary>
        /// <param name="isbn">ISBN Unico del libro</param>
        /// <returns></returns>
        public bool EsPrestado(string isbn)
        {
            //Se obtiene prestamo de libro por isbn
            var libroPrestado = prestamoRepositorio.ObtenerLibroPrestadoPorIsbn(isbn);

            //Devuelve respuesta de préstamo
            return libroPrestado != null ? true : false;
        }
    }
}
