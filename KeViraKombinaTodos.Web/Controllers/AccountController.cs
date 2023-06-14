using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using KeViraKombinaTodos.Core.Services;
using KeViraKombinaTodos.Web.Models;

namespace KeViraKombinaTodos.Web.Controllers
{
    [Authorize]
	public class AccountController : Controller {

        #region Inject

        private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;
		private IUsuarioService _usuarioService;
		private IPerfilService _perfilService;
	
		public AccountController(IUsuarioService usuarioService, IPerfilService perfilService) {
            _usuarioService = usuarioService ?? throw new ArgumentException(nameof(usuarioService));
			_perfilService = perfilService ?? throw new ArgumentException(nameof(perfilService));
		}
        #endregion
  //      public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager) {
		//	UserManager = userManager;
		//	SignInManager = signInManager;
		//}

        #region Var's
        public ApplicationSignInManager SignInManager
		{
			get
			{
				return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
			}
			private set
			{
				_signInManager = value;
			}
		}

		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}
        #endregion
        
        [AllowAnonymous]
		public ActionResult Login(string returnUrl) {
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}
		
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl) {
			if (!ModelState.IsValid) {
				return View(model);
			}

			// This doesn't count login failures towards account lockout
			// To enable password failures to trigger account lockout, change to shouldLockout: true
			var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);


			switch (result) {
				case SignInStatus.Success:					
					return RedirectToLocal(returnUrl);
					
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.RequiresVerification:
					return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
				case SignInStatus.Failure:
				default:
					ModelState.AddModelError("", "Tentativa de login inválida.");
					return View(model);
			}
		}
        		
		[AllowAnonymous]
		public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe) {
			// Require that the user has already logged in via username/password or external login
			if (!await SignInManager.HasBeenVerifiedAsync()) {
				return View("Error");
			}
			return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
		}
		
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model) {
			if (!ModelState.IsValid) {
				return View(model);
			}

			// The following code protects for brute force attacks against the two factor codes. 
			// If a user enters incorrect codes for a specified amount of time then the user account 
			// will be locked out for a specified amount of time. 
			// You can configure the account lockout settings in IdentityConfig
			var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
			switch (result) {
				case SignInStatus.Success:
					return RedirectToLocal(model.ReturnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.Failure:
				default:
					ModelState.AddModelError("", "Invalid code.");
					return View(model);
			}
		}
		
		[AllowAnonymous]
		public ActionResult Register() {
			return View();
		}
		
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model) {
			if (ModelState.IsValid) {

                var user = new ApplicationUser
                {
                    Nome = model.Nome,
                    UserName = model.Email,
                    Email = model.Email,
                    PasswordHash = model.ConfirmPassword,
                    EmailConfirmed = false,
                    SecurityStamp = "",
                    PhoneNumber = model.PhoneNumber,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = true,
                    LockoutEnabled = false,
                    LockoutEndDateUtc = DateTime.Now,
                    AccessFailedCount = 0,
                    CPF = model.CPF,
                    //Password = HashPassWord(model.Senha),
                    DataCriacao = DateTime.Now,
                    IsEnabled = true
                };

                IdentityResult result = null;

                try
                {

				 result = await UserManager.CreateAsync(user, model.Senha);
                }
                catch (Exception ex)
                {
                    var e = ex.Message;
                    throw;
                }

				if (result.Succeeded)
                {
                    /* Precisa verificar no email o pq do SMTP ñ quer ajudar nós 
                   string code = await GenerateEmailConfirmationToken(user);
                   var callbackUrl = Url.Action(
                      "ConfirmEmail", "Account",
                      new { userId = user.Id, code = code },
                      protocol: Request.Url.Scheme);                    

                   try
                   {                        
                       await SendEmail(new IdentityMessage()
                       {
                           Subject = "Email de Confirmação de Cadastro",
                           Body = "Por favor clique no link pra confirmar o seu email:" + callbackUrl,
                           Destination = user.Email
                       });
                   }
                   catch (Exception ex)
                   {
                       throw new Exception(ex.Message);
                   }
                   */

                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
			}

			return View(model);
		}        
        public Task SendEmail(IdentityMessage message)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            //client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("dsistemaweb@gmail.com", "123456abc@RE");

            Task task = null;

            try
            {
                task = client.SendMailAsync("dsistemaweb@gmail.com", message.Destination, message.Subject, message.Body);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return task;
        }
        private async Task<string> GenerateEmailConfirmationToken(ApplicationUser user)
        {
            return await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        }
        
        [AllowAnonymous]
		public async Task<ActionResult> ConfirmEmail(int userId, string code) {
			if (userId == default(int) || code == null) {
				return View("Error");
			}
			var result = await UserManager.ConfirmEmailAsync(userId, code);
			return View(result.Succeeded ? "ConfirmEmail" : "Error");
		}
		
		[AllowAnonymous]
		public ActionResult ForgotPassword() {
			return View();
		}
		
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model) {
			if (ModelState.IsValid) {
				var user = await UserManager.FindByNameAsync(model.Email);
				if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id))) {
					// Don't reveal that the user does not exist or is not confirmed
					return View("ForgotPasswordConfirmation");
				}

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);                

                try
                {
                    await SendEmail(new IdentityMessage()
                    {
                        Subject = "Recuperação de Senha",
                        Body = "Clique no link para cadastrar uma nova senha:" + callbackUrl,
                        Destination = user.Email
                    });
                }
                catch (Exception ex)
                {
                    //nada faz por enquanto se dá erro.
                }


                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

			// If we got this far, something failed, redisplay form
			return View(model);
		}
		
		[AllowAnonymous]
		public ActionResult ForgotPasswordConfirmation() {
			return View();
		}

		[AllowAnonymous]
		public ActionResult ResetPassword(string code) {
			return code == null ? View("Error") : View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model) {
			if (!ModelState.IsValid) {
				return View(model);
			}
			var user = await UserManager.FindByNameAsync(model.Email);
			if (user == null) {
				// Don't reveal that the user does not exist
				return RedirectToAction("ResetPasswordConfirmation", "Account");
			}
			var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
			if (result.Succeeded) {
				return RedirectToAction("ResetPasswordConfirmation", "Account");
			}
			AddErrors(result);
			return View();
		}
		
		[AllowAnonymous]
		public ActionResult ResetPasswordConfirmation() {
			return View();
		}
		
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLogin(string provider, string returnUrl) {
			// Request a redirect to the external login provider
			return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
		}
		
		[AllowAnonymous]
		public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe) {
			var userId = await SignInManager.GetVerifiedUserIdAsync();
			if (userId == default(int)) {
				return View("Error");
			}
			var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
			var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
			return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
		}
		
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SendCode(SendCodeViewModel model) {
			if (!ModelState.IsValid) {
				return View();
			}

			// Generate the token and send it
			if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider)) {
				return View("Error");
			}
			return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
		}
		
		[AllowAnonymous]
		public async Task<ActionResult> ExternalLoginCallback(string returnUrl) {
			var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
			if (loginInfo == null) {
				return RedirectToAction("Login");
			}

			// Sign in the user with this external login provider if the user already has a login
			var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
			switch (result) {
				case SignInStatus.Success:
					return RedirectToLocal(returnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.RequiresVerification:
					return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
				case SignInStatus.Failure:
				default:
					// If the user does not have an account, then prompt the user to create an account
					ViewBag.ReturnUrl = returnUrl;
					ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
					return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
			}
		}
		
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl) {
			if (User.Identity.IsAuthenticated) {
				return RedirectToAction("Index", "Manage");
			}

			if (ModelState.IsValid) {
				// Get the information about the user from the external login provider
				var info = await AuthenticationManager.GetExternalLoginInfoAsync();
				if (info == null) {
					return View("ExternalLoginFailure");
				}
				var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
				var result = await UserManager.CreateAsync(user);
				if (result.Succeeded) {
					result = await UserManager.AddLoginAsync(user.Id, info.Login);
					if (result.Succeeded) {
						await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
						return RedirectToLocal(returnUrl);
					}
				}
				AddErrors(result);
			}

			ViewBag.ReturnUrl = returnUrl;
			return View(model);
		}
		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff() {
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			return RedirectToAction("Index", "Home");
		}

		[AllowAnonymous]
		public ActionResult ExternalLoginFailure() {
			return View();
		}
		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (_userManager != null) {
					_userManager.Dispose();
					_userManager = null;
				}

				if (_signInManager != null) {
					_signInManager.Dispose();
					_signInManager = null;
				}
			}

			base.Dispose(disposing);
		}

		#region Helpers
		// Used for XSRF protection when adding external logins
		private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private string HashPassWord(string passWord) {
			byte[] salt;
			new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

			var pbkdf2 = new Rfc2898DeriveBytes(passWord, salt, 10000);
			byte[] hash = pbkdf2.GetBytes(20);

			byte[] hashBytes = new byte[36];
			Array.Copy(salt, 0, hashBytes, 0, 16);
			Array.Copy(hash, 0, hashBytes, 16, 20);

			string savedPasswordHash = Convert.ToBase64String(hashBytes);

			return savedPasswordHash;
		}

		private void AddErrors(IdentityResult result) {
			foreach (var error in result.Errors) {				

				switch (error) {
					case "Passwords must have at least one non letter or digit character. Passwords must have at least one uppercase ('A'-'Z').":
						ModelState.AddModelError("", "As senhas devem ter pelo menos um caractere que não seja letra ou dígito. As senhas devem ter pelo menos uma letra maiúscula ('A' - 'Z').");
				break;

					case "Passwords must have at least one non letter or digit character. Passwords must have at least one lowercase ('a'-'z'). Passwords must have at least one uppercase ('A'-'Z').":
						ModelState.AddModelError("", "As senhas devem ter pelo menos um caractere que não seja letra ou dígito. As senhas devem ter pelo menos uma letra minúscula ('a' - 'z'). As senhas devem ter pelo menos uma letra maiúscula ('A' - 'Z').");
						break;

					case "Passwords must have at least one non letter or digit character.":
						ModelState.AddModelError("", "As senhas devem ter pelo menos um caractere que não seja letra ou dígito.");
						break;

					case "Passwords must have at least one uppercase ('A'-'Z').":
						ModelState.AddModelError("", "As senhas devem ter pelo menos uma letra maiúscula ('A' - 'Z').");
						break;

					default:
						ModelState.AddModelError("", error.Replace(" is already taken", " Já existe cadastrado"));
						break;
				}				
			}
		}

		private ActionResult RedirectToLocal(string returnUrl) {
			if (Url.IsLocalUrl(returnUrl)) {
				return Redirect(returnUrl);
			}
			return RedirectToAction("Index", "Home");
		}

		internal class ChallengeResult : HttpUnauthorizedResult {
			public ChallengeResult(string provider, string redirectUri)
				: this(provider, redirectUri, null) {
			}
			public ChallengeResult(string provider, string redirectUri, string userId) {
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}
			public string LoginProvider { get; set; }
			public string RedirectUri { get; set; }
			public string UserId { get; set; }
			public override void ExecuteResult(ControllerContext context) {
				var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
				if (UserId != null) {
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}
		#endregion
	}
}