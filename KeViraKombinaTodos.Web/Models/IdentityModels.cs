using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace KeViraKombinaTodos.Web.Models {
	public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim> {

		#region Propriedades			
		//public DateTime DataCadastro { get; set; }
		//[Required]
		[Display(Name = "CPF")]
		public string CPF { get; set; }
		//public string Password { get; set; }
  //      public string email { get; set; }
		public string Nome { get; set; }
        public string SobreNome { get; set; }
        public bool? IsEnabled { get; set; }
        //public int PerfilID { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime? DataModif { get; set; }
        #endregion
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager) {
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType

			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

			userIdentity.AddClaim(new Claim("NomeID", this.Nome.ToString()));
			//userIdentity.AddClaim(new Claim("IDMaster", this.IDMaster.ToString()));
			userIdentity.AddClaim(new Claim("Id", this.Id.ToString()));

			// Add custom user claims here
			return userIdentity;
		}
	}

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim> {
		public ApplicationDbContext()
				: base("DefaultConnection") {
		}

		public static ApplicationDbContext Create() {
			return new ApplicationDbContext();
		}
	}

	public class CustomUserRole : IdentityUserRole<int> { }
	public class CustomUserClaim : IdentityUserClaim<int> { }
	public class CustomUserLogin : IdentityUserLogin<int> { }

	public class CustomRole : IdentityRole<int, CustomUserRole> {
		public CustomRole() { }
		public CustomRole(string name) { Name = name; }
	}

	public class CustomUserStore : UserStore<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim> {
		public CustomUserStore(ApplicationDbContext context)
				: base(context) {
		}
	}

	public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole> {
		public CustomRoleStore(ApplicationDbContext context)
				: base(context) {
		}
	}
}