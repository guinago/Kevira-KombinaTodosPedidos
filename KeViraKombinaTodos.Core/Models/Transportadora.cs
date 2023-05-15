using System;

namespace KeViraKombinaTodos.Core.Models {
	public class Transportadora : EntityBase {

        #region Public Properties
        public int TransportadoraID { get; set; }
        public string Descricao { get; set; }
		public string Codigo { get; set; }		
		public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }

        #endregion
    }
}
