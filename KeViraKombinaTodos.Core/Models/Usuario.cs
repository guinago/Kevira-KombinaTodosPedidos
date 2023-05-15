using System;

namespace KeViraKombinaTodos.Core.Models {
	public class Usuario : EntityBase {

        #region Public Properties
        public int UsuarioID { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }

        #endregion
    }
}
