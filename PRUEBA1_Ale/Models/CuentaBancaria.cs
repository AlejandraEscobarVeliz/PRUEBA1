namespace PRUEBA1_Ale.Models
{
    public class CuentaBancaria
    {
        private decimal saldo;
        public decimal Saldo => saldo;
        
        public CuentaBancaria(decimal saldoInicial)
        {
            if (saldoInicial < 0)
                throw new ArgumentException("El saldo inicial no puede ser negativo.");

            saldo = saldoInicial;
        }
        
        public void Depositar(decimal monto)
        {
            if (monto <= 0)
                throw new ArgumentException("El depósito debe ser mayor a 0.");

            saldo += monto;
        }
        
    }
    public class DepositoRequest
    {
        public decimal Monto { get; set; }
    }
}
