using System;

namespace KeViraKombinaTodos.Core.Models
{
    public class AspNetUsers : EntityBase
    {
		#region Public Properties

		public int Id { get; set; }
		public string Email { get; set; }
		public bool EmailConfirmed { get; set; }
		public string PasswordHash { get; set; }
		public string SecurityStamp { get; set; }
		public string PhoneNumber { get; set; }
		public bool PhoneNumberConfirmed { get; set; }
		public bool TwoFactorEnabled { get; set; }
		public DateTime? LockoutEndDateUtc { get; set; }
		public bool LockoutEnabled { get; set; }
		public int AccessFailedCount { get; set; }
		public string UserName { get; set; }
		public int IDMaster { get; set; }
		public DateTime DataNascimento { get; set; }
		public DateTime DataCadastro { get; set; }
		public string CGC { get; set; }
		public string Razao { get; set; }
		public string Contato { get; set; }
		public string EnderecoNumero { get; set; }
		public string Endereco { get; set; }
		public string Estado { get; set; }
		public string Municipio { get; set; }
		public string Bairro { get; set; }
		public string Password { get; set; }
		public int TipoCGC { get; set; }
		public string Nome { get; set; }
		public bool IsEnabled { get; set; }
		public string CEP { get; set; }
		public int PerfilID { get; set; }
		#endregion
	}
}
