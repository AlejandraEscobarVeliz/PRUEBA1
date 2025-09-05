using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRUEBA1_Ale.Models;
using System.Text.RegularExpressions;

namespace PRUEBA1_Ale.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase //heredamos todos los atributos del controller, osea se le da funcionalidades
    {
        //============EJEMPLOS=============
        #region 1. Hola MUndo
        [HttpGet] //como llamar ; en etiquetas
        public IActionResult Get()  // creacion del metodo                          llar a un enpoint, devolver un get
        {
            return Ok("Hola Mundo"); //devolver un ,ok es el estado 200   y devuelva un texto
        }
        #endregion

        #region 2. Saludo
        [HttpGet("saludar/{nombre}")]//necesita el parametro 
        public IActionResult Saludar(string nombre)//en la clase
        {
            return Ok($"Hola {nombre}");  //metodo de la interpolacion, como contenacion de valores
        }

        #endregion

        #region 3. Sumar 2 numeros
        [HttpGet("sumar/{a}/{b}")]
        public IActionResult Sumar(int a, int b)
        {
            int resultado = a + b;
            return Ok($"La suma de {a} + {b} es {resultado}");
        }
        #endregion

        #region 4. Par-Impar
        [HttpGet("par-impar/{numero}")]
        public IActionResult VerificarParImpar(int numero)
        {
            if (numero % 2 == 0)  //sacamos el modulo
            {
                return Ok($"El numero {numero} es par");
            }
            else
            {
                return Ok($"El numero {numero} es impar");  //400 badrequest, son parametros incorrectos
            }
        }
        #endregion

        #region 5. Lista de Frutas
        //accesible solo por la clase que esta dentro, solo lo ve la clase de VALUES
        private static readonly List</*poner el tipo de dato*/ string> frutas =
            //con new creamos una instancia del obtjeto
            new List<string>()  //solo esta lista deja que se vea, static usamos para no instanciar= es reservar un espacio en memoria
            //si no instanciamos no nos deja
            {
                "manzana",
                "banana",
                "fresa",
                "naranja"
            };
        //muestro la lista con el endpoint ->
        [HttpGet("frutas")]
        public IActionResult ObtenerFrutas()
        {
            return Ok(frutas);
        }
        #endregion
        //I enumerable hablamos de listas, de cualquier tipo

        #region 7. Filtro pares

        [HttpGet("filtrar-pares")]
        public IActionResult FiltrarPares(
            /*[Frombody]*/ List<int> numeros)
        {
            List<int> pares = new();
            //yo necesito una lista que me haga la interacion
            //usaremos lo que es un foreach
            foreach (int num in numeros)  //ponemos el tipo de dato de la interacion, en donde?, en numero
            {
                if (num % 2 == 0)
                {
                    pares.Add(num);//guardamos en la lista anterior
                }
            }
            return Ok(pares); //retornamos los pares
        }

        //las listas no se pueden enviar por rutas, asi que lo mandamos por
        //lista de vilisous
        //la que vemos es la de ruta osea nuestro /(slash) y su parametro
        //existe el frombody tambien = envia parametros por abajo no la URL; y se hace por postman, creando un json
        //pero si establecemos una etiqueta
        #endregion

        //================================10 EJERCICIOS========================================
        //1.
        #region TRADUCTOR DE PALABRAS
        private static readonly Dictionary<string, string> Diccionario =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "hello", "hola" },
                { "world", "mundo" },
                { "goodbye", "adiós" },
                { "cat", "gato" },
                { "dog", "perro" },
                { "computer", "computadora" },
                { "keyboard", "teclado" }
            };
        [HttpGet("traducir/{palabra}")]
        public IActionResult Traducir(string palabra)
        {
            if (Diccionario.TryGetValue(palabra, out string traduccion))
            {
                return Ok(new { palabra, traduccion });
            }

            return NotFound(new { error = $"No hay traduccion para esta palabra '{palabra}'." });
        }

        #endregion

        //2.
        #region CONTADOR DE PALABRAS
        [HttpPost("contar-palabras")]                    // POST /api/values/contar-palabras
        public IActionResult ContarPalabras([FromBody] string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return BadRequest("Debes enviar texto en el body.");

            // \b borde de palabra, \p{L} letras unicode, \p{N} números, _ guión bajo
            int cantidad = Regex.Matches(texto, @"\b[\p{L}\p{N}_]+\b").Count;

            return Ok(new { texto, cantidad });
        }
        #endregion

        //3.
        #region RETORNAR PRODUCTOS
        private static readonly List<Producto> Productoss = new List<Producto>
        {
            new Producto { Id = 1, Nombre = "Lápiz",        Precio = 3 },
            new Producto { Id = 2, Nombre = "Cuaderno",     Precio = 20 },
            new Producto { Id = 3, Nombre = "Mochila",      Precio = 120 },
            new Producto { Id = 4, Nombre = "Audífonos",    Precio = 230 },
            new Producto { Id = 5, Nombre = "Resaltador",   Precio = 8 }
        };

        [HttpGet("productos")]                           // GET /api/values/productos
        public IActionResult ObtenerProductos()
        {
            return Ok(Productoss);
        }
        #endregion

        //4.
        #region EMPLEADO Y GERENTES
        [HttpGet("empleados")]                           // GET /api/values/empleados
        public IActionResult ObtenerEmpleados()
        {
            List<Empleado> empleados = new List<Empleado>
            {
                new Gerente { Id = 1, Nombre = "Catalina",  Salario = 8000,   Area = "COnta" },
                new Desarrollador { Id = 2, Nombre = "Guzman", Salario = 5000, LenguajePrincipal = "C#" },
                new Gerente { Id = 3, Nombre = "Alejandra",   Salario = 9000,   Area = "Operaciones" },
                new Desarrollador { Id = 4, Nombre = "Sara",  Salario = 5200, LenguajePrincipal = "JavaScript" }
            };

            var salida = empleados.Select(e => new
            {
                e.Id,
                e.Nombre,
                e.Salario,
                Tipo = e.GetType().Name,
                Bono = e.CalcularBono()
            });

            return Ok(salida);
        }
        #endregion

        //5.
        #region CUENTA BANCARIA
        private static readonly CuentaBancaria Cuenta =
            new CuentaBancaria(saldoInicial: 100);
        [HttpPost("depositar")]                          // POST /api/values/depositar
        public IActionResult Depositar([FromBody] DepositoRequest req)
        {
            if (req == null) return BadRequest("Debes enviar un monto.");
            try
            {
                Cuenta.Depositar(req.Monto);
                return Ok(new { mensaje = "Deposito exitoso", saldoActual = Cuenta.Saldo });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        #endregion

        //6.
        #region VALIDAR EDAD
        [HttpGet("validar-edad/{edad}")]                // GET /api/values/validar-edad/17
        public IActionResult ValidarEdad(int edad)
        {
            if (edad < 18)
            {
                return BadRequest(new { error = "La edad mínima es 18." });
            }
            return Ok(new { mensaje = "Edad Valida." });
        }
        #endregion

        //7.
        #region FILTRAR PRECIO
        [HttpGet("productos/caros")]                    // GET /api/values/productos/caros
        public IActionResult ProductosCaros()
        {
            var caros = Productoss.Where(p => p.Precio > 100).ToList();
            return Ok(caros);
        }
        #endregion

        //8.
        #region ORDENAR USUARIOS POR EDAD
        private static readonly List<Usuario> usuarios = new List<Usuario>
        {
            new Usuario { Nombre = "Ana",  Email = "ana@example.com",  Edad = 22 },
            new Usuario { Nombre = "Luis", Email = "luis@example.com", Edad = 19 },
            new Usuario { Nombre = "Mia",  Email = "mia@example.com",  Edad = 31 },
            new Usuario { Nombre = "Pau",  Email = "pau@example.com",  Edad = 27 },
        };

        [HttpGet("usuarios/ordenados")]                 // GET /api/values/usuarios/ordenados?direccion=asc|desc
        public IActionResult UsuariosOrdenados([FromQuery] string? direccion = "asc")
        {

            string dir = (direccion ?? "asc").Trim().ToLower();

            var ordenados = (dir == "desc")
                ? usuarios.OrderByDescending(u => u.Edad)
                : usuarios.OrderBy(u => u.Edad);

            return Ok(ordenados);
        }
        #endregion

        //9.
        #region DIVIDIR CON Try-Catch
        [HttpGet("dividir/{a}/{b}")]              // GET /api/values/dividir/10/2
        public IActionResult Dividir(int a, int b)
        {
            try
            {
                // Intentamos la división
                int resultado = a / b;
                return Ok(new { a, b, resultado });
            }
            catch (DivideByZeroException)         
            {
                return BadRequest(new { error = "No se puede dividir entre cero." });
            }
        }
        #endregion

        //10.
        #region ELEVAR AL CUADRADO

        [HttpGet("elevar/{numero}")]              // GET /api/values/elevar/5
        public IActionResult Elevar(int numero)
        {
            
            Func<int, int> cuadrado = x => x * x;

            
            int resultado = cuadrado(numero);

            return Ok(new { numero, cuadrado = resultado });
        }
        #endregion
    }
}
    
