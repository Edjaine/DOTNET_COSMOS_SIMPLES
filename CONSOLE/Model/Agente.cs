namespace CONSOLE.Model
{
    public class Agente<T>
    {
        public string id { get; set; }
        public T documento { get; set; }
        public string tipo {get; set; } 
        
    }

    public class Cliente {
        public string nome { get; set; }
        public int idade { get; set; }

    }
}