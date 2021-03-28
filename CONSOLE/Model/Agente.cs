namespace CONSOLE.Model
{
    public class AgenteViewModel<T> 
    {
        public string id { get; set; }
        public T documento { get; set; }
        public string tipo {get; set; } 
        
    }

    public class ClienteViewModel {
        public string nome { get; set; }
        public object idade { get; set; }

    }

    public class NovoClienteViewModel {

        public NovoClienteViewModel()
        {

        }        

        public NovoClienteViewModel(string nome, int idade)
        {
            this.nome = nome;
            this.idade = idade;
        }

        public string nome { get; set; }
        public int idade { get; set; }
    }
}