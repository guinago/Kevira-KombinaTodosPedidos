using System;

namespace KeViraKombinaTodos.Core.Models {
	public class CondicaoPagamento : EntityBase {

        #region Public Properties
        public int CondicaoPagamentoID { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public int? CondPgto { get; set; }

        #endregion
    }
}
