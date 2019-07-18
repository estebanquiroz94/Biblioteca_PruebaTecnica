using BibliotecaDominio.IRepositorio;
using System;
using System.Linq;

namespace BibliotecaDominio
{
    public class Bibliotecario
    {
        public const string EL_LIBRO_NO_SE_ENCUENTRA_DISPONIBLE = "El libro no se encuentra disponible";
        private bool esPalindromo = false;
        private IRepositorioLibro libroRepositorio;
        private IRepositorioPrestamo prestamoRepositorio;

        public Bibliotecario(IRepositorioLibro libroRepositorio, IRepositorioPrestamo prestamoRepositorio)
        {
            this.libroRepositorio = libroRepositorio;
            this.prestamoRepositorio = prestamoRepositorio;
        }

        public void Prestar(string isbn, string nombreUsuario)
        {
            string respuesta = string.Empty;
            int sumaIsbn = 0;
            //Se obtiene el libro del repositorio para verificar existencia.
            var libroPrestamo = libroRepositorio.ObtenerPorIsbn(isbn);

            if (libroPrestamo != null)
            {
                //Se obtiene prestamo de libro por isbn
                var libroPrestado = prestamoRepositorio.ObtenerLibroPrestadoPorIsbn(isbn);

                //Se verifica si puede ser prestado o no el libro.
                if (libroPrestado == null)
                {
                    //Se valida si el ISBN es palíndromo o no
                    esPalindromo = EsPalindromo(isbn);

                    if (esPalindromo)
                        respuesta = "los libros palíndromos solo se pueden utilizar en la biblioteca";
                    else
                    {
                        for (int i = 0; i < isbn.Length; i++)
                        {
                            if(Char.IsNumber(isbn[i]))
                                sumaIsbn += Convert.ToInt32(isbn[i].ToString());
                        }
                    }

                    //Instancia objeto libro
                    Prestamo a = new Prestamo(DateTime.Now, libroPrestamo, DateTime.Now, nombreUsuario);

                    //Se agrega préstamo a repositorio
                    prestamoRepositorio.Agregar(a);
                }
                else
                {

                }
            }
            throw new Exception("se debe implementar este método");
        }

        private bool EsPalindromo(string isbn)
        {
            if (isbn.Length <= 2) return true;
            else
                //Llamado método de recursividad
                return isbn[0] == isbn[isbn.Length - 1] ? EsPalindromo(isbn.Substring(1, isbn.Length - 2)) : false;
        }

        public bool EsPrestado(string isbn)
        {
            return true;
        }
    }
}
