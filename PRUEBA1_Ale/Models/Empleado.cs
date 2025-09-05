namespace PRUEBA1_Ale.Models
{
    public abstract class Empleado
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public decimal Salario { get; set; }

        public abstract decimal CalcularBono();
    }

    public class Desarrollador : Empleado
    {
        public string? LenguajePrincipal { get; set; }

        public override decimal CalcularBono()
        {
            return Salario * 0.10m; 
        }
    }

    public class Gerente : Empleado
    {
        public string? Area { get; set; }

        public override decimal CalcularBono()
        {
            return Salario * 0.20m; 
        }
    }
}
