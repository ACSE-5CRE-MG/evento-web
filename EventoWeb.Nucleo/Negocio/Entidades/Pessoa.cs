using EventoWeb.Nucleo.Negocio.Excecoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoWeb.Nucleo.Negocio.Entidades
{
    public enum SexoPessoa { Masculino, Feminino }

    public class Pessoa
    {
        private string m_Nome;
        private DateTime m_DataNascimento;
        private Endereco m_Endereco;
        private Boolean m_EhVegetariano;
        private String m_MedicamentosUsados;
        private SexoPessoa m_Sexo;
        private String m_InstituicoesEspiritasFrequenta;
        private String m_Observacoes;
        private string m_Email;

        protected Pessoa()
        {
        }

        public string Cidade { get; set; }
        public string UF { get; set; }

        public Pessoa(string nome, Endereco endereco, DateTime dataNascimento, SexoPessoa sexo, string email)
        {
            Nome = nome;
            DataNascimento = dataNascimento;
            Email = email;
            m_Endereco = endereco ?? throw new ExcecaoNegocioAtributo("Pessoa", "endereco", "Endereço não informado");
            m_Sexo = sexo;
        }

        public virtual string Nome
        {
            get
            {
                return m_Nome;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ExcecaoNegocioAtributo("Pessoa", "Nome", "Nome esta vazio");

                m_Nome = value;
            }
        }

        public virtual String NomeCracha { get; set; }

        public virtual string TelefoneFixo { get; set; }

        public virtual string Celular { get; set; }

        public virtual string Email 
        {
            get => m_Email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ExcecaoNegocioAtributo("Pessoa", "Email", "Email esta vazio");

                m_Email = value;
            }
        }

        public virtual DateTime DataNascimento
        {
            get
            {
                return m_DataNascimento;
            }

            set
            {
                DateTime dataHoje = DateTime.Now;
                dataHoje = dataHoje.AddMilliseconds(dataHoje.Millisecond * -1);
                dataHoje = dataHoje.AddSeconds(dataHoje.Second * -1);

                if (value > dataHoje)
                    throw new ArgumentException("Data deve ser menor que a data atual do sistema.", "DataNascimento");

                m_DataNascimento = value;
            }
        }

        public virtual Endereco Endereco { get { return m_Endereco; } }

        public virtual Boolean EhVegetariano 
        {
            get { return m_EhVegetariano; }
            set { m_EhVegetariano = value; }
        }

        public virtual Boolean EhDiabetico { get; set; }

        public virtual Boolean UsaAdocanteDiariamente { get; set; }

        public virtual String AlergiaAlimentos { get; set; }

        public virtual String TiposCarneNaoCome { get; set; }

        public virtual String MedicamentosControlados { get; set; }

        public virtual String MedicamentosUsados
        {
            get { return m_MedicamentosUsados; }
            set { m_MedicamentosUsados = value; }
        }      

        public virtual SexoPessoa Sexo
        {
            get { return m_Sexo; }
            set { m_Sexo = value; }
        }

        public virtual String InstituicoesEspiritasFrequenta
        {
            get { return m_InstituicoesEspiritasFrequenta; }
            set { m_InstituicoesEspiritasFrequenta = value; }
        }

        public virtual String Observacoes
        {
            get { return m_Observacoes; }
            set { m_Observacoes = value; }
        }

        public virtual int CalcularIdadeEmAnos(DateTime dataAtual)
        {
            if (dataAtual < m_DataNascimento)
                throw new ArgumentException("A data de comparação deve ser maior ou igual a do nascimento.", "dataAtual");

            return (dataAtual.Year - m_DataNascimento.Year - 1) +
                    (((dataAtual.Month > m_DataNascimento.Month) ||
                    ((dataAtual.Month == m_DataNascimento.Month) && (dataAtual.Day >= m_DataNascimento.Day))) ? 1 : 0);
        }
    }
}
