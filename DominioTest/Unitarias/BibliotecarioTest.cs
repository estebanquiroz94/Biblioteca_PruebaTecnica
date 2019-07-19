using System;
using System.Collections.Generic;
using System.Text;
using BibliotecaDominio;
using BibliotecaDominio.IRepositorio;
using DominioTest.TestDataBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DominioTest.Unitarias
{
    [TestClass]
    public class BibliotecarioTest
    {
        public BibliotecarioTest()
        {

        }
        public Mock<IRepositorioLibro> repositorioLibro;
        public Mock<IRepositorioPrestamo> repositorioPrestamo;
        public const String ISBN_PALINDROMO = "1A55A1";
        public const String ISBN_NO_PALINDROMO = "109A8r56";
        public const String ISBN_MAYOR_TREINTA = "25875aRZ38";
        public const String ISBN_MENOR_TREINTA = "123k51wYXC2L3";

        [TestInitialize]
        public void setup()
        {
            repositorioLibro = new Mock<IRepositorioLibro>();
           repositorioPrestamo = new Mock<IRepositorioPrestamo>();
        }

        [TestMethod]
        public void EsPrestado()
        {
                // Arrange
                var libroTestDataBuilder = new LibroTestDataBuilder();
                Libro libro = libroTestDataBuilder.Build();

                repositorioPrestamo.Setup(r => r.ObtenerLibroPrestadoPorIsbn(libro.Isbn)).Returns(libro);

                // Act
                Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro.Object, repositorioPrestamo.Object);
                var esprestado = bibliotecario.EsPrestado(libro.Isbn);

                // Assert
                Assert.AreEqual(esprestado, true);

        }

        [TestMethod]
        public void LibroNoPrestadoTest()
        {
            // Arrange
            var libroTestDataBuilder = new LibroTestDataBuilder();
            Libro libro = libroTestDataBuilder.Build();
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro.Object, repositorioPrestamo.Object);
            repositorioPrestamo.Setup(r => r.ObtenerLibroPrestadoPorIsbn(libro.Isbn)).Equals(null);

            // Act
            var esprestado = bibliotecario.EsPrestado(libro.Isbn);

            // Assert
            Assert.IsFalse(esprestado);
        }

        [TestMethod]
        public void IsbnEsPalindromoTest()
        {
            // Act
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro.Object, repositorioPrestamo.Object);


            var esPalindomo = bibliotecario.EsPalindromo(ISBN_PALINDROMO);

            // Assert
            Assert.AreEqual(esPalindomo, true);

        }
        [TestMethod]
        public void IsbnNoEsPalindromoTest()
        {
            // Act
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro.Object, repositorioPrestamo.Object);


            var esPalindomo = bibliotecario.EsPalindromo(ISBN_NO_PALINDROMO);

            // Assert
            Assert.AreEqual(esPalindomo, false);

        }

        [TestMethod]
        public void SumarIsbnTest()
        {
            // Act
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro.Object, repositorioPrestamo.Object);

            var sumaIsbn = bibliotecario.SumarIsbn(ISBN_MAYOR_TREINTA);

            if (sumaIsbn > 0)
                Assert.IsTrue(true);
            else
                Assert.Fail();

        }

        [TestMethod]
        public void FechaEntregaSumaMayorTreintaTest()
        {
            DateTime fechaMaximaEsperada = new DateTime(2019, 08, 5);

            // Act
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro.Object, repositorioPrestamo.Object);

            //Se realiza suma de Isbn
            var sumaIsbn = bibliotecario.SumarIsbn(ISBN_MAYOR_TREINTA);

            //Calculo fecha máxima
            var fechaMaxima = bibliotecario.CalcularFechaMaximaEntrega(sumaIsbn);

            Assert.AreEqual(fechaMaximaEsperada, fechaMaxima);

        }

        [TestMethod]
        public void FechaEntregaSumaMenorATreintaTest()
        {
            DateTime fechaMaximaEsperada = new DateTime(0001, 01, 01);

            // Act
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro.Object, repositorioPrestamo.Object);

            //Se realiza suma de Isbn
            var sumaIsbn = bibliotecario.SumarIsbn(ISBN_MENOR_TREINTA);

            //Calculo fecha máxima
            var fechaMaxima = bibliotecario.CalcularFechaMaximaEntrega(sumaIsbn);

            Assert.AreEqual(fechaMaximaEsperada, fechaMaxima);

        }
    }
}
