using System;

namespace KeViraKombinaTodos.Core.Models
{
    public class Produto : EntityBase
    {

        #region Public Properties
        public int ProdutoID { get; set; }
        public string Descricao { get; set; }
        public string Codigo { get; set; }
        public decimal Valor { get; set; }
        public decimal Quantidade { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }

        #endregion
    }
}
