using System;

namespace KeViraKombinaTodos.Core.Models
{
    public class Perfil : EntityBase
    {
		#region Public Properties

		public int PerfilID { get; set; }
		public string Descricao { get; set; }
		public string Codigo { get; set; }
		public bool SouTodoPoderoso { get; set; }
        public bool SouComprador { get; set; }
        public bool SouTransportador { get; set; }
        public DateTime? Datacriacao { get; set; }
        public DateTime? DataModificacao { get; set; }

        #endregion
    }
}
