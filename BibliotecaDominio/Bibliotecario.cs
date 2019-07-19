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
                    //Se valida si el Isbn es palíndromo o no
                    esPalindromo = EsPalindromo(isbn);

                    if (esPalindromo)
                        throw new Exception("los libros palíndromos solo se pueden utilizar en la biblioteca");
                    else
                    {
                        //Suma caracteres Isbn
                        sumaIsbn = SumarIsbn(isbn);

                        //Calcula fecha máxima de entrega
                        fechaEntregaMaxima = CalcularFechaMaximaEntrega(sumaIsbn);
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
                throw new Exception("El libro solicitado no se encuentra registrado");
        }
        /// <summary>
        /// Suma los caracteres del Isbn
        /// </summary>
        /// <param name="isbn">Isbn unico del libro</param>
        /// <returns></returns>
        public int SumarIsbn(string isbn)
        {
            int sumaIsbn = 0;

            //Se realiza la suma de los numeros
            for (int i = 0; i < isbn.Length; i++)
            {
                if (Char.IsNumber(isbn[i]))
                    sumaIsbn += Convert.ToInt32(isbn[i].ToString());
            }

            return sumaIsbn;
        }
        /// <summary>
        /// Calcula fecha máxima de entrega 
        /// </summary>
        /// <param name="sumaIsbn">Suma de caracteres de Isbn</param>
        /// <returns></returns>
        public DateTime CalcularFechaMaximaEntrega(int sumaIsbn)
        {
            DateTime fechaEntregaMaxima = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            if (sumaIsbn > 30)
            {
                fechaEntregaMaxima = fechaEntregaMaxima.AddDays(17);

                if (fechaEntregaMaxima.DayOfWeek == DayOfWeek.Sunday)
                    fechaEntregaMaxima.AddDays(1);

                return fechaEntregaMaxima;
            }
            else
                return new DateTime(0001, 01, 01);

        }

        /// <summary>
        /// Verifica si el ISBN del libro es palíndromo
        /// </summary>
        /// <param name="isbn">ISBN Unico del libro</param>
        /// <returns></returns>
        public bool EsPalindromo(string isbn)
        {
            if (isbn.Length <= 2) return true;
            else
                return isbn[0] == isbn[isbn.Length - 1] ? EsPalindromo(isbn.Substring(1, isbn.Length - 2)) : false;
        }

        /// <summary>
        /// Verifica si un libro se encuentra prestado o no
        /// </summary>
        /// <param name="isbn">Isbn Unico del libro</param>
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
