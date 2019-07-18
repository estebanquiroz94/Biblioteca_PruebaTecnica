using BibliotecaDominio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDominio
{
    public class Bibliotecario
    {
        public const string EL_LIBRO_NO_SE_ENCUENTRA_DISPONIBLE = "El libro no se encuentra disponible";
        private bool esPalindromo = false;
        private  IRepositorioLibro libroRepositorio;
        private  IRepositorioPrestamo prestamoRepositorio;

        public Bibliotecario(IRepositorioLibro libroRepositorio, IRepositorioPrestamo prestamoRepositorio)
        {
            this.libroRepositorio = libroRepositorio;
            this.prestamoRepositorio = prestamoRepositorio;
        }

        public void Prestar(string isbn, string nombreUsuario)
        {
            //Se valida si el ISBN es palíndromo o no
            esPalindromo = EsPalindromo(isbn);

            //prestamoRepositorio.Agregar();

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
