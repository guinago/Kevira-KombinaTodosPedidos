using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KeViraKombinaTodos.Web.Models
{
	public class ExternalLoginConfirmationViewModel {
		[Required, Display(Name = "E-mail")]
		public string Email { get; set; }
	}
	public class ExternalLoginListViewModel {
		public string ReturnUrl { get; set; }
	}
	public class SendCodeViewModel {
		public string SelectedProvider { get; set; }
		public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
		public string ReturnUrl { get; set; }
		public bool RememberMe { get; set; }
	}
	public class VerifyCodeViewModel {
		[Required]
		public string Provider { get; set; }
		[Required, Display(Name = "Código")]
		public string Code { get; set; }
		public string ReturnUrl { get; set; }
		[Display(Name = "Lembre-se deste navegador?")]
		public bool RememberBrowser { get; set; }
		public bool RememberMe { get; set; }
	}
	public class ForgotViewModel {
		[Required, Display(Name = "E-mail")]
		public string Email { get; set; }
	}
	public class LoginViewModel {
		[Required, Display(Name = "E-mail"), EmailAddress]
		public string Email { get; set; }
		[Required, DataType(DataType.Password), Display(Name = "Senha")]
		public string Password { get; set; }
		[Display(Name = "Lembre-me?")]
		public bool RememberMe { get; set; }
	}
	public class RegisterViewModel {
		[Required (ErrorMessage = "O e-mail é obrigatório"), EmailAddress, Display(Name = "E-mail")]
		public string Email { get; set; }
		[Required(ErrorMessage = "O nome é obrigatório"), Display(Name = "Nome")]
		public string Nome { get; set; }
        [Required(ErrorMessage = "A senha é obrigatória"), Display(Name = "Senha"), DataType(DataType.Password)]
		[StringLength(100, ErrorMessage = "O {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
		public string Senha { get; set; }
		[DataType(DataType.Password), Display(Name = "Confirmar Senha")]
		[Compare("Senha", ErrorMessage = "A senha de confirmação não corresponde.")]
		public string ConfirmPassword { get; set; }
		[Required(ErrorMessage = "O CPF é obrigatório"), Display(Name = "CPF")]
		public string CPF { get; set; }
        [Required(ErrorMessage = "O telefone é obrigatório"), Display(Name = "Telefone")]
        public string PhoneNumber { get; set; }
    }
	public class ResetPasswordViewModel {
		[Required, EmailAddress, Display(Name = "E-mail")]
		public string Email { get; set; }
		[Required, Display(Name = "Senha"), DataType(DataType.Password)]
		[StringLength(100, ErrorMessage = "O {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
		public string Password { get; set; }
		[DataType(DataType.Password), Display(Name = "Confirmar Senha")]
		[Compare("Password", ErrorMessage = "A senha de confirmação não corresponde.")]
		public string ConfirmPassword { get; set; }
		public string Code { get; set; }
	}
	public class ForgotPasswordViewModel {
        [Required, EmailAddress, Display(Name = "E-mail")]
        public string Email { get; set; }
	}
}
