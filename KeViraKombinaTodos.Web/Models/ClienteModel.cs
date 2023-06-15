using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeViraKombinaTodos.Web.Models
{
    public class ClienteModel
    {
        public int ClienteID { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string CEP { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string Bairro { get; set; }
        public string Endereco { get; set; }
        public string Complemento { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }

    }
}